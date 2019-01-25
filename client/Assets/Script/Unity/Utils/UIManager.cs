using SGF.Core;
using UnityEngine;

namespace SGF.Unity.Utils {
    public class UIManager {
        private Game game;
        private GameObject UIRoot;
        public UIManager (Game game) {
            this.game = game;
        }

        public GameObject Root {
            get {
                if (this.UIRoot == null) {
                    Logger.PrintWarring ("没有设置UIRoot,尝试直接使用Canvas节点作为UIRoot！");
                    this.UIRoot = GameObject.Find ("Canvas");
                    if (this.UIRoot == null) {
                        Logger.PrintError ("当前Sence无可用的UI节点!");
                    }
                }
                return this.UIRoot;
            }
            set {
                this.UIRoot = value;
            }
        }

        public GameObject Open (string uiname) {
            var go = PrefabsHelper.CreateUI (this.Root, toResName (uiname));
            go.name = toUIname (uiname);
            go.SetUIParent (Root);
            return go;
        }

        public void Close (string uiname, bool destory = true) {
            var go = this.Root.FindChildWithTagName (uiname);
            this.Close (go);
        }

        public void Close (GameObject go, bool destory = true) {
            go.SetUIParent (null);
            if (destory == true) {
                GameObject.DestroyImmediate (go);
            }
        }

        private static string toUIname (string name) {
            return name.Replace ("/", "->");
        }

        private static string toResName (string uiname) {
            return uiname.Replace ("->", "/");
        }
    }
}