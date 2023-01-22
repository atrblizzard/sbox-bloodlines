using Sandbox;
using Editor;

namespace bloodlines.entities.vampire
{
	[Model]
	[HammerEntity]
	[Library( "prop_hacking", Description = "A point entity that used as a hackable computer/terminal with useful information." )]
	public partial class HackingProp : VAnimEntity, IUse
	{
		[Property( "hack_file", Title = "Definition File. This is related script file that contains the certain computer's data and it's interactions." )]
		public string HackFile { get; set; }

		[Property("difficulty")]
		public int Difficulty { get; set; }

		#region Outputs
		protected Output OnTrigger0 { get; set; }
		protected Output OnTrigger1 { get; set; }
		protected Output OnTrigger2 { get; set; }
		protected Output OnTrigger3 { get; set; }
		protected Output OnTrigger4 { get; set; }
		protected Output OnTrigger5 { get; set; }
		protected Output OnTrigger6 { get; set; }
		protected Output OnTrigger7 { get; set; }
		#endregion

		public bool isHacking = false;

		public override void Spawn()
		{
			base.Spawn();

			SetModel( GetModelName() );

			SetupPhysicsFromModel( PhysicsMotionType.Keyframed, true );
		}

		public bool IsUsable( Entity user )
		{
			return !isHacking;
		}

		public bool OnUse( Entity user )
		{
			var screenBone = Model.GetAttachment( "screen_axis" );

			HandleCamera( user );

#if HACKING_CAMERA
			if ( user is VampirePlayer player )
			{
				if ( player.MainCamera is not HackingCamera )
					player.MainCamera = new HackingCamera();
			}
#endif

			isHacking = !isHacking;

			return true;
		}

#if HACKING_WORLD_PANEL
		[Event.Frame]
		public void OnFrame()
		{
			if ( worldPanelTest is null )
			{
				worldPanelTest = new();
			}
			var screen = GetAttachment( "screen" ) ?? default;
			worldPanelTest.Position = screen.Position;
			worldPanelTest.Rotation = screen.Rotation;
			//worldPanelTest.Position = Position + Rotation.Up * 3.2f * Scale - Rotation.Forward * 20f * Scale + Rotation.Right * 20f * Scale;
			//worldPanelTest.Rotation = Rotation.RotateAroundAxis( Vector3.Up, 90f );
			worldPanelTest.WorldScale = Scale * 0.9f;

			//var RTTexture = Texture.Load( "scene:worldTestScene", false );

			worldPanelTest.screenPanel.Style.BackgroundColor = Color.White;
			worldPanelTest.screenPanel.Style.Width = 900;

			worldPanelTest.screenPanel?.Style.Dirty();
		}

		protected override void OnDestroy()
		{
			worldPanelTest?.Delete();
		}
#endif

		public void HandleCamera( Entity user )
		{
			for ( int i = 0; i < All.Count; i++ )
			{
				//Don't delete other peoples cameras
#if HACKING_CAMERA
				if ( All[i] is HackingCameraEntity hc ) //&& tc.Owner == this.Owner )
				{
					hc.Delete();
				}
#endif
			}

			var screenBone = Model.GetAttachment( 0 );

			var screenAxis = GetAttachment( "screen_axis" ) ?? default;
			var screen = GetAttachment( "screen" ) ?? default;
			var pos = screenAxis.Position;
			var rot = screenAxis.Rotation;

			Log.Info( screenAxis );

#if HACKING_CAMERA
			var ent = new HackingCameraEntity
			{
				Position = pos,
				Rotation = rot,
				Owner = user
			};


			Log.Info( screenAxis.RotationToLocal( screenAxis.Rotation ) );
			Log.Info( screenAxis.Rotation );

			ent.SetPhys( false );
#endif
		}
	}
}
