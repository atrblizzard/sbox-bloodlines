using Amper.FPS;

namespace Vampire
{
    partial class VampireGameRules
    {
        public override void PlayerHurt(SDKPlayer player, ExtendedDamageInfo info)
        {
            base.PlayerHurt(player, info);

            EventDispatcher.InvokeEvent(new PlayerHurtEvent()
            {
                Victim = player,
                Attacker = info.Attacker,
                Assister = null,
                Inflictor = info.Weapon,
                Tags = info.Tags.ToArray(),
                Position = info.ReportPosition,
                Damage = info.Damage
            });
        }

        public override void PlayerRespawn(SDKPlayer player)
        {
            base.PlayerRespawn(player);

            if (player is not VampirePlayer vampirePlayer)
                return;

            EventDispatcher.InvokeEvent(new PlayerSpawnEvent()
            {
                Client = player,
                Team = vampirePlayer.Team,
                Clan = vampirePlayer.PlayerClan
            });
        }
    }
}
