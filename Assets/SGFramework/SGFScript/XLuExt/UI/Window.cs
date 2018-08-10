namespace SGF.XLuaUI
{
	using UnityEngine;

	public class EventArgs:System.EventArgs {

	}

	public delegate void UIComponeAction(object sender,EventArgs);

	public interface IWindow {
		
	}

	public class Window :IWindow {
		
	}
}
