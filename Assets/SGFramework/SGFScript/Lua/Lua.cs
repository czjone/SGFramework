﻿namespace SGF.Lua {

	using System.Collections.Generic;
	using System.Collections;
	using System.Text;
	using SGF.Core;
	using SGF.Security;
	using UnityEngine;
	using XLua;

	[LuaCallCSharp]
	[CSharpCallLua]
	public delegate byte[] LuaLoader (ref string luaPath);

	[LuaCallCSharp]
	public class SGFLua : System.IDisposable {

		private readonly List<string> LuaExtentions = new List<string> () {
			"?.lua",
			"?.lc",
			"?.luac",
			"?/init.lua",
			"?/init.lc",
			"?/init.luac",
		};

		public XLua.LuaEnv LuaEnv { get; private set; }

		public Game Game { get; private set; }

		public SGFLuaCustomLoaderManager CustomLoaderMgr { get; private set; }

		public SGFLua (Game game) {
			this.Game = game;
			this.LuaEnv = new LuaEnv ();
			this.CustomLoaderMgr = new SGFLuaCustomLoaderManager ();
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

		public void AddG<T> (string key, T t) {
			this.LuaEnv.Global.Set<string, T> (key, t);
		}

		public void AddLuaSearchPath (string searchpath) {
			// DOC
			// ------------------------------------------------------------------------------
			// package.path = '/usr/local/share/lua/5.1/?.lua;/home/resty/?/init.lua;'
			// string setSearchPathLua = "package.path = package.path..\";" + searchpath + "/?.lua\"";
			// setSearchPathLua += "package.path = package.path..\";" + searchpath + "/?.lc\"";
			// setSearchPathLua += "package.path = package.path..\";" + searchpath + "/?.luac\"";
			// setSearchPathLua += "package.path = package.path..\";" + searchpath + "/?/init.lua\";";
			// setSearchPathLua += "package.path = package.path..\";" + searchpath + "/?/init.lc\"";
			// setSearchPathLua += "package.path = package.path..\";" + searchpath + "/?/init.luac\"";
			// this.LuaEnv.DoString (setSearchPathLua);
			// ------------------------------------------------------------------------------
			StringBuilder luaPakagePathSB = new StringBuilder ();
			foreach (var item in LuaExtentions) {
				string itrText = "package.path = package.path..\";{0}/{1}\"".FormatWith (searchpath, item);
				luaPakagePathSB.Append (itrText);
			}
			this.LuaEnv.DoString (luaPakagePathSB.ToString ());
		}

		public void RemoveSearchPath (string searchPath) {
			//TODO:不实现，实现的逻辑有一点不好理解
		}

		private byte[] LoadEnctyptionLua (ref string filepath) {
			return this.CustomLoaderMgr.Load (ref filepath);
		}

		public void Dispose () {
			if (LuaEnv != null) {
				LuaEnv.Dispose ();
				LuaEnv = null;
			}
		}
	}

	public abstract class SGFLuaSafety : MonoBehaviour {

		protected byte[] Key { get; private set; }

		public SGFLuaSafety (byte[] key) {
			this.Key = key;
		}

		public SGFLuaSafety (string key) {
			this.Key = System.Text.ASCIIEncoding.UTF8.GetBytes (key);
		}

		abstract public byte[] Encryption (byte[] dCode);

		abstract public byte[] Decryption (byte[] eCode);
	}

	public class SGFLuaDesEncryption : SGFLuaSafety {

		private SGF.Security.DesEncryption des;

		public SGFLuaDesEncryption (byte[] key) : base (key) {
			SGF.Security.DesEncryption.CheckKeyArgumentWithException (key);
			des = new SGF.Security.DesEncryption ();
		}

		public override byte[] Decryption (byte[] eCode) {
			return des.Decryption (eCode, this.Key);
		}

		public override byte[] Encryption (byte[] dCode) {
			return des.Encryption (dCode, key : this.Key);
		}
	}

	public interface ISGFLuaLoader {
		byte[] Load (ref string fname);
	}

	public sealed class SGFLuaCustomLoaderManager : ISGFLuaLoader {

		private List<ISGFLuaLoader> loaders = new List<ISGFLuaLoader> ();

		public byte[] Load (ref string fname) {
			foreach (var loader in loaders) {
				byte[] bytes = loader.Load (ref fname);
				if (bytes == null || bytes.Length == 0) {
					continue;
				}
				return bytes;
			}
			return null;
		}
	}

	[XLua.CSharpCallLua]
	public delegate void LuaTimeTick (int timespan);

	public sealed class LuaTick {

		private SGFLua SGFLua;
		private System.DateTime preDoTime;
		private bool isRunning = true;

		public LuaTick (SGFLua lua) {
			this.SGFLua = lua;
		}

		public void DoTick () {
			if (isRunning == false) return;
			var current = System.DateTime.Now;
			if (preDoTime != null) {
				var luaDoTick = this.SGFLua.LuaEnv.Global.Get<string, LuaTimeTick> ("LuaTimeTick");
				if (luaDoTick == null) throw new LuaException ("not set global lua function 'LuaTimeTick' to supports timer tickor!");
				var timespan = current - preDoTime;
				luaDoTick.Invoke (timespan.Milliseconds);
			}
			this.preDoTime = current;
		}

		public void Stop () {
			this.isRunning = false;
		}
	}
}