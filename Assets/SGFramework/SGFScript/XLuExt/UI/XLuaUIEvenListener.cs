namespace SGF.XLuaExt {
	
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;

	public class LuaUIEvenListener : MonoBehaviour {
		public string LuaHandlerName;

		public void EventHandler() {
			SGF.Unity.ULog.D(LuaHandlerName);
		}
	}
}