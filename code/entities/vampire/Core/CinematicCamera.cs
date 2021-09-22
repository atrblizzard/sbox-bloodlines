using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace bloodlines.entities.vampire.Core
{
	[Library( "camera_cinematic")]
	public partial class CinematicCamera : Entity
	{
		#region Outputs
		/// <summary>
		/// OnCameraBegin.
		/// </summary>
		protected Output OnCameraBegin { get; set; }

		/// <summary>
		/// OnCameraComplete.
		/// </summary>
		protected Output OnCameraComplete { get; set; }


		#endregion

		[Property( "shotname" )]
		public string ShotName { get; set; }

		[Property( "startent")]
		[FGDType( "target_destination")]
		public string StartEntity { get; set; }

		[Property( "endent")]
		[FGDType( "target_destination" )]
		public string EndEntity { get; set; }

		[Property( "target1" )]
		[FGDType( "target_destination" )]
		public string Target1 { get; set; }

		[Property( "target2" )]
		[FGDType( "target_destination" )]
		public string Target2 { get; set; }

		[Property( "sep_camanim" )]
		public string SepCamAnim { get; set; }

		[Property( "animname" )]
		public string AnimationName { get; set; }

		[Property( "point_player" )]
		public int PointPlayer { get; set; }

	}
}
