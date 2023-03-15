using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using MCInvasion.Projectiles;
using Terraria.GameContent.Creative;

namespace MCInvasion.Items
{
	public class powder : ModItem
	{
		public override void SetStaticDefaults()
		{

		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.damage = 12;
			Item.shootSpeed = 16f;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = 1;
			Item.rare = ItemRarityID.Gray;
			Item.autoReuse = true;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.ammo = Item.type;
			Item.shoot = ModContent.ProjectileType<GhostFire>();
		}

		public override bool CanUseItem(Player player)
		{
			return false;
		}
	}
}