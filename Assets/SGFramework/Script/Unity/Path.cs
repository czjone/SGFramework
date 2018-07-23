namespace SGF {
	using UnityEngine;

	namespace Unity {
		public sealed class Path {
			public static string GetTargetPlatformPath (string pat) {
				RuntimePlatform runPlayer = Application.platform;
				if (runPlayer == RuntimePlatform.WindowsEditor ||
					runPlayer == RuntimePlatform.WindowsPlayer) {
					return pat.Replace ("/", GetTargetPathLeag);
				} else {
					return pat.Replace ("\\", GetTargetPathLeag);
				}
			}

			public static string GetTargetPathLeag {
				get {
					RuntimePlatform runPlayer = Application.platform;
					if (runPlayer == RuntimePlatform.WindowsEditor ||
						runPlayer == RuntimePlatform.WindowsPlayer) {
						return "\\";
					} else {
						return "/";
					}
				}
			}
		}
	}
}