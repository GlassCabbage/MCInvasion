using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using MCInvasion.Buffs;
using MCInvasion.Items.WitherBossItem;

namespace MCInvasion.Items.consumer
{
	public class FlaskofWither : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useTime = 17;
			Item.value = Item.sellPrice(0,0,5,0);
			Item.rare = ItemRarityID.LightRed;
			Item.autoReuse = false;
			Item.maxStack = 9999;
			Item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			return true;
		}

        public override bool? UseItem(Player player)
        {
			player.AddBuff(ModContent.BuffType<WitherAttackPlayerEffect>(), 60 * 60 * 20);
			return true;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(5);
			recipe.AddIngredient(ItemID.BottledWater, 5);
			recipe.AddIngredient(ModContent.ItemType<NetherStar>(), 1);
			recipe.AddTile(TileID.ImbuingStation);
			recipe.Register();
		}

    }
}