﻿
namespace Sandbox
{
    public class VampireThirdPersonCamera : Camera
    {
        [ConVar.Replicated]
        public static bool thirdperson_orbit { get; set; } = false;

        [ConVar.Replicated]
        public static bool thirdperson_collision { get; set; } = true;

        private Angles orbitAngles;
        private float orbitDistance = 150;

        public override void Update()
        {
            var pawn = Local.Pawn as AnimEntity;
            var client = Local.Client;

            if (pawn == null)
                return;

            Pos = pawn.Position;
            Vector3 targetPos;

            var center = pawn.Position + Vector3.Up * 54;

            if (thirdperson_orbit)
            {
                Pos += Vector3.Up * (pawn.CollisionBounds.Center.z * pawn.Scale);
                Rot = Rotation.From(orbitAngles);

                targetPos = Pos + Rot.Backward * orbitDistance;
            }
            else
            {
                Pos = center;
                Rot = Input.Rotation; //Rotation.FromAxis(Vector3.Up, 4) * 

                float distance = 130.0f * pawn.Scale;
                targetPos = Pos + Input.Rotation.Up * ((pawn.CollisionBounds.Maxs.x) * pawn.Scale);
                targetPos += Input.Rotation.Forward * -distance;
            }

            if (thirdperson_collision)
            {
                var tr = Trace.Ray(Pos, targetPos)
                    .Ignore(pawn)
                    .Radius(8)
                    .Run();

                Pos = tr.EndPos;
            }
            else
            {
                Pos = targetPos;
            }

            FieldOfView = 70;

            Viewer = null;
        }

        public override void BuildInput(InputBuilder input)
        {
            if (thirdperson_orbit && input.Down(InputButton.Walk))
            {
                if (input.Down(InputButton.Attack2))
                {
                    orbitDistance += input.AnalogLook.pitch;
                    orbitDistance = orbitDistance.Clamp(0, 1000);
				}
                else
                {
                    orbitAngles.yaw += input.AnalogLook.yaw;
                    orbitAngles.pitch += input.AnalogLook.pitch;
                    orbitAngles = orbitAngles.Normal;
                    orbitAngles.pitch = orbitAngles.pitch.Clamp(-89, 89);
                }

                input.AnalogLook = Angles.Zero;

                input.Clear();
                input.StopProcessing = true;
            }

            base.BuildInput(input);
        }
    }
}
