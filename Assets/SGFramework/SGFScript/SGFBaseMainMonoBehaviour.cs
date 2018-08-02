namespace SGF {
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;

    public class SGFBaseMainMonoBehaviour : MonoBehaviour {
        public SGFManager Manager { get; private set; }

        private string _UIRootName = "UIMainCamera";
        protected string UIRootName {
            get {
                return _UIRootName;
            }
        }

        public SGFBaseMainMonoBehaviour () {
            this.Manager = new SGFManager (this);
        }

        virtual protected void Awake () {
            this.gameObject.name = UIRootName;
        }

        virtual protected void OnDestroy () {
            this.Manager.Dispose ();
        }
    }
}