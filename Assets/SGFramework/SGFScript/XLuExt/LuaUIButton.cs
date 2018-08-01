using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaUIButton : UnityEngine.UI.Button {
    public string LuaActionName;

    private void Awake() {
        this.onClick.AddListener(this.InvorkLuaHandler);
    }

    public void InvorkLuaHandler () {
        SGF.Unity.ULog.D(this.LuaActionName);
    }
}