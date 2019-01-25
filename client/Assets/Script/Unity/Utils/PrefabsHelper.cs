namespace SGF.Unity.Utils {
    using SGF.Core;
    using UnityEngine;

    public class PrefabsHelper {
        
        /// <summary>
        /// 创建UI Prefabs
        /// </summary>
        /// <param name="uiRoot"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject CreateUI (GameObject uiRoot, string name) {
            ResMgr mgr = IoC.Get<ResMgr> ();
            var parent = uiRoot;
            if (parent == null) {
                Logger.PrintError ("不能添加节点空null中!,父节点UIRoot为空。");
                return null;
            }
            var loadGO = mgr.Load<GameObject> (name);
            var cloneGO = CloneUI (loadGO);
            if (cloneGO == null) {
                Logger.PrintError ("尝试添加null 到GameObject中!");
                return null;
            }
            cloneGO.transform.parent = parent.transform;
            return cloneGO;
        }


        private static GameObject CloneUI (GameObject go) {
            if (Application.isEditor) {
                var manager = IoC.Get<UIManager> ();
                if (manager == null) {
                    Logger.PrintError ("UIRoot is null，游戏中不包含2d的uiroot.");
                    return null;
                }
                var instance = GameObject.Instantiate (go, manager.Root.transform, false);
                return instance;
            } else {
                return go;
            }
        }
    }
}