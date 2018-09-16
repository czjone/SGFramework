using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugInfo {
    using System;
    using UnityEngine;

    public interface IInfo {
        void OnInit ();
        void OnUpdate ();
        void OnDestory ();
        void OnGUI ();
    }

    public class FPS : IInfo {

        public float fpsMeasuringDelta = 0.3f;
        private float timePassed;
        private int m_FrameCount = 0;
        private float m_FPS = 0.0f;

        public void OnInit () {
            timePassed = 0.0f;
        }

        public void OnUpdate () {
            m_FrameCount = m_FrameCount + 1;
            timePassed = timePassed + Time.deltaTime;

            if (timePassed > fpsMeasuringDelta) {
                m_FPS = m_FrameCount / timePassed;
                timePassed = 0.0f;
                m_FrameCount = 0;
            }
        }

        public void OnGUI () {
            GUIStyle bb = new GUIStyle ();
            bb.normal.background = null; //这是设置背景填充的
            bb.normal.textColor = new Color (1.0f, 0.5f, 0.0f); //设置字体颜色的
            bb.fontSize = 40; // 当然，这是字体大小
            //居中显示FPS
            GUI.Label (new Rect ((Screen.width / 2) - 40, 0, 200, 200), "FPS: " + String.Format ("{0:F}", m_FPS), bb);
        }

        public void OnDestory () {

        }
    }

    public class Info : MonoBehaviour {

        List<IInfo> infos;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake () {
            infos = new List<IInfo> ();
            infos.Add (new FPS ());
        }

        void Start () {
            infos.ForEach (x => x.OnInit ());
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update () {
            infos.ForEach (x => x.OnUpdate ());
        }

        void OnGUI () {
            infos.ForEach (x => x.OnGUI ());
        }

        void OnDestory () {
            infos.ForEach (x => x.OnDestory ());
        }
    }
}