	namespace SGF.Lua {

		using System.Collections.Generic;
		using System.Collections;
		using UnityEngine;
		using XLua;

		[XLua.LuaCallCSharp]
		public class Game : System.IDisposable {

			public Lua.ResMgr ResMgr { get; private set; }

			public MainRootBehaviour RootBehaviour { get; private set; }

			public LuaTick LuaTick { get; private set; }

			public SGFLua Lua { get; private set; }

			// public HotUpdate HotUpdate { get; private set; }

			public Config Conf {
				get {
					return this.RootBehaviour.Conf;
				}
			}

			private GameObject _uiRoot = null;
			public virtual GameObject UIRoot {
				get {
					if (_uiRoot == null) {
						_uiRoot = GameObject.Find ("Canvas");
					}
					return _uiRoot;
				}
			}

			private string _resFullRoot = null;
			public virtual string ResRoot {
				get {
					if (string.IsNullOrEmpty (_resFullRoot)) {

						var platform = Application.platform;
						if (platform == RuntimePlatform.LinuxEditor ||
							platform == RuntimePlatform.OSXEditor ||
							platform == RuntimePlatform.WindowsEditor) {
							_resFullRoot = this.Conf.DevDir + Core.Path.DirSplitor + this.Conf.DresRootName;
						} else {
							_resFullRoot = Application.persistentDataPath + Core.Path.DirSplitor + this.Conf.DresRootName;
						}
					}
					return _resFullRoot;
				}
			}

			public Game (MainRootBehaviour rootBehaviour) {
				this.RootBehaviour = rootBehaviour;
				this.ResMgr = new SGF.Lua.ResMgr ();
				this.Lua = new SGFLua (this);
				// this.HotUpdate = new HotUpdate (this);
				this.LuaTick = new LuaTick (this.Lua);
			}

			[XLua.DoNotGen]
			public void Start () {
				this.Lua.AddG ("SGFGame", this); // global lua interface
				this.Lua.AddLuaSearchPath (this.ResRoot);
				this.Lua.Run (this.Conf.LuaMain);
			}

			[XLua.DoNotGen]
			public void Update () {
				this.LuaTick.DoTick ();
			}

			[XLua.DoNotGen]
			public void Dispose () {
				this.LuaTick.Stop ();
				// this.Lua.Dispose ();
			}
		}
	}