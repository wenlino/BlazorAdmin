using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AdminSenyun.Data.Security
{
    public static class LgbCryptography
    {
        /// <summary>
        /// 生成 Salt 方法
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string GenerateSalt(int length = 36)
        {
            RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            byte[] array = new byte[length];
            randomNumberGenerator.GetBytes(array);
            return Convert.ToBase64String(array);
        }


        /// <summary>
        /// 加密算法
        /// </summary>
        /// <param name="rgbKey">加密 Key</param>
        /// <param name="rgbIV">加密 IV</param>
        /// <param name="data">加密原始数据</param>
        /// <param name="algo">加密算法</param>
        /// <returns>密文集合</returns>
        public static byte[] Encrypt(byte[] rgbKey, byte[] rgbIV, byte[] data, SymmetricAlgorithm algo)
        {
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, algo.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);

            cryptoStream.Write(data, 0, data.Length);

            return memoryStream.ToArray();
        }


        /// <summary>
        /// 解密算法
        /// </summary>
        /// <param name="rgbKey">解密 Key</param>
        /// <param name="rgbIV">解密 IV</param>
        /// <param name="cpText">解密原始数据</param>
        /// <param name="algo">解密算法</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] rgbKey, byte[] rgbIV, byte[] cpText, SymmetricAlgorithm algo)
        {
            byte[] array = new byte[200];
            using var memoryStream = new MemoryStream();
            using var stream = new MemoryStream(cpText);

            using var cryptoStream = new CryptoStream(stream, algo.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Read);
            int num = 0;
            while ((num = cryptoStream.Read(array, 0, array.Length)) > 0)
            {
                memoryStream.Write(array, 0, num);
            }

            return memoryStream.ToArray();
        }


        /// <summary>
        /// 加密算法
        /// </summary>
        /// <param name="permissionKey">秘钥</param>
        /// <param name="plainText">待加密数据</param>
        /// <param name="privateEncrypt">是否为私钥加密，默认为 false 即公钥加密</param>
        /// <returns></returns>
        public static byte[] Encrypt(string permissionKey, byte[] plainText, bool privateEncrypt = false)
        {
            using RSA rSA = RSA.Create();
            rSA.FromXmlString2(permissionKey);
            return privateEncrypt ? rSA.PrivareEncryption(plainText) : rSA.Encrypt(plainText, RSAEncryptionPadding.Pkcs1);
        }

 
        /// <summary>
        /// 解密许可证书
        /// </summary>
        /// <param name="cipherText">加密的许可证书密文</param>
        /// <param name="publicDecrypt">是否为公钥解密，默认为 false 即私钥解密</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] cipherText, bool publicDecrypt = false)
        {
            return Decrypt(cipherText, "<RSAKeyValue><Modulus>rmvnJiAZfqxQNXEHQTHCQq6rt48X1lrkv66byRg/OD2aHR+3PJtxwBFGTfuR+8XqoQ1d5G5qAiqU6HBheUYemAmwhibeqjzxVM+h4Xu4Pxoy9KBTXjmOv+23tsuAPtavNJCs2uBrttUEXfJF/8aerl64pPyspejcWs+m9n0wWXk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", publicDecrypt);
        }


        /// <summary>
        /// 解密许可证书
        /// </summary>
        /// <param name="cipherText">加密的许可证书密文</param>
        /// <param name="permissionKey">权限使用钥匙密文</param>
        /// <param name="publicDecrypt">是否为公钥解密，默认为 false 即私钥解密</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] cipherText, string permissionKey, bool publicDecrypt = false)
        {
            using var rSA = RSA.Create();
            rSA.FromXmlString2(permissionKey);
            return publicDecrypt ? rSA.PublicDecryption(cipherText) : rSA.Decrypt(cipherText, RSAEncryptionPadding.Pkcs1);
        }

        /// <summary>
        /// 计算哈希值方法 内部使用 SHA256CryptoServiceProvider 算法
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="salt">Salt 值</param>
        /// <returns></returns>
        public static string ComputeHash(string data, string salt)
        {
            return ComputeHash(data, salt, SHA256.Create());
        }

        /// <summary>
        /// 计算哈希值方法
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="salt">Salt 值</param>
        /// <param name="algorithm">算法实例</param>
        /// <returns></returns>
        public static string ComputeHash(string data, string salt, HashAlgorithm algorithm)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data + salt);
            return Convert.ToBase64String(algorithm.ComputeHash(bytes));
        }

        /// <summary>
        /// 来自xml字符串。
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="xmlString"></param>
        /// <exception cref="Exception"></exception>
        public static void FromXmlString2(this RSA rsa, string xmlString)
        {
            RSAParameters parameters = default(RSAParameters);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);
            if (xmlDocument.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode childNode in xmlDocument.DocumentElement.ChildNodes)
                {
                    if (childNode != null)
                    {
                        switch (childNode.Name)
                        {
                            case "Modulus":
                                parameters.Modulus = Convert.FromBase64String(childNode.InnerText);
                                break;
                            case "Exponent":
                                parameters.Exponent = Convert.FromBase64String(childNode.InnerText);
                                break;
                            case "P":
                                parameters.P = Convert.FromBase64String(childNode.InnerText);
                                break;
                            case "Q":
                                parameters.Q = Convert.FromBase64String(childNode.InnerText);
                                break;
                            case "DP":
                                parameters.DP = Convert.FromBase64String(childNode.InnerText);
                                break;
                            case "DQ":
                                parameters.DQ = Convert.FromBase64String(childNode.InnerText);
                                break;
                            case "InverseQ":
                                parameters.InverseQ = Convert.FromBase64String(childNode.InnerText);
                                break;
                            case "D":
                                parameters.D = Convert.FromBase64String(childNode.InnerText);
                                break;
                        }
                    }
                }

                rsa.ImportParameters(parameters);
                return;
            }

            throw new Exception("Invalid XML RSA key.");
        }

        /// <summary>
        /// 抛出xml字符串。
        /// 果设置为true，则包括私有参数。
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="includePrivateParameters"></param>
        /// <returns>返回xml字符串</returns>
        public static string ToXmlString2(this RSA rsa, bool includePrivateParameters)
        {
            RSAParameters rSAParameters = rsa.ExportParameters(includePrivateParameters);
            if (includePrivateParameters)
            {
                return $"<RSAKeyValue><Modulus>{Convert.ToBase64String(rSAParameters.Modulus)}</Modulus><Exponent>{Convert.ToBase64String(rSAParameters.Exponent)}</Exponent><P>{Convert.ToBase64String(rSAParameters.P)}</P><Q>{Convert.ToBase64String(rSAParameters.Q)}</Q><DP>{Convert.ToBase64String(rSAParameters.DP)}</DP><DQ>{Convert.ToBase64String(rSAParameters.DQ)}</DQ><InverseQ>{Convert.ToBase64String(rSAParameters.InverseQ)}</InverseQ><D>{Convert.ToBase64String(rSAParameters.D)}</D></RSAKeyValue>";
            }

            return $"<RSAKeyValue><Modulus>{Convert.ToBase64String(rSAParameters.Modulus)}</Modulus><Exponent>{Convert.ToBase64String(rSAParameters.Exponent)}</Exponent></RSAKeyValue>";
        }

        /// <summary>
        /// 私钥加密方法
        /// </summary>
        /// <param name="rsa">RSA 实例</param>
        /// <param name="data">待加密数据</param>
        /// <returns>加密后数据 字节数组</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static byte[] PrivareEncryption(this RSA rsa, byte[] data)
        {
            int num = rsa.KeySize / 8 - 6;
            if (data.Length > num)
            {
                throw new ArgumentOutOfRangeException("data", $"Maximum data length for the current key size ({rsa.KeySize} bits) is {num} bytes (current length: {data.Length} bytes)");
            }

            BigInteger big = GetBig(AddPadding(data));
            RSAParameters rSAParameters = rsa.ExportParameters(includePrivateParameters: true);
            BigInteger big2 = GetBig(rSAParameters.D);
            BigInteger big3 = GetBig(rSAParameters.Modulus);
            return BigInteger.ModPow(big, big2, big3).ToByteArray();
        }

        private static BigInteger GetBig(byte[] data)
        {
            byte[] array = (byte[])data.Clone();
            Array.Reverse(array);
            byte[] array2 = new byte[array.Length + 1];
            Array.Copy(array, array2, array.Length);
            return new BigInteger(array2);
        }

        private static byte[] AddPadding(byte[] data)
        {
            Random random = new Random();
            byte[] array = new byte[4];
            random.NextBytes(array);
            array[0] = (byte)(array[0] | 0x80u);
            byte[] array2 = new byte[data.Length + 4];
            Array.Copy(array, array2, 4);
            Array.Copy(data, 0, array2, 4, data.Length);
            return array2;
        }

        /// <summary>
        /// 公钥解密方法
        /// </summary>
        /// <param name="rsa">RSA 实例</param>
        /// <param name="cipherData">待解密数据 字节数组</param>
        /// <returns>解密后数据 字节数组</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] PublicDecryption(this RSA rsa, byte[] cipherData)
        {
            if (cipherData == null)
            {
                throw new ArgumentNullException("cipherData");
            }

            BigInteger value = new BigInteger(cipherData);
            RSAParameters rSAParameters = rsa.ExportParameters(includePrivateParameters: false);
            BigInteger big = GetBig(rSAParameters.Exponent);
            BigInteger big2 = GetBig(rSAParameters.Modulus);
            byte[] array = BigInteger.ModPow(value, big, big2).ToByteArray();
            byte[] array2 = new byte[array.Length - 1];
            Array.Copy(array, array2, array2.Length);
            array2 = RemovePadding(array2);
            Array.Reverse(array2);
            return array2;
        }

        private static byte[] RemovePadding(byte[] data)
        {
            byte[] array = new byte[data.Length - 4];
            Array.Copy(data, array, array.Length);
            return array;
        }
    }
}
