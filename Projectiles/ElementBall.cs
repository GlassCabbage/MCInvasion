using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MCInvasion.Projectiles
{
	public class ElementBall : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Creeper");
		}

		public override void SetDefaults() {
			Projectile.width = 500;
			Projectile.height = 500;
			Projectile.timeLeft = 5;
			Projectile.hostile = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
		}












	}
}