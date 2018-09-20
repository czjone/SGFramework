namespace SGF.Lua {

    using System.Collections.Generic;
    using SGF.Unity;

    [System.Serializable]
    public class FVersion {

        public string hash { get; set; }

        public int size { get; set; }

    }

    /// <summary>
    /// 当前可操作的文件列表
    /// </summary>
    public class VersionFile {

        public string FileName { get; set; }

        public string Hash { get; set; }

    }

    /// <summary>
    /// 资源版本
    /// </summary>
    [System.Serializable]
    public class ResVersion : JsonSerializable<ResVersion> {

        public int VersioCode { get; set; }

        public string VersionName { get; set; }

        public Dictionary<string, FVersion> ResFiles { get; set; }

        public ResVersion () {
            this.ResFiles = new Dictionary<string, FVersion> ();
        }

        /// <summary>
        /// 返回高版本的新增的文件列表
        /// </summary>
        /// <param name="newVersioList"></param>
        /// <returns></returns>
        public List<VersionFile> GetNewFiles (ResVersion newVersionConf) {
            List<VersionFile> files = new List<VersionFile> ();
            foreach (var item in newVersionConf.ResFiles) {
                var f = item.Key;
                var hash = item.Value.hash;
                if (this.ResFiles.ContainsKey (f) == true && this.ResFiles[f].hash == hash) {
                    continue;
                } else {
                    VersionFile fv = new VersionFile ();
                    fv.FileName = f;
                    fv.Hash = hash;
                    files.Add (fv);
                }
            }

            return files;
        }

        /// <summary>
        /// 返回在新版本中不存在的文件 
        /// </summary>
        /// <param name="newVersion"></param>
        /// <returns></returns>
        public List<VersionFile> GetObsoleteFiles (ResVersion newVersion) {
            List<VersionFile> files = new List<VersionFile> ();
            foreach (var item in this.ResFiles) {
                var f = item.Key;
                var hash = item.Value.hash;
                if (newVersion.ResFiles.ContainsKey (f) == false) {
                    VersionFile fv = new VersionFile ();
                    fv.FileName = f;
                    fv.Hash = hash;
                    files.Add (fv);
                }
            }
            return files;
        }
    }

    [XLua.LuaCallCSharp]
    public class CheckStateChangedArg : System.EventArgs {

        public string FileName { get; set; }

        public int Size { get; set; }

        public CheckStateChangedArg () {

        }
    }

    [XLua.LuaCallCSharp]
    public class NeedDownLoadArgs : System.EventArgs {

        public int FileCount { get; set; }

        public int Size { get; set; }

        public NeedDownLoadArgs () {

        }
    }

    /// <summary>
    /// 下载文件过程
    /// </summary>
    /// <param name="file">文件名</param>
    /// <param name="size">文件大小</param>
    [XLua.LuaCallCSharp]
    [XLua.CSharpCallLua]
    public delegate void CheckStateChanged (object sender, CheckStateChangedArg arg);

    /// <summary>
    /// 当有新资源可下载时候的
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="arg"></param>
    [XLua.LuaCallCSharp]
    [XLua.CSharpCallLua]
    public delegate void NeedDownload (object sender, NeedDownLoadArgs arg);

    /// <summary>
    /// 热更新
    /// </summary>
    [XLua.LuaCallCSharp]
    public class HotUpdate {

        protected Game Game { get; private set; }

        public event CheckStateChanged OnDownloadEvent;

        public event NeedDownload OnCheckVersionComplateEvent;

        public bool IsPause { get; set; }

        protected Config Confg {
            get {
                return this.Game.Conf;
            }
        }

        public bool Enabled {
            get {
                return this.Confg.HotUpdateEnabled;
            }
        }

        public HotUpdate (Game game) {
            this.Game = game;
        }
    }

    public class ValidationLocalRes {

        protected Game Game { get; private set; }

        public ValidationLocalRes (Game game) {
            this.Game = game;
        }
    }

    public class UnPakageResToLocal {
        public event CheckStateChanged OnCompressEvent;
        protected Game Game { get; private set; }
        public UnPakageResToLocal (Game game) {
            this.Game = game;
        }
    }

    [XLua.LuaCallCSharp]
    public class ValidationRes {

        public UnPakageResToLocal unCompressTask { get; private set; }

        public HotUpdate HotUpdate { get; private set; }

        public ValidationLocalRes ValidationLocalRes { get; private set; }

        public bool PauseHotUpdate {
            get {
                return this.HotUpdate.IsPause;
            }
            set {
                this.HotUpdate.IsPause = value;
            }
        }

        public ValidationRes (Game game) {
            this.unCompressTask = new UnPakageResToLocal (game: game);
            this.HotUpdate = new HotUpdate (game: game);
            this.ValidationLocalRes = new ValidationLocalRes (game: game);
        }

        public void SetUnCompressTaskAction (CheckStateChanged action) {
            this.unCompressTask.OnCompressEvent += action;
        }

        public void SetCheckRemoteNeedDownload (NeedDownload action) {
            this.HotUpdate.OnCheckVersionComplateEvent += action;
        }

        public void SetHotUpdateTaskAction (CheckStateChanged action) {
            this.HotUpdate.OnDownloadEvent += action;
        }

        public void SetValidationLocalRes (CheckStateChanged action) {
            this.unCompressTask.OnCompressEvent += action;
        }

        public void ValidationResCompate () {
            
        }
    }
}