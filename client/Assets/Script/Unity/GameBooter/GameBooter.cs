namespace SGF.Unity.GameBooter {

    using SGF.Core;
    using SGF.Unity.Flow;
    using SGFHot = SGF.Unity.HotUpdate;
    using SGF.Unity.Utils;

    /// <summary> 游戏启动任务流 </summary>
    public class GameBootFlow : FlowGroup {

        private Game game;

        public GameBootFlow (Game game) {
            this.game = game;
            this.InitFlow (
                new BootLoadLanguageFlow (this.game), //加载本地化语言
                new BootHotUpdateFlow (this.game), //检查热更新
                new BootCheckHotUpdateResFlow (this.game), //检查下载的资源是否完全正确
                new BootLoadLuadFlow (this.game), //加载本地最新版本的lua
                new BootStartGameFlow (this.game) //启动游戏
            );
        }
    }

    public class GameRunnerEventArgs {
        public IFlow flow { get; private set; }

        public GameRunnerEventArgs (IFlow flow) {
            this.flow = flow;
        }
    }

    public delegate void OnBootFail (GameBoot sender, GameRunnerEventArgs args);
    public delegate void OnBootSuccess (GameBoot sender, GameRunnerEventArgs args);
    public delegate void OnPercentChanged (GameBoot sender, GameRunnerEventArgs args);

    /// <summary> 游戏启动器 </summary>
    public class GameBoot {

        private Game game;
        public event OnBootFail OnBootFailEvent;
        public event OnBootSuccess OnBootSuccessEvent;
        public event OnPercentChanged OnPercentChangedEvent;

        public GameBoot (Game game) {
            this.game = game;
            this.InitGameBootFlow ();
        }

        private void InitGameBootFlow () {
            IoC.Register (new GameBootFlow (game));
        }

        public void RunGame () {
            this.RegisterEvent ();
            IoC.Get<GameBootFlow> ().StartAsy ();
        }

        void RegisterEvent () {
            var bootFlow = IoC.Get<GameBootFlow> ();
            bootFlow.OnFlowGropErrorEvent += OnBootFail;
            bootFlow.OnFlowGropCompateEvent += OnBootSuccess;
            bootFlow.OnFlowPercentChangedEvent += OnPercentChanged;
        }

        void UnRegisterEvent () {
            var bootFlow = IoC.Get<GameBootFlow> ();
            bootFlow.OnFlowGropErrorEvent -= OnBootFail;
            bootFlow.OnFlowGropCompateEvent -= OnBootSuccess;
            bootFlow.OnFlowPercentChangedEvent -= OnPercentChanged;
        }

        GameRunnerEventArgs GetArgs (IFlow flow) {
            return new GameRunnerEventArgs (flow);
        }

        void OnBootFail (IFlow flow) {
            this.UnRegisterEvent ();
            Logger.PrintFail ("游戏启动失败");
            if (this.OnBootFailEvent != null) this.OnBootFailEvent (this, GetArgs (flow));
        }

        void OnBootSuccess () {
            // Logger.PrintSuccess ("游戏启动成功！ ");
            if (this.OnBootSuccessEvent != null) this.OnBootSuccessEvent (this, GetArgs (null));
        }

        void OnPercentChanged (IFlow flow) {
            // Logger.PrintSuccess ("加载进度: " + (flow.Percent * 100).ToString (" 0.00 ") + " %");
            if (this.OnPercentChangedEvent != null) this.OnPercentChangedEvent (this, GetArgs (flow));
        }
    }

    public abstract class GameBoofFlowBase : BaseFlow {
        protected Game Game {
            get;
            private set;
        }

        public GameBoofFlowBase (Game game) : base () {
            this.Game = game;
        }

        override protected void OnInit () {
            this.OnFlowStateChangedEvent += this.StateChangeProcessor;
        }

        override protected void OnUnInit () {
            this.OnFlowStateChangedEvent -= this.StateChangeProcessor;
        }

        private void StateChangeProcessor (IFlow flow) {
            //不要在这个地方写设置状态的逻辑。会出现overstackflow.
            if (flow.State == FlowState.EXCTEUS) {
                this.Game.StartCoroutine (this.SynchronousProcessor ());
            }
        }

        virtual protected System.Collections.IEnumerator SynchronousProcessor () {
            yield return null;
            Logger.PrintSuccess ("空任务已完!");
            SetComplate ();
        }

    }
}