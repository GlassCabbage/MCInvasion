
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MCInvasion.Projectiles;

namespace MCInvasion.Items
{
	public class CreeperHead : ModItem
	{
		public override void SetStaticDefaults()
		{
		}

		public override void SetDefaults()
		{
			Item.damage = 500;
			Item.DamageType = DamageClass.Magic;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.rare = ItemRarityID.Pink;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CreeperHead_Projectile>();
			Item.noMelee = true;
			Item.shootSpeed = 20;
			Item.useTime = 120;
			Item.mana = 100;
		}

		// Already no need the following part since there is Item.shoot

		//public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        //{
			//Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			//Projectile.NewProjectile(player.GetSource_FromThis(), player.Center,-7f * player.DirectionFrom(target), ModContent.ProjectileType<CreeperHead_Projectile>(), Item.damage, Item.knockBack, Main.myPlayer);
			//return false;
		//}

	}
}