using MCInvasion.Common.Players;
using Terraria.ModLoader;

namespace MCInvasion.Utils
{
    public class Utils
    {
        public bool betaMobSpawnCheck(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<MinecraftPlayer>().hasMinecraftEffect == true)
                return true;
            return false;
        }
    }
}
