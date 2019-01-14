namespace SGF.Unity.SDK {

    public class ChannelConfigBase {
        public string ChannelName {
            get;
            set;
        }
    }

    /// <summary>
    /// 测试包使用的配置
    /// </summary>
    public class TestConfig : ChannelConfigBase {
        public string appid { get; set; }
        public string channelId { get; set; }
        public string appkey { get; set; }
    }

    public class SDKBuildConfig {
        public string appid { get; set; }
        public string channelId { get; set; }
        public string appkey { get; set; }

        //下面的为渠道相关的配置信息
        public TestConfig _testConfig { get; set; }
    }
}