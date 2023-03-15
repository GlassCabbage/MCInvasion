using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using MCInvasion.NPCs;
using Microsoft.Xna.Framework;
using MCInvasion.Projectiles.WitherBossProjectile;

namespace MCInvasion.Items
{
	public class SpawnProjectile : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spawn Wither Projectile");
			Tooltip.SetDefault("use to spawn a wither explosion");
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.value = 10000;
			Item.rare = 2;
			Item.autoReuse = true;
		}

		public override bool CanUseItem(Player player)
		{
			return true;
		}

		public override bool? UseItem(Player player)
		{
			Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			Projectile.NewProjectile(player.GetSource_FromThis(), target, Vector2.Zero, ModContent.ProjectileType<WitherExplosion>(), 0, 0);
			return true;
		}

		public override void AddRecipes()
		{
			// test item

			//Recipe recipe = CreateRecipe();
			//recipe.AddIngredient(ItemID.DirtBlock, 1);
			//recipe.AddTile(TileID.WorkBenches);
			//recipe.Register();
		}
	}
}