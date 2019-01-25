using System;
using SGF.Core;
using SGF.Unity.Utils;

namespace SGF.Unity.HotUpdate {

    public enum HotUpdateStep {
        CHECK_VERSION,
        DOWNLOADFILES,
        COMPLATE,
        ERROR,
    }

    public enum State {
        NETWORK_ERROR,
        FILE_INVALID,
        SUCCRESS,
    }

    public class OnHotUpdateStateChangedArgs {
        public HotUpdateStep step { get; set; }
        public float percent { get; set; }
        public State State { get; set; }
    }

    public delegate void OnHotUpdateStateChangedAction (OnHotUpdateStateChangedArgs args);

    public class HotUpdate {

        private Game game;

        private Config conf;
        private PatchFiles needDownLoadFiles;
        private const int DownLoadFileThreadMax = 4;

        public event OnHotUpdateStateChangedAction OnHotUpdateStateChangedEvent;

        public HotUpdate (Game game) {
            this.game = game;
        }

        private void LoadConfig () {
            // var json = IoC.Get<ResMgr> ().LoadJsonFromResources ("conf.json");
            // conf = Config.LoadWithJson (json);
            conf = IoC.Get<ResMgr> ().LoadFromJson<Config> ("config");
        }

        public bool CheckVersion () {
            this.LoadConfig ();
            string url = this.conf.CDN + "/" + this.conf.VersionFileName;
            HttpHelper http = IoC.Get<HttpHelper> ();
            http.DownLoadFileAsy (url, this.OnCheckVersionResult);
            return false;
        }

        private void OnCheckVersionResult (DownloadArgs args) {
            var json = System.Text.UTF8Encoding.ASCII.GetString (args.data);
            Version remoteVersion = Version.LoadWithJson (json);
            bool needDownload = false;
            var localVersionFile = UnityEngine.Application.persistentDataPath + "/" + conf.DownloadDir + "/" + conf.VersionFileName;
            if (System.IO.File.Exists (localVersionFile) == false) {
                needDownload = true;
                Logger.PrintLog ("local version:{0}  remote version:{1}".FormatWith (0, remoteVersion.VersionCode));
            } else {
                var locaVersion = Version.LoadWithFile (localVersionFile);
                needDownload = remoteVersion.VersionCode > locaVersion.VersionCode;
                Logger.PrintLog ("local version:{0}  remote version:{1}".FormatWith (0, remoteVersion.VersionCode));
            }
        }
    }
}