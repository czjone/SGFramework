namespace SGF.Lua.UI {
    using System.Collections.Generic;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine;
    using XLua;

    [XLua.LuaCallCSharp]
    [XLua.CSharpCallLua]
    public delegate void UIVent (BaseEventData data);

    [XLua.LuaCallCSharp]
    public class UIHelper {

        private static UIHelper instance;

        static UIHelper () {
            instance = new UIHelper ();
        }

        public static UIHelper GetInstance () {
            return instance;
        }

        public void SetText (GameObject utxtGo, string val) {
            var txt = utxtGo.GetComponent<UnityEngine.UI.Text> ();
            txt.text = val;
        }

        public void SetVisible (GameObject uGo, bool visible) {
            uGo.SetActive (visible);
        }

        public void SetImage (GameObject uImgGo, UnityEngine.Sprite uImage) {
            var img = uImgGo.GetComponent<UnityEngine.UI.Image> ();
            img.sprite = uImage;
        }

        public void AddChild (GameObject parentGO, GameObject childGO) {
            childGO.transform.SetParent (parentGO.transform, false);
        }

        public void RemoveFromeParent (GameObject childGO, bool destroy = true) {
            childGO.transform.parent = null;
            if (destroy == true) {
                GameObject.Destroy (childGO);
            }
        }

        public GameObject LoadPrefab (string nm) {
            return ResMgr.GetInstance ().LoadPrefab (nm);
        }

        public GameObject DeepFindChild (GameObject parent, string name) {
            GameObject go = null;
            var trans = DeepFindChild (parent.transform, name);
            if (trans != null) go = trans.gameObject;
            return go;
        }

        private Transform DeepFindChild (Transform root, string childName) {
            Transform result = null;
            result = root.Find (childName);
            if (result == null) {
                foreach (Transform trs in root) {
                    result = DeepFindChild (trs, childName);
                    if (result != null)
                        return result;
                }
            }
            return result;
        }

        public void AddEvent (GameObject gameObject, UIVent lua,
            UnityEngine.EventSystems.EventTriggerType type = UnityEngine.EventSystems.EventTriggerType.PointerClick) {

            EventTrigger trigger = gameObject.GetComponent<EventTrigger> ();
            if (trigger == null) trigger = gameObject.AddComponent<EventTrigger> ();
            trigger.triggers = new List<EventTrigger.Entry> ();

            EventTrigger.Entry entry = new EventTrigger.Entry ();
            entry.eventID = type;
            entry.callback = new EventTrigger.TriggerEvent ();
            UnityAction<BaseEventData> callback = new UnityAction<BaseEventData> (lua.Invoke);
            entry.callback.AddListener (callback);

            trigger.triggers.Add (entry);
        }
    }
}