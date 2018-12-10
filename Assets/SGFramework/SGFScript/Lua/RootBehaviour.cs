namespace SGF.Lua {
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;
    using XLua;

    public abstract class MainRootBehaviour : MonoBehaviour {
        /// <summary>
        /// gameEnv setting
        /// </summary>
        /// <value></value>
        public abstract Config Conf { get; }

        private string uiRootName = string.Empty;
        /// <summary>
        /// root ui name.
        /// </summary>
        /// <value></value>
        public virtual string UIRootName {
            get {
                if (string.IsNullOrEmpty (this.uiRootName)) {
                    this.uiRootName = gameObject.name;
                }
                return this.uiRootName;
            }
        }
    }

    public class RootBehaviour : MainRootBehaviour {

        public SGF.Lua.Game Game { get; private set; }

        private Config config;
        //get default config
        public override Config Conf {
            get {
                if (this.config == null) {
                    this.config = new Config () {
                    DresRootName = "dres",
                    DevDir = SGF.Core.Path.Legalization (Application.dataPath + "/FZSG"),
                    LuaMain = "main", // main.lua
                    };
                }
                return this.config;
            }
        }

        public RootBehaviour () {

        }

        void OnDestroy () {
            this.Game.Dispose ();
        }

        void Awake () {
            this.Game = new Game (this);
        }

        void Start () {
            this.Game.Start ();
        }

        void Update () {
            this.Game.Update ();
        }
    }
}