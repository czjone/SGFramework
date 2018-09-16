namespace SGF.Unity.UI {
    using UnityEngine;

    /// <summary>
    /// Event type
    /// </summary>
    public enum EventType {
        ON_DOWN,
        ON_UP,
        ON_DRAG,
    }

    public class UIHelper {

        /// <summary>
        /// Deep to find child nodes
        /// </summary>
        /// <param name="go">root node</param>
        /// <param name="name">find child with name</param>
        /// <returns>find result node transform</returns>
        public Transform FindChildTransformDeep (GameObject go, string name) {
            Transform resultTrs = null;
            resultTrs = go.transform.Find (name);
            if (resultTrs == null) {
                foreach (Transform trs in go.transform) {
                    resultTrs = FindChildTransformDeep (trs.gameObject, name);
                    if (resultTrs != null)
                        return resultTrs;
                }
            }
            return resultTrs;
        }

        /// <summary>
        /// find game object transform with target name.
        /// </summary>
        /// <param name="go">root node</param>
        /// <param name="name">target name.</param>
        /// <param name="deep">need deep find?</param>
        /// <returns>find result game object transform</returns>
        public Transform FindChildTransform (GameObject go, string name, bool deep = true) {
            if (deep == false) return go.transform.Find (name);
            else return FindChildTransformDeep (go, name);
        }

        /// <summary>
        /// find game object with target name.
        /// </summary>
        /// <param name="go">root node</param>
        /// <param name="name">target name.</param>
        /// <param name="deep">need deep find?</param>
        /// <returns>find result game object</returns>
        public GameObject FindChild (GameObject go, string name, bool deep = true) {
            var tras = FindChild (go, name, deep);
            if (tras == null) return null;
            return tras.gameObject;
        }

        // /// <summary>
        // /// add evnet to game object.
        // /// </summary>
        // /// <param name="go">game object</param>
        // /// <param name="action">ui action</param>
        // /// <param name="etype">event type</param>
        // public void AddEvent (GameObject go, UIAction action, EventType etype = EventType.ON_UP) {
        //     //TODO: add event process.
        // }

   
        // public GameObject LoadGameObjectWithPrefab(string name){
        //     var go = GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/FZSG/FZSGRes/res/wins/Main.prefab"));
        //     // Debug.Log(go);
        //     // go.transform.SetParent(UIRoot.transform,false);
        //     return go;
        // }
    }
}