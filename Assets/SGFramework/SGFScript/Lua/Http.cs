namespace SGF.Lua {
    using System.Collections.Generic;
    using System.Collections;
    using SGF.Core;
    using UnityEngine.Networking;
    using UnityEngine;

    [XLua.LuaCallCSharp]
    public class HttpResult {
        public virtual int Ret { get; internal set; }
        public virtual string Msg { get; internal set; }
        public virtual int DataLength { get; internal set; }
        public virtual byte[] Data { get; internal set; }
    }

    [XLua.CSharpCallLua]
    public delegate void OnHttpResult (HttpResult data);

    [XLua.LuaCallCSharp]
    public sealed class Http {
        public enum Method {
            GET = 0,
            POST = 1,
        }

        public string URL { get; private set; }

        public Method HttpMethod { get; private set; }

        private OnHttpResult OnResultAction;
        private Game game;
        private Dictionary<string, string> sendData;
        public MainRootBehaviour UnityBehaviour {
            get {
                return this.game.RootBehaviour;
            }
        }

        public Http (Game game, string url, OnHttpResult onHttpResult, Method method = Method.GET) {
            this.game = game;
            this.URL = url;
            this.HttpMethod = method;
            this.OnResultAction = onHttpResult;
        }

        public Http (Game game, string url, Method method = Method.GET):
            this (game, url, null, method) {

            }

        public void SendAsy (Dictionary<string, string> data = null) {
            this.sendData = data;
            if (this.HttpMethod == Method.GET) {
                UnityBehaviour.StartCoroutine (this.GetRequeset ());
            } else if (this.HttpMethod == Method.POST) {
                UnityBehaviour.StartCoroutine (this.PostRequeset ());
            }
        }

        private IEnumerator PostRequeset () {
            WWWForm form = this.MakePostData ();
            UnityWebRequest www = UnityWebRequest.Post (this.URL, form);
            yield return www.SendWebRequest ();
            this.OnHttpResult (www);
        }

        private IEnumerator GetRequeset () {
            string url = this.MakeGetURL ();
            UnityWebRequest www = UnityWebRequest.Get (url);
            yield return www.SendWebRequest ();
            this.OnHttpResult (www);
        }

        private void OnHttpResult (UnityWebRequest www) {
            HttpResult ret = new HttpResult ();
            if (www.isNetworkError || www.isHttpError) {
                SGF.Unity.ULog.D ("request url:{0}".FormatWith (www.url));
                SGF.Unity.ULog.D (www.error);
                ret.Ret = -1;
                ret.Msg = www.error;
            } else {
                ret.Ret = 0;
                ret.Data = www.downloadHandler.data;
                ret.DataLength = ret.Data.Length;
                SGF.Unity.ULog.D ("download (size:{0} url:{1})".FormatWith (ret.Data.Length, www.url));
            }
            if (this.OnResultAction != null) {
                this.OnResultAction.Invoke (ret);
            }

        }

        private string MakeGetURL () {
            string url = this.URL;
            if (this.sendData == null || this.sendData.Count == 0) return url;
            if (this.sendData.Count > 0) {
                if (url.Contains ("?") == false) {
                    url += "?";
                }
                foreach (var item in this.sendData) {
                    if (url.EndsWith ("&") == false) {
                        url += "&";
                    }
                    url += (item.Key + "=" + item.Value);
                }
            }
            return url;
        }

        private WWWForm MakePostData () {
            WWWForm form = new WWWForm ();
            if (this.sendData == null || this.sendData.Count == 0) return form;
            foreach (var item in this.sendData) {
                form.AddField (item.Key, item.Value);
            }
            return form;
        }
    }
}