namespace SGF.Lua {
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;
    using XLua;

    [LuaCallCSharp]
    public class SGFBaseMainMonoBehaviour : MonoBehaviour {

        [DoNotGen]
        public SGFManager Manager { get; private set; }

        public SGF.Lua.UI.WinMgr WinMgr { get; private set; }

        [DoNotGen]
        private string _UIRootName = "UIMainCamera";

        protected string UIRootName {
            get { return _UIRootName; }
        }

        [DoNotGen]
        public SGFBaseMainMonoBehaviour () {
            this.Manager = new SGFManager (this);
        }

        [DoNotGen]
        virtual protected void Awake () {
            this.gameObject.name = UIRootName;
        }

        [DoNotGen]
        virtual protected void OnDestroy () {
            this.Manager.Dispose ();
        }
    }
}