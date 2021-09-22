using Sandbox;
using Sandbox.UI;

namespace Bloodlines.UI.HUD
{
	public partial class BloodBarHud : Panel
	{
		public static BloodBarHud Current;

		public Label DisciplineLabel;

		public Image BloodBarBackgroundImage;
		public Image BloodBarIconBackgroundImage;

		public Image BloodBarFillImage;

		public Image BloodBarDisciplineImage;

		private static bool enabled = false;

		[ClientCmd( name: "togglehud" )]
		public static void ToggleInspector()
		{
			enabled = !enabled;
		}

		public BloodBarHud()
		{
			Current = this;
			StyleSheet.Load( "swb_base/deathmatch_dep/ui/scss/BloodBarHud.scss" );

			AddChild( out BloodBarIconBackgroundImage, "bloodbariconbackgroundimage" );
			BloodBarIconBackgroundImage.SetTexture( "ui/hud/new_ui/bloodbariconbg.png" );

			AddChild( out BloodBarBackgroundImage, "bloodbarbackgroundimage" );
			BloodBarBackgroundImage.SetTexture( "ui/hud/new_ui/bloodbarframe-upscaled.png" );

			AddChild( out BloodBarFillImage, "bloodbarfillimage" );
			BloodBarFillImage.SetTexture( "ui/hud/new_ui/bloodbar.png" );

			AddChild( out BloodBarDisciplineImage, "bloodbardisciplineicon" );
			BloodBarDisciplineImage.SetTexture( "ui/hud/disciplines/thaumaturgy_base.png" );


			AddChild( out DisciplineLabel, "disciplinelabel" );
			DisciplineLabel.SetText( "Thaumaturgy" );

			Current.Style.Dirty();
		}

		public override void Tick()
		{
			var player = Local.Pawn;
			if ( player == null ) return;

			this.SetClass( "enabled", enabled );

			var scopeSize = Screen.Height * ScaleFromScreen * 0.9f; // Screen.Height * ScaleFromScreen * 1f;			
			var widthSize = ScaleFromScreen < 2.8 ? Screen.Width * ScaleFromScreen / 2.3f : Screen.Width * ScaleFromScreen / 1.9f;

			BloodBarBackgroundImage.PanelPositionToScreenPosition(Vector2.Zero);

			BloodBarBackgroundImage.Style.Width = widthSize;
			BloodBarBackgroundImage.Style.Height = scopeSize;
			BloodBarBackgroundImage.Style.Dirty();

			BloodBarIconBackgroundImage.Style.Width = widthSize;
			BloodBarIconBackgroundImage.Style.Height = scopeSize;
			BloodBarIconBackgroundImage.Style.Dirty();

			BloodBarFillImage.Style.Width = widthSize;
			BloodBarFillImage.Style.Height = scopeSize;
			BloodBarFillImage.Style.Dirty();

			BloodBarDisciplineImage.Style.Width = widthSize;
			BloodBarDisciplineImage.Style.Height = scopeSize;
			BloodBarDisciplineImage.Style.Dirty();			

			DisciplineLabel.Text = "Thaumaturgy";
			DisciplineLabel.Style.Dirty();

			Current.Style.Dirty();

			base.Tick();
		}
	}
}
