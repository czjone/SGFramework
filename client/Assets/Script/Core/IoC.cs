using System.Collections;

namespace SGF.Core {
    public sealed class IoC {

        private static Hashtable cache = Hashtable.Synchronized (new Hashtable ());

        public static T Register<T> (T obj) {
            if (obj == null) {
                throw new System.ArgumentNullException ("obj");
            }
            var typename = obj.GetType ().FullName;
            if (cache.ContainsKey (typename)) {
                throw new System.Exception ("don't register the same type instase. typename:{0}".FormatWith (typename));
            }
            cache[typename] = obj;
            return obj;
        }

        public static T Get<T> () {
            var typename = typeof (T).FullName;
            foreach (DictionaryEntry item in cache) {
                if (item.Value is T) {
                    return (T) item.Value;
                }
            }
            return default (T);
        }

        public static void UnRegister<T> () {
            var typename = typeof (T).FullName;
            if (cache.ContainsKey (typename)) {
                cache.Remove (typename);
            }
        }

        public static void UnRegister<T> (T obj) {
            string typename = null;
            foreach (var item in cache) {
                if (item.Equals (typename)) {
                    typename = (string) item;
                    break;
                }
            }

            if (!string.IsNullOrEmpty (typename)) {
                if (cache.ContainsKey (typename)) {
                    cache.Remove (typename);
                }
            }
        }
    }
}