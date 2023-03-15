using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace MCInvasion.Items
{
	public class BottleEvil : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Maybe you can let Villager use this to caraft posion");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = 1;
			Item.rare = ItemRarityID.White;
			Item.autoReuse = true;
			Item.maxStack = 9999;
		}

		public override bool CanUseItem(Player player)
		{
			return false;
		}
	}
}