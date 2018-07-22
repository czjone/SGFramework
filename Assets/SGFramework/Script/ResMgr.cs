
namespace SGF
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface IResLoader
    {
        byte[] GetBytes(string fpat);
    }

    /// <summary>
    /// unity Stream asserts  loader.
    /// </summary>
    public class UnityStreamAssertLoader : IResLoader
    {

        public byte[] GetBytes(string fpat)
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// http request res loader.
    /// </summary>
    public class HttpResLoader : IResLoader
    {
        public string URLRoot
        {
            get;
            set;
        }

        public byte[] GetBytes(string fpat)
        {
            throw new System.NotImplementedException();
        }
    }


    /// <summary>
    /// mobile document res loader.
    /// </summary>
    public class DocumentResLoader : IResLoader
    {

        public byte[] GetBytes(string fpat)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed class ResMgr
    {
        public List<IResLoader> sortLoader;
        public static ResMgr instance;

        public ResMgr()
        {
            sortLoader = new List<IResLoader>();
        }

        public static ResMgr GetInstance()
        {
            if (instance == null)
            {
                instance = new ResMgr();
                instance.AddLoader(new DocumentResLoader());
                instance.AddLoader(new UnityStreamAssertLoader());
                instance.AddLoader(new HttpResLoader());
            }
            return instance;
        }

        public string RemoteResUrlRoot
        {
            get
            {
                IResLoader loader = this.GetLoader<HttpResLoader>();
                if (loader != null && loader is HttpResLoader)
                {
                    HttpResLoader httpResLoader = loader as HttpResLoader;
                    return httpResLoader.URLRoot;
                }
                return null;
            }

            set
            {
                IResLoader loader = this.GetLoader<HttpResLoader>();
                if (loader != null && loader is HttpResLoader)
                {
                    HttpResLoader httpResLoader = loader as HttpResLoader;
                    httpResLoader.URLRoot = value;
                }
            }
        }

        public void AddLoader(IResLoader loader)
        {
            this.sortLoader.Add(loader);
        }

        public void AddLoaderAt(IResLoader loader, int index)
        {
            this.sortLoader.Insert(index, loader);
        }

        private IResLoader GetLoader<T>() where T : IResLoader
        {
            IResLoader loader = this.sortLoader.Find(x => x is UnityStreamAssertLoader);
            if (loader == null) return null;
            return loader;
        }

        private byte[] GetResBytes<T>(string pat) where T : IResLoader
        {
            IResLoader loader = this.GetLoader<T>();
            if (loader == null) return null;
            return loader.GetBytes(pat);
        }

        public byte[] GetFromUnityStreamAssert(string pat)
        {
            pat = this.GetTargetPlatformPath(pat);
            return this.GetResBytes<UnityStreamAssertLoader>(pat);
        }

        public byte[] GetRemoteResWithHttp(string pat)
        {
            return this.GetResBytes<HttpResLoader>(pat);
        }

        public byte[] GetDocumentRes(string pat)
        {
            pat = this.GetTargetPlatformPath(pat);
            return this.GetResBytes<DocumentResLoader>(pat);
        }

        public byte[] GetRes(string pat)
        {
            var bytes = this.GetDocumentRes(pat);
            if (bytes == null) bytes = this.GetFromUnityStreamAssert(pat);
            if (bytes != null) bytes = this.GetRemoteResWithHttp(pat);

            return bytes;
        }

        public string GetTargetPlatformPath(string pat)
        {
            RuntimePlatform runPlayer = Application.platform;
            if (runPlayer == RuntimePlatform.WindowsEditor ||
                runPlayer == RuntimePlatform.WindowsPlayer)
            {
                return pat.Replace("/", "\\");
            }
            else
            {
                return pat.Replace("\\", "/");
            }
        }
    }
}