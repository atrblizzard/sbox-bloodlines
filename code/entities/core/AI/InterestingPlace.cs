using Sandbox;
using Editor;

namespace bloodlines.entities.core.AI
{
	[ClassName( "intersting_place" )]
	[HammerEntity]
	[EditorSprite( "materials/editor/npcinterestingplace.vmat" )]
	[Title( "intersting_place" ), Category( "AI" ), Icon( "select_all" )]
	public partial class InterestingPlace : Entity
	{
		[Property( "holster_weapon" )]
		public int HolsterWeapon { get; set; } = 0;

		/// <summary>
		/// Maximum number of NPCs that can use this entity.
		/// Game maps contains number from 1 to 50.
		/// </summary>
		[Property( "max_npcs" )]
		public int MaxNPCs { get; set; } = 0;

		[Property( "min_time" )]
		public float MinTime { get; set; } = 0;

		[Property( "min_bounds" )]
		public Vector3 MinBounds { get; set; } = 0;

		[Property( "max_time" )]
		public float MaxTime { get; set; } = 0;

		[Property( "max_bounds" )]
		public Vector3 MaxBounds { get; set; } = 0;
		[Property( "rating" )]
		public int Rating { get; set; } = 0;

		[Property( "group_id" )]
		public int GroupID { get; set; } = 0;

		[Property( "type" )]
		public string Type { get; set; }

		protected Output OnNPCArrived { get; set; }
		protected Output OnNPCLeft { get; set; }
	}

	/// <summary>
	/// Same as Intersting_Place.
	/// Conversation marks nodes that attract 2 NPCs at the same time...
	/// </summary>
	[Library("intersting_place_conversation")]
	public partial class InterestingPlaceConversation : Entity
	{
		/// <summary>
		/// interesting_places(target_destination) : "Interesting Place" : : "Target 'intersting_place' entity to be used [not tested]."
		/// </summary>
		[Property( "interesting_places")]
		[FGDType("target_destination" )]
		public string InterestingPlaces { get; set; }

		[Property( "min_time" )]
		public float MinTime { get; set; } = 0;
		[Property( "max_time" )]
		public float MaxTime { get; set; } = 0;

		/// <summary>
		/// Distance from Player
		/// </summary>
		[Property( "player_dist" )]
		public float PlayerDistance { get; set; } = 0;

		/// <summary>
		/// Turn Towards Talker
		/// </summary>
		[Property( "turn_towards_talker" )]
		public int TurnTowardsTalker { get; set; }

		/// <summary>
		/// Sound Occluded
		/// </summary>
		[Property( "sound_occluded" )]
		public int SoundOccluded { get; set; }

		/// <summary>
		/// "Fired when converstation ends."
		/// </summary>		
		protected Output OnConversationEnd { get; set; }

		/// <summary>
		/// Fired when new talker appears 
		/// </summary>
		protected Output OnNewTalker { get; set; }

		/// <summary>
		/// Fired when sound stopped playing
		/// </summary>
		protected Output OnOneOffSoundComplete { get; set; }

		/// <summary>
		/// Fired when player left given radius (what of?)
		/// </summary>
		protected Output OnPlayerLeftRadius { get; set; }

		/// <summary>
		/// Fired when player is too close to target
		/// </summary>
		protected Output OnPlayerTooClose { get; set; }

		[Input]
		public void PlayOneOffSound()
		{

		}
	}
}
