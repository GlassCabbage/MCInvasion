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
	public class Fireball : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0; 
		}

		public override void SetDefaults() {
			Projectile.width = 8; 
			Projectile.height = 8; 
			Projectile.aiStyle = 1; 
			Projectile.friendly = false; 
			Projectile.hostile = true; 
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.penetrate = 1; 
			Projectile.timeLeft = 6000; 
			Projectile.alpha = 0; 
			Projectile.light = 0.5f; 
			Projectile.ignoreWater = false; 
			Projectile.tileCollide = true; 
			Projectile.extraUpdates = 1;

			AIType = ProjectileID.Bullet; 
		}

		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}

		public override void Kill(int timeLeft) {
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			for (int i = 1; i < 10; i++)
			{
				Dust.NewDust(Projectile.position, 55, 55, DustID.SolarFlare, Main.rand.Next(4) - 2, Main.rand.Next(4) - 2);
				Dust.NewDust(Projectile.position, 55, 55, DustID.Torch, Main.rand.Next(4) - 2, Main.rand.Next(4) - 2);
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{

				target.AddBuff(BuffID.OnFire, 800, quiet: false);
			
		}
		public override void AI()
		{
			Dust.NewDust(Projectile.position, 10, 10, DustID.Torch);


		}

	}
}