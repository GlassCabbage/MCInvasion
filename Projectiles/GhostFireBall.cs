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
	public class GhostFireBall : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0; 
		}

		public override void SetDefaults() {
			Projectile.width = 71; 
			Projectile.height = 71; 
			Projectile.friendly = false; 
			Projectile.hostile = true; 
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.penetrate = 1; 
			Projectile.timeLeft = 6000; 
			Projectile.alpha = 0; 
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

		public override void Kill(int timeLeft) {
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			for (int i = 1; i < 100; i++)
			{
				Dust.NewDust(Projectile.position, 55, 55, DustID.Pixie, Main.rand.Next(20) - 10, Main.rand.Next(20) - 10);
				Dust.NewDust(Projectile.position, 55, 55, DustID.Torch, Main.rand.Next(20) - 10, Main.rand.Next(20) - 10);
				Dust.NewDust(Projectile.position, 55, 55, DustID.Fireworks, Main.rand.Next(20) - 10, Main.rand.Next(20) - 10);
				Dust.NewDust(Projectile.position, 55, 55, DustID.Shadowflame, Main.rand.Next(20) - 10, Main.rand.Next(20) - 10);
				Dust.NewDust(Projectile.position, 55, 55, DustID.GoldFlame, Main.rand.Next(20) - 10, Main.rand.Next(20) - 10);
			}
			switch (Main.rand.Next(4))
			{
				case 0:
					SoundEngine.PlaySound(ex1, Projectile.Center);
					break;
				case 1:
					SoundEngine.PlaySound(ex2, Projectile.Center);
					break;
				case 2:
					SoundEngine.PlaySound(ex3, Projectile.Center);
					break;
				default:
					SoundEngine.PlaySound(ex4, Projectile.Center);
					break;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.velocity = 20*Projectile.DirectionTo(target.Center);
			target.AddBuff(BuffID.OnFire, 600);
			Projectile.Kill();
		}


		public override void AI()
        {
			Dust.NewDust(Projectile.position, 55, 55, DustID.Torch);
			Dust.NewDust(Projectile.position, 55, 55, DustID.Pixie);
			Dust.NewDust(Projectile.position, 55, 55, DustID.Fireworks);
			Dust.NewDust(Projectile.position, 55, 55, DustID.Shadowflame);
			Dust.NewDust(Projectile.position, 55, 55, DustID.GoldFlame);

			
        }


		SoundStyle ex1 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Creeper/Explosion1")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle ex2 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Creeper/Explosion2")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle ex3 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Creeper/Explosion3")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle ex4 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Creeper/Explosion4")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
	}
}