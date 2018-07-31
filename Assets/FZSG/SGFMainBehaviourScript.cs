namespace SGF {

	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;

	public class SGFMainBehaviourScript : SGFBaseMainMonoBehaviour {

		private string[] ResChildDir = new string[] {
			"res",
			"src"
		};

		override protected void Awake () {
            base.Awake();
        } 

		void Start () {
			// set dres root.
			string appResRoot = 
		#if UNITY_EDITOR
			Application.dataPath +  Core.Path.DirSplitor + "FZSG/FZSGRes";
		#else
			Application.persistentDataPath + Core.Path.DirSplitor + HotUpdate.TaskDecompressionPKGRes.DresFolder;
		#endif
			//add lua search path.
			this.Manager.Lua.AddLuaSearchPath (appResRoot);
			foreach(var item in this.ResChildDir) {
				this.Manager.Lua.AddLuaSearchPath (appResRoot + Core.Path.DirSplitor +"src");
			}
			//run main lua.
			this.Manager.Lua.RunLuaModel ("src.main");
			SGF.Unity.ULog.D("Screen width:",Screen.width,"Screen height:", Screen.height);
		}

		void Update () {

		}

	}

}