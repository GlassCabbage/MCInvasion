using Terraria;
using Terraria.ModLoader;
using MCInvasion.Common.Players;

namespace MCInvasion.Buffs
{
	public class MinecraftEffect : ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Minecraft Mod!");
			Description.SetDefault("Spawn Minecraft mobs now!");
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<MinecraftPlayer>().hasMinecraftEffect = true;
		}
	}
}
