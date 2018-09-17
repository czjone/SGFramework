using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		EventTrigger trigger = transform.gameObject.GetComponent<EventTrigger> ();
		if (trigger == null)
			trigger = transform.gameObject.AddComponent<EventTrigger> ();

		// 实例化delegates
		trigger.triggers = new List<EventTrigger.Entry> ();

		EventTrigger.Entry entry = new EventTrigger.Entry ();
		entry.eventID = EventTriggerType.Drag;
		entry.callback = new EventTrigger.TriggerEvent ();
		UnityAction<BaseEventData> callback = new UnityAction<BaseEventData> (XLua => {
			Debug.Log("ssssssssss1");
		});
		entry.callback.AddListener (callback);
		trigger.triggers.Add (entry);
	}

	// Update is called once per frame
	void Update () {

	}
}