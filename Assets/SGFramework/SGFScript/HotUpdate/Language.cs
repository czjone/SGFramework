namespace SGF {
    using UnityEngine;
    namespace HotUpdate {
        public class Language : SGF.Core.JsonSerializable<Language> {
            public string CHECK_DECOMPRESS_PKG_RES_TASK_DES { get; set; }

            [System.NonSerialized]
            private static readonly string languageFilePath = "SGRunner/Language";

            [System.NonSerialized]
            private static Language instance;
            
            public static Language GetInstance () {
                if (instance == null) {
                    var txtAsset = Resources.Load<TextAsset> (languageFilePath);
                    if (txtAsset == null) {
                        throw new System.IO.FileNotFoundException (" Resources.Load<TextAsset> load language file error!" + languageFilePath);
                    }
                    instance = Language.LoadWithJson (txtAsset.text);
                }
                return instance;
            }
        }
    }
}