using UnityEngine;

namespace SGF.Unity.Utils  {
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
                return null;
            }

            return findTrans.gameObject;
        }
    }
}