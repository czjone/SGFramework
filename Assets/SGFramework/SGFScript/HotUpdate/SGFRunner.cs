namespace SGF {
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine.SceneManagement;
	using UnityEngine.UI;
	using UnityEngine;

	public class SGFRunner : MonoBehaviour {

		private SGFRunnerTaskMgr taskMgr;
		public Text RunnerStateText;
		public string MainSceneName;

		private void Awake () {
			taskMgr = new SGFRunnerTaskMgr (this);
			taskMgr.OnStateChangedEvent += this.OnStateChanged;
			taskMgr.OnComplateEvent += this.OnComplate;
		}
		void Start () {
			taskMgr.StartAsy ();
		}

		void Update () {
			taskMgr.OnUnityUpdate ();
		}

		private void OnComplate (object sender) {
			if (!string.IsNullOrEmpty (this.MainSceneName)) {
				SceneManager.LoadSceneAsync (this.MainSceneName);
				SceneManager.UnloadSceneAsync (SceneManager.GetActiveScene ().name);
			}
		}

		private void OnStateChanged (object sender, RunnerStateChangedEventArgs args) {
			if (RunnerStateText != null) {
				RunnerStateText.text = args.Des;
			}
		}
	}
}