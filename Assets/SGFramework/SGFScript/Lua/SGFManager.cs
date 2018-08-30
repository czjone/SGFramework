namespace SGF.Lua {

	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;
	using XLua;

	[LuaCallCSharp]
	public class SGFManager : System.IDisposable {

		public ResMgr ResMgr { get; private set; }

		public LuaRunner Lua { get; private set; }

		public MonoBehaviour Bev { get; private set; }

		public SGFManager (MonoBehaviour bev) {
			this.Bev = bev;
			this.ResMgr = new SGF.Lua.ResMgr ();
			this.Lua = new SGF.Lua.LuaRunner (this);
		}

		public void Dispose () {
			this.Lua.Dispose ();
			this.ResMgr.Dispose ();
		}
	}
}