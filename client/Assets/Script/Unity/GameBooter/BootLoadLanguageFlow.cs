namespace SGF.Unity.GameBooter {
    using SGF.Core;
    using SGF.Unity.Flow;
    using SGFHot = SGF.Unity.HotUpdate;
    using SGF.Unity.Utils;
    /// <summary>加载本地化语言</summary>
    public class BootLoadLanguageFlow : GameBoofFlowBase {

        public BootLoadLanguageFlow (Game game) : base (game) { }

        override protected System.Collections.IEnumerator SynchronousProcessor () {
            yield return null;
            Logger.PrintSuccess ("LoadLanguageFlow 任务已完成!");
            SetComplate ();
        }
    }
}