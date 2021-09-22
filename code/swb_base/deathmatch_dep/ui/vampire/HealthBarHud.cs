using Sandbox;
using Sandbox.UI;
using SWB_Base;

namespace Bloodlines.UI.HUD
{
	public partial class HealthBarHud : Panel
	{
		public static HealthBarHud Current;

		public Label WeaponLabel;
		public Label InventoryLabel;

		public Image HealthBarBackgroundImage;
		public Image HealthBarFillBackgroundImage;
		public Image HealthBarIconBackgroundImage;

		public Image HealthBarFillImage;

		public Image HealthBarDisciplineImage;

		private static bool enabled = false;

		[ClientCmd( name: "togglehud" )]
		public static void ToggleInspector()
		{
			enabled = !enabled;
		}

		public HealthBarHud()
		{
			Current = this;

			StyleSheet.Load( "swb_base/deathmatch_dep/ui/scss/HealthBarHud.scss" );

			AddChild( out HealthBarIconBackgroundImage, "healthbariconbackgroundimage" );
			HealthBarIconBackgroundImage.SetTexture( "ui/hud/new_ui/healthbariconbg.png" );

			AddChild( out HealthBarBackgroundImage, "healthbarbackgroundimage" );
			HealthBarBackgroundImage.SetTexture( "ui/hud/new_ui/healthbarframe-upscaled.png" );

			AddChild( out HealthBarFillBackgroundImage, "healthbarfillbackgroundimage" );
			HealthBarFillBackgroundImage.SetTexture( "ui/hud/new_ui/healthbarfillblack.png" );

			AddChild( out HealthBarFillImage, "healthbarfillimage" );
			HealthBarFillImage.SetTexture( "ui/hud/new_ui/healthbarfill.png" );		

			AddChild( out HealthBarDisciplineImage, "healthbarweaponicon" );
			HealthBarDisciplineImage.SetTexture( "ui/hud/inventory_images/weapons_melee/fists.png" );

			AddChild( out WeaponLabel, "weaponlabel" );
			WeaponLabel.SetText( "N/A" );

			AddChild( out InventoryLabel, "inventorylabel" );
			InventoryLabel.SetText( "0/0" );

			HealthBarDisciplineImage.Style.Dirty();

			Current.Style.Dirty();
		}

		public override void Tick()
		{
			var player = Local.Pawn;
			if ( player == null ) return;

			this.SetClass( "enabled", enabled );

			UpdateWeaponTick( player );

			base.Tick();

			var scopeSize = Screen.Height * ScaleFromScreen * 0.9f; // Screen.Height * ScaleFromScreen * 1f;			
			var widthSize = ScaleFromScreen < 2.8 ? Screen.Width * ScaleFromScreen / 2.3f : Screen.Width * ScaleFromScreen / 1.9f;

			HealthBarBackgroundImage.PanelPositionToScreenPosition( Vector2.Zero );

			HealthBarBackgroundImage.Style.Width = widthSize;
			HealthBarBackgroundImage.Style.Height = scopeSize;
			HealthBarBackgroundImage.Style.Dirty();

			//HealthBarFillImage
			HealthBarFillBackgroundImage.Style.Width = widthSize;
			HealthBarFillBackgroundImage.Style.Height = scopeSize;
			HealthBarFillBackgroundImage.Style.Dirty();

			HealthBarIconBackgroundImage.Style.Width = widthSize;
			HealthBarIconBackgroundImage.Style.Height = scopeSize;
			HealthBarIconBackgroundImage.Style.Dirty();

			HealthBarFillImage.Style.Width = widthSize;
			HealthBarFillImage.Style.Height = scopeSize;
			HealthBarFillImage.Style.Dirty();

			HealthBarDisciplineImage.Style.Width = widthSize;
			HealthBarDisciplineImage.Style.Height = scopeSize;			
			HealthBarDisciplineImage.Style.Dirty();

			Current.Style.Dirty();

		}

		private void UpdateWeaponTick( Entity player )
		{
			var weapon = player.ActiveChild as WeaponBase;
			SetClass( "active", weapon != null );

			if ( weapon == null || weapon is WeaponBaseMelee )
			{
				WeaponLabel.Text = "";
				InventoryLabel.Text = "";
				return;
			};

			var inv = weapon.AvailableAmmo();

			if ( weapon.Primary.ClipSize != -1 )
			{
				WeaponLabel.Text = $"{weapon.WeaponName}";
				InventoryLabel.Text = $"{weapon.Primary.Ammo} / {inv}";
			}
			else
			{
				WeaponLabel.Text = "";
				InventoryLabel.Text = $"{inv}";
			}

			InventoryLabel.SetClass( "active", inv >= 0 );
			HealthBarDisciplineImage.SetTexture( $"{weapon.Icon}" );
		}
	}
}
