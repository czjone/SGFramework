namespace SGF.XLuaUI.Editor {

	using UnityEngine;

	[UnityEditor.CustomEditor (typeof (XLuaUIButton))]
	public class XLuaButtonInspector : UnityEditor.UI.ButtonEditor {

		private XLuaUIButton luiBt;

		public override void OnInspectorGUI () {
			luiBt = target as XLuaUIButton;
			base.DrawDefaultInspector ();
		}
	}
}