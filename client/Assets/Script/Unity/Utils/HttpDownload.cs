namespace SGF.Unity.Utils {
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;
    using System;
    using UnityEngine;

    public class DownloadArgs {
        public string url { get; set; }
        public byte[] data { get; set; }
        public int errorcode { get; set; }

        public bool IsSuccess {
            get {
                return this.errorcode == 0;
            }
        }
    }

    public delegate void OnDownLoadResultAction (DownloadArgs args);

    public class HttpHelper {
        private Game game;

        public HttpHelper (Game game) {
            this.game = game;
        }

        public void DownLoadFileAsy (string url, OnDownLoadResultAction onresult) {
            this.game.StartCoroutine (this.Download (url, onresult));
        }

        private IEnumerator Download (string _url, OnDownLoadResultAction onresult) {  
            DownloadArgs args = new DownloadArgs ();
            args.url = _url;
            Uri url = new Uri (_url);        
            WebRequest request = WebRequest.Create (url);        
            WebResponse response = request.GetResponse ();
            Stream stream = response.GetResponseStream ();                         
            int len = 0; 
            args.data = new byte[response.ContentLength];       
            while (len < args.data.Length) { 
                byte[] data = new byte[1024 * 1024];                
                int _len = stream.Read (args.data, len, data.Length - len);                     
                len += _len;
                yield return new WaitForEndOfFrame ();        
            }  
            if (onresult != null)
                onresult.Invoke (args);                  
        }

        public void DownLoadFileAsy (string url, string outpath) {
            this.game.StartCoroutine (this.Download (url, outpath));
        }

        private IEnumerator Download (string _url, string outpath) {        
            Uri url = new Uri (_url);        
            WebRequest request = WebRequest.Create (url);        
            WebResponse response = request.GetResponse ();
            Stream stream = response.GetResponseStream ();     
            SGF.Core.File.CheckDirWithFile (outpath, true); //检查目录。不存在就创建      
            FileStream file = new FileStream (outpath, FileMode.OpenOrCreate, FileAccess.Write);              
            int max = (int) response.ContentLength;        
            int len = 0;        
            while (len < max) { 
                byte[] data = new byte[10240000];                
                int _len = stream.Read (data, 0, data.Length);           
                file.Write (data, 0, _len);          
                len += _len;
                yield return new WaitForEndOfFrame ();        
            }               
            file.Close ();        
            stream.Close ();      
        }

        private IEnumerator Login (string _url) {
            //设置链接
            Uri url = new Uri (_url);
            //设置http请求
            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create (url);
            request.Method = "POST";
            //表单数据
            byte[] _data = Encoding.UTF8.GetBytes ("account=" + "CarefreeQ" + "&password=" + "CarefreeQ");
            //内容类型
            request.ContentType = "application/x-www-form-urlencoded";
            //内容长度
            request.ContentLength = _data.Length;
            //设置cookie，如要获取则不能为空
            request.CookieContainer = new CookieContainer ();
            //创建流
            Stream stream = request.GetRequestStream ();
            //写入数据
            stream.Write (_data, 0, _data.Length);
            stream.Close ();
            //开始接收响应
            HttpWebResponse response = (HttpWebResponse) request.GetResponse ();
            //获取cookie
            string cookie = request.CookieContainer.GetCookieHeader (url);
            //接收流
            stream = response.GetResponseStream ();
            //内容长度
            int max = (int) response.ContentLength;
            int len = 0;
            //数据长度
            _data = new byte[max];
            while (len < max) {
                //写入响应数据
                int _len = stream.Read (_data, len, _data.Length);
                len += _len;
                yield return new WaitForEndOfFrame ();
            }
            //读取数据
            string text = Encoding.UTF8.GetString (_data);
        }

    }
}