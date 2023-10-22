using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MCInvasion.Buffs
{
	public class WitherDebuffForPlayer : ModBuff
    {
		public override void SetStaticDefaults() {
			Main.pvpBuff[Type] = true; 
			BuffID.Sets.LongerExpertDebuff[Type] = true;
			Main.debuff[Type] = true;
		}	
		public override void Update(Player player, ref int buffIndex) {
			if (player.lifeRegen > 0)
				player.lifeRegen = 0;
			player.lifeRegenTime = 0;
			player.lifeRegen -= 16;
			player.statDefense -= 10;
			player.statLifeMax2 -= 100;
		}
	}
}			


  