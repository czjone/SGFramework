namespace SGF
{
    namespace Unity
    {
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;

        /// <summary>
        /// 下载器
        /// </summary>
        public class Downloader : IDownload
        {
            private IDownload services;

            public event DownLoadComplateAction OnComplate;

            public event DownLoadErrorAction OnError;

            public Downloader(string tagPath)
            {
                var checkPath = tagPath.ToLower();
                if (checkPath.StartsWith("http://") == true ||
                    checkPath.StartsWith("https://") == true)
                {
                    services = new HttpDownloader(tagPath);
                }
                
               
            }

            public void Request()
            {
                services.Request();
            }

            public void RequestAsy()
            {
                services.RequestAsy();
            }


            public HttpMethod Method
            {
                get
                {
                   return services.Method;
                }
                set
                {
                    services.Method = value;
                }
            }

            public HttpResponse GetResponse()
            {
                return services.GetResponse();
            }

            public void SetRequestData(params HttpParam[] httpParams)
            {
                services.SetRequestData(httpParams);
            }

            public void SetRequestData(List<HttpParam> httpParams)
            {
                services.SetRequestData(httpParams);
            }
        }
    }
}