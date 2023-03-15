using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MCInvasion.Projectiles.GhostHeadProjectiles
{
	public class GhostBoom : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Ghast");
		}

		public override void SetDefaults() {
			Projectile.width = 500;
			Projectile.height = 500;
			Projectile.timeLeft = 5;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
		}












	}
}