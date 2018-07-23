namespace SGF {
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;
    namespace Unity {
        public class GetBytesAsyArgs {
            public byte[] bytes { get; private set; }

            public string error { get; private set; }

            public GetBytesAsyArgs (byte[] data, string error = null) {
                this.bytes = data;
                this.error = error;
            }
        }

        public class ResIOException : System.Exception {
            public ResIOException (string path) : base ("Read Res Error:" + path) {

            }

            public ResIOException (string path, string error) : base (error + "," + path) {

            }
        }

        public delegate void GetBytesAsyAction (object sender, GetBytesAsyArgs args);

        public interface IResLoader {
            byte[] GetBytesSys (string fpat);
            void GetBytesAsy (string fpat, GetBytesAsyAction oncomplate);
        }

        /// <summary>
        /// unity Stream asserts  loader.
        /// </summary>
        public class UnityStreamAssetsLoader : IResLoader {

            //android:"jar:file://" + Application.dataPath + "!/assets/" + fpat
            //ios: Application.dataPath + "/Raw" + fpat
            //desktop:Application.dataPath + "/StreamingAssets" + fpat
            public ResMgr ResMgr { get; private set; }

            public UnityStreamAssetsLoader (ResMgr resMgr) {
                this.ResMgr = resMgr;
            }

            /// <summary>
            /// 同步读取StreamAssets
            /// </summary>
            /// <param name="fpat"></param>
            /// <returns></returns>
            public byte[] GetBytesSys (string fpat) {
                fpat = Unity.Path.GetTargetPlatformPath (Unity.Path.GetTargetPathLeag);
                if (fpat.StartsWith (Unity.Path.GetTargetPathLeag) == false) {
                    fpat = Unity.Path.GetTargetPathLeag + fpat;
                }

                fpat = UnityEngine.Application.streamingAssetsPath + fpat;
                byte[] bytes = null;
                if (Application.platform == RuntimePlatform.Android) {
                    using (WWW www = new WWW (fpat)) {
                        while (!www.isDone) { }
                        if (www.error != null) throw new ResIOException (www.error, fpat);
                        bytes = www.bytes;
                    }
                } else {
                    bytes = System.IO.File.ReadAllBytes (fpat);
                }
                return bytes;
            }

            public void GetBytesAsy (string fpat, GetBytesAsyAction oncomplate) {
                this.ResMgr.MonoBehaviour.StartCoroutine (this.loadRes (fpat, oncomplate));
            }

            private IEnumerator loadRes (string fpat, GetBytesAsyAction oncomplate) {
                byte[] data = null;
                string error = null;
                try {
                    data = this.GetBytesSys (fpat);
                } catch (System.Exception ex) {
                    error = ex.ToString ();
                }
                yield return null;
                oncomplate.Invoke (this, new GetBytesAsyArgs (data, error));
            }
        }

        /// <summary>
        /// http request res loader.
        /// </summary>
        public class HttpResLoader : IResLoader {

            public ResMgr ResMgr { get; private set; }

            public HttpResLoader (ResMgr resMgr) {
                this.ResMgr = resMgr;
            }

            public string URLRoot {
                get;
                set;
            }

            public void GetBytesAsy (string fpat, GetBytesAsyAction oncomplate) {
                throw new System.NotImplementedException ();
            }

            public byte[] GetBytesSys (string fpat) {
                throw new System.NotImplementedException ();
            }
        }

        /// <summary>
        /// mobile document res loader.
        /// </summary>
        public class DocumentResLoader : IResLoader {
            public DocumentResLoader (ResMgr resMgr) {

            }
            public void GetBytesAsy (string fpat, GetBytesAsyAction oncomplate) {
                throw new System.NotImplementedException ();
            }

            public byte[] GetBytesSys (string fpat) {
                throw new System.NotImplementedException ();
            }
        }

        public sealed class ResMgr {
            public List<IResLoader> sortLoader;
            public static ResMgr instance;

            public UnityEngine.MonoBehaviour MonoBehaviour {
                get;
                private set;
            }

            public ResMgr (UnityEngine.MonoBehaviour beh) {
                sortLoader = new List<IResLoader> ();
                instance.AddLoader (new DocumentResLoader (this));
                instance.AddLoader (new UnityStreamAssetsLoader (this));
                instance.AddLoader (new HttpResLoader (this));
            }

            public string RemoteResUrlRoot {
                get {
                    IResLoader loader = this.GetLoader<HttpResLoader> ();
                    if (loader != null && loader is HttpResLoader) {
                        HttpResLoader httpResLoader = loader as HttpResLoader;
                        return httpResLoader.URLRoot;
                    }
                    return null;
                }

                set {
                    IResLoader loader = this.GetLoader<HttpResLoader> ();
                    if (loader != null && loader is HttpResLoader) {
                        HttpResLoader httpResLoader = loader as HttpResLoader;
                        httpResLoader.URLRoot = value;
                    }
                }
            }

            public void AddLoader (IResLoader loader) {
                this.sortLoader.Add (loader);
            }

            public void AddLoaderAt (IResLoader loader, int index) {
                this.sortLoader.Insert (index, loader);
            }

            private IResLoader GetLoader<T> () where T : IResLoader {
                IResLoader loader = this.sortLoader.Find (x => x is UnityStreamAssetsLoader);
                if (loader == null) return null;
                return loader;
            }

            private byte[] GetResBytes<T> (string pat) where T : IResLoader {
                IResLoader loader = this.GetLoader<T> ();
                if (loader == null) return null;
                return loader.GetBytesSys (pat);
            }

            public byte[] GetFromUnityStreamAssert (string pat) {
                pat = GetTargetPlatformPath (pat);
                return this.GetResBytes<UnityStreamAssetsLoader> (pat);
            }

            public byte[] GetRemoteResWithHttp (string pat) {
                return this.GetResBytes<HttpResLoader> (pat);
            }

            public byte[] GetDocumentRes (string pat) {
                pat = GetTargetPlatformPath (pat);
                return this.GetResBytes<DocumentResLoader> (pat);
            }

            public byte[] GetRes (string pat) {
                var bytes = this.GetDocumentRes (pat);
                if (bytes == null) bytes = this.GetFromUnityStreamAssert (pat);
                if (bytes != null) bytes = this.GetRemoteResWithHttp (pat);

                return bytes;
            }

            private static string GetTargetPlatformPath (string pat) {
                return Unity.Path.GetTargetPlatformPath (pat);
            }
        }
    }
}