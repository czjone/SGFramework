using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// EventTrigger trigger = transform.gameObject.GetComponent<EventTrigger> ();
		// if (trigger == null)
		// 	trigger = transform.gameObject.AddComponent<EventTrigger> ();

		// // 实例化delegates
		// trigger.triggers = new List<EventTrigger.Entry> ();

		// EventTrigger.Entry entry = new EventTrigger.Entry ();
		// entry.eventID = EventTriggerType.Drag;
		// entry.callback = new EventTrigger.TriggerEvent ();
		// UnityAction<BaseEventData> callback = new UnityAction<BaseEventData> (XLua => {
		// 	Debug.Log("ssssssssss1");
		// });
		// entry.callback.AddListener (callback);
		// trigger.triggers.Add (entry);

		// // UnityEngine.U2D.SpriteAtlas spriteAtlas = Resources.Load<UnityEngine.U2D.SpriteAtlas>("ssssss");
		// UnityEngine.U2D.SpriteAtlas spriteAtlas = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.U2D.SpriteAtlas >("Assets/FZSG/dres/res/Arts/UI/Atlas/ShharedUI.spriteatlas");
		// if(spriteAtlas == null){
		// 	Debug.Log("没有找到相就的资源!");
		// }else {
		// 	int counts = spriteAtlas.spriteCount;
		// 	Sprite[] sprites = new Sprite[counts];
		// 	int loadCount = spriteAtlas.GetSprites(sprites);
		// 	Debug.Log(loadCount);
		// }
	}

	// Update is called once per frame
	void Update () {

	}
}