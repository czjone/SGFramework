namespace SGF.Unity.GameBooter {
    using SGF.Core;
    using SGF.Unity.Flow;
    using SGFHot = SGF.Unity.HotUpdate;
    using SGF.Unity.Utils;
    /// <summary> 加载lua </summary>
    public class BootLoadLuadFlow : GameBoofFlowBase {

        public BootLoadLuadFlow (Game game) : base (game) { }

        override protected System.Collections.IEnumerator SynchronousProcessor () {
            yield return null;
            Logger.PrintSuccess ("LoadLuadFlow 任务已完成!");
            SetComplate ();
        }

    }

}