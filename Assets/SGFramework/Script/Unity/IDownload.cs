namespace SGF
{
    namespace Unity
    {
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;

        /// <summary>
        /// 下载完成事件委托
        /// </summary>
        public delegate void DownLoadComplateAction();

        /// <summary>
        /// 下载出现错误的事件委托
        /// </summary>
        public delegate void DownLoadErrorAction();

        public enum HttpMethod
        {
            GET,
            POST,
        }

        public class HttpResponse
        {

        }

        public class HttpParam
        {
            public string Key { get; private set; }

            public string Value { get; private set; }

            public HttpParam(string key, string val)
            {
                this.Key = key;
                this.Value = Value;
            }
        }

        public interface IDownload
        {
            /// <summary>
            /// 下载完成事件
            /// </summary>
            event DownLoadComplateAction OnComplate;

            /// <summary>
            /// 下载异常时间
            /// </summary>
            event DownLoadErrorAction OnError;

            HttpMethod Method { get; set; }

            HttpResponse GetResponse();

            void SetRequestData(params HttpParam[] httpParams);

            void SetRequestData(List<HttpParam> httpParams);

            /// <summary>
            /// 同步请求
            /// </summary>
            void Request();

            /// <summary>
            /// 异步请求
            /// </summary>
            void RequestAsy();
        }
    }
}