namespace SGF {

	using System.Collections.Generic;
    using UnityEngine;

    public class RunnerStateChangedEventArgs : System.EventArgs {
		public string Name { get;  set; }

		/// <summary>
		/// current processed percent.
		/// </summary>
		/// <value>0~1</value>
		public float Percent { get;  set; }

		public string Des { get;  set; }

		public RunnerStateChangedEventArgs (float percent, string Name, string des) {
			this.Name = Name;
			this.Percent = percent;
			this.Des = des;
		}

		public RunnerStateChangedEventArgs (float percent) : this (percent, string.Empty, string.Empty) {

		}

		public RunnerStateChangedEventArgs (float percent, string Name) : this (percent, Name, string.Empty) {

		}

	}

	public delegate void RunnerStateChangedAction (object sender, RunnerStateChangedEventArgs args);

	public delegate void RunnerComplateAction (object sender);

	public interface IRunnerTask {
		event RunnerStateChangedAction OnStateChangedEvent;

		event RunnerComplateAction OnComplateEvent;

		void StartAsy ();
	}

	public class SGFRunnerTaskMgr : IRunnerTask {
		private Queue<IRunnerTask> tasks;

		private IRunnerTask currentTask;

		public event RunnerStateChangedAction OnStateChangedEvent;

		public event RunnerComplateAction OnComplateEvent;

		private static object onStateChanegLocker = new object ();
		private object OnStateChangedSender; //设置一次处理一次。并清理

		private RunnerStateChangedEventArgs OnStateChangedArgs; //设置一次处理一次。并清理

		private static object OnComplateSenderLocker = new object ();
		private object OnComplateSender; //设置一次处理一次。并清理

		public bool isComplate { get; private set; }

		public SGFRunnerTaskMgr (MonoBehaviour mbh) {
			this.tasks = new Queue<IRunnerTask> ();
			this.tasks.Enqueue (new SGF.HotUpdate.TaskDecompressionPKGRes (mbh));
			// this.tasks.Enqueue (new SGF.HotUpdate.TaskCheckRes ());
			// this.tasks.Enqueue (new SGF.HotUpdate.TaskDownLoadRes ());
			// this.tasks.Enqueue (new SGF.HotUpdate.TaskCheckRes ());
		}

		private void RegisterEvent () {
			foreach (var task in tasks) {
				task.OnStateChangedEvent += (object sender, RunnerStateChangedEventArgs args) => {
					lock (onStateChanegLocker) {
						OnStateChangedSender = sender;
						OnStateChangedArgs = args;
					}
				};

				task.OnComplateEvent += this.TrigComplateEvent;
			}
			if (tasks.Count == 0) this.TrigComplateEvent (this);
			else this.currentTask = this.tasks.Dequeue ();
		}

		private void TrigComplateEvent (object sender) {
			if (this.tasks.Count == 0) {
				lock (OnComplateSenderLocker) {
					this.OnComplateSender = sender;
					this.isComplate = true;
				}
			} else {
				this.currentTask = this.tasks.Dequeue ();
				this.currentTask.StartAsy();
			}
		}

		public void StartAsy () {
			this.RegisterEvent ();
			if (!isComplate) this.currentTask.StartAsy ();
		}

		internal void OnUnityUpdate () {
			if (OnStateChangedArgs != null && this.OnStateChangedEvent != null) {
				lock (onStateChanegLocker) {
					if (OnStateChangedArgs != null) {
						this.OnStateChangedEvent.Invoke (OnStateChangedSender, OnStateChangedArgs);
					}
					OnStateChangedSender = null;
					OnStateChangedArgs = null;
				}
			}

			if (OnComplateSender != null && this.OnComplateEvent != null) {
				lock (OnComplateSenderLocker) {
					if (OnComplateSender != null) {
						this.OnComplateEvent (OnComplateSender);
					}
					OnComplateSender = null;
				}
			}
		}
	}
}