namespace SGF {

	namespace HotUpdate {
		using System.Collections.Generic;
		using System.Collections;
		using UnityEngine;

		[System.Serializable]
		public class FileVersion {
			public string Path { get; set; }
			public string MD5 { get; set; }

			public static bool operator == (FileVersion _this, FileVersion file) {
				return _this.Path == file.Path;
			}

			public static bool operator != (FileVersion _this, FileVersion file) {
				return !(_this == file);
			}
		}

		public enum PlatForm {
			UNSUPPORTS = 0, IOS = 1, ANDROID = 2, WIN = 3, MAC = 4
		}

		[System.Serializable]
		public class ResVersion : SGF.Core.JsonSerializable<ResVersion> {

			public int Version { get; set; }

			public int devVersion { get; set; }

			public PlatForm Platform { get; set; }

			public FileVersion[] ResVer { get; set; }

			public const string VerFileName = "version.json";
		}

		public class TaskCheckRes : IRunnerTask {

			public event RunnerStateChangedAction OnStateChangedEvent;

			public event RunnerComplateAction OnComplateEvent;

			public void StartAsy () {
				throw new System.NotImplementedException ();
			}
		}

		public class TaskDecompressionPKGRes : IRunnerTask {
			public event RunnerStateChangedAction OnStateChangedEvent;
			public event RunnerComplateAction OnComplateEvent;

			public const string DresFolder = "dres";

			private HotUpdate.ResVersion localVer;
			private HotUpdate.ResVersion pkgVer;

			private MonoBehaviour MonoBeh;

			private Language lag;

			public TaskDecompressionPKGRes (MonoBehaviour MonoBeh) {
				this.MonoBeh = MonoBeh;
				this.lag = Language.GetInstance ();
			}

			public void StartAsy () {
				if (this.GetNeedDecompressState () == false) {
					this.TrigerComplateEvent ();
					return;
				}

				this.MonoBeh.StartCoroutine (TaskProcessor ());
			}

			private IEnumerator TaskProcessor () {
				var pkgFiles = this.GetNeedDecompressFiles ();
				int index = 0;
				ResMgr resMgr = new ResMgr ();
				RunnerStateChangedEventArgs args = new RunnerStateChangedEventArgs (0);
				string dresRoot = DresFolder + Core.Path.DirSplitor;
				while (index < pkgFiles.Length) {
					var bytes = resMgr.GetStreamAssetsBytes (dresRoot + pkgFiles[index].Path);
					resMgr.WritePersistentDataBytes (dresRoot + pkgFiles[index].Path, bytes);
					Debug.Log ("[Unity Log] Decompress PKG Files:" + dresRoot + pkgFiles[index].Path);
					if (OnStateChangedEvent != null) {
						args.Percent = (index + 1.0f) / (float) pkgFiles.Length;
						args.Des = string.Format (lag.CHECK_DECOMPRESS_PKG_RES_TASK_DES, (args.Percent * 100).ToString ("0.00"));
						OnStateChangedEvent.Invoke (this, args);
					}
					index++;
					yield return null;
				}
				var newVersionStr = this.pkgVer.ToJson ();
				resMgr.WritePersistentDataString (dresRoot + HotUpdate.ResVersion.VerFileName, newVersionStr);
				this.TrigerComplateEvent ();
				yield return null;
			}

			private void TrigerComplateEvent () {
				if (OnComplateEvent != null) {
					this.OnComplateEvent.Invoke (this);
				}
			}

			private bool GetNeedDecompressState () {
				using (ResMgr mgr = new ResMgr ()) {
					var dresroot = DresFolder + Core.Path.DirSplitor;
					string locaJson = mgr.GetPersistentDataString (dresroot + HotUpdate.ResVersion.VerFileName);
					string pkgJson = mgr.GetStreamAssetsString (dresroot + HotUpdate.ResVersion.VerFileName);
					localVer = HotUpdate.ResVersion.LoadWithJson (locaJson ?? "{}");
					localVer.ResVer = localVer.ResVer ?? new HotUpdate.FileVersion[0];
					pkgVer = HotUpdate.ResVersion.LoadWithJson (pkgJson ?? "{}");
					pkgVer.ResVer = pkgVer.ResVer ?? new HotUpdate.FileVersion[0];
					return pkgVer.Version > localVer.Version
#if UNITY_EDITOR
						||
						//pkgVer.devVersion > localVer.devVersion
						true //编辑器模式下强制更行
#endif
					;
				}
			}

			private HotUpdate.FileVersion[] GetNeedDecompressFiles () {
				return pkgVer.ResVer;
			}

		}

		public class TaskDownLoadRes : IRunnerTask {
			public event RunnerStateChangedAction OnStateChangedEvent;
			public event RunnerComplateAction OnComplateEvent;

			public void StartAsy () {
				throw new System.NotImplementedException ();
			}
		}

	}
}