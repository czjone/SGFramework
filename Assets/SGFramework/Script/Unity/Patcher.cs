namespace SGF {
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;

	namespace Patcher {

		/// <summary>
		/// 文件版本
		/// </summary>
		public class FVersion {

			/// <summary>
			/// 读取或设置文件名
			/// </summary>
			/// <value>文件名</value>
			public string FName { get; set; }

			/// <summary>
			/// 读取或设置Hash
			/// </summary>
			/// <value></value>
			public string FHash { get; set; }

			/// <summary>
			/// 读取或设置大小
			/// </summary>
			/// <value></value>
			public long FSize { get; set; }
		}

		/// <summary>
		/// 补丁配置
		/// </summary>
		public class PatcherLocalData {
			/// <summary>
			/// 补丁版本
			/// </summary>
			/// <value></value>
			public long Version { get; set; }

			/// <summary>
			/// 远程补丁Root
			/// </summary>
			/// <value></value>
			public string RemoteHost { get; set; }

			/// <summary>
			/// 本地补丁路径
			/// </summary>
			/// <value></value>
			public string LocalPath { get; set; }

			/// <summary>
			/// 需要下载的文件
			/// </summary>
			/// <value></value>
			public List<FVersion> NeedDownLoad { get; set; }
		}

		public class TaskStateChangeArgs {
			public string TaskName { get; private set; }

			public float Percent { get; private set; }

			public bool IsComplent { get; private set; }

			public int TaskCount { get; private set; }

			public int CurrentTaskIndex { get; private set; }

			private TaskStateChangeArgs (string taskName, float percent) : this (taskName, percent, 1, 1) {

			}

			public TaskStateChangeArgs (string TaskName, float percent, int allTaskCount, int currentTaskIndex) {
				this.TaskName = TaskName;
				this.Percent = percent;
				this.IsComplent = false;
				this.TaskCount = allTaskCount;
				this.CurrentTaskIndex = currentTaskIndex;
			}

			internal void SetPercent (float percent) {
				this.Percent = percent;
			}

			internal void SetIsComplent (bool isComplate = false) {
				this.IsComplent = isComplate;
			}

			internal void SetTaskName (string taskName) {
				this.TaskName = taskName;
			}

			internal void SetTaskCount (int count) {
				this.TaskCount = count;
			}
		}

		public delegate void TaskStateChanged (object sender, TaskStateChangeArgs args);

		public interface IPatcherTask {

			event TaskStateChanged OnTaskStateChanged;

			void Update (int timespine);
		}

		/// <summary>
		/// 检查本地文件与服务器文件的差异性
		/// </summary>
		public class CheckLocalResVersionWhithRemoteVersion : IPatcherTask {
			public event TaskStateChanged OnTaskStateChanged;

			public CheckLocalResVersionWhithRemoteVersion (PatcherLocalData localConfigData) {

			}

			public void Update (int timespine) {
				throw new System.NotImplementedException ();
			}
		}

		/// <summary>
		/// 下载服务器的新版本文件
		/// </summary>
		public class DownloadAllPatches : IPatcherTask {
			public event TaskStateChanged OnTaskStateChanged;

			public DownloadAllPatches (PatcherLocalData localConfigData) {

			}

			public void Update (int timespine) {
				throw new System.NotImplementedException ();
			}
		}

		/// <summary>
		/// 检查下载文件的完整性
		/// </summary>
		public class CheckDownLoad : IPatcherTask {
			public event TaskStateChanged OnTaskStateChanged;

			public CheckDownLoad (PatcherLocalData localConfigData) {

			}

			public void Update (int timespine) {
				throw new System.NotImplementedException ();
			}
		}

		/// <summary>
		/// 检查本地文件的完整性，防止有的文件被地三方工具破坏
		/// </summary>
		public class CheckAllLocalFilesWithHash : IPatcherTask {
			public event TaskStateChanged OnTaskStateChanged;

			public CheckAllLocalFilesWithHash (PatcherLocalData localConfigData) {

			}

			public void Update (int timespine) {
				throw new System.NotImplementedException ();
			}
		}

		public class Patcher {
			private PatcherLocalData localConfigData;
			private static object locker = new object ();
			private System.Threading.Thread runTaskThread;

			private List<IPatcherTask> tasks;
			private bool IsSwitchTask;
			private readonly int TaskCheckTimespane = 20; //ms

			private int RunTaskCunrrentIndex;
			public Patcher (PatcherLocalData localConfigData) {
				this.localConfigData = localConfigData;
				tasks = new List<IPatcherTask> ();

				tasks.Add (new CheckLocalResVersionWhithRemoteVersion (localConfigData));
				tasks.Add (new DownloadAllPatches (localConfigData));
				//tasks.Add (new CheckDownLoad (localConfigData)); //以为检查了所有文件的完整性，不用单独检查下载文件的完整性
				tasks.Add (new CheckAllLocalFilesWithHash (localConfigData));
			}

			public event TaskStateChanged OnTaskStateChanged;

			public event TaskStateChanged OnAllTaskComplate;

			private TaskStateChangeArgs TaskArgsToPatcherArgs (TaskStateChangeArgs args,ref TaskStateChangeArgs onTaskStateChangedArgs) {
				onTaskStateChangedArgs.SetTaskName (args.TaskName);
				onTaskStateChangedArgs.SetTaskCount (args.TaskCount);
				onTaskStateChangedArgs.SetPercent (args.Percent);
				return onTaskStateChangedArgs;
			}

			public void Run () {
				runTaskThread = new System.Threading.Thread (x => {
					IPatcherTask task;
					TaskStateChangeArgs onTaskStateChangedArgs = new TaskStateChangeArgs ("", 0.0f, tasks.Count, RunTaskCunrrentIndex);
					while (tasks.Count > RunTaskCunrrentIndex) {
						task = tasks[RunTaskCunrrentIndex];
						if (this.IsSwitchTask == false) {
							task.OnTaskStateChanged += (sender, args) => {
								var taskOnstateChangeEvent = this.OnTaskStateChanged;
								if (taskOnstateChangeEvent != null) {
									TaskArgsToPatcherArgs(args,ref onTaskStateChangedArgs);
									taskOnstateChangeEvent (this, onTaskStateChangedArgs);
								}
								if (args.IsComplent == true) {
									this.RunTaskCunrrentIndex++;
								}
								if (RunTaskCunrrentIndex >= tasks.Count && this.OnAllTaskComplate != null) {
									TaskArgsToPatcherArgs(args,ref onTaskStateChangedArgs);
									this.OnAllTaskComplate (this, onTaskStateChangedArgs);
								}
							};
							this.IsSwitchTask = true;
						}
						var timespane = this.TaskCheckTimespane;
						task.Update (timespane);
						System.Threading.Thread.Sleep (timespane);
					}
				});
				runTaskThread.Start ();
			}
		}

		/// <summary>
		/// 生成文件夹的补丁
		/// </summary>
		public class PatcherCreator {
			public PatcherCreator(string dir) {

			}
			
		}

		/// <summary>
		/// 补丁检查器，发现新的版本会下载文件
		/// </summary>
		public class PatcherMonoBehaviour : MonoBehaviour {

			private Patcher _Patcher;
			private bool pathcerIsRun;

			private TaskStateChangeArgs taskOnStateChangedArgs;
			private object taskOnChangedSender;

			private bool _isComplate = false;
			public bool IsComplate {
				get { return this._isComplate; } private set {
					if (this._isComplate != value && value == true) {
						this.OnPatcherComplate ();
					}
					this.OnPatcherComplate ();
				}
			}

			public PatcherMonoBehaviour () {
				this.SetPatcher (null);
			}

			private void Start () {

			}

			public void SetPatcher (Patcher patcher) {
				this._Patcher = patcher;
				this.pathcerIsRun = false;
			}

			private void Update () {
				if (this.pathcerIsRun == false && this._Patcher != null) {
					this._Patcher.OnTaskStateChanged += this.OnPatcherStateChangedWithPatherThread;
					this._Patcher.OnAllTaskComplate += this.OnPatcherComplatePatherThread;
					this._Patcher.Run ();
				}
				//实时反馈结果
				this.OnPatcherStateChanged (this.taskOnChangedSender, this.taskOnStateChangedArgs);
			}

			private void OnPatcherStateChangedWithPatherThread (object sender, TaskStateChangeArgs args) {
				this.taskOnChangedSender = sender;
				this.taskOnStateChangedArgs = args;
			}

			private void OnPatcherComplatePatherThread (object sender, TaskStateChangeArgs args) {
				this.taskOnChangedSender = null;
				this.taskOnStateChangedArgs = null;
				this.IsComplate = true;
			}

			virtual protected void OnPatcherStateChanged (object sender, TaskStateChangeArgs args) {
				
			}

			virtual protected void OnPatcherComplate () {

			}
		}
	}
}