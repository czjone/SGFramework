namespace SGF.Lua {
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;
    using XLua;

    [LuaCallCSharp]
    public sealed class RootBehaviour : MonoBehaviour {

        [System.NonSerialized]
        public string MainLuaModel = "main"; // run main .lua

        private readonly string devResRoot = "FZSG"; //相对assets 的相对目录

        private string _resFullRoot = null;

        public string ResRoot {
            get {
                if (string.IsNullOrEmpty (_resFullRoot)) {
                    var platform = Application.platform;
                    if (platform == RuntimePlatform.LinuxEditor ||
                        platform == RuntimePlatform.OSXEditor ||
                        platform == RuntimePlatform.WindowsEditor) {
                        _resFullRoot = Application.dataPath + SGF.Core.Path.DirSplitor + devResRoot;
                    } else {
                        _resFullRoot = Application.persistentDataPath + Core.Path.DirSplitor + HotUpdate.TaskDecompressionPKGRes.DresFolder;
                    }
                }
                return _resFullRoot;
            }
        }

        public SGF.Lua.Game Game { get; private set; }

        private GameObject _uiRoot = null;
        public GameObject UIRoot {
            get {
                if (_uiRoot == null) {
                    _uiRoot = GameObject.Find ("Canvas");
                }
                return _uiRoot;
            }
        }

        [DoNotGen]
        public RootBehaviour () {
            this.Game = new Game (this);
        }

        [DoNotGen]
        void OnDestroy () {
            this.Game.Dispose ();
        }

        [DoNotGen]
        void Start () {
            Game.Lua.AddLuaSearchPath (ResRoot);
            Game.Lua.AddG ("RootBehaviour", this); // add gloable lua  RootBehaviour.
            Game.Lua.Run (this.MainLuaModel);
        }

        void Update () {
            this.Game.Update ();
        }
    }
}