#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SGF.Core;
using SGF.Unity;
using UnityEditor;
using UnityEngine;

public class BuildLuaEditor {
	const string BUILD_MENU = "SGFTools/编译资源";

	[MenuItem (BUILD_MENU + "/编译Lua")]
	static void BuildLua () {
		BuildLua (false);
	}

	static void BuildLua (bool isDev) {

		Config conf = Config.LoadDefaultConfig ();

		SGF.Unity.HotUpdate.Config outConfig = BuildHotUpdatePatch.GetHotUpdateConfig ();

		string dresPath = Application.dataPath + "/" + conf.ProjecResourceDir + "/";

		List<string> luas = SGF.Core.File.GetDirFileList (dresPath);

		if (luas == null || luas.Count == 0) {

			SGF.Unity.Utils.Logger.PrintWarring ("not found lua files in project.");

			return;
		}
		string luaOutPat = Application.streamingAssetsPath + "/" + outConfig.PatchsStreamAssetsPath + "/";

		//检查目录是否已创建好。
		SGF.Core.File.CheckDir (luaOutPat, true);

		int i = 0;

		foreach (var lua in luas) {

			if (lua.ToLower ().EndsWith (".lua")) {

				var fname = lua.ReplateWith (dresPath, "");

				var fnameHash = SGF.Core.Security.Md5.GetSHA1WithString (fname);

				var srcpath = dresPath + "/" + fname;

				var tagPath = luaOutPat + fnameHash + outConfig.PatchsFileExt;

				SGF.Core.File.CheckDirWithFile (tagPath);

				System.IO.File.Copy (srcpath, tagPath, true);

				SGF.Unity.Utils.Logger.PrintLog ("[ Build Lua ] :'{0}' --> '{1}'".FormatWith (fname, tagPath));

				i++;
			}
		}
		SGF.Unity.Utils.Logger.PrintSuccess ("[ Build Lua ] build success, update file count:{0}".FormatWith (i));

		AssetDatabase.Refresh ();
	}

	[MenuItem (BUILD_MENU + "/清理Lua")]
	static void CleanLuaBuildResult () {

	}
}

#endif