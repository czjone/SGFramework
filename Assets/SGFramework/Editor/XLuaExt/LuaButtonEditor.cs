using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UnityEditor.CustomEditor(typeof(LuaUIButton))]
public class LuaButtonEditor : UnityEditor.UI.ButtonEditor {

	private LuaUIButton luiBt;

	public override void OnInspectorGUI(){
		luiBt = target as LuaUIButton;
		base.DrawDefaultInspector();
	}
}
