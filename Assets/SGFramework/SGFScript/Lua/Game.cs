namespace SGF.Lua {

	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;
	using XLua;

	[LuaCallCSharp]
	public class Game : System.IDisposable {

		public Lua.ResMgr ResMgr { get; private set; }

		public MonoBehaviour RootBehaviour { get; private set; }

		public LuaTick LuaTick { get; private set; }

		public SGFLua Lua { get; private set; }

		public Game (MonoBehaviour rootBehaviour) {
			this.RootBehaviour = rootBehaviour;
			this.ResMgr = new SGF.Lua.ResMgr ();
			this.Lua = new SGFLua (this);
			this.LuaTick = new LuaTick (this.Lua);
		}

		public void Update () {
			this.LuaTick.DoTick();
		}

		public void Dispose () {
			//this.ResMgr.Dispose ();
			//this.RootBehaviour.Dispose ();
			this.Lua.Dispose ();
		}
	}
}