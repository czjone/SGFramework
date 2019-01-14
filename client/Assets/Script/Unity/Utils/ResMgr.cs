namespace SGF.Unity.Utils {

    using SGF.Core;
    using UnityEngine;

    public class ResMgr {
        public Game game;
        public ResMgr (Game game) {
            this.game = game;
        }

        /// <summary>
        /// 相对于资源目录的路径，当前的资源目录是 Assets/DResources
        /// </summary>
        /// <param name="name">当前的资源目录是 Assets/DResources</param>
        /// <returns></returns>
        public GameObject LoadUIPrefab (string name) {
            GameObject loadObj = null;
#if UNITY_EDITOR
            if (Application.isEditor) {
                var path = Fname2FullPath (name) + ".prefab";
                loadObj = (GameObject) UnityEditor.AssetDatabase.LoadAssetAtPath (path, typeof (GameObject));
            }
#else
            if (Application.isEditor = false) {
                throw new System.Exception ("发布版本的资源加载还没有实现!");
            }
#endif
            if (loadObj == null) {
                Logger.PrintError ("加载资源失败:{0}".FormatWith (name));
                return null;
            }
            var manager = IoC.Get<UIManager> ();
            if (manager == null) {
                Logger.PrintError ("UIRoot is null，游戏中不包含2d的uiroot.");
                return null;
            }
            var instance = GameObject.Instantiate (loadObj, manager.Root.transform, false);
            if (instance == null) {
                Logger.PrintError ("Res Clone成GameObject 失败:{0}".FormatWith (name));
                return null;
            }
            if (!(instance is GameObject)) { 
                Logger.PrintError ("加载的资源不是 GameObject:{0}".FormatWith (name));
                return null;
            }
            return instance as GameObject;
        }

        /// <summary>
        /// 相对于资源目录的路径，当前的资源目录是 Assets/DResources
        /// </summary>
        /// <param name="name">当前的资源目录是 Assets/DResources</param>
        /// <returns></returns>
        public string loadJson (string name) {
            TextAsset assetText = null;
#if UNITY_EDITOR
            if (Application.isEditor) {
                var path = Fname2FullPath (name) + ".json";
                assetText = (TextAsset) UnityEditor.AssetDatabase.LoadAssetAtPath (path, typeof (TextAsset));
            }
#else
            if (Application.isEditor = false) {
                throw new System.Exception ("发布版本的资源加载还没有实现!");
            }
#endif
            if (assetText == null) {
                Logger.PrintError ("加载资源失败:{0}".FormatWith (name));
                return string.Empty;
            }

            return assetText.text;
        }
        
        private string Fname2FullPath (string name) {
            if (Application.isEditor) {
                Config conf = IoC.Get<Config> ();
                return "Assets/" + conf.ProjecResourceDir + "/" + name;
            }
            return string.Empty;
        }
    }
}