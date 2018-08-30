namespace SGF.Lua.UI {

    using System.Collections.Generic;
    using UnityEngine;

    public class UIAction {

    }

    public class NotFoundGameObjectException : System.Exception {
        public NotFoundGameObjectException (string id):
            base (string.Format ("not found gameobject with id:{0}", id)) {

            }
    }

    public class PrefabUI : MonoBehaviour {

        public string PrefabUIName { get; private set; }

        private Dictionary<string, GameObject> cacheNamedGameObject;

        /// <summary>
        /// get named game object.
        /// </summary>
        /// <param name="id">target id</param>
        /// <returns>result game object.</returns>
        protected GameObject GetNamedGameObject (string id) {
            // initzation cache.
            if (cacheNamedGameObject == null) {
                cacheNamedGameObject = new Dictionary<string, GameObject> ();
            }

            //select gameobjef from cache.
            if (cacheNamedGameObject.ContainsKey (id)) {
                return cacheNamedGameObject[id];
            }

            // deep find child with gameobject.
            var findGo = UIHelper.FindChild (this.gameObject, id, true);
            if (findGo != null) {
                this.cacheNamedGameObject.Add (id, findGo);
            }
            return findGo;
        }
 
        public PrefabUI (string prefabUI) {
            this.PrefabUIName = prefabUI;
        }

        public void AddEvent (string id, UIAction action, EventType etype) {
            var go = this.GetNamedGameObject (id);
            if (go == null) {
                throw new NotFoundGameObjectException (id);
            }
            UIHelper.AddEvent (go, action, etype);
        }
    
        public void AddClick(string id,UIAction action) {
            //click using event type is on touch up.
            //make sure the animation smooth.
            this.AddEvent(id,action,EventType.ON_UP); 
        }
    }
}