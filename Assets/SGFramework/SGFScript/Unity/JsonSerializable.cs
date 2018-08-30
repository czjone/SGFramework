namespace SGF.Unity {

    using UnityEngine;

    public class JsonSerializable<T> where T : class, new () {
        public static T LoadWithJson (string json) {
            return JsonUtility.FromJson<T> (json);
        }

        public static T LoadWithFile (string fname) {
            string json = System.IO.File.ReadAllText (fname);
            return JsonUtility.FromJson<T> (json);
        }

        public string ToJson () {
            return JsonUtility.ToJson (this); 
        }
        
        public void ToFile (string fname) {
            string json = this.ToJson ();
            System.IO.File.WriteAllText (fname, json);
        }
    }
}