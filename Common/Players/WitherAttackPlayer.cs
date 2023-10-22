using MCInvasion.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace MCInvasion.Common.Players
{
	// 这个用于给使用了灌注的玩家添加攻击效果
	public class WitherAttackPlayer : ModPlayer
	{
		public bool hasWitherAttackEffect = false;
		public override void ResetEffects()
		{
			hasWitherAttackEffect = false;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			if (hasWitherAttackEffect)
				target.AddBuff(ModContent.BuffType<WitherDebuffForMob>(), 360);
        }





	}
}
