namespace SGF.Unity.Utils  {
    using SGF.Core;
    using UnityEngine;

    public class UIHelper {
        Game game;
        public UIHelper (Game game) {
            this.game = game;
        }

        /// <summary>
        /// 添加节点到
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="name">当前的资源目录是 Assets/DResources</param>
        /// <returns></returns>
        public GameObject AddPrefaGameObject (GameObject parent, string name) {
            ResMgr mgr = IoC.Get<ResMgr> ();
            var go = mgr.LoadUIPrefab (name);
            go.transform.SetParent (parent.transform, false);
            return go;
        }

        /// <summary>
        /// 在UIRoot上显示Prefab.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject ShowPrefab (string name) {
            UIManager uimanager = IoC.Get<UIManager> ();
            ResMgr mgr = IoC.Get<ResMgr> ();
            var go = mgr.LoadUIPrefab (name);
            if (uimanager == null) {
                Logger.PrintError ("无可用用的UIMnaager!");
                return null;
            }
            var parent = uimanager.Root;
            if (parent == null) {
                Logger.PrintError ("不能添加节点空null中!,父节点UIRoot为空。");
                return null;
            }
            var cloneGO = mgr.LoadUIPrefab (name);
            if (cloneGO == null) {
                Logger.PrintError ("尝试添加null 到GameObject中!");
                return null;
            }
            cloneGO.transform.parent = parent.transform;
            return cloneGO;
        }
    }
}