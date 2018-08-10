

/// <summary>
/// 对unity的工具库的扩展
/// </summary>
namespace SGF.Core
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine;

    namespace Security
    {
        /// <summary>
        /// MD5工具类
        /// </summary>
        public class Md5
        {

            /// <summary>
            /// 生成字符串的SHA1码
            /// </summary>
            /// <param name="str">原字符串</param>
            /// <returns></returns>
            public static string GetSHA1WithString(string str)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                return GetSHA1WithBytes(bytes);
            }

            /// <summary>
            /// 生成byte数组的SHA1码
            /// </summary>
            /// <param name="bytes">bytes</param>
            /// <returns></returns>
            public static string GetSHA1WithBytes(byte[] bytes)
            {
                SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                byte[] str2 = sha1.ComputeHash(bytes);
                sha1.Clear();
                (sha1 as IDisposable).Dispose();
                StringBuilder strbul = new StringBuilder(40);
                for (int i = 0; i < str2.Length; i++)
                {
                    strbul.Append(str2[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

                }
                return strbul.ToString();
            }

            /// <summary>
            /// 生成文件的SHA1码
            /// </summary>
            /// <param name="path">文件路径</param>
            /// <returns></returns>
            public static string GetSHA1WithFile(string path)
            {
                byte[] bytes = System.IO.File.ReadAllBytes(path);
                return GetSHA1WithBytes(bytes);
            }

#if UTEST
            // [UExt.Core.EnterpriseServerBase.Aop.MethodAopSwitcher(true)]
            public static void Test()
            {
                string rawstr = "stringstringstringstringstringstringstringstring";
                string sha1 = Md5.GetSHA1WithString(rawstr);
                if (sha1 != "8e3500ff375737d5ab9298fdeef878e53a0261c9") throw new UExt.UTestException("md5 的sha1 算法测试失败");
            }
#endif
        }
    }
}