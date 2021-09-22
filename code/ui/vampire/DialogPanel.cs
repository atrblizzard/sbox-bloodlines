using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bloodlines.ui.Vampire
{
	public class DialogPanel : Panel
	{
		private static bool enabled = false;

		public VirtualScrollPanel DialogList { get; set; }

		[ClientCmd( name: "dialog" )]
		public static void ToggleInspector()
		{
			enabled = !enabled;
		}

		public Image DialogPanelImage;
		//public Image HealthBarFillBackgroundImage;
		//public Image HealthBarIconBackgroundImage;

		Dictionary<int, string> Entries = new();

		public Label DialogText;
		public Label DialogOption1;
		public Label DialogOption2;
		public Label DialogOption3;

		protected override void PostTemplateApplied()
		{
			base.PostTemplateApplied();

			DialogList.OnCreateCell = CreateDialogButton;
		}

		private void CreateDialogButton( Panel cell, object obj )
		{
			//if ( obj is Package package )
			//{
			//	var ac = new DialogOptionItem( cell, package );
			//	ac.AddEventListener( "onclick", () => OnSelected( package.FullIdent ) );
			//}

			if ( obj is string str )
			{
				var ac = new DialogOptionItem( cell, str);
				ac.AddEventListener( "onclick", () => OnSelected( str ) );
			}
		}

		private void OnSelected( string str )
		{
			Log.Info( str );
			//CreateValueEvent( "dialog", str );
			//CreateEvent( "navigate_return" );
		}

		public DialogPanel()
		{
			StyleSheet.Load( "ui/vampire/DialogPanel.scss" );

			AddChild( out DialogPanelImage, "dialogpanelimage" );
			DialogPanelImage.SetTexture( "ui/hud/dialog.png" );

			AddChild( out DialogText, "dialoglabel" );
			DialogText.SetText( "What a scene man! Hoo-wee! Then they just plop you out here like a naked baby in the woods. How 'bout that? Look kiddo, this is probably a lot for you to take in, so uh, why don't you let me show you the ropes. Whattaya say?" );

			//AddChild( out DialogOption1, "optionlabel" );
			//DialogOption1.SetText( "1. Who are you?" );

			//AddChild( out DialogOption2, "optionlabel" );
			//DialogOption2.SetText( "2. The rain of ages plot again to wash away revelation." );

			//AddChild( out DialogOption3, "optionlabel" );
			//DialogOption3.SetText( "3. What do I do?" );

			DialogList = new VirtualScrollPanel();
			DialogList.Data.Add( "Test" );
			DialogList.Data.Add( "Test1" );
			DialogList.Data.Add( "Test2" );
		}

		public override void Tick()
		{
			base.Tick();

			var player = Local.Pawn;
			if ( player == null ) return;

			this.SetClass( "enabled", enabled );

			var scopeSize = Screen.Height * ScaleFromScreen * 0.9f; // Screen.Height * ScaleFromScreen * 1f;			
			var widthSize = ScaleFromScreen < 2.8 ? Screen.Width * ScaleFromScreen / 2.3f : Screen.Width * ScaleFromScreen / 1.9f;

			DialogPanelImage.PanelPositionToScreenPosition( Vector2.Zero );

			DialogPanelImage.Style.Width = widthSize;
			DialogPanelImage.Style.Height = scopeSize;
			DialogPanelImage.Style.Dirty();


			//WeaponLabel.Text = "32/32";
			//WeaponLabel.Style.Dirty();

			//InventoryLabel.Text = "32/32";
			//InventoryLabel.Style.Dirty();

			Style.Dirty();

		}
	}

	public class DialogOptionItem : Panel
	{
		public DialogOptionItem( Panel parent, string title )
		{
			Parent = parent;

			StyleSheet.Load( "/MenuUI/Components/AssetCard.scss" );

			//var icon = Add.Panel( "icon" );
			//icon.Style.Set( "background-image", $"url( {image} )" );

			Add.Label( title, "title" );
		}
	}
}
