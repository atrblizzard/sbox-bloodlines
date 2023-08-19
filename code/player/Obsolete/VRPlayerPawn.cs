using System;
using Sandbox.Controls;
using Sandbox.Controls.Gestures;
using System.Collections.Generic;

namespace Sandbox.Pawns
{
	[Obsolete]
	public partial class VRPlayerPawn : Entity
	{
		[Net, Local] public VRHand LeftHand { get; private set; }
		[Net, Local] public VRHand RightHand { get; private set; }

        [Net, Local] public VRTeleportDestination TeleportDestination { get; private set; }

		private List<IVRGesture> _gestures = new();

		public override void Spawn()
		{
			LeftHand = new LeftVRHand();
			LeftHand.Owner = this;

			RightHand = new RightVRHand();
			RightHand.Owner = this;

			TeleportDestination = new VRTeleportDestination();
            TeleportDestination.Owner = this;
            TeleportDestination.EnableDrawing = false;

			var turnGesture = new TurnGesture( LeftHand );
			turnGesture.OnEmitted += OnTurn;
			_gestures.Add( turnGesture );

            var teleportGesture = new TeleportGesture(LeftHand, TeleportDestination);
            teleportGesture.OnEmitted += OnTeleport;
            _gestures.Add(teleportGesture);

            base.Spawn();

        }

        private void OnTeleport(object sender, Vector3 destination)
        {
            var headOffset = Position - Input.VR.Head.Position;
            Position = destination  + new Vector3(headOffset.x, headOffset.y, 0);
        }

        private void OnTurn( object sender, TurnGestureResult e )
		{
			var direction = e == TurnGestureResult.TurnLeft ? -1 : 1;
            Transform = Transform.RotateAround(
                    Input.VR.Head.Position.WithZ(Position.z),
                    Rotation.FromAxis(Vector3.Up, -45 * direction)
                );
        }

		public override void Simulate( IClient cl )
		{
			base.Simulate( cl );

			LeftHand?.Simulate( cl );
			RightHand?.Simulate( cl );

			foreach ( var gesture in _gestures )
				gesture.Simulate();
		}

		public override void FrameSimulate( IClient cl )
		{
			base.FrameSimulate( cl );

			LeftHand?.FrameSimulate( cl );
			RightHand?.FrameSimulate( cl );
		}
	}
}
