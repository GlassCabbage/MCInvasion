using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using MCInvasion.NPCs;

namespace MCInvasion.Items
{
	public class SpawnSkeleton : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spawn Skeleton");
			// Tooltip.SetDefault("use to spawn a skeleton");
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
			NPCLoader.SpawnNPC(ModContent.NPCType<Skeleton>(), (int)((Main.mouseX + player.position.X + (-Main.screenWidth / 2))/16), (int)((Main.mouseY + player.position.Y - Main.screenHeight / 2))/16);
			return true;
		}

		public override void AddRecipes()
		{
			// TEST ITEM

			//Recipe recipe = CreateRecipe();
			//recipe.AddIngredient(ItemID.DirtBlock, 1);
			//recipe.AddTile(TileID.WorkBenches);
			//recipe.Register();
		}
	}
}