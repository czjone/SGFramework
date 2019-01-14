namespace SGF.Core
{
    using System.IO;
    using System.Security.Cryptography;
    using System;

    namespace Security
    {

        /// <summary>
        /// 加密解密异常
        /// </summary>
        public class EncryptExeption : System.Exception {
            public EncryptExeption (string msg) : base (msg) {

            }
        }

        /// <summary>
        /// 加密解决接口
        /// </summary>
        public interface IEncryption {
            /// <summary>
            /// 加密数据
            /// </summary>
            /// <param name="data">明文</param>
            /// <param name="key">key(长度不能小于16)</param>
            /// <returns></returns>
            byte[] Encryption (byte[] data, byte[] key);
            /// <summary>
            /// 解密数据
            /// </summary>
            /// <param name="data">密文</param>
            /// <param name="key">key(长度不能小于16)</param>
            /// <returns></returns>
            byte[] Decryption (byte[] data, byte[] key);
        }

        public class DesEncryption : IEncryption {

            class DesKey {
                public byte[] key { get; set; }

                public byte[] iv { get; set; }
            }

            // private static byte[] key_8 = { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef };
            // private static byte[] IV_8 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
            private const int keyIvMinCount = 16;
            private byte[] Encrypt (byte[] data, byte[] key, byte[] iv_8) {
                var des = new DESCryptoServiceProvider ();
                using (var ms = new MemoryStream ()) {
                    try {
                        using (var cs = new CryptoStream (ms, des.CreateEncryptor (key, iv_8), CryptoStreamMode.Write)) {
                            cs.Write (data, 0, data.Length);
                            cs.FlushFinalBlock ();
                        }
                        return ms.ToArray ();
                    } catch (Exception ex) {
                        throw ex;
                    }
                }
            }

            private byte[] Decrypt (byte[] data, byte[] key, byte[] iv) {
                var des = new DESCryptoServiceProvider ();
                using (var ms = new MemoryStream ()) {
                    try {
                        using (var cs = new CryptoStream (ms, des.CreateDecryptor (key, iv), CryptoStreamMode.Write)) {
                            cs.Write (data, 0, data.Length);
                            cs.FlushFinalBlock ();
                        }

                        return ms.ToArray ();
                    } catch (Exception ex) {
                        throw ex;
                    }
                }
            }

            /// <summary>
            /// 外部传入的key 转换成 des的key（前8个字节） 和 iv(反8字节)
            /// </summary>
            /// <param name="key">原始的key</param>
            /// <param name="desKey">原始的desKey</param>
            private void outKeyIV (byte[] key, ref DesKey desKey) {
                desKey.key = new byte[keyIvMinCount / 2];
                desKey.iv = new byte[keyIvMinCount / 2];
                for (int i = 0; i < keyIvMinCount; i++) {
                    if (i < desKey.key.Length) desKey.key[i] = key[i];
                    else desKey.iv[i - desKey.key.Length] = key[i];
                }
            }

            public byte[] Encryption (byte[] data, byte[] key) {
                if (this.checkKey (key) == true) {
                    if (data == null || data.Length == 0) throw new EncryptExeption ("加密的数据为空！");
                    DesKey desKey = new DesKey ();
                    this.outKeyIV (key, ref desKey);
                    return this.Encrypt (data, desKey.key, desKey.iv);
                } else return null;
            }

            public byte[] Decryption (byte[] data, byte[] key) {
                if (this.checkKey (key) == true) {
                    if (data == null || data.Length == 0) throw new EncryptExeption ("解密的数据为空！");
                    DesKey desKey = new DesKey ();
                    this.outKeyIV (key, ref desKey);
                    return this.Decrypt (data, desKey.key, desKey.iv);
                } else return null;
            }

            /// <summary>
            /// key 的长度必须大于16。
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public bool checkKey (byte[] key) {
                if (key == null || key.Length < keyIvMinCount) throw new EncryptExeption (string.Format ("key 的长度必须大于等于{0}!", keyIvMinCount));
                return true;
            }

#if UTEST
            public static void Test () {
                DesEncryption encryption = new DesEncryption ();
                byte[] key = { 0x98, 0x89, 0x21, 0x78, 0x98, 0x89, 0x21, 0x78, 0x98, 0x89, 0x21, 0x78, 0x98, 0x89, 0x21, 0x78 };
                string raw = "DesEncryption encrypDesEncryption encryption = new DesEncryption()tion DesEncryption encryption = new DesEncryption()= new DesEncryption()";
                byte[] rawBytes = System.Text.ASCIIEncoding.UTF8.GetBytes (raw);
                byte[] edata = encryption.Encryption (rawBytes, key);
                byte[] dedata = encryption.Decryption (edata, key);
                string destr = System.Text.ASCIIEncoding.UTF8.GetString (dedata);
                if (destr != raw) throw new UExt.UTestException ("Des 加密解密接口测试失败!");
            }
#endif
        }
    }
}