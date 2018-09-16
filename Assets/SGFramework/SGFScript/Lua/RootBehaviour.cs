namespace SGF.Lua {
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;
    using XLua;

    public class MainRootBehaviour : MonoBehaviour {
        /// <summary>
        /// gameEnv setting
        /// </summary>
        /// <value></value>
        public virtual Config Conf { get; protected set; }

        /// <summary>
        /// root ui name.
        /// </summary>
        /// <value></value>
        public virtual string UIRootName { get; private set; }
    }

    public class RootBehaviour : MainRootBehaviour {

        public SGF.Lua.Game Game { get; private set; }

        [DoNotGen]
        public RootBehaviour () {

        }

        [DoNotGen]
        void OnDestroy () {
            this.Game.Dispose ();
        }

        [DoNotGen]
        void Awake () {
            // default setting.
            this.Conf = new Config () {
                DresRootName = "dres",
                DevDir = SGF.Core.Path.Legalization (Application.dataPath + "/FZSG"),
                LuaMain = "main", // main.lua
            };
            this.Game = new Game (this);
        }

        [DoNotGen]
        void Start () {
            this.Game.Start ();
        }

        void Update () {
            this.Game.Update ();
        }
    }
}