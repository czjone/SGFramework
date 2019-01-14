namespace SGF.Unity.GameBooter {
    using SGF.Core;
    using SGF.Unity.Flow;
    using SGFHot = SGF.Unity.HotUpdate;
    using SGF.Unity.HotUpdate;
    using SGF.Unity.Utils;

    /// <summary> 热更新 </summary>
    public class BootHotUpdateFlow : GameBoofFlowBase {

        public OnHotUpdateStateChangedArgs HotUpdateState { get; private set; }

        SGFHot.HotUpdate hotUpdate;
        public BootHotUpdateFlow (Game game) : base (game) {
            this.hotUpdate = IoC.Register (new SGFHot.HotUpdate (game));
            this.hotUpdate.OnHotUpdateStateChangedEvent += x => {
                this.HotUpdateState = x;
            };
        }

        override protected System.Collections.IEnumerator SynchronousProcessor () {
            while (true) {
                yield return null;
                if (HotUpdateState != null) {
                    base.RefPercent (System.Math.Max (1.0f, HotUpdateState.percent));
                    if (HotUpdateState.step == HotUpdateStep.COMPLATE) {
                        SetComplate ();
                        break;
                    }

                    if (HotUpdateState.step == HotUpdateStep.ERROR) {
                        //TODO:set error.
                        break;
                    }
                }
            }
            IoC.UnRegister (this.hotUpdate);
            Logger.PrintSuccess ("资源更新完成!");
        }

    }
}