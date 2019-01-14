namespace SGF.Unity {
	using SGF.Core;
	using UnityEngine;
	using Logger = SGF.Unity.Utils.Logger;
	using SGF.Unity.GameBooter;
	using SGF.Unity.Utils;

	public class Game : MonoBehaviour {

		private static bool isInited = false;

		void Awake () {
			if (isInited == true) {
				Logger.PrintError ("Game脚本只能挂载一次.");
			}

			this.Init ();

			isInited = true;
		}

		void Init () {
			if (Application.isEditor) {
				IoC.Register (Config.LoadDefaultConfig ());
			}
			IoC.Register (new ResMgr (this));
			IoC.Register (new UIHelper (this));
			IoC.Register (new HttpHelper (this));
			IoC.Register (new UIManager (this));
			IoC.Register (new GameBoot (this));
		}

		void Start () {
			//开始启动
			var booter = IoC.Get<GameBoot> ();
			//绑定界面
			this.BindBootUI (booter);
			if (booter == null) {
				Logger.PrintError ("游戏启动器不存在.");
				return;
			}
			booter.RunGame ();
		}

		virtual protected GameBootBaseUI BindBootUI (GameBoot booter) {
			return new GameBootBaseUI (booter);
		}
	}
}