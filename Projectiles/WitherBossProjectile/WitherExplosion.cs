using MCInvasion.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MCInvasion.Projectiles.WitherBossProjectile
{
	public class WitherExplosion: ModProjectile
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("WitherExplosion"); 
			Main.projFrames[Projectile.type] = 16;
		}

		public override void SetDefaults() {
			Projectile.width = 50; 
			Projectile.height = 50; 
			Projectile.aiStyle = -1; 
			Projectile.friendly = false; 
			Projectile.hostile = true; 
			Projectile.DamageType = DamageClass.Magic; 
			Projectile.penetrate = -1; 
			Projectile.timeLeft = 6000; 
			Projectile.alpha = 0; 
			Projectile.light = 0.5f; 
			Projectile.ignoreWater = true; 
			Projectile.tileCollide = false; 
		}

		public override void OnKill(int timeLeft) {
			
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{

		}

		int temp;
		int t = 0;
		public override void AI()
		{
			t += 1;
			if (Projectile.damage != 0)
				temp = Projectile.damage;
			Projectile.damage = 0;
			if (Projectile.frame < Main.projFrames[Projectile.type]-1 && t%4==0 )
				Projectile.frame++;
			else
				Projectile.alpha+=5;
			if (t > 50)
				Projectile.damage = temp;
			if (Projectile.alpha>255)
				Projectile.Kill();


		}

	}
}