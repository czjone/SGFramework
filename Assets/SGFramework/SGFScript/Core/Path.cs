namespace SGF {
	namespace Core {
		using System.Collections.Generic;
		using System.Collections;
		using UnityEngine;

		public class Path {

			public static string DirSplitor {
				get {
					return "/";

				}
			}

			public static string UnDirSplitor {
				get {

					return "\\";

				}
			}

			public static string Legalization (string rawPath) {
				return rawPath.Replace (UnDirSplitor, DirSplitor);
			}
		}
	}
}