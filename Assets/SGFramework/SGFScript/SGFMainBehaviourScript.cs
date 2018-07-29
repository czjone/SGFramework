namespace SGF {

	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;

	[XLua.LuaCallCSharp]
	public class SGFMainBehaviourScript : MonoBehaviour {
		public SGFManager Manager { get; private set; }

		private readonly string UIRootName = "UIMainCamera";

		private void Awake () {
			this.Manager = new SGFManager (this);
			this.gameObject.name = UIRootName;
		}

		// Use this for initialization
		void Start () {
			this.Manager.Lua.AddLuaSearchPath (Application.persistentDataPath);
			var downloadDir = Application.persistentDataPath + Core.Path.DirSplitor + HotUpdate.TaskDecompressionPKGRes.DresFolder;
			this.Manager.Lua.AddLuaSearchPath (downloadDir);
			this.Manager.Lua.AddLuaSearchPath (downloadDir + "/src");
			this.Manager.Lua.AddLuaSearchPath (downloadDir + "/res");
			this.Manager.Lua.RunLuaModel ("src.main");
		}

		// Update is called once per frame
		void Update () {

		}

		private void OnDestroy () {
			this.Manager.Dispose ();
		}
	}

}