namespace AdminSenyun.Data.Service;

public class SysFileService(ISqlSugarClient db, ISysSetting setting)
{
    private readonly string RootFolder = setting.GetSettingString("SysFileRootPath") ??
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UploadFile");

    private readonly ISqlSugarClient db = db;

    /// <summary>
    /// 数据库中获取文件属性
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public SysFile GetSysFile(long id)
    {
        return db.Queryable<SysFile>()
            .Where(it => it.Id == id)
            .OrderByDescending(it => it.CreateTime)
            .First();
    }

    /// <summary>
    /// 获取文件的byte[]
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public byte[]? GetFileBytes(long id)
    {
        var file = GetSysFile(id);

        if (file is null)
            return null;

        //获取文件
        var filename = file.Id + file.Ext;

        string filePath = Path.Combine(RootFolder, file.Folder, filename);

        //获取文件
        var bytes = File.ReadAllBytes(filePath);

        //返回数据
        return bytes;
    }

    /// <summary>
    /// 获取文件的 byte[]
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public byte[] GetFileBytes(SysFile file)
    {
        //获取文件
        var filename = file.Id + file.Ext;

        string filePath = Path.Combine(RootFolder, file.Folder, filename);

        //获取文件
        var bytes = File.ReadAllBytes(filePath);

        //返回数据
        return bytes;
    }

    /// <summary>
    /// 获取文件的 Stream
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public Stream GetFileStream(SysFile file)
    {
        var bytes = GetFileBytes(file);
        return new MemoryStream(bytes);
    }

    /// <summary>
    /// 获取文件的 Stream
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Stream GetFileStream(long id)
    {
        var bytes = GetFileBytes(id);
        return new MemoryStream(bytes);
    }
}
