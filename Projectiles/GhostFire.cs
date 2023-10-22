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
	public class GhostFire : ModProjectile
	{
		public ref float count => ref Projectile.ai[0];
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0; 
		}

		public override void SetDefaults() {
			Projectile.width = 3; 
			Projectile.height = 3; 
			Projectile.friendly = false; 
			Projectile.hostile = false; 
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.penetrate = 1; 
			Projectile.timeLeft = 6000; 
			Projectile.alpha = 255; 
			Projectile.light = 0.5f; 
			Projectile.ignoreWater = true; 
			Projectile.tileCollide = false; 
			Projectile.extraUpdates = 1; 
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

		public override void OnKill(int timeLeft) {
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
		}

		Vector2 dir;
		float speed;
		public override void AI()
		{
			if (count == 0)
            {
				dir = Projectile.velocity;
            }
			count++;
			if (count % 20 == 0)
            {
				speed = (float)Math.Pow(count / 20, 1.01);
				Projectile.velocity = dir * speed;
			}
			if (Projectile.alpha>0 && count%5==0)
				Projectile.alpha--;

			if (count>=180)
				Projectile.Kill();
		}
	}
}