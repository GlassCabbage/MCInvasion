using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MCInvasion.Projectiles
{
	public class CreeperHead_Projectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Creeper Head");
		}

		public override void SetDefaults() {
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.timeLeft = 600;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.ignoreWater = true;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.alpha = 0;
			Projectile.tileCollide = true;
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
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			for (int i = 0; i <= 120; i++)
			{
				if (i <= 30)
					Dust.NewDust(Projectile.position + new Vector2(0, 16), 10, -10, DustID.Pixie, 5/(float)Math.Tan(3*i-45), -5);
				else if (i > 30 && i <= 60)
					Dust.NewDust(Projectile.position + new Vector2(0, 16), 10, -10, DustID.Pixie, 5, -5 * (float)Math.Tan(3 * i - 45));
				else if (i>60 && i<=90)
					Dust.NewDust(Projectile.position + new Vector2(0, 16), 10, -10, DustID.Pixie, -5 / (float)Math.Tan(3 * i - 45), 5);
				else
					Dust.NewDust(Projectile.position + new Vector2(0, 16), 10, -10, DustID.Pixie, -5, 5 * (float)Math.Tan(3 * i - 45));
			}
			int wichsound = Main.rand.Next(4);
			switch (wichsound)
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
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ElementBall>(), Projectile.damage, 10f, Main.myPlayer);
			Projectile.Kill();
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ElementBall>(), Projectile.damage, 10f, Main.myPlayer);
			Projectile.Kill();
			int wichsound = Main.rand.Next(3);
			switch (wichsound)
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
			for (int i = 0; i <= 120; i++)
			{
				if (i <= 30)
					Dust.NewDust(Projectile.position + new Vector2(0, 16), 10, -10, DustID.Pixie, 5 / (float)Math.Tan(3 * i - 45), -5);
				else if (i > 30 && i <= 60)
					Dust.NewDust(Projectile.position + new Vector2(0, 16), 10, -10, DustID.Pixie, 5, -5 * (float)Math.Tan(3 * i - 45));
				else if (i > 60 && i <= 90)
					Dust.NewDust(Projectile.position + new Vector2(0, 16), 10, -10, DustID.Pixie, -5 / (float)Math.Tan(3 * i - 45), 5);
				else
					Dust.NewDust(Projectile.position + new Vector2(0, 16), 10, -10, DustID.Pixie, -5, 5 * (float)Math.Tan(3 * i - 45));
			}
		}
		public override void AI()
        {
			Projectile.rotation = Projectile.rotation+0.05f;
			Dust.NewDust(Projectile.Center, 1, 1, DustID.Torch);
        }
	}
}