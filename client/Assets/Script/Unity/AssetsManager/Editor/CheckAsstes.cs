#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckAsstes {
	const string BUILD_MENU = "SGFTools";

	[MenuItem (BUILD_MENU + "/检查资源/检查UI资源设置")]
	static void CheckUITextures () {
		
	}
}
#endif