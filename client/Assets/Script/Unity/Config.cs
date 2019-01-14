namespace SGF.Unity {

    public enum DevelopMethod {
        LOCAL_RES = 0,
        HOT_UPDATE_RES = 1,
    }

    public class Config : SGF.Core.JsonSerializable<Config> {

        public static string CONF_PATH = UnityEngine.Application.dataPath + "/../SGF_AUTO_PROJECT_CONFIG.json";

        /// <summary>
        /// 当前开发的资源加载模式
        /// </summary>
        public DevelopMethod DevType = DevelopMethod.LOCAL_RES;

        /// <summary>
        /// 开发资源目录
        /// </summary>
        public string ProjecResourceDir = "DResources"; //相对于unity assets目录

        public static Config LoadDefaultConfig () {
            return Config.LoadWithFile (CONF_PATH, true);
        }
    }
}