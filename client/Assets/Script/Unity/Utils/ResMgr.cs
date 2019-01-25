namespace SGF.Unity.Utils {

    using System.Linq;
    using System.Text;
    using SGF.Core;
    using SGF.Unity.XLuaExt;
    using UnityEngine;

    /// <summary>
    /// 资源加载顺序
    /// 
    /// 1、检查下载目录是否有这个文件->
    /// 2、检查StreamAssets是否存在->
    /// 
    /// 如果都不存在就返回为空
    /// 
    /// 资源打包约定：一个文件夹就打成一个ab,不管文件夹中的资源有多大。
    /// 如： 文件的路径为src/uis/main.lua 会打包成src/uis,中包含main.lua
    /// </summary>
    public class ResMgr {

        public Game game;

        public ResMgr (Game game) {
            this.game = game;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="name">资源名</param>
        /// <typeparam name="T">加载的类型</typeparam>
        /// <returns>加载后的结果</returns>
        public T Load<T> (string name) where T : Object {
#if UNITY_EDITOR
            return LoadOnEditor<T> (name);
#else
            return LoadOnPlayer<T> (name);
#endif 
        }

        /// <summary>
        /// 获取文件的名字，包括扩展名，只处理开发目录，其它目录会进来会有问题
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns></returns>
#if UNITY_EDITOR
        private string GetFilenameWithExtentionsInDevDir (string path) {
            string name = SGF.Core.File.GetFileName (path);
            string dir = SGF.Core.File.GetParentDir (path);
            var conf = IoC.Get<Config> ();
            var fulldir = Application.dataPath + "/" + conf.ProjecResourceDir + "/" + dir + "/";
            // var files = System.IO.Directory.GetFiles (devRootFullPath, name + "*.*");
            var files = System.IO.Directory.GetFiles (fulldir, name + ".*").Where (x => x.EndsWith (".meta") == false);
            //是否找到合法的文件名
            if (files == null || files.Count () == 0) {
                SGF.Unity.Utils.Logger.PrintError ("file not foudnd:{0}".FormatWith (path));
                return string.Empty;
            }
            //如果文件名有多个就是有同名的
            if (files.Count () > 1) {
                StringBuilder sb = new StringBuilder ();
                foreach (var item in files) {
                    sb.Append (item);
                    sb.Append (";");
                }
                SGF.Unity.Utils.Logger.PrintError ("同一目录不能有多个文件名相同的，但是缀名不相同:{0}".FormatWith (sb.ToString ()));
                return string.Empty;
            }
            var ret = files.ToArray () [0].Replace (fulldir, ""); //去掉目录部分
            return ret;
        }
#endif

        /// <summary>
        /// 编辑器模式下加载方式,资源必须放到指定的目录中。其它目录的资源还是用Unity原生的读取方式
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>加载后的结果</returns>
#if UNITY_EDITOR
        private T LoadOnEditor<T> (string path) where T : Object {
            string folder = SGF.Core.File.GetParentDir (path);
            string fname = GetFilenameWithExtentionsInDevDir (path);
            if (fname == null) {
                SGF.Unity.Utils.Logger.PrintError ("Get file fail:{0}".FormatWith (path));
                return null;
            }
            Config conf = IoC.Get<Config> ();
            //Lua的加载方式
            if (fname.EndsWith (".lua")) {
                var tagPath = Application.dataPath + "/" + conf.ProjecResourceDir + "/" + folder + "/" + fname;
                if (System.IO.File.Exists (tagPath) == false) {
                    SGF.Unity.Utils.Logger.PrintError ("read lua file fail:{0}".FormatWith (path));
                    return default (T);
                }
                if (typeof (T).Equals (typeof (LuaAssets)) == false) {
                    SGF.Unity.Utils.Logger.PrintError ("lua one load to LuaAssets. file:{0}".FormatWith (path));
                    return default (T);
                }
                var luastr = System.IO.File.ReadAllText (tagPath);
                LuaAssets assets = new LuaAssets ();
                assets.bytes = System.Text.ASCIIEncoding.UTF8.GetBytes (luastr);
                return assets as T;
            } else {
                //unity 默认的资源的加载方式
                var tagPath = "Assets/" + conf.ProjecResourceDir + "/" + folder + "/" + fname;
                var t = UnityEditor.AssetDatabase.LoadAssetAtPath (tagPath, typeof (T)) as T;
                if (t == null) {
                    SGF.Unity.Utils.Logger.PrintError ("Get file fail,fullname: {0}".FormatWith (tagPath));
                    return default (T);
                }
                return t;
            }
        }
#endif

        /// <summary>
        /// Fi
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T LoadOnPlayer<T> (string path) where T : Object {
            string ab = SGF.Core.File.GetParentDir (path);
            string fname = SGF.Core.File.GetFileName (path);
            return default (T);
        }

        /// <summary>
        /// 加载json为对象
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LoadFromJson<T> (string name) where T : JsonSerializable<T>, new () {
            TextAsset textAsset = this.Load<TextAsset> (name);
            if (textAsset == null) return default (T);
            var json = textAsset.text;
            return JsonSerializable<T>.LoadWithJson (json);
        }

        //         #region 内部资源处理接口
        //         /// <summary>
        //         /// 相对于资源目录的路径，当前的资源目录是 Assets/DResources
        //         /// </summary>
        //         /// <param name="name">当前的资源目录是 Assets/DResources</param>
        //         /// <returns></returns>
        //         private GameObject LoaPrefabFromResources (string name) {
        //             GameObject loadObj = null;
        // #if UNITY_EDITOR
        //             if (Application.isEditor) {
        //                 var path = Fname2FullPath (name) + ".prefab";
        //                 loadObj = (GameObject) UnityEditor.AssetDatabase.LoadAssetAtPath (path, typeof (GameObject));
        //             }
        // #else
        //             if (Application.isEditor = false) {
        //                 throw new System.Exception ("发布版本的资源加载还没有实现!");
        //             }
        // #endif
        //             if (loadObj == null) {
        //                 Logger.PrintError ("加载资源失败:{0}".FormatWith (name));
        //                 return null;
        //             }
        //             var manager = IoC.Get<UIManager> ();
        //             if (manager == null) {
        //                 Logger.PrintError ("UIRoot is null，游戏中不包含2d的uiroot.");
        //                 return null;
        //             }
        //             var instance = GameObject.Instantiate (loadObj, manager.Root.transform, false);
        //             if (instance == null) {
        //                 Logger.PrintError ("Res Clone成GameObject 失败:{0}".FormatWith (name));
        //                 return null;
        //             }
        //             if (!(instance is GameObject)) { 
        //                 Logger.PrintError ("加载的资源不是 GameObject:{0}".FormatWith (name));
        //                 return null;
        //             }
        //             return instance as GameObject;
        //         }

        //         /// <summary>
        //         /// 相对于资源目录的路径，当前的资源目录是 Assets/DResources
        //         /// </summary>
        //         /// <param name="name">当前的资源目录是 Assets/DResources</param>
        //         /// <returns></returns>
        //         private string LoadJsonFromResources (string name) {
        //             TextAsset assetText = null;
        // #if UNITY_EDITOR
        //             if (Application.isEditor) {
        //                 var path = Fname2FullPath (name) + ".json";
        //                 assetText = (TextAsset) UnityEditor.AssetDatabase.LoadAssetAtPath (path, typeof (TextAsset));
        //             }
        // #else
        //             if (Application.isEditor = false) {
        //                 throw new System.Exception ("发布版本的资源加载还没有实现!");
        //             }
        // #endif
        //             if (assetText == null) {
        //                 Logger.PrintError ("加载资源失败:{0}".FormatWith (name));
        //                 return string.Empty;
        //             }

        //             return assetText.text;
        //         }

        //         private string Fname2FullPath (string name) {
        //             if (Application.isEditor) {
        //                 Config conf = IoC.Get<Config> ();
        //                 return "Assets/" + conf.ProjecResourceDir + "/" + name;
        //             }
        //             return string.Empty;
        //         }

        //         /// <summary>
        //         /// 从streamassets中读取 bytes;
        //         /// </summary>
        //         /// <param name="fname">相对于Application.dataPath</param>
        //         /// <returns>assetsBundle</returns>
        //         private byte[] LoadStreamAssets (string fname) {
        //             if (Application.platform == RuntimePlatform.Android || !Application.isEditor) {
        //                 var filePath = "jar:file:///" + Application.dataPath + "!/assets/";
        //                 WWW www = new WWW (filePath);
        //                 while (true) {
        //                     if (www.isDone) return www.bytes;
        //                 }
        //             } else {
        //                 return System.IO.File.ReadAllBytes (Application.streamingAssetsPath + "/" + fname);
        //             }
        //         }

        //         /// <summary>
        //         /// 从streamassets中读取ab.
        //         /// </summary>
        //         /// <param name="fname">相对于Application.dataPath</param>
        //         /// <returns>assetsBundle</returns>
        //         private AssetBundle LoadAssetsBundle (string fname) {
        //             // string path = string.Empty;
        //             // if (Application.platform == RuntimePlatform.Android && !Application.isEditor)
        //             //     path =Application.dataPath + "!assets/" + fname;
        //             // else
        //             //     path = Application.streamingAssetsPath
        //             string path = Application.streamingAssetsPath + "/" + fname;
        //             AssetBundle assetbundle = AssetBundle.LoadFromFile (path);
        //             return assetbundle;
        //         }
        //         #endregion
    }
}