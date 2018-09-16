namespace SGF.Lua.UI {

    using UnityEngine;

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
            // #if UNITY_EDITOR
            var go = GameObject.Instantiate (UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject> (nm));
            return go;
            // #else
            //     return null;
            // #endif
        }

        public GameObject DeepFindChild (GameObject parent, string name) {
            GameObject go = null;
            var trans = DeepFindChild (parent.transform, name);
            if (trans != null) go = trans.gameObject;
            return go;
        }

        private static Transform DeepFindChild (Transform root, string childName) {
            Transform result = null;
            result = root.Find (childName);
            if (result == null) {
                foreach (Transform trs in root) {
                    result = DeepFindChild (trs, childName);
                    if (result != null) {
                        return result;
                    }
                }
            }
            return result;
        }
    }
}