using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using MCInvasion.Common.Players;

namespace MCInvasion.NPCs
{
	public class Slime : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Slime");

			Main.npcFrameCount[Type] = 3;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { 
				Velocity = 1f 
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public override void SetDefaults() {
			NPC.width = 33;
			NPC.height = 28;
			NPC.damage = 7;
			NPC.defense = 2;
			NPC.lifeMax = 25;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 10f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = NPCAIStyleID.Slime; 

			AIType = NPCID.BlueSlime; 
			AnimationType = NPCID.BlueSlime; 
			Banner = Item.NPCtoBanner(NPCID.BlueSlime); 
			BannerItem = Item.BannerToItem(Banner); 
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			var slimeDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.BlueSlime, false);
			foreach (var slimeDropRule in slimeDropRules)
			{
				npcLoot.Add(slimeDropRule);
			}
		}

		public override void FindFrame(int frameHeight)
        {
			NPC.spriteDirection = NPC.direction;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.GetModPlayer<MinecraftPlayer>().hasMinecraftEffect == true && spawnInfo.Player.ZoneOverworldHeight && Main.dayTime)
				return 1f;
			else
				return 0f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
		}

		public override void HitEffect(int hitDirection, double damage) {
			for (int i = 0; i < 10; i++) {
				var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.SlimeBunny);

				dust.velocity.X += Main.rand.NextFloat(-0.05f, 0.05f);
				dust.velocity.Y += Main.rand.NextFloat(-0.05f, 0.05f);

				dust.scale *= 1f + Main.rand.NextFloat(-0.03f, 0.03f);
			}
		}
	}
}
