namespace SGF.Unity.Utils  {
    using UnityEngine;

    public class Logger {

        private static string ColorToHex (Color32 color) {
            string hex = color.r.ToString ("X2") + color.g.ToString ("X2") + color.b.ToString ("X2");
            return hex;
        }

        public static void PrintLog (string msg) {
            if (Application.isEditor) {
                Debug.Log ("<color=#" + ColorToHex (Color.white) + ">" + "# " + msg + "</color>");
            }
        }

        public static void PrintError (string msg) {
            if (Application.isEditor) {
                Debug.LogError ("<color=#" + ColorToHex (Color.red) + ">" + "# " + msg + "</color>");
            }
        }

        public static void PrintSuccess (string msg) {
            if (Application.isEditor) {
                Debug.Log ("<color=#" + ColorToHex (Color.green) + ">" + "# " + msg + "</color>");
            }
        }

        public static void PrintFail (string msg) {
            if (Application.isEditor) {
                Debug.Log ("<color=#" + ColorToHex (Color.red) + ">" + "# " + msg + "</color>");
            }
        }

        public static void PrintWarring (string msg) {
            if (Application.isEditor) {
                Debug.Log ("<color=#" + ColorToHex (Color.yellow) + ">" + "# " + msg + "</color>");
            }
        }
    }
}