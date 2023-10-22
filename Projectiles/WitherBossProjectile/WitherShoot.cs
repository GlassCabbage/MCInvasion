using MCInvasion.NPCs.WitherBossNPC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MCInvasion.Projectiles.WitherBossProjectile
{
	public class WitherShoot : AbstractWitherHead
	{
		private int shootSpeed = 20;
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Wither Shoot"); 
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0; 
		}

		public override void SetDefaults() {
			Projectile.width = 10; 
			Projectile.height = 10; 
			Projectile.aiStyle = -1; 
			Projectile.friendly = false; 
			Projectile.hostile = true; 
			Projectile.DamageType = DamageClass.Magic; 
			Projectile.penetrate = -1; 
			Projectile.timeLeft = 600; 
			Projectile.alpha = 255; 
			Projectile.light = 0.5f; 
			Projectile.ignoreWater = true; 
			Projectile.tileCollide = false; 
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

			if (Projectile.ai[0] > 120 && Projectile.ai[0] < 150)
			{
				Vector2 predict = WitherBoss.target.Center + WitherBoss.target.velocity * 300/shootSpeed;
				float rotation = (predict - Projectile.Center).ToRotation();
				Color color = Color.Black;
				float scale = 2;
				color.A = (byte)(5*(Projectile.ai[0] - 120));
				Rectangle rectangleSource = new Rectangle(0, 0, 5000,2);
				Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Projectile.Center - Main.screenPosition,rectangleSource , color, rotation,rectangleSource.Size()/2f,scale,SpriteEffects.None,0);
			}

			return true;
		}


		public override void OnKill(int timeLeft) {
			
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			base.OnHitPlayer(target, info);
		}

		public override void AI()
		{
			
			Projectile.ai[0]++;
			if (Projectile.alpha>0)
				Projectile.alpha-=10;
			if (Projectile.timeLeft < 10)
			{
				Projectile.damage = 0;
				Projectile.alpha += 10;
			}
			if (Projectile.ai[0]<150)
            {
				frame();
				Projectile.velocity = Projectile.DirectionTo(WitherBoss.target.Center)*0.01f;
            }
			else if (Projectile.ai[0]==151)
				Projectile.velocity = Projectile.DirectionTo(WitherBoss.target.Center+WitherBoss.target.velocity*300/shootSpeed) * shootSpeed;
		}

		private void frame()
        {
			Vector2 predict = WitherBoss.target.Center + WitherBoss.target.velocity * 300 / shootSpeed;
			if (Projectile.Center.X < WitherBoss.target.Center.X)	
            {
				Projectile.spriteDirection = -1;
				Projectile.rotation= (predict - Projectile.Center).ToRotation();
			}
			else
            {
				Projectile.rotation = (predict - Projectile.Center).ToRotation() + (float)Math.PI;
            }
        }

	}
}