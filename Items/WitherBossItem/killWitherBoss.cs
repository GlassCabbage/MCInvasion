using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using MCInvasion.NPCs.WitherBossNPC;

namespace MCInvasion.Items.WitherBossItem
{
	public class killWitherBoss : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Use to kill Wither Boss");
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
			foreach (NPC witherBoss in Main.npc)
				if (witherBoss.type == ModContent.NPCType<WitherBoss>())
					witherBoss.life =0;
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