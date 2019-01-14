namespace SGF.Unity.GameBooter {
    using SGF.Core;
    using SGF.Unity.Flow;
    using SGFHot = SGF.Unity.HotUpdate;
    using SGF.Unity.Utils;

    /// <summary> 检查更新的资源是否完整，有的用户喜欢手动去删除数据 </summary>
    public class BootCheckHotUpdateResFlow : GameBoofFlowBase {

        public BootCheckHotUpdateResFlow (Game game) : base (game) { }

        override protected System.Collections.IEnumerator SynchronousProcessor () {
            yield return null;
            Logger.PrintSuccess ("CheckHotUpdateResFlow 任务已完成!");
            SetComplate ();
        }
    }
}