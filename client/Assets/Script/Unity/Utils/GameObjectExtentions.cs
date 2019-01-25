using SGF.Core;
using UnityEngine;
using UnityEngine.Events;

namespace SGF.Unity.Utils {

    public static class GameObjectExtentions {
        /// <summary>
        /// 有性能对比： https://www.jianshu.com/p/6726cad6f65a
        /// </summary>
        /// <param name="go"></param>
        /// <param name="fname"></param>
        /// <returns></returns>
        public static GameObject FindChildWithTagName (this GameObject go, string fname) {
            var trans = go.transform;
            var findTrans = trans.Find (fname);
            if (findTrans == null) {
                SGF.Unity.Utils.Logger.PrintWarring ("gameobject 中不包含 {0} 节点".FormatWith (fname));
                return null;
            }

            return findTrans.gameObject;
        }

        public static GameObject SetUIParent (this GameObject obj, GameObject parent) {
            if (obj.GetComponent<CanvasRenderer> () == null || obj.GetComponent<RectTransform> () == null) {
                SGF.Unity.Utils.Logger.PrintError ("obj 不是有效的UGUI对象!");
            }

            if (parent == null) {
                obj.transform.SetParent (null);
                return obj;
            }

            if ((
                    parent.GetComponent<CanvasRenderer> () == null && //ui对象必须包含的
                    parent.GetComponent<Canvas> () == null //Canvas必须包含的
                ) || parent.GetComponent<RectTransform> () == null) {
                SGF.Unity.Utils.Logger.PrintError ("parent 不是有效的UGUI对象!");
            }

            obj.transform.SetParent (parent.transform, false);
            return obj;
        }

        public static GameObject SetClick (this GameObject btn, UnityAction onclick) {
            var btnCom = btn.GetComponent<UnityEngine.UI.Button> ();
            if (btnCom == null) {
                btnCom = btn.AddComponent<UnityEngine.UI.Button> ();
            }
            btnCom.onClick.AddListener (onclick);
            return btn;
        }
    }
}