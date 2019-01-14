using UnityEngine;

namespace SGF.Unity.Utils  {
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
    }
}