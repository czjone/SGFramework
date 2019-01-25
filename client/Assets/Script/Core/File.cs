namespace SGF {
	namespace Core {
		using System.Collections.Generic;
		using System.Collections;
		using System.IO;
		using System.Security.Cryptography;
		using System.Text;
		using System;
		using UnityEngine;

		public class File {
			/// <summary>
			/// 复制文件夹中的所有内容
			/// </summary>
			/// <param name="sourceDirPath">源文件夹目录</param>
			/// <param name="saveDirPath">指定文件夹目录</param>
			public static void CopyDirectory (string sourceDirPath, string saveDirPath) {
				try {
					if (!Directory.Exists (saveDirPath)) {
						Directory.CreateDirectory (saveDirPath);
					}
					string[] files = Directory.GetFiles (sourceDirPath);
					foreach (string file in files) {
						string pFilePath = saveDirPath + Path.DirSplitor + System.IO.Path.GetFileName (file);
						if (System.IO.File.Exists (pFilePath))
							continue;
						System.IO.File.Copy (file, pFilePath, true);
					}

					string[] dirs = Directory.GetDirectories (sourceDirPath);
					foreach (string dir in dirs) {
						CopyDirectory (dir, saveDirPath + Path.DirSplitor + System.IO.Path.GetFileName (dir));
					}
				} catch (Exception ex) {
					Debug.Log (ex);
				}
			}

			public static void DelDirectory (string sourceDirPath) {
				if (System.IO.Directory.Exists (sourceDirPath) == true)
					System.IO.Directory.Delete (sourceDirPath, true);
			}

			/// <summary>
			/// 拷贝目录下的所有文件到目的目录。
			/// </summary>
			/// <param >源路径</param>
			/// <param >目的路径</param>
			public static void CopyFile (System.IO.DirectoryInfo path, string desPath) {
				string sourcePath = path.FullName;
				System.IO.FileInfo[] files = path.GetFiles ();
				foreach (System.IO.FileInfo file in files) {
					string sourceFileFullName = file.FullName;
					string destFileFullName = sourceFileFullName.Replace (sourcePath, desPath);
					if (System.IO.File.Exists (destFileFullName))
						System.IO.File.Delete (destFileFullName);
					System.IO.File.Copy (sourceFileFullName, destFileFullName);
				}
			}

			public static List<string> GetDirFileList (string fpat, List<string> outList = null) {
				if (outList == null)
					outList = new List<string> ();
				if (!System.IO.Directory.Exists (fpat)) {
					fpat = fpat.Replace (Path.UnDirSplitor, Path.DirSplitor);
					outList.Add (fpat);
					return outList;
				}

				string[] infos = Directory.GetFiles (fpat);
				foreach (var item in infos) outList.Add (item);
				string[] dires = Directory.GetDirectories (fpat);
				foreach (var dir in dires) GetDirFileList (dir, outList);
				return outList;
			}

			/// <summary>
			///  检查目录是否存在，不存在就创建目录
			/// </summary>
			/// <param name="dir"></param>
			/// <param name="isNotExistCreate"></param>
			/// <returns></returns>
			public static bool CheckDir (string dir, bool isNotExistCreate = false) {
				if (Directory.Exists (dir) == true) return true;
				dir = dir.Replace ("\\", "/");
				var dirs = dir.Split ('/');
				StringBuilder dirsb = new StringBuilder ();
				foreach (var folder in dirs) {
					dirsb.Append ("/");
					dirsb.Append (folder);
					var tagDir = dirsb.ToString ();
					if (Directory.Exists (tagDir) == false) {
						Directory.CreateDirectory (tagDir);
					}
				}
				return true;
			}  

			public static bool CheckDirWithFile (string file, bool isNotExistCreate = false) {
				string dir = GetParentDir (file);
				return CheckDir (dir);
			}  

			public static string GetParentDir (string path) {
				var index = -1;
				if (path.Contains ("\\")) {
					index = path.LastIndexOf ("\\");
				} else {
					index = path.LastIndexOf ("/");
				}
				return path.Substring (0, index);        
			}

			public static string GetFileName (string path) {
				var index = -1;
				if (path.Contains ("\\")) {
					index = path.LastIndexOf ("\\");
				} else {
					index = path.LastIndexOf ("/");
				}
				return path.Substring (index + 1, path.Length - index - 1);        
			}

			public static void WriteFile (string path, byte[] data) {
				var parRoot = System.IO.Path.GetPathRoot (path);
				CheckDir (parRoot, true);
				System.IO.File.WriteAllBytes (path, data);
			}

			public static void WriteFile (string path, string data) {
				var parRoot = System.IO.Path.GetPathRoot (path);
				CheckDir (parRoot, true);
				System.IO.File.WriteAllText (path, data);
			}

			public static string EncryptWithMD5 (string fname) {
				byte[] sor = System.IO.File.ReadAllBytes (fname);
				MD5 md5 = MD5.Create ();
				byte[] result = md5.ComputeHash (sor);
				StringBuilder strbul = new StringBuilder (40);
				for (int i = 0; i < result.Length; i++) {
					strbul.Append (result[i].ToString ("x2")); //加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位
				}
				return strbul.ToString ();
			}
		}
	}
}