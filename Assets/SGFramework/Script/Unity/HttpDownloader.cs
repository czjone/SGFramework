namespace SGF
{
    namespace Unity
    {
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;
        using System.Net;
        using System;

        public class HttpDownloader : IDownload
        {
            
            public event DownLoadComplateAction OnComplate;

            public event DownLoadErrorAction OnError;

            private WebRequest httRequest;

            public HttpDownloader(string urlstr)
            {
                Uri url = new Uri(urlstr);
                httRequest = HttpWebRequest.Create(url);
            }

            public void Request()
            {
                throw new System.NotImplementedException();
            }

            public void RequestAsy()
            {
                throw new System.NotImplementedException();
            }


            public HttpMethod Method
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public HttpResponse GetResponse()
            {
                throw new NotImplementedException();
            }

            public void SetRequestData(params HttpParam[] httpParams)
            {
                throw new NotImplementedException();
            }

            public void SetRequestData(List<HttpParam> httpParams)
            {
                throw new NotImplementedException();
            }
        }
    }
}