using SGF.Core;
using SGF.Unity.Flow;
using SGF.Unity.HotUpdate;

namespace SGF.Unity.GameBooter {
    // 游戏格式要统一
    public class BootLanguage : JsonSerializable<BootLanguage> {

        private string LOAD_LOCAL_LANGUAGE = "Load ({0}%)";

        private string LOAD_HOT_UPDATE_CHECK_RES_VERSION = "检查版本({0}%)";

        private string LOAD_HOT_UPDATE_DOWNLOWD = "下载文件({0}%)";

        private string LOAD_CHECK_HOTUPDATE_RES = "验证本地资源({0}%)";

        private string LOAD_LOAD_ALL_LUA_RES = "加载资源({0}%)";

        private string LOAD_START_GAME = "启动游戏({0}%)";

        public static BootLanguage LoadDefault () {
            return new BootLanguage ();
        }

        public string GetVal (IFlow flow) {
            if (flow == null) return "";
            var percentstr = (flow.Percent * 100.0f).ToString ("0.00");
            if (flow is BootLoadLanguageFlow) {
                return LOAD_LOCAL_LANGUAGE.FormatWith (percentstr);
            }

            if (flow is BootHotUpdateFlow) {
                var hotupdate = flow as BootHotUpdateFlow;
                switch (hotupdate.HotUpdateState.step) {
                    case HotUpdateStep.CHECK_VERSION:
                        return LOAD_HOT_UPDATE_CHECK_RES_VERSION.FormatWith (percentstr);
                    case HotUpdateStep.DOWNLOADFILES:
                        return LOAD_HOT_UPDATE_DOWNLOWD.FormatWith (percentstr);
                    default:
                        return "";
                }
            }

            if (flow is BootCheckHotUpdateResFlow) {
                return LOAD_CHECK_HOTUPDATE_RES.FormatWith (percentstr);
            }

            if (flow is BootLoadLuadFlow) {
                return LOAD_LOAD_ALL_LUA_RES.FormatWith (percentstr);
            }

            if (flow is BootStartGameFlow) {
                return LOAD_START_GAME.FormatWith (percentstr);
            }

            return "";
        }
    }
}