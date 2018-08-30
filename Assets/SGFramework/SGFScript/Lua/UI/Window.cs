namespace SGF.Lua.UI {

    using System.Collections.Generic;
    using UnityEngine;

    [XLua.LuaCallCSharp]
    [XLua.CSharpCallLua]
    public delegate void OnShowComplate ();

    [XLua.LuaCallCSharp]
    [XLua.CSharpCallLua]
    public delegate void OnCloseComplate ();

    public interface IWindow {

        string Name { get; }

        /// <summary>
        /// Window weight
        /// </summary>
        /// <value></value>
        WinsWeight Weight { get; }
        /// <summary>
        /// on ui inited todo.
        /// </summary>
        void OnUIInited ();
        /// <summary>
        /// show window when window ready.
        /// </summary>
        void Show ();
        /// <summary>
        /// close windows
        /// </summary>
        void Close ();
        /// <summary>
        /// set window hide.
        /// </summary>
        void Hide ();
        /// <summary>
        /// set windows weight.
        /// </summary>
        /// <param name="weigth"></param>
        void SetWeight (int weigth);
    }

    public enum WinsWeight {
        /// <summary>
        /// default window
        /// </summary>
        DEFAULT = 0,
        /// <summary>
        /// Child main window.
        /// </summary>
        CHILD_MAIN = 1,
        /// <summary>
        /// Props win
        /// </summary>
        PROPS = 999,
    }

    [XLua.LuaCallCSharp]
    public class PrefabWindow : IWindow {

        public PrefabWindow (string name) {
            this.Weight = WinsWeight.DEFAULT;
            this.Name = name;
        }

        public WinsWeight Weight {
            get;
            private set;
        }

        public string Name {
            get;
            private set;
        }

        public static IWindow LoadWindow (string prefab) {
            PrefabWindow wins = new PrefabWindow (prefab);
            return wins;
        }

        public void Close () {

        }

        public void Hide () {
            throw new System.NotImplementedException ();
        }

        public void OnUIInited () {
            throw new System.NotImplementedException ();
        }

        public void SetWeight (int weigth) {
            throw new System.NotImplementedException ();
        }

        public void Show () {
            this.ShowAsyProcess (() => Debug.Log ());
        }

        virtual protected void ShowAsyProcess (OnShowComplate callback) {
            if (callback != null) callback.Invoke ();
        }

        virtual protected void CloseAsyProcess (OnCloseComplate callback) {
            if (callback != null) callback.Invoke ();
        }
    }

    [XLua.LuaCallCSharp]
    public sealed class WinMgr {

        protected Queue<IWindow> Wins { get; private set; }

        public void Open (string windName) {

        }
        public void Close (IWindow win) {

        }
    }
}