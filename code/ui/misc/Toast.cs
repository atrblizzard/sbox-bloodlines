using Sandbox;
using Sandbox.UI;

namespace Bloodlines.UI.HUD
{
	public class Toast : Panel
	{
		private const float MessageDuration = 5.0f;

		private readonly Panel _toastContainer;
		private readonly Label _toastLabel;

		private float _lastOpenTime;
		private bool _isOpen;

		public Toast()
		{
			StyleSheet.Load( "/ui/misc/Toast.scss" );
			AddChild( out _toastContainer, "container" );
			_toastContainer.AddChild( out _toastLabel );
		}

		public void Show( string message )
		{
			_lastOpenTime = Time.Now;
			_toastLabel.Text = message;
			_isOpen = true;
			AddClass( "show" );
		}

		public override void Tick()
		{
			base.Tick();

			if ( _isOpen && Time.Now - _lastOpenTime > MessageDuration )
			{
				_isOpen = false;
				RemoveClass( "show" );
			}
		}
	}
}
