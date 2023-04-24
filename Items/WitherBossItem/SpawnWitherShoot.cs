using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MCInvasion.Projectiles.WitherBossProjectile;

namespace MCInvasion.Items.WitherBossItem
{
	// This Item can only be used after a Wither Boss summoned, or there's not "target"
	// 仅在当次启动存档后召唤过凋零BOSS可以使用，否则会因为不存在攻击对象而报错

	public class SpawnWitherShoot : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Use to spawn a wither shoot");
			Tooltip.SetDefault("TEST ITEM");
		}

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = false;
		}

		public override bool CanUseItem(Player player) {
			return true;
		}

		public override bool? UseItem(Player player) {
			Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);

			// Change the following code to change the Projectile
			// Upper is shoot with aim, lower is track projecile
			// 下面两条启用上面的召唤瞄准攻击，启用下面的启用追踪攻击

			//Projectile.NewProjectile(player.GetSource_FromThis(), target, new Vector2(0, 0), ModContent.ProjectileType<WitherShoot>(), 0, 0);
			Projectile.NewProjectile(player.GetSource_FromThis(), target, new Vector2(10, 10), ModContent.ProjectileType<TrackHead>(), 0, 0);
			return true;
		}


		public override void AddRecipes()
		{
            //Recipe recipe = CreateRecipe();
            //recipe.AddIngredient(ItemID.DirtBlock, 1);
            //recipe.AddTile(TileID.WorkBenches);
            //recipe.Register();
        }
	}
}