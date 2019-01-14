#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SDKManager {
	const string BUILD_MENU = "SGFTools";
	const string BUILD_SDK_CHANNEL_CONFIGPATH = "SDK_CHANNEL_CONFIGS.json";
	static readonly string sdkconfigPath = Application.dataPath + "../" + BUILD_SDK_CHANNEL_CONFIGPATH;

	[MenuItem (BUILD_MENU + "/SDKAdapter/SDKConfig")]
	static void ConfigSDKAdapter () {
		if(System.IO.File.Exists(sdkconfigPath) == false) {
			
		}
	}

	[MenuItem (BUILD_MENU + "/SDKAdapter/Build Channel APK")]
	static void BuildChannelAPK () {

	}

	[MenuItem (BUILD_MENU + "/SDKAdapter/Build Channel XCodeProject")]
	static void BuildChannelXCodeProject () {

	}

	[MenuItem (BUILD_MENU + "/SDKAdapter/Build Test APK")]
	static void BuildTestAPK () {

	}

	[MenuItem (BUILD_MENU + "/SDKAdapter/Build Test XCodeProject")]
	static void BuildTestXCodeProject () {

	}
}
#endif