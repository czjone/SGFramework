namespace SGF.XLuaUI {
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;

    public class Button : UnityEngine.UI.Button {
        public SGF.XLuaUI.Action Lua;

        private void Awake () {
            this.onClick.AddListener (this.InvorkLuaHandler);
        }

        public void InvorkLuaHandler () {
            SGF.Unity.ULog.D (this.Lua.InvorkActionName);
        }
    }

}