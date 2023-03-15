using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using MCInvasion.Common.Players;

namespace MCInvasion.NPCs
{
	public class Zombie : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Zombie");

			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { 
				Velocity = 1f 
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public override void SetDefaults() {
			NPC.width = 38;
			NPC.height = 48;
			NPC.damage = 14;
			NPC.defense = 6;
			NPC.lifeMax = 200;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 3; 

			AIType = NPCID.FaceMonster; 
			AnimationType = NPCID.Zombie; 
			Banner = Item.NPCtoBanner(NPCID.Zombie); 
			BannerItem = Item.BannerToItem(Banner); 
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			
			var zombieDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.Zombie, false); // false is important here
			foreach (var zombieDropRule in zombieDropRules) {
				npcLoot.Add(zombieDropRule);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.GetModPlayer<MinecraftPlayer>().hasMinecraftEffect == true && spawnInfo.Player.ZoneOverworldHeight && !Main.dayTime)
				return 0.3f;
			else if (spawnInfo.Player.GetModPlayer<MinecraftPlayer>().hasMinecraftEffect == true && (spawnInfo.Player.ZoneDirtLayerHeight || spawnInfo.Player.ZoneRockLayerHeight))
				return 0.3f;
			else
				return 0f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
			});
		}

		public override void HitEffect(int hitDirection, double damage) {
			for (int i = 0; i < 10; i++) {
				var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Blood);

				dust.velocity.X += Main.rand.NextFloat(-0.05f, 0.05f);
				dust.velocity.Y += Main.rand.NextFloat(-0.05f, 0.05f);

				dust.scale *= 1f + Main.rand.NextFloat(-0.03f, 0.03f);
			}
		}
	}
}
