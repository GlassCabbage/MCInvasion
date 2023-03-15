using MCInvasion.Buffs;
using MCInvasion.NPCs.WitherBossNPC;
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
	public class TrackHead : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("trackHead");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults() {
			Projectile.width = 10; 
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = -1; 
			Projectile.timeLeft = 1200; 
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

			return true;
		}

		public override void Kill(int timeLeft) {
			
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{


			
		}

		public static int maxSpeed = 13;
		private int aiTimer = 0;
		private int nowSpeed = 0;
		public static float trackPower = 0.02f;
		private Player target;
		public override void AI()
		{
			aiTimer++;
			if (aiTimer == 1)
            {
				target = WitherBoss.target;
				/*
				if (WitherBoss.isSecondStage)
                {
					maxSpeed = 10;
					trackPower = 0.03f;
                }
				*/
				Projectile.spriteDirection = -1;
			}
			Projectile.rotation=Projectile.velocity.ToRotation();
			if (aiTimer < 120)
            {
				Projectile.alpha -= 10;
				Projectile.velocity.X *= 0.9f;
				Projectile.velocity.Y *= 0.9f;
            }
			else if (aiTimer==120)
            {
				Projectile.alpha = 0;
            }
			else
            {
				nowSpeed = (aiTimer - 108)/12;
				if (nowSpeed>maxSpeed)
					nowSpeed = maxSpeed;
				Projectile.velocity.X = (float)(nowSpeed * (Projectile.velocity.X / (float)Math.Sqrt(Math.Pow(Projectile.velocity.X, 2) + Math.Pow(Projectile.velocity.Y, 2)) + Projectile.DirectionTo(target.Center).X * trackPower));
				Projectile.velocity.Y = (float)(nowSpeed * (Projectile.velocity.Y / (float)Math.Sqrt(Math.Pow(Projectile.velocity.X, 2) + Math.Pow(Projectile.velocity.Y, 2)) + Projectile.DirectionTo(target.Center).Y * trackPower));
			}

			if (Projectile.timeLeft<60)
            {
				Projectile.alpha += 10;
				Projectile.damage = 0;

            }

		}



	}

	
}