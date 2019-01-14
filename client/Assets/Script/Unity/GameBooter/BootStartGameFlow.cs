namespace SGF.Unity.GameBooter {
    using SGF.Core;
    using SGF.Unity.Flow;
    using SGFHot = SGF.Unity.HotUpdate;
    using SGF.Unity.Utils;

    /// <summary> 进入游戏逻辑 </summary>
    public class BootStartGameFlow : GameBoofFlowBase {

        public BootStartGameFlow (Game game) : base (game) { }

        override protected System.Collections.IEnumerator SynchronousProcessor () {
            yield return null;
            Logger.PrintSuccess ("StartGameFlow 任务已完成!");
            SetComplate ();
        }
    }
}