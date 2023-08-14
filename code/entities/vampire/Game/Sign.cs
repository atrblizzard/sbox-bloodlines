using System;
using bloodlines.entities.vampire.Data;
using System.Text.Json.Serialization;
using Sandbox;
using System.IO;
using System.Linq;
using Bloodlines.Globals;
using System.Text.Json;

[Obsolete]
public class SignPanel
{

}

namespace bloodlines.entities.vampire.Game
{
	[Library( "game_sign", Description = "An entity that dumps the misc text signs (like hints and tables) on screen overlay. \nOriginally mainly used on the Tutorial mission." )]
	public partial class Sign : Entity, ISignData
	{
		[Property( "fade_in", Title = "Fade In" )]
		public float FadeIn { get; set; }

		[Property( "fade_out", Title = "Fade Out" )]
		public float FadeOut { get; set; }

		[Property( "pause", Title = "Pause When Reading" )]
		public int Pause { get; set; }

		[Property( "definition_file", Title = "Scripts containing the definition files" )]
		public string DefinitionFile { get; set; }
		public SignData SignData { get; set; }

		public static SignPanel GetSignPanel() => All.OfType<SignPanel>().FirstOrDefault();

		/// <summary>
		/// Changes sign definition file.
		/// </summary>
		/// <param name="file"></param>
		[Input]
		public void ChangeFile( string file )
		{
			LoadFile( file );
			Log.Info( $"Change sign file to {file}" );
		}

		/// <summary>
		/// Opens sign window.
		/// </summary>
		[Input]
		public void OpenWindow( Entity activator = null )
		{
			if ( SignData == null )
				LoadFile();

			InitializeHUD( activator );
		}

		/// <summary>
		/// Closes sign window.
		/// </summary>
		[Input]
		public void CloseWindow()
		{
			// TODO: Implement closing window panels
			Log.Info( "Close sign window" );
		}
		
		public void InitializeHUD( Entity activator = null )
		{
			LoadFile();

			try
			{
				if ( activator is VampirePlayer player )
				{
					JsonSerializerOptions options = new()
					{
						WriteIndented = true
					};

					var json = JsonSerializer.Serialize( SignData, options );

					RPCs.OpenSignPanel( json );
				}
				
			}
			catch ( System.Exception e )
			{
				Log.Warning( e, "Cannot set signpanel signdata." );
			}
		}

		public void LoadFile(string newFile = null)
		{
			var filename = @"vdata\signs\tutorial_popup_masquerade1.txt";

			string json = string.Empty;

			var mounted = FileSystem.Mounted;

			if ( mounted.FileExists( filename ) )
			{
				Log.Info( $"Found {filename}!" );
				json = mounted.ReadAllText( filename );
			}

			if ( string.IsNullOrWhiteSpace( json ) )
				return;

			Deserialize( json );
		}

		public override void Spawn()
		{
			base.Spawn();

			LoadFile();
		}

		public void Deserialize( string json )
		{
			SignData = new SignData();

			if ( string.IsNullOrWhiteSpace( json ) )
				return;

			try
			{
				SignData = JsonSerializer.Deserialize<SignData>( json );
				SignData.BackgroundImage.Name = SignData.BackgroundImage.Name + ".png";
			}
			catch ( System.Exception e )
			{
				Log.Warning( e, "Error deserailizing sign data." );
			}					
		}
	}
}
