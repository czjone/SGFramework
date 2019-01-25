using SGF.Core;
using UnityEngine;

namespace SGF.Unity.Utils {
    public static class UITextHelper {

        private static UnityEngine.UI.Text setText (UnityEngine.UI.Text text, string val) {
            if (text == null) {
                Logger.PrintError ("text is null! set value is :{0}".FormatWith (val));
                return null;
            }
            text.text = val;
            return text;
        }

        public static UnityEngine.UI.Text SetText (this UnityEngine.UI.Text text, string val) {
            return setText (text, val);
        }
        
        public static UnityEngine.UI.Text SetText (this GameObject go, string val) {
            var text = go.GetComponent<UnityEngine.UI.Text> ();
            return setText (text, val);
        }

        public static GameObject SetVisible (this GameObject go, bool val) {
            go.SetActive (go);
            return go;
        }

        public static UnityEngine.UI.Text SetVisible (this UnityEngine.UI.Text text, bool val) {
            text.gameObject.SetActive (val);
            return text;
        }

        private static UnityEngine.UI.Slider setSlider (UnityEngine.UI.Slider slider, float val) {
            if (slider == null) {
                Logger.PrintError ("slider is null,cann't set value!");
                return null;
            }
            slider.value = val;
            return slider;
        }

        public static UnityEngine.UI.Slider SetSlider (this UnityEngine.UI.Slider slider, float val) {
            return setSlider (slider, val);
        }

        public static UnityEngine.UI.Slider SetSlider (this GameObject go, float val) {
            var slider = go.GetComponent<UnityEngine.UI.Slider> ();
            if (slider == null) {
                Logger.PrintError ("当前GameObject不包含Slider组件");
                return null;
            }
            return setSlider (slider, val);
        }
    }
}