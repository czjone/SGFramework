#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SGF.Core;
using SGF.Unity;
using UnityEditor;
using UnityEngine;

public class BuildHotUpdatePatch {
	const string BUILD_MENU = "SGFTools/AssetsBundle Manager";
	static readonly string HOT_UPDATE_CONFIG = Application.streamingAssetsPath + "/../../SGF_AUTO_HOTUPDATE_CONFG.json";

	public static SGF.Unity.HotUpdate.Config GetHotUpdateConfig () {
		//由于unity的默认目录streamassets不一已创建好，这儿要检查一下
		if (Directory.Exists (Application.streamingAssetsPath) == false) {

			Directory.CreateDirectory (Application.streamingAssetsPath);
		}

		var cfg = SGF.Unity.HotUpdate.Config.LoadWithFile (HOT_UPDATE_CONFIG, true);
		return cfg;
	}

	[MenuItem (BUILD_MENU + "/Build Patchs")]
	public static void BuildPatchs () {

		var conf = GetHotUpdateConfig ();

		if (CheckBuildPatchsEnv (conf) == false) {
			return;
		}
		var hashInfo = CalculationResHashCode (conf);

		ModifyPatchVersion (hashInfo, conf);

		var version = GetVersion (conf);

		var msg = "Generate patchs success. current version:{0}";
		SGF.Unity.Utils.Logger.PrintSuccess (msg.FormatWith (version.VersionCode));

		AssetDatabase.Refresh ();
	}

	[MenuItem (BUILD_MENU + "/ReBuild Patchs")]
	public  static void ReBuildPatchs () {
		CleanPatchs ();
		BuildPatchs ();
	}

	[MenuItem (BUILD_MENU + "/Clean Patchs")]
	public static void CleanPatchs () {

	}

	public static string GetDownladPath (SGF.Unity.HotUpdate.Config cfg) {
		string val = Application.persistentDataPath + "/" + cfg.DownloadDir;
		return val;
	}

	public static string GetDownladVersionPath (SGF.Unity.HotUpdate.Config cfg) {
		string val = GetPackagePatchPath (cfg) + "/" + cfg.VersionFileName;
		return val;
	}

	public static string GetPatchFilePath (SGF.Unity.HotUpdate.Config cfg) {
		string val = GetPackagePatchPath (cfg) + "/" + cfg.VersionFilesFileName;
		return val;
	}

	public static string GetPackagePatchPath (SGF.Unity.HotUpdate.Config cfg) {
		string val = Application.streamingAssetsPath + "/" + cfg.PatchsStreamAssetsPath;
		return val;
	}

	/// <summary>
	/// 检查当前的的编译目录是否合法
	/// </summary>
	/// <returns></returns>
	static bool CheckBuildPatchsEnv (SGF.Unity.HotUpdate.Config cfg) {

		string dresPatchsDir = GetPackagePatchPath (cfg);

		string dresVersionFile = GetDownladVersionPath (cfg);

		if (Directory.Exists (dresPatchsDir) == false) {

			Directory.CreateDirectory (dresPatchsDir);
		}

		if (System.IO.File.Exists (dresVersionFile) == false) {

			SGF.Unity.HotUpdate.Version beginVer = new SGF.Unity.HotUpdate.Version ();

			beginVer.ToFile (dresVersionFile);

		}
		return true;
	}

	static SGF.Unity.HotUpdate.Version GetVersion (SGF.Unity.HotUpdate.Config cfg) {

		string dresVersionFile = GetDownladVersionPath (cfg);

		SGF.Unity.HotUpdate.Version ver = SGF.Unity.HotUpdate.Version.LoadWithFile (dresVersionFile);

		return ver;
	}

	static void SaveVersion (SGF.Unity.HotUpdate.Config cfg, SGF.Unity.HotUpdate.Version ver) {

		string dresVersionFile = GetDownladVersionPath (cfg);

		ver.ToFile (dresVersionFile);
	}

	static bool isNotHashFile (SGF.Unity.HotUpdate.Config conf, string fname) {
		return fname.EndsWith (conf.VersionFileName) ||
			fname.EndsWith (".meta") ||
			fname.EndsWith (".ds_store") ||
			fname.EndsWith (conf.VersionFilesFileName);
	}

	static SGF.Unity.HotUpdate.PatchFiles CalculationResHashCode (SGF.Unity.HotUpdate.Config cfg) {

		string dresPatchsDir = GetPackagePatchPath (cfg);

		List<string> files = SGF.Core.File.GetDirFileList (dresPatchsDir);

		SGF.Unity.HotUpdate.PatchFiles pathDes = new SGF.Unity.HotUpdate.PatchFiles ();

		if (files != null && files.Count > 0) {

			foreach (var file in files) {

				var checkFileTag = file.ToLower ();

				if (isNotHashFile (cfg, checkFileTag)) continue;

				var fname = file.ReplateWith (dresPatchsDir + (!dresPatchsDir.EndsWith ("/") ? "/" : ""), ""); //删除目录部分

				var md5 = SGF.Core.Security.Md5.GetSHA1WithFile (file);

				pathDes.Add (fname, md5);
			}
		}
		return pathDes;
	}

	static SGF.Unity.HotUpdate.Version GetPatchList (SGF.Unity.HotUpdate.Config cfg) {

		string dresVersionFile = GetDownladVersionPath (cfg);

		SGF.Unity.HotUpdate.Version ver = SGF.Unity.HotUpdate.Version.LoadWithFile (dresVersionFile);

		return ver;
	}

	static void ModifyPatchVersion (SGF.Unity.HotUpdate.PatchFiles files, SGF.Unity.HotUpdate.Config conf) {

		var oleVer = GetVersion (conf);

		//更新版本文件
		string patchFname = GetPatchFilePath (conf);

		SGF.Unity.HotUpdate.Patch patch = SGF.Unity.HotUpdate.Patch.LoadWithFile (patchFname, true);

		patch.Files = files;

		patch.Version.VersionCode++;

		patch.ToFile (patchFname);

		//更新版本号
		SaveVersion (conf, patch.Version);
	}
}

#endif