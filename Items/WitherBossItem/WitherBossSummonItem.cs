using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using MCInvasion.NPCs.WitherBossNPC;

namespace MCInvasion.Items.WitherBossItem
{
	public class WitherBossSummonItem : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Wither Boss Summon Item");
			// Tooltip.SetDefault("Summons Wither Boss");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
		}

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 20;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
		}

		public override bool CanUseItem(Player player) {
			return !NPC.AnyNPCs(ModContent.NPCType<WitherBoss>());
		}

		public override bool? UseItem(Player player) {
			if (player.whoAmI == Main.myPlayer) {
				SoundEngine.PlaySound(SoundID.Roar, player.position);

				int type = ModContent.NPCType<WitherBoss>();

				if (Main.netMode != NetmodeID.MultiplayerClient) {
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else {
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
				}
			}

			return true;
		}

		public override void AddRecipes()
		{
            // TODO: Maybe use item to summon Wither Boss, maybe use struct to summon.

            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Bone, 30);
			recipe.AddIngredient(1508, 5);
			recipe.AddIngredient(172, 10);
			recipe.AddTile(412);
            recipe.Register();
        }
	}
}