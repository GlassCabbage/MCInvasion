using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.DataStructures;
using System.Collections.Generic;
using ReLogic.Content;
using Terraria.ModLoader.IO;
using MCInvasion.NPCs;
using MCInvasion.Common.Players;

namespace MCInvasion.NPCs
{
	// 使用了Github上的Example Mod汉化项目
	// [AutoloadHead] 和 NPC.townNPC 都是必须的，缺失会导致城镇NPC不能正常生效
	[AutoloadHead]
	public class Villager : ModNPC
	{
		public int NumberOfTimesTalkedTo = 0;

		public override void SetStaticDefaults() {
			// NPC显示的名字会自动从本地化文件（localization files）中选取
			Main.npcFrameCount[Type] = 25; // NPC的贴图帧数

			NPCID.Sets.ExtraFramesCount[Type] = 9; // 城镇NPC的额外帧数量，比如坐在椅子上与其他NPC交谈
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700; // 城镇NPC的索敌范围（像素）
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90; // 从NPC攻击开始至一次攻击结束的时间（90帧=1.5秒）
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 4; // 进入派对状态时派对帽子的y值偏移

			// 影响NPC在生物图鉴中的展示状态
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				Velocity = 1f, // 在生物图鉴中NPC以+1图格的速度前进（向右）
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			// 使用NPCHappiness来设置NPC的群系偏好。你可以在本地化文件中为NPC添加不同愉悦度的交谈文本（详见本地化文件 MCInvasion/Localization/zh-Hans.hjson）
			// 注意：下面使用了链式代码来设置NPC的幸福属性，由于SetXAffection总是返回一个NPCHappiness实例，所以可以使用链式代码这一技巧
			NPC.Happiness
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Like) 
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike) // 不喜欢雪地
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Love) 
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Love) 
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Like) // 喜欢向导
				.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Dislike) 
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate) // 讨厌爆破专家
			; // 👈别忘了这个分号
		}

		public override void SetDefaults() {
			NPC.townNPC = true; // 将NPC标记为城镇NPC
			NPC.friendly = true; // NPC不会攻击玩家
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// 我们使用AddRange而不是多次使用Add来添加多个条目
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// 设置生物图鉴中展示NPC的背景
				// 一般来说我们选择城镇NPC最喜欢的环境作为他的展示背景
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// 设置生物图鉴中的描述文字
				new FlavorTextBestiaryInfoElement("The villager approciate you for avoid witch's monopoly about posion craft"),

				// 如果你想的话可以加更多东西
				// 使用本地化文件进行翻译 (Localization/zh-Hans.hjson)
				new FlavorTextBestiaryInfoElement("Mods.MCInvasion.Bestiary.Villager")
			});
		}

		public override void HitEffect(NPC.HitInfo hit) {
			
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */ { // 判断城镇NPC生成条件
			for (int k = 0; k < 255; k++) {
				Player player = Main.player[k];
				if (!player.active) {
					continue;
				}

				// 测试性内容
				if (!player.GetModPlayer<MinecraftPlayer>().hasMinecraftEffect)
                {
					continue;
                }

				NPC npc;
				npc = ModContent.GetModNPC(ModContent.NPCType<Witch>()).NPC;
				if (Main.BestiaryTracker.Kills.GetKillCount(npc)>0) {
					return true;
				}
			}

			return false;
		}


		public override ITownNPCProfile TownNPCProfile() {
			return new VillagerProfile();
		}

		public override List<string> SetNPCNameList() {
			return new List<string>() {
				"亻",
				"尔",
				"女",
				"子"
			};
		}

		public override void FindFrame(int frameHeight) {
			/*npc.frame.Width = 40;
			if (((int)Main.time / 10) % 2 == 0)
			{
				npc.frame.X = 40;
			}
			else
			{
				npc.frame.X = 0;
			}*/
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

			int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
			if (partyGirl >= 0 && Main.rand.NextBool(4)) {
				chat.Add(Language.GetTextValue("Mods.MCInvasion.Dialogue.Villager.PartyGirlDialogue", Main.npc[partyGirl].GivenName));
			}
			// 这是NPC的交谈内容，这里使用了key，指向了本地化文件，查看 Localization/zh-Hans.hjson 了解具体格式要求
			chat.Add(Language.GetTextValue("Mods.MCInvasion.Dialogue.Villager.StandardDialogue1"));
			chat.Add(Language.GetTextValue("Mods.MCInvasion.Dialogue.Villager.StandardDialogue2"));
			chat.Add(Language.GetTextValue("Mods.MCInvasion.Dialogue.Villager.StandardDialogue3"));
			chat.Add(Language.GetTextValue("Mods.MCInvasion.Dialogue.Villager.CommonDialogue"), 5.0);
			chat.Add(Language.GetTextValue("Mods.MCInvasion.Dialogue.Villager.RareDialogue"), 0.1);

			//NumberOfTimesTalkedTo++;
			//if (NumberOfTimesTalkedTo >= 10) {
			//	// 这个 NumberOfTimesTalkedTo 计数器是绑定在NPC身上的，如果NPC寄了，新的NPC是一个新的个体，就会丢失掉这个计数器
			//	chat.Add(Language.GetTextValue("Mods.MCInvasion.Dialogue.Villager.TalkALot"));
			//}

			return chat; // 上面的那些key会在游戏中被自动转换为本地化文件 Localization/zh-Hans.hjson 中的字符串
		}

		public override void SetChatButtons(ref string button, ref string button2) { // 设置聊天UI中的交谈按钮位置
			button = Language.GetTextValue("LegacyInterface.28");
			button2 = "酿造";
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shopName) {
			if (firstButton) {
				// 这里我们设计3种不同的聊天按钮，我们使用 HasItem 这一条件判断来决定 一号按钮 是打开商店还是将蜂巢背包升级为蜂窝

				//if (Main.LocalPlayer.HasItem(ItemID.HiveBackpack)) {
				//	SoundEngine.PlaySound(SoundID.Item37); // 播放重铸装备音效

				//	// 设置交谈文本，告知玩家你的装备被我敲了
				//	Main.npcChatText = $"I upgraded your {Lang.GetItemNameValue(ItemID.HiveBackpack)} to a {Lang.GetItemNameValue(ModContent.ItemType<WaspNest>())}";

				//	// 找到蜂巢背包在玩家背包中的位置
				//	int hiveBackpackItemIndex = Main.LocalPlayer.FindItem(ItemID.HiveBackpack);
				//	var entitySource = NPC.GetSource_GiftOrReward();

				//	// 删掉蜂巢背包，然后赐予玩家蜂窝
				//	Main.LocalPlayer.inventory[hiveBackpackItemIndex].TurnToAir();
				//	Main.LocalPlayer.QuickSpawnItem(entitySource, ModContent.ItemType<WaspNest>());

				//	return;
				//}

				//shop = true;
			}
		}

		

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			//npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ExampleCostume>()));
		}

		//// 使国王雕像或者女王雕像触发时把这个NPC传送过去
		//public override bool CanGoToStatue(bool toKingStatue) => true;

		//// 当NPC被传送到雕像时触发一些内容（下面的这段代码），注意这些内容仅在服务端调用，任何视觉上的效果（如尘埃）需要手动在所有客户端进行同步
		//public override void OnGoToStatue(bool toKingStatue) {
		//	// 这里if的内容就是在服务端执行的
		//	if (Main.netMode == NetmodeID.Server) {
		//		ModPacket packet = Mod.GetPacket();
		//		packet.Write((byte)MCInvasion.MessageType.ExampleTeleportToStatue);
		//		packet.Write((byte)NPC.whoAmI);
		//		packet.Send();
		//	}
		//	else {
		//		// 这个else的内容就是在客户端执行的视觉效果
		//		StatueTeleport();
		//	}
		//}

		//// 当NPC传送时在NPC周围创造一块正方形像素块
		//public void StatueTeleport() {
		//	for (int i = 0; i < 30; i++) {
		//		Vector2 position = Main.rand.NextVector2Square(-20, 21);
		//		if (Math.Abs(position.X) > Math.Abs(position.Y)) {
		//			position.X = Math.Sign(position.X) * 20;
		//		}
		//		else {
		//			position.Y = Math.Sign(position.Y) * 20;
		//		}

		//		Dust.NewDustPerfect(NPC.Center + position, ModContent.DustType<Sparkle>(), Vector2.Zero).noGravity = true;
		//	}
		//}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
			cooldown = 30;
			randExtraCooldown = 30;
		}

		// 以下注释代码内容在当前版本还未实现，可能在未来版本可以使用
		// public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
		// 	projType = ProjectileType<SparklingBall>();
		// 	attackDelay = 1;
		// }

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
			multiplier = 12f;
			randomOffset = 2f;
		}
	}

	public class VillagerProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) {
			if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
				return ModContent.Request<Texture2D>("MCInvasion/NPCs/Villager");

			if (npc.altTexture == 1)
				return ModContent.Request<Texture2D>("MCInvasion/NPCs/Villager_Party");

			return ModContent.Request<Texture2D>("MCInvasion/NPCs/Villager");
		}

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("MCInvasion/NPCs/Villager_Head");
	}
}