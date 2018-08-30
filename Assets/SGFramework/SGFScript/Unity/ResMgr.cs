namespace SGF.Unity {

	using System.Collections.Generic;
	using System.Collections;
	using SGF.Core;
	using UnityEngine;

	public class ResMgr : System.IDisposable {

		// public List<string> SearchPath { get; private set; }

		public ResMgr () {

		}

		public byte[] GetStreamAssetsBytes (string fname) {
			string path = Application.streamingAssetsPath + Path.DirSplitor + Path.Legalization (fname);
			if (Application.platform == RuntimePlatform.Android) {
				using (WWW www = new WWW (path)) {
					while (!www.isDone) { }
					if (www.error == null) {
						return www.bytes;
					} else {
						Debug.LogError (www.error + "|" + fname);
						return null;
					}
				}

			} else {
				if (!checkFileExist (path)) return null;
				return System.IO.File.ReadAllBytes (path);
			}
		}

		public string GetStreamAssetsString (string fname) {
			string path = Application.streamingAssetsPath + Path.DirSplitor + Path.Legalization (fname);
			if (Application.platform == RuntimePlatform.Android) {
				using (WWW www = new WWW (path)) {
					while (!www.isDone) { }
					if (www.error == null) {
						return www.text;
					} else {
						Debug.LogError (www.error + "|" + fname);
						return null;
					}
				}

			} else {
				if (!checkFileExist (path)) return null;
				return System.IO.File.ReadAllText (path);
			}
		}

		public byte[] GetPersistentDataBytes (string fname) {
			string path = Application.persistentDataPath + Path.DirSplitor + Path.Legalization (fname);
			if (!checkFileExist (path)) return null;
			return System.IO.File.ReadAllBytes (path);
		}

		public string GetPersistentDataString (string fname) {
			string path = Application.persistentDataPath + Path.DirSplitor + Path.Legalization (fname);
			if (!checkFileExist (path)) return null;
			return System.IO.File.ReadAllText (path);
		}

		public void WritePersistentDataString (string fname, string str) {
			string path = Application.persistentDataPath + Path.DirSplitor + Path.Legalization (fname);
			System.IO.File.WriteAllText (path, str);
		}

		public void WritePersistentDataBytes (string fname, byte[] bytes) {
			string path = Application.persistentDataPath + Path.DirSplitor + Path.Legalization (fname);
			string dir = path.Substring (0, path.Length - System.IO.Path.GetFileName (path).Length);
			System.IO.Directory.CreateDirectory (dir);
			System.IO.File.WriteAllBytes (path, bytes);
		}

		public AssetBundle GetAssetsBundleWithBytes (byte[] buf) {
			AssetBundle ab;
			using (var stream = new System.IO.MemoryStream (buf)) {
				ab = AssetBundle.LoadFromStream (stream);
				stream.Close ();
			}
			return ab;
		}

		public AssetBundle GetAssetsBundleFromStreamAssets (string fname) {
			byte[] buf = GetStreamAssetsBytes (fname);
			return GetAssetsBundleWithBytes (buf);
		}

		public AssetBundle GetAssetsBundleFromPersistentData (string fname) {
			byte[] buf = GetPersistentDataBytes (fname);
			return GetAssetsBundleWithBytes (buf);
		}

		public string GetString (string path) {
			return this.GetPersistentDataString (path);
		}

		private bool checkFileExist (string path) {
			if (System.IO.File.Exists (path) == false) {
				Debug.Log ("file not found:" + path);
				return false;
			} else return true;
		}

		public void Dispose () {
			//TODO:: dispose resources.
		}
	}
}