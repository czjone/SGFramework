namespace SGF.Core.Zip {

    using System.Collections.Generic;
    using System.Collections;
    using System.IO;
    using System;
    using ICSharpCode.SharpZipLib.Checksums;
    using ICSharpCode.SharpZipLib.Zip;
    using GFile = System.IO.File;
    using System.Text;

    public class UnZipInfo {
        public string ZipPath { get; set; }

        public string UnZipPath { get; set; }

        public string password { get; set; }
    }

    public class UnZipArgs {
        public long Count { get; set; }
        public double Index { get; set; }
        public string fname { get; set; }
    }
    public class ZipHelper {

        public static bool isXUnix {
            get { return true; }
        }

        public static string pathSplit {
            get {
                if (isXUnix == true) return "/";
                else return "\\";
            }
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件</param>
        /// <param name="zipedFile">压缩后的文件</param>
        /// <param name="compressionLevel">压缩等级</param>
        /// <param name="blockSize">每次写入大小</param>
        public static void ZipFile (string fileToZip, string zipedFile, int compressionLevel, int blockSize) {
            //如果文件没有找到，则报错
            if (!System.IO.File.Exists (fileToZip)) {
                throw new System.IO.FileNotFoundException ("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (System.IO.FileStream ZipFile = System.IO.File.Create (zipedFile)) {
                using (ZipOutputStream ZipStream = new ZipOutputStream (ZipFile)) {
                    using (System.IO.FileStream StreamToZip = new System.IO.FileStream (fileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read)) {
                        string fileName = fileToZip.Substring (fileToZip.LastIndexOf (pathSplit) + 1);

                        ZipEntry ZipEntry = new ZipEntry (fileName);

                        ZipStream.PutNextEntry (ZipEntry);

                        ZipStream.SetLevel (compressionLevel);

                        byte[] buffer = new byte[blockSize];

                        int sizeRead = 0;

                        try {
                            do {
                                sizeRead = StreamToZip.Read (buffer, 0, buffer.Length);
                                ZipStream.Write (buffer, 0, sizeRead);
                            }
                            while (sizeRead > 0);
                        } catch (System.Exception ex) {
                            throw ex;
                        }

                        StreamToZip.Close ();
                    }

                    ZipStream.Finish ();
                    ZipStream.Close ();
                }

                ZipFile.Close ();
            }
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要进行压缩的文件名</param>
        /// <param name="zipedFile">压缩后生成的压缩文件名</param>
        public static void ZipFile (string fileToZip, string zipedFile) {
            //如果文件没有找到，则报错
            if (!GFile.Exists (fileToZip)) {
                throw new System.IO.FileNotFoundException ("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (FileStream fs = GFile.OpenRead (fileToZip)) {
                byte[] buffer = new byte[fs.Length];
                fs.Read (buffer, 0, buffer.Length);
                fs.Close ();

                using (FileStream ZipFile = GFile.Create (zipedFile)) {
                    using (ZipOutputStream ZipStream = new ZipOutputStream (ZipFile)) {
                        string fileName = fileToZip.Substring (fileToZip.LastIndexOf (pathSplit) + 1);
                        ZipEntry ZipEntry = new ZipEntry (fileName);
                        ZipStream.PutNextEntry (ZipEntry);
                        ZipStream.SetLevel (5);

                        ZipStream.Write (buffer, 0, buffer.Length);
                        ZipStream.Finish ();
                        ZipStream.Close ();
                    }
                }
            }
        }

        /// <summary>
        /// 压缩多层目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="zipedFile">The ziped file.</param>
        public static void ZipFileDirectory (string strDirectory, string zipedFile) {
            using (System.IO.FileStream ZipFile = System.IO.File.Create (zipedFile)) {
                using (ZipOutputStream s = new ZipOutputStream (ZipFile)) {
                    ZipSetp (strDirectory, s, "");
                }
            }
        }

        /// <summary>
        /// 递归遍历目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="s">The ZipOutputStream Object.</param>
        /// <param name="parentPath">The parent path.</param>
        private static void ZipSetp (string strDirectory, ZipOutputStream s, string parentPath) {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar) {
                strDirectory += Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32 ();

            string[] filenames = Directory.GetFileSystemEntries (strDirectory);

            foreach (string file in filenames) // 遍历所有的文件和目录
            {

                if (Directory.Exists (file)) // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string pPath = parentPath;
                    pPath += file.Substring (file.LastIndexOf (pathSplit) + 1);
                    pPath += pathSplit;
                    ZipSetp (file, s, pPath);
                } else // 否则直接压缩文件
                {
                    //打开压缩文件
                    using (FileStream fs = GFile.OpenRead (file)) {

                        byte[] buffer = new byte[fs.Length];
                        fs.Read (buffer, 0, buffer.Length);

                        string fileName = parentPath + file.Substring (file.LastIndexOf (pathSplit) + 1);
                        ZipEntry entry = new ZipEntry (fileName);

                        entry.DateTime = DateTime.Now;
                        entry.Size = fs.Length;

                        fs.Close ();

                        crc.Reset ();
                        crc.Update (buffer);

                        entry.Crc = crc.Value;
                        s.PutNextEntry (entry);

                        s.Write (buffer, 0, buffer.Length);
                    }
                }
            }
        }

        // static string LanChange(string str)
        // {
        //     Encoding utf8;
        //     Encoding gb2312;
        //     utf8 = Encoding.GetEncoding("UTF-8");
        //     gb2312 = Encoding.GetEncoding("GB2312");
        //     byte[] gb = gb2312.GetBytes(str);
        //     gb = Encoding.Convert(gb2312, utf8, gb);
        //     return utf8.GetString(gb);
        // }

        // static string ChangeLan(string text)
        // {
        //     byte[] bs = Encoding.GetEncoding("UTF-8").GetBytes(text);
        //     bs = Encoding.Convert(Encoding.GetEncoding("UTF-8"), Encoding.GetEncoding("GB2312"), bs);
        //     return Encoding.GetEncoding("GB2312").GetString(bs);
        // }

        /// <summary>
        /// 解压缩一个 zip 文件。
        /// </summary>
        /// <param name="unzipInfo">解压设置</param>
        /// <param name="overWrite">重写</param>
        /// <param name="onUnzip">解压回传事件</param>
        [Obsolete ("有性能问题，由于要有进度条，两次打开了文件并分析文件")]
        public static void UnZip (UnZipInfo unzipInfo, bool overWrite, Action<UnZipArgs> onUnzip = null) {

            if (unzipInfo.UnZipPath == "")
                unzipInfo.UnZipPath = Directory.GetCurrentDirectory ();

            if (!unzipInfo.UnZipPath.EndsWith (pathSplit))
                unzipInfo.UnZipPath = unzipInfo.UnZipPath + pathSplit;

            ICSharpCode.SharpZipLib.Zip.ZipFile zipFile = new ICSharpCode.SharpZipLib.Zip.ZipFile (unzipInfo.ZipPath);
            using (ZipInputStream s = new ZipInputStream (GFile.OpenRead (unzipInfo.ZipPath))) {
                if (unzipInfo.password != null) {
                    s.Password = unzipInfo.password;
                }
                ZipEntry theEntry;
                UnZipArgs args;
                double index = 0;
                while ((theEntry = s.GetNextEntry ()) != null) {
                    string directoryName = "";
                    string pathToZip = "";
                    theEntry.IsUnicodeText = false;
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName (pathToZip) + pathSplit;

                    string fileName = Path.GetFileName (pathToZip);

                    Directory.CreateDirectory (unzipInfo.UnZipPath + directoryName);

                    if (fileName != "") {
                        if ((GFile.Exists (unzipInfo.UnZipPath + directoryName + fileName) && overWrite) || (!GFile.Exists (unzipInfo.UnZipPath + directoryName + fileName))) {

                        args = new UnZipArgs () {
                        Count = zipFile.Count,
                        Index = index++,
                        fname = fileName
                            };
                            onUnzip (args);

                            using (FileStream streamWriter = GFile.Create (unzipInfo.UnZipPath + directoryName + fileName)) {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true) {
                                    //防止卡死
                                    args = new UnZipArgs () {
                                        Count = zipFile.Count,
                                        Index = index += 0.0001,
                                        fname = fileName
                                    };
                                    onUnzip (args);
                                    size = s.Read (data, 0, data.Length);

                                    if (size > 0)
                                        streamWriter.Write (data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close ();
                            }
                        }
                    }
                }

                s.Close ();
            }
        }

#if UTEST

        [UTest]
        public static void Test () {
            string indir = "Assets/Editor";
            string outzip = "Assets/Editor.zip";
            string outdir = "Assets/Editor1";

            DirectoryInfo folder = new DirectoryInfo ("Assets/Editor");
            FileSystemInfo[] files = folder.GetFileSystemInfos ();
            ZipHelper.ZipFileDirectory (indir, outzip);

            UnZipInfo unzipInfo = new UnZipInfo () {
                ZipPath = outzip,
                UnZipPath = outdir,
            };
            ZipHelper.UnZip (unzipInfo, true);

            folder = new DirectoryInfo ("Assets/Editor1");
            FileSystemInfo[] ofiles = folder.GetFileSystemInfos ();
            if (files.Length != ofiles.Length)
                throw new Exception ("解压后的文件与压缩前的不一样！");

            System.IO.Directory.Delete (outdir, true);
            System.IO.File.Delete (outzip);
        }
#endif
    }
}