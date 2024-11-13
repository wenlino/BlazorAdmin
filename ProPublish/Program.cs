using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;


namespace ProPublish
{
    internal class Program
    {
        //此项目主要是一个配套发布工具，使用请自行修改路径等


        public static string ProFolder = "D:\\CSDATA\\AdminSenyun\\AdminSenyun";
        public static string ProServerFolder = $"{ProFolder}\\AdminSenyun.Server";
        public static string OutputFolder = $"{ProServerFolder}\\bin\\publish";
        static void Main(string[] args)
        {



            //获取所有项目
            Console.OutputEncoding = Encoding.UTF8;

            //删除全部发布文件
            if (Directory.Exists(OutputFolder))
            {
                Console.WriteLine("删除发布目录：" + OutputFolder);
                Directory.Delete(OutputFolder, true);
            }



            EditVersion();

            //发布主项目
            DotNetPublish($"{ProServerFolder}\\AdminSenyun.Server.csproj", true);


            //删除发布目录下的所有pdb文件
            Directory.GetFiles(OutputFolder, "*.pdb", SearchOption.AllDirectories).ToList().ForEach(File.Delete);


            //创建发布目录下的插件文件夹
            var pfolder = $"{OutputFolder}\\Plugins";
            Directory.CreateDirectory(pfolder);


            //获取全部插件
            var files = Directory.GetFiles($"{ProFolder}\\Plugins", "*.csproj", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var folder = Path.GetDirectoryName(file);
                var oldfolder = Path.Combine(folder, "bin\\publish");
                var name = Path.GetFileNameWithoutExtension(file);

                var olddllFile = Path.Combine(folder, "bin\\publish", name + ".dll");
                var copydllFile = Path.Combine(pfolder, name + ".dll");

                //删除发布目录下的文件
                if (Directory.Exists(oldfolder))
                    Directory.Delete(oldfolder, true);

                DotNetPublish(file, false);

                //复制文件到项目中
                File.Copy(olddllFile, copydllFile, true);
            }

            FilesTo7ZipExe();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("已经全部发布完成");
            Console.ReadKey();
        }

        private static void EditVersion()
        {

            //获取现有版本号

            string path = $"{ProFolder}\\Directory.Build.props";

            var xml = new XmlDocument();
            xml.Load(path);

            var version = xml.SelectSingleNode("Project/PropertyGroup/Version");


            var vis = Version.Parse(version.InnerText);

            var newvis = version.InnerText;

        s:;

            Console.WriteLine("当前版本号：" + vis);
            Console.WriteLine("输入版本号或者选择新版本号：");
            ConsoleFg();

            var a = $"{vis.Major}.{vis.Minor}.{vis.Build}.{vis.Revision + 1}";
            var b = $"{vis.Major}.{vis.Minor + 1}.{vis.Build}.{vis.Revision}";
            var c = $"{vis.Major}.{vis.Minor + 1}.{vis.Build}.{vis.Revision + 1}";
            var d = $"{vis.Major + 1}.{0}.{vis.Build}.{vis.Revision + 1}";
            Console.WriteLine("A:" + a);
            Console.WriteLine("B:" + b);
            Console.WriteLine("C:" + c);
            Console.WriteLine("D:" + d);
            Console.WriteLine($"Q(Major+1),W(Minor+1),E(Build+1),R(Revision+1)；(例如：QE {vis.Major + 1}.{vis.Minor}.{vis.Build + 1}.{vis.Revision})");
            ConsoleFg();

            var inputText = Console.ReadLine();

            var major = vis.Major < 0 ? 1 : vis.Major;
            var minor = vis.Minor < 0 ? 1 : vis.Minor;
            var build = vis.Build < 0 ? 1 : vis.Build;
            var revision = vis.Revision < 0 ? 1 : vis.Revision;

            if (string.IsNullOrWhiteSpace(inputText))
            {
                return;
            }
            else if (inputText.ToLower().Contains("q") || inputText.ToLower().Contains("w") || inputText.ToLower().Contains("e") || inputText.ToLower().Contains("r"))
            {
                switch (inputText.ToLower())
                {
                    case "q":
                        major++;
                        break;
                    case "w":
                        minor++;
                        break;
                    case "e":
                        build++;
                        break;
                    case "r":
                        revision++;
                        break;
                    default:
                        break;
                }
            }
            else if (inputText.ToLower() == "a")
            {
                revision++;
            }
            else if (inputText.ToLower() == "b")
            {
                minor++;
            }
            else if (inputText.ToLower() == "c")
            {
                minor++;
                revision++;
            }
            else if (inputText.ToLower() == "d")
            {
                major++;
                minor = 0;
                revision++;
            }
            else
            {
                try
                {
                    var nv = new Version(inputText);

                    if (vis.CompareTo(nv) > 0)
                    {
                        throw new Exception("版本号不能小于历史版本号");
                    }

                    major = nv.Major < 0 ? 0 : nv.Major;
                    minor = nv.Minor < 0 ? 0 : nv.Minor;
                    build = nv.Build < 0 ? 0 : nv.Build;
                    revision = nv.Revision < 0 ? 0 : nv.Revision;

                }
                catch (Exception ex)
                {
                    Console.Clear();
                    ConsoleFg();
                    Console.WriteLine(ex.Message);
                    ConsoleFg();
                    goto s;
                }
            }
            newvis = $"{major}.{minor}.{build}.{revision}";

            Console.WriteLine($"确定新版本（V{newvis} 确认：Y）:");
            var sta = Console.ReadLine();
            if (sta.ToLower() == "y")
            {
                version.InnerText = newvis;
                xml.Save(path);
            }
            else
            {
                Console.Clear();
                ConsoleFg();
                Console.WriteLine("取消确定，请输入版本号");
                ConsoleFg();
                goto s;
            }
        }

        private static void ConsoleFg()
        {
            Console.WriteLine(new string('-', 100));
        }

        private static void DotNetPublish(string path, bool selfContained)
        {
            var folder = Path.GetDirectoryName(path);
            var oldfolder = Path.Combine(folder, "bin\\publish");
            var name = Path.GetFileNameWithoutExtension(path);

            var arguments = $"publish \"{path}\" -r win-x64 -c Release -o \"{oldfolder}\" {(selfContained ? "--self-contained true" : "")}";

            Console.WriteLine("发布 " + path);
            StartProcess(GetProcessStartInfo("dotnet", arguments));
            Console.WriteLine("发布成功");
            Console.WriteLine();
            Console.WriteLine();
        }


        private static void FilesTo7ZipExe()
        {
            var bin = $"{ProServerFolder}\\bin";
            var publishFolder = $"{bin}\\publish";

            //获取文件版本信息
            var fileVersion = FileVersionInfo.GetVersionInfo(publishFolder + "\\AdminSenyun.Server.exe").FileVersion;

            var arguments = $"a -t7z -sfx7z.sfx \"{bin}\\Senyun_V{fileVersion}.exe\" \"{publishFolder}\"";
            Console.WriteLine("开始压缩 " + publishFolder);
            StartProcess(GetProcessStartInfo("C:\\Program Files\\7-Zip\\7z.exe", arguments));
            Console.WriteLine("压缩成功 ");
            Console.WriteLine();
        }


        private static ProcessStartInfo GetProcessStartInfo(string fileName, string arguments)
        {
            return new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments, // 你可以在这里添加其他参数
                RedirectStandardOutput = true, // 重定向标准输出
                RedirectStandardError = true, // 重定向标准错误输出
                UseShellExecute = false, // 不使用操作系统的外壳来启动进程
                CreateNoWindow = false // 不创建窗口
            };
        }


        private static void StartProcess(ProcessStartInfo processStartInfo)
        {
            try
            {
                using var process = new Process();

                process.StartInfo = processStartInfo;

                // 启动进程
                process.Start();

                // 读取输出
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                // 等待进程结束
                process.WaitForExit();

                // 打印输出和错误信息
                Console.WriteLine("Output:");
                Console.WriteLine(output);
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine("Error:");
                    Console.WriteLine(error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
