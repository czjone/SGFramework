#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildAssetsBundles {

	const string BUILD_MENU = "SGFTools";

	[MenuItem (BUILD_MENU + "/编译资源/编译资源")]
	static void BuildAndroidAssetsBundles () {
		BuildAssetsBundle (BuildTarget.Android); //取当前设置的平台
	}

	static void BuildAssetsBundle (BuildTarget target) {
		// 	BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions ();
		// 	buildPlayerOptions.scenes = GetAllScenesPath ();
		// 	buildPlayerOptions.locationPathName = target.ToString ();
		// 	buildPlayerOptions.target = target;
		// 	buildPlayerOptions.options = BuildOptions.None;

		// 	BuildReport report = BuildPipeline.BuildPlayer (buildPlayerOptions);
		// 	BuildSummary summary = report.summary;

		// 	if (summary.result == BuildResult.Succeeded) {
		// 		Debug.Log ("Build succeeded: " + summary.totalSize + " bytes");
		// 	}

		// 	if (summary.result == BuildResult.Failed) {
		// 		Debug.Log ("Build failed");
		// }
		Debug.Log ("Build success");
	}
}
#endif