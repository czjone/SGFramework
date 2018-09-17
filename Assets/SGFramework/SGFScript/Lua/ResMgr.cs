namespace SGF.Lua {
    using UnityEngine;
    using XLua;

    [LuaCallCSharp]
    public sealed class ResMgr {

        public string ResRoot {
            get;
            private set;
        }

        private static ResMgr instance;
        public static ResMgr GetInstance () {
            if (instance == null) {
                instance = new ResMgr ();
            }
            return instance;
        }

        public ResMgr () {
            if (Application.platform == RuntimePlatform.LinuxEditor ||
                Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.OSXEditor
            ) {
                this.ResRoot = Application.dataPath;
            } else {
                this.ResRoot = Application.persistentDataPath;
            }
        }

        public void SetResRoot (string root) {
            this.ResRoot = root;
        }

        /// <summary>
        /// 支持的格式：    1、wins.aaa.bbb
        ///               2、wins/aaa/bbb
        ///               3、wins/aaa/bbb.prefab
        /// </summary>
        /// <param name="nm"></param>
        /// <returns></returns>
        public GameObject LoadPrefab (string nm) {
#if UNITY_EDITOR
            if (nm.Contains ("/") == false) {
                nm = nm.ToNormalPathWithoutFileExtention ();
            }
            var prefabSuffix = ".prefab";
            if (nm.ToLower ().EndsWith (prefabSuffix) == false) {
                nm += prefabSuffix;
            }
            // editor 模式的加载方式 Assets/FZSG/dres/res/Wins/General/MessageBox.prefab
            nm = "Assets/FZSG/dres/res/" + nm;
            var go = GameObject.Instantiate (UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject> (nm));
            return go;
#else
            return null;
#endif
        }
    }
}