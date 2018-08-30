namespace SGF.Lua {

	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;
	using XLua;

	[LuaCallCSharp]
	[CSharpCallLua]
	public delegate byte[] LuaLoader (ref string luaPath);

	[LuaCallCSharp]
	public class LuaRunner : System.IDisposable {

		public XLua.LuaEnv LuaEnv { get; private set; }

		public SGFManager Manager { get; private set; }

		public LuaRunner (SGFManager manager) {
			this.Manager = manager;
			this.LuaEnv = new LuaEnv ();
			this.LuaEnv.AddLoader (this.LoadEnctyptionLua);
		}

		public void Require (string model) {
			this.LuaEnv.DoString (string.Format ("require \"{0}\"", model));
		}

		public void RunLuaModel (string model) {
			this.Require (model);
		}

		public void Run (string model) {
			this.Require (model);
		}

		public void AddLuaLoader (LuaLoader loader) {
			this.LuaEnv.AddLoader ((ref string x) => loader (ref x));
		}

		public void AddLuaSearchPath (string searchpath) {
			//package.path = '/usr/local/share/lua/5.1/?.lua;/home/resty/?/init.lua;'
			string setSearchPathLua = 		"package.path = package.path..\";" + searchpath + "/?.lua\"";
			setSearchPathLua += 			"package.path = package.path..\";" + searchpath + "/?.lc\"";
			setSearchPathLua += 			"package.path = package.path..\";" + searchpath + "/?.luac\"";
			setSearchPathLua += 			"package.path = package.path..\";" + searchpath + "/?/init.lua\";";
			setSearchPathLua += 			"package.path = package.path..\";" + searchpath + "/?/init.lc\"";
			setSearchPathLua += 			"package.path = package.path..\";" + searchpath + "/?/init.luac\"";
			this.LuaEnv.DoString (setSearchPathLua);
		}

		private byte[] LoadEnctyptionLua (ref string filepath) {
			return null;
		}

		public void Dispose () {
			if (LuaEnv != null) {
				LuaEnv.Dispose ();
				LuaEnv = null;
			}
		}
	}

}