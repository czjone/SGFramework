namespace SGF.Lua {

    using System.Collections.Generic;
    using SGF.Unity;

    [System.Serializable]
    public class FileInfo {

        public string Hash { get; set; }

        public int Size { get; set; }
    }

    public class ResVersion : SGF.Unity.JsonSerializable<ResVersion> {

        public long VerCode { get; set;  }

        public long VerName { get; set; }

        public Dictionary<string, FileInfo> Files { get; set; }

        public ResVersion () {
            this.Files = new Dictionary<string, FileInfo> ();
        }
    }
}