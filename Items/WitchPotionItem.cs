using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using MCInvasion.NPCs;
using Microsoft.Xna.Framework;
using MCInvasion.Projectiles;
using Terraria.ModLoader.IO;

namespace MCInvasion.Items
{
	public class WitchPotionItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Potion Item");
			Tooltip.SetDefault("use to throw a bottle of potion");
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
			Item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			return true;
		}

		int effect;
		public override void SaveData(TagCompound tag)
		{
			// TODO: Maybe a better way not use Item.stringColor
			tag.Add("posionEffect", Item.stringColor);
			tag.Add("name", Item.Name);
		}

		public override void LoadData(TagCompound tag)
		{
			Item.stringColor = tag.GetInt("posionEffect");
			Item.SetNameOverride(tag.GetString("name"));
		}

		public override bool? UseItem(Player player)
		{
			//TagCompound tag=Item.SerializeData();
			//effect = tag.GetInt("posionEffect");
			//Main.NewText(effect);
			Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			int temp = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, player.DirectionTo(target)*5f, ModContent.ProjectileType<WitchPotion>(), 0, 0f);
			Projectile projectile = Main.projectile[temp];
			projectile.ai[0] = Item.stringColor;
			return true;
		}

		public override void AddRecipes()
		{
			// TEST ONLY
            //Recipe recipe = CreateRecipe();
            //recipe.AddIngredient(ItemID.DirtBlock, 1);
            //recipe.AddTile(TileID.WorkBenches);
            //recipe.Register();
        }
    }
}