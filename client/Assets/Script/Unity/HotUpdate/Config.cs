namespace SGF.Unity.HotUpdate {

    public class Config : SGF.Core.JsonSerializable<Config> {

        public string CDN = "http://hotupdate.cnd.com"; //CDN地址
        public string DownloadDir = "dres"; //相对于可写的目录
        /// <summary>  补丁的生成路径，相对于streamassets. </summary>
        public string PatchsStreamAssetsPath = "dres"; //打到包中的资源
        public string VersionFileName = "version.json"; //版本描述文件
        public string VersionFilesFileName = "versionFiles.json"; //版本描述文件
        public string PatchsFileExt = ""; //生成的补丁的文件包扩展
    }
}