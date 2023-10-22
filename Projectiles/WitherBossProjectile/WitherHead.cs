using MCInvasion.NPCs.WitherBossNPC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MCInvasion.Projectiles.WitherBossProjectile
{
	public class WitherHead : AbstractWitherHead
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults() {
			Projectile.width = 10; 
			Projectile.height = 10; 
			Projectile.aiStyle = -1; 
			Projectile.friendly = false; 
			Projectile.hostile = true; 
			Projectile.DamageType = DamageClass.Magic; 
			Projectile.penetrate = -1; 
			Projectile.timeLeft = 6000; 
			Projectile.alpha = 255; 
			Projectile.light = 0.5f; 
			Projectile.ignoreWater = true; 
			Projectile.tileCollide = false;
			Projectile.frame = 0;
			Projectile.ai[1] = 0;
		}

		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture;
			if (Projectile.ai[1]==1)
				texture = TextureAssets.Projectile[ModContent.ProjectileType<BlueHead>()].Value;
			else
				texture = TextureAssets.Projectile[ModContent.ProjectileType<BlackHead>()].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}

		public override void OnKill(int timeLeft) {
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			base.OnHitPlayer(target, info);

		}
		int timer = 0;
		public override void AI()
		{
			if (Projectile.alpha>0)
				Projectile.alpha-=10;
			if (Projectile.timeLeft < 10)
			{
				Projectile.damage = 0;
				Projectile.alpha += 35;
			}


		}

	}
}