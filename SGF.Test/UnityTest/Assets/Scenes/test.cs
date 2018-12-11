using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour {

	public Text text;

	// Use this for initialization
	void Start () {
		Application.RegisterLogCallback (LogCallback);
	}

	void LogCallback (string condition, string stackTrace, LogType type) {
		switch(type) {
			case LogType.Assert: 
			text.text += string.Format("<color=#00FF00> -> </color> {0}<color=#00FF00>{1} </color>",condition,stackTrace);
			break;
			case LogType.Error: 
			text.text += string.Format("<color=#00FF00> -> </color> {0}<color=#FF0000>{1} </color>",condition,stackTrace);
			break;
			case LogType.Exception: 
			text.text += string.Format("<color=#00FF00> -> </color> {0}<color=#FF0000>{1} </color>",condition,stackTrace);
			break;
			case LogType.Log: 
			text.text += string.Format("<color=#00FF00> -> </color> {0}<color=#00FF00>{1} </color>",condition,stackTrace);
			break;
			case LogType.Warning: 
			text.text += string.Format("<color=#00FF00> -> </color> {0}<color=#00FF00>{1} </color>",condition,stackTrace);
			break;
		}
		
	}

	public void Out () {
		Debug.Log ("Debug message!");
		Debug.Assert(false,"Asert message!");
		Debug.LogError("Error message!");
		Debug.LogWarning("Warning message!");
		Debug.LogException(new System.Exception("Exception message!"));
	}

	// Update is called once per frame
	void Update () {

	}
}