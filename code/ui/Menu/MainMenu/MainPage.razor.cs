using System.Linq;
using Sandbox;
using Sandbox.UI;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Bloodlines.Menu.ButtonDetails;
using System;
using Bloodlines.UI;

namespace Bloodlines.Menu;

public partial class MainPage : Panel
{
	BlogInfo latestBlog;

    private Label FooterText { get; set; }

    private ButtonDetails[] Details => new[]
	{
        new ButtonDetails("New Game", "Start New Game", OnClickNewGame, false, MenuType.MainMenuOnly),
        new ButtonDetails("Resume Game", "Resume Game", OnClickResumeGame, false, MenuType.InGameOnly),
        new ButtonDetails("Load Game", "Load Game", OnClickLoadGame, true),
        new ButtonDetails("Save Game", "Save Game", OnClickSaveGame, true, MenuType.InGameOnly),
        new ButtonDetails("Multiplayer", "Find your Community", () => { _ = OnClickCreateGame(); }, false, MenuType.MainMenuOnly),
        new ButtonDetails("Options", "Adjust User Settings", OnClickSettings),
        new ButtonDetails("Community", "Join our Discord", ()=> Log.Info("Ha, gottem!"), true, MenuType.MainMenuOnly),
        new ButtonDetails("Quit", "Quit Game", OnClickQuit)
    };

    ServerList List;
	public MainPage()
	{
		BindClass( "ingame", () => Game.InGame );

		_ = GetLatestBlog();
	}

    private void OnButtonMouseEnter(ButtonDetails details)
    {
        FooterText.Text = details.Title.ToLower();
    }

    public void OnClickSaveGame()
	{
	}

    public void OnClickLoadGame()
    {
	    
    }
    
    public void OnClickMultiplayer()
	{
	    
	}

	public void OnClickCommunity()
	{
	    
	}

	private async Task GetLatestBlog()
	{
        latestBlog = new BlogInfo
        {
            Name = "opening-the-source",
            Title = "Opening The Source",
            Description = "In today's blog-post, we would like to update you about the state of the VtM: Bloodlines Source 2 project, what has happened over the past few months, what we have planned for the future and what you should expect from the project.",
            URL = "http:\\projectvaulderie.com\\post\\opening-the-source",
            Thumbnail = @"https://cdn.vox-cdn.com/thumbor/zAuPJZW2Ps9H2vmoIAr9Qlia4J4=/0x0:1920x1080/1600x900/cdn.vox-cdn.com/uploads/chorus_image/image/44218884/28529-project-vaulderie.0.0.jpg"
        };

        StateHasChanged();
	}

	public void OnClickResumeGame()
	{
		Game.Menu.HideMenu();
	}
	
	public void OnClickNewGame()
	{
		//this.Navigate( "/newgame" );
		Game.Menu.StartServerAsync( 1, "Bloodlines S2 Test", GameInfo.startmap );
		if (Game.Menu.Lobby != null)
			Game.Menu.Lobby.Public = false;
		Game.Menu.HideMenu();
	}
	
	public async Task OnClickCreateGame()
	{
		var maxPlayers = Game.Menu.Package.GetMeta<int>( "MaxPlayers", 1 ); ;
		await Game.Menu.CreateLobbyAsync( maxPlayers );
	}
	public void OnClickViewLobby()
	{
		this.Navigate( "/lobby" );
	}
	public void OnClickLoadout()
	{
		this.Navigate( "/loadout" );
	}
	public void OnClickJoinGame()
	{
		//List.SetClass("visible", !List.HasClass("visible"));
	}

	public void OnClickSettings()
	{
		this.Navigate( "/settings" );
	}

	public void OnClickQuit()
	{
		if(Game.InGame)
		{
			MenuOverlay.Open<QuitDialog>();
		}
		else
		{
			Game.Menu.Close();
		}
	}

	public void OnClickClassSelection()
	{
		if ( !Game.InGame ) return;

		//HudOverlay.Open<ClassSelection>();
	}

	public void OnClickTeamSelection()
	{
		if ( !Game.InGame ) return;

		//HudOverlay.Open<TeamSelection>();
	}

	public void OnClickBlog()
	{
		var blog = MenuOverlay.Open<BlogView>();
		blog.Url = GetBlogURL();
	}

	public string GetBlogTitle() => latestBlog?.Title ?? "NULL";
	public string GetBlogDescription() => latestBlog?.Description ?? "NULL";
	public string GetBlogURL() => latestBlog?.URL ?? "NULL";
	public string GetBlogThumbnail() => latestBlog?.Thumbnail ?? "NULL";

    protected override int BuildHash()
    {
		return HashCode.Combine(Game.Menu.GetHashCode, base.BuildHash());
    }
}
