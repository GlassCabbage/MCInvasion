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
			DisplayName.SetDefault("WitherExplosion"); 
			Main.projFrames[Projectile.type] = 66;
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

		public override void Kill(int timeLeft) {
			
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{

		}

		int temp;
		public override void AI()
		{
			if (Projectile.damage != 0)
				temp = Projectile.damage;
			Projectile.damage = 0;
			if (Projectile.frame < Main.projFrames[Projectile.type]-1)
				Projectile.frame++;
			else
				Projectile.alpha+=5;
			if (Projectile.frame > 50)
				Projectile.damage = temp;
			if (Projectile.alpha>255)
				Projectile.Kill();


		}

	}
}