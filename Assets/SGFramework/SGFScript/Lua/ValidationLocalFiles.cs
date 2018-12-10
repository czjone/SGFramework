namespace SGF.Lua {
    using System.Collections.Generic;
    using System.Collections;
    using SGF.Unity;

    public delegate void ValidationChangeAction (int state, string file);

    public delegate void ValidationComplateAction (int state, string msg);

    [XLua.LuaCallCSharp]
    public class ValidationLocalFiles {

        public string Folder { get; private set; }

        public ResVersion TagVersion { get; private set; }

        public Game Game { get; private set; }

        private MainRootBehaviour UnityBehaviour {
            get {
                return this.Game.RootBehaviour;
            }
        }

        private ValidationChangeAction onChanged;

        private ValidationComplateAction onComplate;

        public ValidationLocalFiles (Game game, string folder, ResVersion tagVersion) {
            this.Folder = InitFolder (folder);
            this.TagVersion = tagVersion;
            this.Game = game;
        }

        private string InitFolder (string folder) {
            var tagFolder = folder;
            if (tagFolder.EndsWith (SGF.Core.Path.DirSplitor) == false) {
                tagFolder += SGF.Core.Path.DirSplitor;
            }
            return tagFolder;
        }

        public void SetOnChanged (ValidationChangeAction action) {
            this.onChanged = action;
        }

        public void SetOnComplate (ValidationComplateAction action) {
            this.onComplate = action;
        }

        public void ValidAsy () {
            this.UnityBehaviour.StartCoroutine (Valid ());
        }

        private IEnumerator Valid () {
            foreach (var item in this.TagVersion.Files) {
                var fname = item.Key;
                var fversion = item.Value;
                var ret = this.ValidFile (fname, fversion.Hash);
                if (this.onChanged != null) {
                    this.onChanged (ret ? 0 : -1, fname); //do onchanged.
                }
                yield return null;
            }

            if (this.onComplate != null) {
                this.onComplate (0, "valid local files complate."); //oncomplate
            }
            yield return null;
        }

        private bool ValidFile (string f, string hash) {
            var fullPath = this.Folder + SGF.Core.Path.Legalization (f);
            var fhash = SGF.Core.Security.Md5.GetSHA1WithFile (fullPath);
            return fhash == hash;
        }
    }
}