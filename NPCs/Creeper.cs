using MCInvasion.Common.Players;
using MCInvasion.Items;
using MCInvasion.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;


namespace MCInvasion.NPCs
{
	// 手搓的近战ai，显然这个ai在游戏里的表现还非常智障，对于一些物块的判定还有很大问题

	public class Creeper : ModNPC
	{
		private enum ActionState
		{
			Judge,
			Close,
			Jump,
			Ready,
			Boom
		}

		private enum Frame
		{
			Close1,
			Close2,
			Close3,
			Close4,
			Close5,
			Close6,
			Close7,
			Close8,
			Close9,
			Close10,
		}

		public ref float AI_State => ref NPC.ai[0];
		public ref float AI_Timer => ref NPC.ai[1];
		public ref float AI_Time2 => ref NPC.ai[2];

		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Creeper");
			Main.npcFrameCount[NPC.type] = 10;
		}

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.height = 36;
			NPC.damage = 0;
			NPC.defense = 10;
			if (Main.hardMode==false)
				NPC.lifeMax = 150+Main.rand.Next(20)-10;
			else
				NPC.lifeMax = 400 + Main.rand.Next(20) - 10;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Creeper/Creeper_say1")
			{
				Volume = 0.9f,
				PitchVariance = 0.2f,
				MaxInstances = 3,
			};
			NPC.value = 530f;
			NPC.knockBackResist = 0.2f;
			NPC.aiStyle = -1;
		}

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(167, 2));
			if (Main.hardMode == false)
            {
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<powder>(), 1, 1, 5));
            }
			else
            {
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<powder>(), 1, 10, 20));
			}
			if (Main.hardMode == true)
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CreeperHead>(), 50));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			float chance = 0;
			if (spawnInfo.Player.GetModPlayer<MinecraftPlayer>().hasMinecraftEffect == true && spawnInfo.Player.ZoneOverworldHeight && !Main.dayTime)
				chance = 0.3f;
			else if (spawnInfo.Player.GetModPlayer<MinecraftPlayer>().hasMinecraftEffect == true && (spawnInfo.Player.ZoneDirtLayerHeight || spawnInfo.Player.ZoneRockLayerHeight))
				chance = 0.3f;
			else
				return 0f;
			if (!Main.hardMode)
            {
				chance /= 5;
            }
			return chance;

		}

		public override void FindFrame(int frameHeight)
		{
			NPC.spriteDirection = NPC.direction;

			switch (AI_State)
			{
				case (float)ActionState.Judge:
						NPC.frame.Y = (int)Frame.Close1 * frameHeight;
					break;
				case (float)ActionState.Close:
					NPC.frameCounter++;

					if (NPC.frameCounter < 10)
					{
						NPC.frame.Y = (int)Frame.Close1 * frameHeight;
					}
					else if (NPC.frameCounter < 20)
					{
						NPC.frame.Y = (int)Frame.Close2 * frameHeight;
					}
					else if (NPC.frameCounter<30)
						NPC.frame.Y= (int)Frame.Close3 * frameHeight;
					else if (NPC.frameCounter <40)
						NPC.frame.Y= (int)Frame.Close4 * frameHeight;
					else if (NPC.frameCounter<50)
						NPC.frame.Y= (int)Frame.Close5 * frameHeight;
					else if (NPC.frameCounter<60)
						NPC.frame.Y= (int)Frame.Close6 * frameHeight;
					else if (NPC.frameCounter<70)
						NPC.frame.Y= (int)Frame.Close7 * frameHeight;
					else if (NPC.frameCounter<80)
						NPC.frame.Y= (int)Frame.Close8 * frameHeight;
					else if (NPC.frameCounter<90)
						NPC.frame.Y = (int)Frame.Close9 * frameHeight;
					else if (NPC.frameCounter<100)
						NPC.frame.Y= (int)Frame.Close10 * frameHeight;
					else
					{
						NPC.frameCounter = 0;
					}

					break;
				case (float)ActionState.Ready:
					NPC.frame.Y = (int)Frame.Close1 * frameHeight;
					break;
				case (float)ActionState.Boom:
					NPC.frame.Y = (int)Frame.Close1 * frameHeight;
					break;
				
					
			}
		}
		public override void AI()
		{
			switch (AI_State)
			{
				case (float)ActionState.Judge:
					judge();
					break;
				case (float)ActionState.Close:
					close(dir);
					break;
				case (float)ActionState.Ready:
					ready();
					break;
				case (float)ActionState.Boom:
					boom();
					break;
			}
		}

		private void judge()
		{
			NPC.TargetClosest(true);

			if (Main.player[NPC.target].Center.X - NPC.position.X > 0)
			{
				dir = 1;
				AI_State = (float)ActionState.Close;
			}
			else if (Main.player[NPC.target].Center.X - NPC.position.X < 0)
			{
				dir=-1;
				AI_State= (float)ActionState.Close;
			}
			AI_Timer = 0;
			
		}
		int dir;
		int jumpCD;
		private void close(int dir)
		{
			jumpCD--;
			AI_Timer++;
			NPC.TargetClosest(true);
			if (Main.player[NPC.target].Distance(NPC.Center) >= 100f)
			{
				NPC.velocity.X = (float)2.5 * dir;

				int h;
				int d1x = (int)NPC.Center.X / 16 + NPC.direction;
				int npccenterx = (int)NPC.Center.X / 16;
				int npccentery = (int)NPC.Center.Y / 16;
				bool isOnFloor = WorldGen.SolidTile(npccenterx, (int)(NPC.Center.Y + 18) / 16) && NPC.velocity.Y == 0;
				if (jumpCD<0)
				{
					if (((WorldGen.SolidTile(d1x, npccentery + 1) == true || Main.tile[d1x, npccentery + 1].HasUnactuatedTile == true) || (WorldGen.SolidTile(d1x, npccentery+1) == true || Main.tile[d1x, npccentery+1].HasTile == true) || (WorldGen.SolidTile(d1x, npccentery) == true || Main.tile[d1x, npccentery].HasTile == true) || (WorldGen.SolidTile(d1x, npccentery - 1) == true || Main.tile[d1x, npccentery - 1].HasTile == true)) && isOnFloor)
					{
						//Main.NewText(1);
						h = 2;
						NPC.velocity.Y = -4.5f * h;
					}
					else if ((WorldGen.SolidTile(d1x, npccentery + 1) == true || Main.tile[d1x, npccentery + 1].HasTile == true) && WorldGen.SolidTile(npccenterx, npccentery + 3) && isOnFloor)
					{
						//Main.NewText(3);
						h = 1;
						NPC.velocity.Y = -4 * h;
					}

					if (WorldGen.SolidTile(d1x, npccentery + 3) == false && WorldGen.SolidTile(npccenterx, npccentery + 3 ) == true && WorldGen.SolidTile(npccenterx, npccentery + 4) && isOnFloor)
					{
						//Main.NewText(4);
						h = 2;
						NPC.velocity.Y = -4.5f * h;
					}
					jumpCD = 20;
				}
			}

			if (Main.player[NPC.target].Distance(NPC.Center) < 100f)
			{
				AI_State = (float)ActionState.Ready;
				AI_Timer = 0;
			}
			else
			{
				NPC.TargetClosest(true);

				if (!NPC.HasValidTarget || Main.player[NPC.target].Distance(NPC.Center) > 1000f)
				{
					AI_State = (float)ActionState.Judge;
					AI_Timer = 0;
				}
			}

			if(AI_Timer>20)
            {
				AI_Timer = 0;
				AI_State= (float)ActionState.Judge;
            }
		}

		SoundStyle fuse = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Creeper/Creeper_fuse")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		private void ready()
		{
			AI_Timer++;
			if (AI_Timer == 1)
				SoundEngine.PlaySound(fuse, NPC.Center);
			if (AI_Timer%5==0)
			Dust.NewDust(NPC.position+new Vector2(0,16),10, 10, DustID.Torch);

			NPC.velocity.X = 0;
			Dust.NewDust(NPC.position + new Vector2(0, 16), 10, 10, DustID.Torch);

			int readytime = 300;
			if (Main.hardMode)
				readytime = 120;

			if (AI_Timer >= readytime)
			{
				if (Main.player[NPC.target].Distance(NPC.Center) >= 350f)
				{
					AI_State = (float)ActionState.Close;
					AI_Timer = 0;
				}
				else
				{
					tempx = NPC.Center.X;
					tempy = NPC.Center.Y;
					AI_State = (float)ActionState.Boom;
					AI_Timer = 0;
				}
			}
			
		}

		float tempx;
		float tempy;
		SoundStyle ex1 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Creeper/Explosion1")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle ex2 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Creeper/Explosion2")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle ex3 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Creeper/Explosion3")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle ex4 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Creeper/Explosion4")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
	
		private void boom()
		{
			AI_Timer++;
			// TODO: Use Hurt instead of a projectile.  之后把这块代码改成Hurt来对周围生物造成伤害，增加一个随距离衰减。

			NPC.dontTakeDamage = true;
			//NPCLoader.SpawnNPC(614, (int)NPC.position.X/16, (int)NPC.position.Y/16);
			if (AI_Timer == 1)
			{
				if (Main.getGoodWorld)
				{
					for (int i = -4; i < 4; i++)
					{
						for (int j = -4; j < 4; j++)
						{
							if (Math.Sqrt(Math.Pow(i,2) + Math.Pow(j,2)) <= 3)
							{
								WorldGen.KillTile((int)NPC.Center.X / 16 + i, (int)NPC.Center.Y / 16 + j);
								WorldGen.KillWall((int)NPC.Center.X / 16 + i, (int)NPC.Center.Y / 16 + j);
							}
						}
					}
				}
			}
			if (AI_Timer > 5)
			{
				for (int i = 0; i < 100; i++)
					Dust.NewDust(NPC.position + new Vector2(0, 16), 10, 10, DustID.Torch,50-i,500-10*i);
				if (Main.hardMode==false)
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<ElementBall>(), 50, 10f, Main.myPlayer);
				else
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<ElementBall>(), 100, 10f, Main.myPlayer);
				NPC.life = 0;
				int wichsound = Main.rand.Next(3);
				switch (wichsound)
				{
					case 0:
						SoundEngine.PlaySound(ex1, NPC.Center);
						break;
					case 1:
						SoundEngine.PlaySound(ex2, NPC.Center);
						break;
					case 2:
						SoundEngine.PlaySound(ex3, NPC.Center);
						break;
					default:
						SoundEngine.PlaySound(ex4, NPC.Center);
						break;
				}
			}
			
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
		}

		SoundStyle hit1 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Creeper/Creeper_say1")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle hit2 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Creeper/Creeper_say2")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle hit3 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Creeper/Creeper_say3")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle hit4 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Creeper/Creeper_say4")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		public override void HitEffect(NPC.HitInfo hit) {
			for (int i = 0; i < 10; i++) {
				int dustType = DustID.Torch;
				int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
				Dust dust = Main.dust[dustIndex];
				dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
				dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
				dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
			}
			int wichsound = Main.rand.Next(3);
			switch (wichsound)
			{
				case 0:
					SoundEngine.PlaySound(hit1, NPC.Center);
					break;
				case 1:
					SoundEngine.PlaySound(hit2, NPC.Center);
					break;
				case 2:
					SoundEngine.PlaySound(hit3, NPC.Center);
					break;
				default:
					SoundEngine.PlaySound(hit4, NPC.Center);
					break;
			}
		}


	}
}
