using Terraria;
using Terraria.ModLoader;
using MCInvasion.Common.Players;

namespace MCInvasion.Buffs
{
	public class WitherAttackPlayerEffect : ModBuff
	{
		public override void SetStaticDefaults() {
		}
		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<WitherAttackPlayer>().hasWitherAttackEffect = true;
		}
	}
}
