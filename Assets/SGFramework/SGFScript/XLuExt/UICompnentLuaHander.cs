﻿namespace SGF.XLuaExt {
	
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;


	[XLua.LuaCallCSharp]
	public class UICompnentLuaHander : MonoBehaviour {
		public string LuaHandlerName;

		public void EventHandler() {
			SGF.Unity.ULog.D(LuaHandlerName);
		}
	}
}