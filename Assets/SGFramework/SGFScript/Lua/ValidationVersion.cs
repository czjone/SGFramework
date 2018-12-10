namespace SGF.Lua {

    using System.Collections.Generic;
    using SGF.Core;
    using UnityEngine.Networking;
    using UnityEngine;

    [XLua.LuaCallCSharp]
    public enum ValidationVersionMethod {
        WITH_LOCAL_FILES = 1,
        WITH_LOCAL_VERSION_FILES = 2,
    }

    [XLua.LuaCallCSharp]
    public class ValidationVersionData {
        /// <summary>
        /// remote host
        /// </summary>
        /// <value></value>
        public string url { get; set; }
        /// <summary>
        /// version list file name.
        /// </summary>
        /// <value></value>
        public string versioName { get; set; }
        /// <summary>
        /// 本地目录
        /// </summary>
        /// <value></value>
        public string localDir { get; set; }
        /// <summary>
        /// 同时检查的文件个数据
        /// </summary>
        /// <value></value>
        public int MaxThread { get; set; }
    }

    [XLua.CSharpCallLua]
    public class ValidationComplateComplateArgs {
        public int Ret { get; private set; }
        public string Msg { get; private set; }
        public Dictionary<string, FileInfo> files { get; set; }
        public Dictionary<string, FileInfo> delFiles { get; set; }
        public int size { get; set; }
        public ValidationComplateComplateArgs (int state, string msg, Dictionary<string, FileInfo> files, int allfileSize, Dictionary<string, FileInfo> deleteFiles = null) {
            this.Ret = state;
            this.Msg = msg;
            this.files = files;
            this.size = allfileSize;
            this.delFiles = deleteFiles;
        }
    }

    [XLua.CSharpCallLua]
    public delegate void ValidationVersionFilesAction (int state, string file);

    [XLua.CSharpCallLua]
    public delegate void ValidationVersionComplateAction (ValidationComplateComplateArgs args);

    [XLua.LuaCallCSharp]
    /// <summary>
    /// 检查远程文件和本地文件的差异
    /// </summary>
    public class ValidationVersion {

        public ValidationVersionData Vdata { get; private set; }

        public ResVersion RemoteVersion { get; private set; }

        public ResVersion LocalVersion { get; private set; }

        private ValidationVersionFilesAction OnChanged;

        private ValidationVersionComplateAction OnComplate;

        public Game Game { get; private set; }

        public MainRootBehaviour UnityBehaviour {
            get {
                return this.Game.RootBehaviour;
            }
        }

        public ValidationVersion (Game game, ValidationVersionData vdata) {
            this.Game = game;
            this.Vdata = vdata;
        }

        public void SetOnChanged (ValidationVersionFilesAction action) {
            this.OnChanged = action;
        }

        public void SetOnComplate (ValidationVersionComplateAction action) {
            this.OnComplate = action;
        }

        public void ValidAsy () {
            this.UnityBehaviour.StartCoroutine (this.ValidWithVersionFile ());
        }

        private string GetVersionFileURL () {
            string url = this.Vdata.url;
            if (url.EndsWith ("/") == false) url += "/";
            url += this.Vdata.versioName;
            return url;
        }

        private System.Collections.IEnumerator LoadRemoteVersionFile () {
            string verUrl = this.GetVersionFileURL ();
            UnityWebRequest www = UnityWebRequest.Get (verUrl);
            yield return www.SendWebRequest ();
            if (www.isError || www.isHttpError) {
                SGF.Unity.ULog.D ("request error:{0} ,url:{1}".FormatWith (www.error, www.url));
                this.RemoteVersion = null;
                if (this.OnComplate != null) {
                    ValidationComplateComplateArgs args
                        = new ValidationComplateComplateArgs (-1, www.error, null, 0);
                    this.OnComplate.Invoke (args);
                }
            } else {
                string json = System.Text.UTF8Encoding.ASCII.GetString (www.downloadHandler.data);
                this.RemoteVersion = ResVersion.LoadWithJson (json);
                if (this.RemoteVersion == null) {
                    SGF.Unity.ULog.D ("decode remote Json error,url:{0}", www.url);
                }
            }
        }

        private System.Collections.IEnumerator LoadLocalVersionFile () {
            string dir = this.Vdata.localDir;
            if (dir.EndsWith (SGF.Core.Path.DirSplitor) == false) {
                dir += SGF.Core.Path.DirSplitor;
            }
            string fullPath = dir + this.Vdata.versioName;
            this.LocalVersion = ResVersion.LoadWithFile (fullPath);
            yield return null;
        }

        private System.Collections.IEnumerator ValidWithVersionFile () {
            yield return LoadLocalVersionFile ();
            yield return LoadRemoteVersionFile ();
            yield return ValidVersion (this.RemoteVersion, this.LocalVersion);
        }

        #region 检查版本
        private System.Collections.IEnumerator ValidVersion (ResVersion remoteVersion, ResVersion localVersion) {
            bool hasRemoteVersion = remoteVersion != null &&
                remoteVersion.Files != null &&
                remoteVersion.Files.Count > 0;

            //无远程版本
            if (!hasRemoteVersion && this.OnComplate != null) {
                ValidationComplateComplateArgs args
                    = new ValidationComplateComplateArgs (0, "", null, 0);
                this.OnComplate.Invoke (args);
            }

            // 有远程版本
            if (hasRemoteVersion) {
                bool hasLocalVersion = localVersion != null &&
                    localVersion.Files != null &&
                    localVersion.Files.Count > 0;
                //无本地版本
                if (!hasLocalVersion) {
                    if (this.OnComplate != null) {
                        ValidationComplateComplateArgs args
                            = new ValidationComplateComplateArgs (0, "", remoteVersion.Files, remoteVersion.Files.Count);
                        this.OnComplate.Invoke (args);
                    }
                }
                //有本地版本
                else {
                    Dictionary<string, FileInfo> remoteFiles = remoteVersion.Files;
                    Dictionary<string, FileInfo> localFiles = localVersion.Files;
                    Dictionary<string, FileInfo> downLoadFiles = new Dictionary<string, FileInfo> ();
                    Dictionary<string, FileInfo> deletFiles = new Dictionary<string, FileInfo> ();
                    // 要下载的文件
                    foreach (var item in remoteFiles) {
                        string fname = item.Key;
                        FileInfo finfo = item.Value;
                        if (localFiles.ContainsKey (fname) == false || finfo.Hash != localFiles[fname].Hash) {
                            downLoadFiles.Add (fname, finfo);
                        }
                    }
                    //要删除的文件
                    foreach (var item in localFiles) {
                        string fname = item.Key;
                        FileInfo finfo = item.Value;
                        if (!remoteFiles.ContainsKey (fname)) {
                            deletFiles.Add (fname, finfo);
                        }
                    }

                    //返回处理结果
                    if (this.OnComplate != null) {
                        ValidationComplateComplateArgs args
                            = new ValidationComplateComplateArgs (0, "", downLoadFiles, downLoadFiles.Count, deletFiles);
                        this.OnComplate.Invoke (args);
                    }

                }
            }
            yield return null;
        }
        #endregion
    }
}