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

	[MenuItem (BUILD_MENU + "/SDK管理/修改配置")]
	static void ConfigSDKAdapter () {
		if(System.IO.File.Exists(sdkconfigPath) == false) {
			
		}
	}

	[MenuItem (BUILD_MENU + "/SDK管理/发布渠道APK")]
	static void BuildChannelAPK () {

	}

	[MenuItem (BUILD_MENU + "/SDK管理/发布渠道XCode工程")]
	static void BuildChannelXCodeProject () {

	}

	[MenuItem (BUILD_MENU + "/SDK管理/发布APK测试包")]
	static void BuildTestAPK () {

	}

	[MenuItem (BUILD_MENU + "/SDK管理/发布XCode测试工程")]
	static void BuildTestXCodeProject () {

	}
}
#endif