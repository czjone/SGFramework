namespace SGF.Lua {
    using System.Collections.Generic;
    using System.Collections;
    using SGF.Lua;

    public class HotUpdataFile {
        public string RemoteUrl { get; private set; }
        public int DownloadTimes { get; private set; }
        public string LocalPath { get; private set; }
        public string Hash { get; private set; }
        public int Size { get; private set; }
        public string fname { get; private set; }
        public HotUpdataFile (string remoteF, string localF, string remoteHash, int size, string fname) {
            this.RemoteUrl = remoteF;
            this.LocalPath = localF;
            this.DownloadTimes = 0;
            this.Hash = remoteHash;
            this.Size = size;
            this.fname = fname;
        }

        public void AddDownlaodTimes () {
            this.DownloadTimes++;
        }
    }

    [XLua.LuaCallCSharp]
    public class HotUpdateParmers {
        public Game game { get; set; }
        public string url { get; set; }
        public Dictionary<string, FileInfo> download { get; set; }
        public string localDir { get; set; }
        public int maxDownloadThread { get; set; }
    }

    [XLua.LuaCallCSharp]
    public class DownloadArgs {
        public string remoteUrl { get; set; }
        public string fname { get; set; }
        public int size { get; set; }
    }

    [XLua.CSharpCallLua]
    public delegate void HotUpdateDownloadAction (HotUpdate sender, DownloadArgs args);

    [XLua.CSharpCallLua]
    public delegate void HotComplateAction (HotUpdate sender);

    [XLua.CSharpCallLua]
    public delegate void HotErroAction (HotUpdate sender);

    [XLua.LuaCallCSharp]
    public class HotUpdate {

        public Game Game { get; private set; }

        private MainRootBehaviour UnityBehaviour {
            get {
                return this.Game.RootBehaviour;
            }
        }

        public string Url { get; private set; }

        public string localDir { get; private set; }

        private bool _isPause = false;
        public bool IsPause {
            set {
                if (_isPause != value) {
                    if (_isPause == false) {
                        this.StartAsy ();
                    }
                }
            }
            get { return _isPause; }
        }

        public bool IsStop { get; set; }

        private Queue<HotUpdataFile> downloadFiles;

        private Queue<HotUpdataFile> downloadErrorFiles;
        private Dictionary<string, HotUpdataFile> downloadingFiles;

        public HotUpdateDownloadAction OnHotUpdateDownloadAction { get; private set; }

        public HotComplateAction OnHotComplateAction { get; private set; }

        public HotErroAction OnHotErroAction { get; private set; }

        private int maxDownloadThread;

        public HotUpdate (HotUpdateParmers param) {
            this.Game = param.game;
            this.Url = param.url.EndsWith ("/") ? param.url : (param.url + "/");
            this.localDir =
                localDir.EndsWith (SGF.Core.Path.DirSplitor) ?
                localDir : (localDir + SGF.Core.Path.DirSplitor);
            this.downloadFiles = new Queue<HotUpdataFile> ();
            this.downloadErrorFiles = new Queue<HotUpdataFile> ();
            this.downloadingFiles = new Dictionary<string, HotUpdataFile> ();
            foreach (var item in param.download) {
                var df = new HotUpdataFile (this.Url + item.Key,
                    this.localDir + item.Key,
                    item.Value.Hash,
                    item.Value.Size,
                    item.Key);
                this.downloadFiles.Enqueue (df);
            }
            this.maxDownloadThread = maxDownloadThread;
        }

        public void StartAsy () {
            this.UnityBehaviour.StartCoroutine (this.StartDownloadAsy ());
        }

        private IEnumerator StartDownloadAsy () {
            for (int i = 0; i < this.maxDownloadThread; i++) {
                this.UnityBehaviour.StartCoroutine (this.StartDownloadThread ());
            }
            yield return null;
        }

        private IEnumerator StartDownloadThread () {
            if (this.downloadFiles.Count > 0) {
                HotUpdataFile hot = this.downloadErrorFiles.Dequeue ();
                this.downloadingFiles.Add (hot.RemoteUrl, hot);
                Http http = new Http (this.Game, hot.RemoteUrl, x => {
                    //http 下载完成
                    if (x.Ret == 0) {
                        //写文件逻辑
                        SGF.Core.File.WriteFile (hot.LocalPath, x.Data);
                        //验证写入文件的正确性
                        var fHash = SGF.Core.Security.Md5.GetSHA1WithFile (hot.LocalPath);
                        if (fHash == hot.Hash) {
                            //写入文件失败
                            if (this.OnHotUpdateDownloadAction != null) {
                                DownloadArgs args = new DownloadArgs ();
                                args.fname = hot.fname;
                                args.size = hot.Size;
                                args.remoteUrl = hot.RemoteUrl;
                                this.OnHotUpdateDownloadAction.Invoke (this, args);
                                this.downloadingFiles.Remove (hot.RemoteUrl);
                                this.UnityBehaviour.StartCoroutine (this.StartDownloadThread ());
                            }
                        }
                    } else {
。。。。。。。逻辑还没有写完。
                    }
                }, Http.Method.GET);
            } else {
                yield return null;
            }
        }
    }
}