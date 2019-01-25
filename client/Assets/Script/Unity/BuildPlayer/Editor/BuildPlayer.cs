#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildPlayer {

	const string BUILD_MENU = "SGFTools";

	[MenuItem (BUILD_MENU + "/打运行包/APK")]
	static void BuildAndroidPlayer () {
		BuildTargetPlayer (BuildTarget.Android);
	}

	[MenuItem (BUILD_MENU + "/打运行包/XCode工程")]
	static void BuildIosProject () {
		BuildTargetPlayer (BuildTarget.iOS);
	}

	[MenuItem (BUILD_MENU + "/打运行包/Win运行包")]
	static void BuildWindowsPlayer () {
		BuildTargetPlayer (BuildTarget.StandaloneWindows);
	}

	static string[] GetAllScenesPath () {
		var s = SceneManager.GetAllScenes (); //new [] { "Assets/Scene1.unity", "Assets/Scene2.unity" }
		List<string> pathes = new List<string> ();
		foreach (var ite in s) {
			pathes.Add (ite.path);
		}
		return pathes.ToArray ();
	}

	static void BuildTargetPlayer (BuildTarget target) {
		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions ();
		buildPlayerOptions.scenes = GetAllScenesPath ();
		buildPlayerOptions.locationPathName = target.ToString ();
		buildPlayerOptions.target = target;
		buildPlayerOptions.options = BuildOptions.None;

		BuildReport report = BuildPipeline.BuildPlayer (buildPlayerOptions);
		BuildSummary summary = report.summary;

		if (summary.result == BuildResult.Succeeded) {
			Debug.Log ("Build success: " + summary.totalSize + " bytes");
		}

		if (summary.result == BuildResult.Failed) {
			Debug.Log ("Build failed");
		}
	}
}
#endif