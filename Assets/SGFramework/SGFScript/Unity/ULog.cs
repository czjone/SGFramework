namespace SGF {
	
	using UnityEngine;
	using System.Text;

	namespace Unity {

		public class ULog {

			private static readonly string logPre = "[SGF PRINT(Core)] {0}";

			public static bool Enable = true;

			private static void D(string msg) {
				if(Enable == false) return;
				Debug.Log(string.Format(logPre,msg));
			}
			public static void D(params object[] msg) {
				if(Enable == false) return;
				if(msg == null)ULog.D("null"); 
				StringBuilder sb = new StringBuilder();
				foreach(var item in msg){
					sb.Append(item.ToString());
					sb.Append(" ");
				}
				ULog.D(sb.ToString());
			}
		}
	}
}