// 记得生成后将文件名修改为 NetherStar.cs
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ModLoader.IO;

namespace MCInvasion.Items.WitherBossItem
{
	public class NetherStar : ModItem
	{

		public override void SetDefaults()
		{

			Item.width = 40;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			//Item.shoot = ModContent.ProjectileType<JetFlame>();  // 发射生成的射弹种类
			Item.noMelee = true;
			Item.value = 10000;
			//Item.useAmmo = ModContent.ItemType<powder>();   //使用的弹药
			
			Item.mana = 0;
			Item.scale = 1f;
			Item.maxStack = 1;
			Item.consumable = false;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nether Star");
			// Tooltip.SetDefault("");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}



		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// 覆盖掉原先的射弹
			return false;
		}



		public override bool CanUseItem(Player player)
		{
			//判断物品能否被使用
			return false;
		}
	}
}

//  本文件由泰拉瑞亚模组T4模板生成，项目地址：https://github.com/GlassCabbage/Terraria-T4-template