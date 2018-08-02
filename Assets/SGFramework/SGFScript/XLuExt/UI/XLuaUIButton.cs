﻿namespace SGF.XLuaUI {
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;

    public class XLuaUIButton : UnityEngine.UI.Button {
        public XLuaUIAction Lua;

        private void Awake () {
            this.onClick.AddListener (this.InvorkLuaHandler);
        }

        public void InvorkLuaHandler () {
            SGF.Unity.ULog.D (this.Lua.InvorkActionName);
        }
    }

}