using bloodlines.entities.vampire.Data;
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;
using Label = Sandbox.UI.Label;
using Bloodlines.Globals;
using System.Text.Json;

namespace bloodlines.ui.Vampire
{
	public partial class SignPanel : Panel
	{
		//private static bool enabled = false;
		public static bool IsOpen { get; private set; }

		public static SignPanel Current;

		public Image BackgroundImage;
		public List<Image> LabelImage;
		public List<Label> TextBlockLabel;

		public SignData SignData;

		public SignPanel()
		{
			Current = this;

			BackgroundImage = new Image();
			LabelImage = new List<Image>();
			TextBlockLabel = new List<Label>();

			StyleSheet.Load( "/ui/vampire/SignPanel.scss" );

			//UpdatePanel( SignData );
		}

		[ConCmd.Client( name: "signwindow" )]
		public static void ToggleWindow()
		{
			IsOpen = !IsOpen;
		}

		public void UpdatePanel( SignData newSignData )
		{
			if ( newSignData != null )
				SignData = newSignData;			

			Log.Info( newSignData );

			if ( SignData != null )
			{
				Log.Info( "SignData is not null!");

				this.DeleteChildren();
				//Delete();

				PanelTransform backgroundTransform = new();
				backgroundTransform.BuildTransform( 400, 400, Vector2.One );
				backgroundTransform.AddTranslateX( Length.Fraction( -0.05f ) );
				backgroundTransform.AddTranslateY( Length.Fraction( -0.1f ) );
				AddChild( out BackgroundImage, "backgroundimage" );
				BackgroundImage.Style.Width = Screen.Width * ScaleFromScreen * 0.9f;
				BackgroundImage.Style.Height = Screen.Height * ScaleFromScreen * 1.6f; // scopeSize;
				BackgroundImage.Style.Transform = backgroundTransform;
				BackgroundImage.Style.Dirty();
				BackgroundImage.SetTexture( "ui/" + SignData.BackgroundImage.Name );


				LabelImage.Clear();
				TextBlockLabel.Clear();

				var imageTransform = new PanelTransform();
				imageTransform.BuildTransform( 300, 400, Vector2.One);
				var imageShit = 0;

				foreach ( var label in SignData.Label )
				{
					imageShit += 10;
					var newImage = new Image();
					AddChild( out newImage, "label" );
					//imageTransform.AddTranslateX( Length.Percent( -1.0f + imageShit ) );
					//imageTransform.AddTranslateY( Length.Percent( -5.0f ) );
					newImage.SetTexture( $"ui/{label.Image}.png" );
					//newImage.Style.Transform = imageTransform;
					//newImage.Style.Position = PositionMode.Relative;
					newImage.Style.Position = PositionMode.Absolute;
					newImage.Style.SetRect( new Rect( int.Parse( label.XPos ), int.Parse( label.YPos ), int.Parse( label.Wide ), int.Parse( label.Tall ) ) );
					//newImage.Style.Width = 110;
					//newImage.Style.Height = 110;
					LabelImage.Add( newImage );
					Log.Info( imageShit );
				}


				var pt = new PanelTransform();
				var shit = 0;

				//pt.AddTranslateX( Length.Percent( -50.0f ) );
				//pt.AddTranslateY( Length.Percent( -50.0f ) );

				foreach ( var textblock in SignData.TextBlock )
				{
					shit += 10;
					var newTextBlock = new Label();
					AddChild( out newTextBlock, "textblock" );
					newTextBlock.SetText( textblock.Text );
					//pt.BuildTransform( int.Parse( textblock.XPos ), int.Parse( textblock.YPos ) );				
					//pt.AddTranslateX( Length.Percent( -3.0f + shit ) );
					//pt.AddTranslateY( Length.Percent( -10.0f ) );
					newTextBlock.Style.Position = PositionMode.Absolute;
					newTextBlock.Style.SetRect( new Rect( int.Parse( textblock.XPos ), int.Parse( textblock.YPos ), int.Parse( textblock.Wide ), int.Parse( textblock.Tall ) ) );
					//newTextBlock.Style.Transform = pt;
					//newTextBlock.Style.Width = int.Parse(textblock.Wide);//  500;
					//newTextBlock.Style.Height = int.Parse( textblock.Tall ); // 300;
					TextBlockLabel.Add( newTextBlock );
					Log.Info( textblock.Text );
				}
			}
			else
			{
				return;

				//foreach ( var label in TextLabel)
				//{
				//	Delete( label );
				//}
				LabelImage.Clear();
				TextBlockLabel.Clear();

				var newLabel = new Image();			
				AddChild( out newLabel, "label" );
				//newLabel.SetText( "Shit is fucked up really bad!" );
				LabelImage.Add( newLabel );

				AddChild( out BackgroundImage, "backgroundimage" );
				BackgroundImage.SetTexture( "ui/interface/pop_ups/tutorial_popup.png" );

				//var mounted = FileSystem.Mounted;

				//var filename = @"code/interface/pop_ups/tutorial_popup.png";

				//if ( mounted.FileExists( filename ) )
				//{
				//	Log.Info( $"Found {filename}!" );
				//	BackgroundImage.SetTexture( "ui/interface/pop_ups/tutorial_popup.png" );
				//}

			}
		}

		[Event( "bloodlines.signpanel.open")]
		//[GameEvents.SignPanel.Open(SignData)]
		public void Open( string signData )
		{
			if ( !Game.IsClient || IsOpen ) return;
			if ( signData != null )
			{
				JsonSerializerOptions options = new()
				{
					WriteIndented = true
				};

				SignData = JsonSerializer.Deserialize<SignData>( signData, options );
				Log.Info( SignData );
				UpdatePanel( SignData );
				Log.Info( "Opening signdata worked!" );
			}			

			AddClass( "enabled" );
			IsOpen = true;
		}

		[Event( "bloodlines.signpanel.close" )]
		public void Close()
		{
			if ( !Game.IsClient || !IsOpen ) return;
			RemoveClass( "open" );
			IsOpen = false;
		}


		//public static void Show()
		//{
		//	IsOpen = true;
		//}

		//public static void Hide()
		//{
		//	IsOpen = false;
		//}

		public override void Tick()
		{
			base.Tick();

			var player = Game.LocalPawn;
			if ( player == null ) return;

			this.SetClass( "enabled", IsOpen );

			//Style.Dirty();
			return;

			/*
			PanelTransform transform = new();
			transform.AddTranslateX( Length.Fraction( -0.5f ) );
			transform.AddTranslateY( Length.Fraction( -0.1f ) );

			var screenPos = player.EyePos.ToScreen();

			//transform.AddTranslateX( Length.Pixels( screenPos.x * Screen.Width ) );
			//transform.AddTranslateY( Length.Pixels( screenPos.y * Screen.Height ) );


			BackgroundImage.PanelPositionToScreenPosition( Vector2.Zero );

			var scopeSize = Screen.Height * ScaleFromScreen * 1f; // Screen.Height * ScaleFromScreen * 1f;			
			var widthSize = ScaleFromScreen < 2.8 ? Screen.Width * ScaleFromScreen / 2.3f : Screen.Width * ScaleFromScreen / 1.9f;

			PanelTransform backgroundTransform = new();
			backgroundTransform.AddTranslateX( Length.Fraction( -0.05f ) );
			backgroundTransform.AddTranslateY( Length.Fraction( -0.1f ) );


			BackgroundImage.Style.Width = Screen.Width * ScaleFromScreen * 1f;
			BackgroundImage.Style.Height = Screen.Height * ScaleFromScreen * 1f; // scopeSize;
			BackgroundImage.Style.Transform = backgroundTransform;
			BackgroundImage.Style.Dirty();

			foreach ( var label in LabelImage )
			{
				label.Style.Dirty();
				//label.Style.BackgroundColor = Color.Yellow;				
				//label.Style.Position = PositionMode.Relative;
				//label.Style.Transform = transform;
				//label.Style.Width = widthSize;
				//label.Style.Height = scopeSize;
			}

			foreach ( var label in TextBlockLabel )
			{
				label.Style.Dirty();
				//label.Style.BackgroundColor = Color.Yellow;
				//label.Style.FontColor = Color.Red;
				//label.Style.FontFamily = "Arial";
				//label.Style.FontSize = 30;
				//label.Style.Transform = transform;
				//label.Style.Width = widthSize;
				//label.Style.Height = scopeSize;
			}

	

			//WeaponLabel.Text = "32/32";
			//WeaponLabel.Style.Dirty();

			//InventoryLabel.Text = "32/32";
			//InventoryLabel.Style.Dirty();

			Style.Dirty();
			*/
		}

		protected override void OnClick( MousePanelEvent e )
		{
			base.OnClick( e );

			if ( Game.LocalPawn is not Player player ) return;

			if ( e.Button == "mouseleft" )
				Close();


		}


		//public override void OnButtonEvent( ButtonEvent e )
		//{

		//	base.OnButtonEvent( e );

		//	if ( Local.Pawn is not Player player ) return;

		//	if ( e.Button == "mouseleft" )
		//	{
		//		SetMouseCapture( e.Pressed );
		//		Close();
		//	}

		//}

		public override void OnHotloaded()
		{
			base.OnHotloaded();

			//Current = new();

			//StyleSheet.Load( "/ui/vampire/SignPanel.scss" );
			//Style.Dirty();

			this.DeleteChildren();
			//Delete();
			UpdatePanel( SignData );
		}
	}
}
