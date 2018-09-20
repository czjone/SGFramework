namespace SGF.Lua {

	[System.Serializable]
	public class Config : SGF.Unity.JsonSerializable<Config> {
		/// <summary>
		/// lua 入口
		/// </summary>
		/// <value></value>
		public string LuaMain {
			get;
			set;
		}

		/// <summary>
		/// 开发目录
		/// </summary>
		/// <value></value>
		public string DevDir {
			get;
			set;
		}

		/// <summary>
		/// 动态资源文件夹名
		/// </summary>
		/// <value></value>
		public string DresRootName {
			set;
			get;
		}

		/// <summary>
		/// 是否开启用热更新
		/// </summary>
		/// <value></value>
		public bool HotUpdateEnabled {
			get;
			set;
		}

		/// <summary>
		/// 远程资源地址
		/// </summary>
		/// <value></value>
		public bool RemoteResUrl {
			get;
			set;
		}
	}
}