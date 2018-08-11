namespace SGF.XLuaUI {
	using System.Collections.Generic;
	using UnityEngine;

	public class EventArgs : System.EventArgs {

	}

	public delegate void UIComponeAction (object sender, EventArgs args);

	public interface IWindow {
		int Weight { get; }
		void Show ();
		void Close ();
	}

	public class WinMgr : Queue<IWindow> {

		public WinMgr () {

		}

		/// <summary>
		/// 返回的层级
		/// </summary>
		/// <param name="hierarchy">退出几层，默认返回一层</param>
		public void Return (int hierarchy = -1) {
			int hierarchyCount = Mathf.Abs (hierarchy);
			for (int i = hierarchyCount - 1; i >= 0; i--) {
				if(this.Count == 0) return;
				int weight = this.Peek().Weight;
				while(this.Peek().Weight == weight){
					this.Dequeue();
				}
			}
		}
	}

	public partial class Window : IWindow {

		public static int DefaultWeight = 0;

		private static WinMgr _winMgr = null;

		public WinMgr WinMgr {
			get {
				if (_winMgr == null) {
					_winMgr = new WinMgr ();
				}
				return _winMgr;
			}
		}

		private int _wight = Window.DefaultWeight;

		public int Weight {
			get { return this._wight; }
			set { this._wight = value; }
		}

		public Window () {
			this.WinMgr.Enqueue (this);
		}

		virtual public void Close () {
			this.WinMgr.Dequeue();
		}

		/// <summary>
		/// 返回的层级
		/// </summary>
		/// <param name="hierarchy">退出hierarchy层，默认返回一层</param>
		virtual public void Return (int hierarchy = -1) {
			this.WinMgr.Return(hierarchy);
		}

		virtual public void Show () {

		}
	}
}