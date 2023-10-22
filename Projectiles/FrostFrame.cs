using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MCInvasion.Projectiles
{
	public class FrostFrame : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0; 
		}

		public override void SetDefaults() {
			Projectile.width = 3; 
			Projectile.height = 3; 
			Projectile.aiStyle = 1; 
			Projectile.friendly = true; 
			Projectile.hostile = false; 
			Projectile.DamageType = DamageClass.Magic; 
			Projectile.penetrate = -1; 
			Projectile.timeLeft = 120; 
			Projectile.alpha = 255; 
			Projectile.light = 1f; 
			Projectile.ignoreWater = false; 
			Projectile.tileCollide = true; 
			Projectile.extraUpdates = 1; 
			

			AIType = ProjectileID.Bullet; 
		}

		public override bool PreDraw(ref Color lightColor) {
			return false;
		}

		public override void OnKill(int timeLeft) {
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			
			target.AddBuff(BuffID.Frostburn, 180);
			target.AddBuff(BuffID.Slow, 180);
			target.AddBuff(204, 180);
			target.immune[Projectile.owner] = 1;

		}

		public override void AI()
		{
			if (Projectile.timeLeft < 30)
			{
				Projectile.velocity /= 1.1f;
			}
			if (Projectile.timeLeft%4==0 && Projectile.timeLeft>=60)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Frost);
			else if (Projectile.timeLeft < 60 && Main.rand.Next(60 - Projectile.timeLeft) == 0)
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Frost);

			
		}

	}
}