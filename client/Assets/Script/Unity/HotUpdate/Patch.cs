namespace SGF.Unity.HotUpdate {
    using System;
    using SGF.Core;
    [Serializable] public class Version : JsonSerializable<Version> {
        /// <summary>
        /// 版本号
        /// </summary>
        public long VersionCode;

        public Version () {
            this.VersionCode = 0;
        }
    }

    /// <summary>
    /// 补丁文件集合
    /// </summary>
    /// <typeparam name="string">文件路径</typeparam>
    /// <typeparam name="string">文件md5码</typeparam>
    [Serializable] public class PatchFiles : Serialization<string, string> { }

    /// <summary>
    /// 补丁
    /// </summary>
    [Serializable] public class Patch : JsonSerializable<Patch> {

        /// <summary>
        /// 版本号
        /// </summary>
        /// <value></value>
        public Version Version = new Version ();

        /// <summary>
        /// 版本文件
        /// </summary>
        /// <value></value>
        public PatchFiles Files = new PatchFiles ();
    }

    public class CheckLocalPatchAsyArgs {
        public long FileCount { get; set; }
        public long Index { get; set; }
        public string FileName { get; set; }
        public string FileMd5 { get; set; }
    }

    public delegate void OnCheckLocalFileAction (CheckLocalPatchAsyArgs args);
    public delegate void OnCheckLocalFileComplateAction ();

    public class PatchHelper {

        /// <summary>
        /// 要下载的文件
        /// </summary>
        /// <param name="local"></param>
        /// <param name="remote"></param>
        /// <returns></returns>
        public static PatchFiles GetNeedDownload (Patch local, Patch remote) {
            if (local == null || local.Files == null || local.Files.Count == 0) {
                return remote.Files;
            }
            return null;
        }

        /// <summary>
        /// 要删除的文件
        /// </summary>
        /// <param name="local"></param>
        /// <param name="remote"></param>
        /// <returns></returns>
        public static PatchFiles GetNeedDelete (Patch local, Patch remote) {
            if (local == null || local.Files == null || local.Files.Count == 0) {
                return null;
            }

            if (remote == null || remote.Files == null || remote.Files.Count == 0) {
                return null;
            }

            var needDelete = new PatchFiles ();
            local.Files.Foreach ((k, v) => {
                if (remote.Files.ContainsKey (k) == false) {
                    needDelete.Add (k, v);
                }
            });
            return needDelete;
        }

        public static void CheckLocalPatchAsy (HotUpdate hotUpdateBev, Patch local, OnCheckLocalFileAction onChecked, OnCheckLocalFileComplateAction onComplate) {
            // PatchFiles files = local.Files;
            // hotUpdateBev.StartCoroutine (() => {
            //     yield return null;
            // });
        }

        /// <summary>
        /// 检查版本
        /// </summary>
        /// <param name="hotUpdateBev"></param>
        /// <param name="cdn"></param>
        /// <param name="local"></param>
        public static void CheckRemoteVersion (HotUpdate hotUpdateBev, string cdn, string local) {

        }
    }
}