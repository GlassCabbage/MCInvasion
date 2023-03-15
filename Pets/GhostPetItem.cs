using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using MCInvasion.Pets;

namespace MCInvasion.Pets
{
	public class GhostPetItem : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Ghast Tear");
			Tooltip.SetDefault("Summons a small Ghost to follow you");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.ZephyrFish);

			Item.shoot = ModContent.ProjectileType<GhostPetProjectile>();
			Item.buffType = ModContent.BuffType<GhostPetBuff>();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(Item.buffType, 3600);
			}
		}
	}
}
