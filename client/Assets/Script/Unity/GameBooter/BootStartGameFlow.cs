namespace SGF.Unity.GameBooter
{

    using SGF.Core;
    using SGF.Unity.Utils;
    using SGF.Unity.XLuaExt;

    /// <summary> 进入游戏逻辑 </summary>
    public class BootStartGameFlow : GameBoofFlowBase {

        public BootStartGameFlow (Game game) : base (game) { }

        override protected System.Collections.IEnumerator SynchronousProcessor () {
            yield return null;
            SetComplate ();
            this.StartGame ();
            yield return null;
            // Logger.PrintSuccess ("StartGameFlow 任务已完成!");
        }

        private void StartGame () {
            IoC.Get<Lua> ().require ("src.main");
        }
    }
}