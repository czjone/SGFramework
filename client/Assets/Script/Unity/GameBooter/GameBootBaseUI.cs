namespace SGF.Unity.GameBooter {

    using SGF.Core;
    using SGF.Unity.HotUpdate;
    using SGF.Unity.Utils;
    using UnityEngine;

    public class GameBootBaseUI {

        private GameObject booterui = null;
        private GameBoot booter;
        private BootLanguage language;

        public GameBootBaseUI (GameBoot booter) {
            this.booter = booter;
            language = IoC.Register (new BootLanguage ());
            this.InitUI ();
        }

        private void InitUI () {
            var uIManager = IoC.Get<UIManager> ();
            this.booterui = uIManager.Open (GameLoadingUI);
            this.RegisterGameBooterEvent ();
        }

        private void RegisterGameBooterEvent () {
            booter.OnBootFailEvent += this.OnBootFail;
            booter.OnBootSuccessEvent += OnSuccess;
            booter.OnPercentChangedEvent += OnChangedPercent;
        }

        private void UnRegetserGameBooterEvent () {
            booter.OnBootFailEvent -= this.OnBootFail;
            booter.OnBootSuccessEvent -= OnSuccess;
            booter.OnPercentChangedEvent -= OnChangedPercent;
        }

        virtual protected void OnBootFail (GameBoot sender, GameRunnerEventArgs args) {
            Utils.Logger.PrintError ("游戏启动失败!");
            IoC.UnRegister (language);
        }

        virtual protected void OnSuccess (GameBoot sender, GameRunnerEventArgs args) {
            Utils.Logger.PrintSuccess ("游戏启动成功！");
            IoC.UnRegister (language);
            var uIManager = IoC.Get<UIManager> ();
            uIManager.Close (booterui);
        }

        virtual protected void OnChangedPercent (GameBoot sender, GameRunnerEventArgs args) {
            if (booterui == null) {
                Utils.Logger.PrintWarring ("没有加载启动界面.");
                return;
            }
            var percentDes_Text = booterui.FindChildWithTagName ("PercentDes-Text");
            var gameHelper_Text = booterui.FindChildWithTagName ("GameHelper-Text");
            var percent_Slider = booterui.FindChildWithTagName ("Percent-Slider");
            //显示文字
            percentDes_Text.SetText (language.GetVal (args.flow));
            //显示文字 
            gameHelper_Text.SetText (GameHelperDes ?? "")
                .SetVisible (GameHelperDes != null);
            percent_Slider.SetSlider (args.flow == null ? 0.0f : args.flow.Percent); //显示进度
        }

        protected virtual string GameHelperDes {
            get {
                return null;
            }
        }

        protected virtual string GameLoadingUI {
            get {
                return "res/prefabs/game-boot";
            }
        }
    }
}