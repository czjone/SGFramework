namespace SGF.Unity {

	using System.Collections.Generic;
	using System.Collections;
	using SGF.Core;
	using UnityEngine;

	public abstract class UnityRes {

	}

	public class AssetBundle {
		public string Name { get; private set; }
		public UnityEngine.AssetBundle AB { get; private set; }
		public AssetBundle (string name, UnityEngine.AssetBundle ab) {
			this.Name = name;
			this.AB = ab;
		}
	}

	public sealed class ResMgr {

		private const bool UsedCache = true;

		public SGF.Core.Dictionary<string, UnityRes> ResCache { get; private set; }

		public SGF.Core.Dictionary<string, AssetBundle> ABCache { get; private set; }

		public ResMgr () {
			this.ResCache = new SGF.Core.Dictionary<string, UnityRes> ();
			this.ABCache = new SGF.Core.Dictionary<string, AssetBundle> ();
		}

		public AssetBundle LoadAssetBundle (string name) {
			return this.GetAssetBundleWithFileName (name, UsedCache);
		}

		public void DestoryAssetBundle (AssetBundle ab) {
			this.DestoryAssetBundle (ab.Name);
		}

		public void DestoryAssetBundle (string key) {
			if (this.ABCache.ContainsKey (key) == false) return;
			var abo = this.ABCache[key] as AssetBundle;
			if (abo == null) return;
			ULog.D ("release asset bundle:", key);
			UnityEngine.AssetBundle.UnloadAllAssetBundles (abo.AB);
		}

		private AssetBundle LoadAssetBundleWithFile (string name) {
			var abo = UnityEngine.AssetBundle.LoadFromFile (name);
			var assetbundle = new AssetBundle (name, abo);
			return assetbundle;
		}

		private AssetBundle GetAssetBundleWithFileName (string name, bool cached = false) {
			if (cached == true) {
				//set cached.
				if (this.ABCache.ContainsKey (name) == true) return this.ABCache[name];
				//load asset bundle.
				var assetbundle = this.LoadAssetBundle (name);
				this.ABCache.SetValue (name, assetbundle);
				return assetbundle;
			} else {
				// load asset bundle.
				var abo = UnityEngine.AssetBundle.LoadFromFile (name);
				var assetbundle = new AssetBundle (name, abo);
				return assetbundle;
			}
		}

		public T LoadAsset<T> (string abName, string name) where T : Object {
			var abo = this.GetAssetBundleWithFileName (abName);
			if (abo != null) {
				T asset = abo.AB.LoadAsset<T> (name);
				if (asset == null) {
					throw new UnityException ("not found asset {0} with asset bundle {1}.".FormatWith (name, abName));
				}
				return asset;
			}
			throw new UnityException ("load assetbundle faild:{0}.".FormatWith (abName));
		}
	}
}