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

        protected virtual void InitUI () {
            var uihelper = IoC.Get<UIHelper> ();
            this.booterui = uihelper.ShowPrefab ("res/prefabs/game-boot");
            this.RegisterGameBooterEvent ();
        }

        protected void RegisterGameBooterEvent () {
            booter.OnBootFailEvent += this.OnBootFail;
            booter.OnBootSuccessEvent += OnSuccess;
            booter.OnPercentChangedEvent += OnChangedPercent;
        }

        protected void UnRegetserGameBooterEvent () {
            booter.OnBootFailEvent -= this.OnBootFail;
            booter.OnBootSuccessEvent -= OnSuccess;
            booter.OnPercentChangedEvent -= OnChangedPercent;
        }

        void OnBootFail (GameBoot sender, GameRunnerEventArgs args) {
            Utils.Logger.PrintError ("游戏启动失败!");
            IoC.UnRegister (language);
        }

        void OnSuccess (GameBoot sender, GameRunnerEventArgs args) {
            Utils.Logger.PrintError ("游戏启动成功！");
            IoC.UnRegister (language);
        }

        void OnChangedPercent (GameBoot sender, GameRunnerEventArgs args) {
            if (booterui == null) {
                Utils.Logger.PrintWarring ("没有加载启动界面.");
                return;
            }
            var percentDes_Text = booterui.FindChildWithTagName ("PercentDes-Text");
            var gameHelper_Text = booterui.FindChildWithTagName ("GameHelper-Text");
            var percent_Slider = booterui.FindChildWithTagName ("Percent-Slider");
            //显示文字Î
            percentDes_Text.SetText (language.GetVal (args.flow));
            //显示文字 
            gameHelper_Text.SetText (getGameHelperDes ?? "")
                .SetVisible (getGameHelperDes != null);
            percent_Slider.SetSlider (args.flow == null ? 0.0f : args.flow.Percent); //显示进度
        }

        protected virtual string getGameHelperDes {
            get {
                return null;
            }
        }
    }
}