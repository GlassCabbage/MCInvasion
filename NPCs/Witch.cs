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
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;


namespace MCInvasion.NPCs
{
	public class Witch : ModNPC
	{
		private enum ActionState
		{
			Judge,
			Close,
			Heal,
			ThrowDamage,
			ThrowPosion,
			ThrowHeal,
			ThrowWeak,
			ThrowRecover,
			Leave
		}

		private enum Frame
		{
			Walk1,
			Walk2,
			Walk3,
			Walk4,
			Walk5,
			Walk6,
			Walk7,
			Walk8,
			Walk9,
			Walk10,
				Walk11,
				Walk12,
				Walk13,
				Walk14,
				Walk15,
				Walk16,
		}

		public ref float AI_State => ref NPC.ai[0];
		public ref float AI_Timer => ref NPC.ai[1];
		public ref float AI_Time2 => ref NPC.ai[2];

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Witch");
			Main.npcFrameCount[NPC.type] = 16;
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
			NPC.friendly = false;
		}

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BottleEvil>(), 1, 1, 5));
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			//TODO check npc area
			float chance = 0;
			int count = 0;
			foreach(NPC npc in Main.npc)
            {
				if (npc.friendly == false)
					count++;
            }
			bool nightCheck = (!Main.dayTime && !spawnInfo.Player.ZoneSkyHeight && !spawnInfo.Player.ZoneUnderworldHeight);
			bool dayCheck = false;
			if (!nightCheck)
				dayCheck = (Main.dayTime && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneDirtLayerHeight));
			if (spawnInfo.Player.GetModPlayer<MinecraftPlayer>().hasMinecraftEffect == true && (dayCheck || nightCheck) && NPC.AnyNPCs(ModContent.NPCType<Witch>()))
				chance = count/100;
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
			if (noMove)
            {
				NPC.frameCounter = 0;
				NPC.frame.Y = (int)Frame.Walk16 * frameHeight;
				return;
			}

			NPC.frameCounter++;

			if (NPC.frameCounter < 10)
			{
				NPC.frame.Y = (int)Frame.Walk1 * frameHeight;
			}
			else if (NPC.frameCounter < 20)
			{
				NPC.frame.Y = (int)Frame.Walk2 * frameHeight;
			}
			else if (NPC.frameCounter < 30)
				NPC.frame.Y = (int)Frame.Walk3 * frameHeight;
			else if (NPC.frameCounter < 40)
				NPC.frame.Y = (int)Frame.Walk4 * frameHeight;
			else if (NPC.frameCounter < 50)
				NPC.frame.Y = (int)Frame.Walk5 * frameHeight;
			else if (NPC.frameCounter < 60)
				NPC.frame.Y = (int)Frame.Walk6 * frameHeight;
			else if (NPC.frameCounter < 70)
				NPC.frame.Y = (int)Frame.Walk7 * frameHeight;
			else if (NPC.frameCounter < 80)
				NPC.frame.Y = (int)Frame.Walk8 * frameHeight;
			else if (NPC.frameCounter < 90)
				NPC.frame.Y = (int)Frame.Walk9 * frameHeight;
			else if (NPC.frameCounter < 100)
				NPC.frame.Y = (int)Frame.Walk10 * frameHeight;
			else if (NPC.frameCounter < 110)
				NPC.frame.Y = (int)Frame.Walk11 * frameHeight;
			else if (NPC.frameCounter < 120)
				NPC.frame.Y = (int)Frame.Walk12 * frameHeight;
			else if (NPC.frameCounter < 130)
				NPC.frame.Y = (int)Frame.Walk13 * frameHeight;
			else if (NPC.frameCounter < 140)
				NPC.frame.Y = (int)Frame.Walk14 * frameHeight;
			else if (NPC.frameCounter < 150)
				NPC.frame.Y = (int)Frame.Walk15 * frameHeight;
			else if (NPC.frameCounter < 160)
				NPC.frame.Y = (int)Frame.Walk16 * frameHeight;
			else
			{
				NPC.frameCounter = 0;
			}

		}

		int posionCount = 0;
		int posionXSpeed = 15;
		bool noMove;
		public override void AI()
		{
			NPC.TargetClosest();
			targetPlayer = Main.player[NPC.target];
			posionCount++;
			if (AI_Timer % 60 == 0 && Main.rand.Next(100) == 0)
            {
				switch (Main.rand.Next(5))
                {
					case 0:
						SoundEngine.PlaySound(ambient1);
						break;
					case 1:
						SoundEngine.PlaySound(ambient2);
						break;
					case 2:
						SoundEngine.PlaySound(ambient3);
						break;
					case 3:
						SoundEngine.PlaySound(ambient4);
						break;
					case 4:
						SoundEngine.PlaySound(ambient5);
						break;
                }
					
            }

			if (posionCount > 240)
            {
                switch (Main.rand.Next(3))
                {
					case 0:
						SoundEngine.PlaySound(throw1);
						break;
					case 1:
						SoundEngine.PlaySound(throw2);
						break;
					case 2:
						SoundEngine.PlaySound(throw3);
						break;
                }


				if (NPC.lifeMax-NPC.life>200)
                {
					//Main.NewText("Self heal");
					explosion = true;
					heal = true;
					int temp = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.Center.DirectionTo(targetPlayer.Center) * 3, ModContent.ProjectileType<WitchPotion>(), 0, 0f);
					Projectile projectile = Main.projectile[temp];
					projectile.ai[0] = posionEncoder();
					explosion = false;
					heal=false;
					posionCount = 0;
					return;
				}

				bool hasTargetNPC = false;
				foreach(NPC npc in Main.npc)
                {
					if (npc.friendly == false && npc.Center.Distance(NPC.Center)<=500 && (npc.life/npc.lifeMax <0.5 ||npc.lifeMax-npc.life > 200) && npc!=NPC && npc.active==true)
                    {
						hasTargetNPC = true;
						break;
                    }
                }
				if (hasTargetNPC)
                {
					float minDis = 999;
					foreach(NPC npc in Main.npc)
                    {
						if (npc.Center.Distance(NPC.Center)<minDis && npc.friendly == false && npc.Center.Distance(NPC.Center) <= 500 && (npc.life / npc.lifeMax < 0.5 || npc.lifeMax - npc.life > 200) && npc!=NPC && npc.active==true)
                        {
							minDis = npc.Center.Distance(NPC.Center);
							targetNPC = npc;
                        }
                    }
					//Main.NewText("heal npc");
					//Main.NewText(targetNPC.FullName);
					float throwHeight = targetNPC.Center.Y - NPC.Center.Y;
					float throwDistance = targetNPC.Center.X - NPC.Center.X;
					float xTime = throwDistance / posionXSpeed;
					float ySpeed = (float)(throwHeight + 1 / 2 * 0.02 * Math.Pow(xTime, 2)) / xTime;
					if (targetNPC.Center.X > NPC.Center.X)
						throwHeal(posionXSpeed, ySpeed);
					else
						throwHeal(-posionXSpeed, -ySpeed);
					posionCount = 0;
					return;
                }

				float playerthrowHeight = targetPlayer.Center.Y - NPC.Center.Y;
				float playerthrowDistance = targetPlayer.Center.X - NPC.Center.X;
				float playerxTime = playerthrowDistance / posionXSpeed;
				float playerySpeed = (float)(playerthrowHeight + 1 / 2 * 0.02 * Math.Pow(playerxTime, 2)) / playerxTime;
				int xSpeed = posionXSpeed;
				if (targetPlayer.Center.X < NPC.Center.X)
                {
					xSpeed = -xSpeed;
                }

				//Main.NewText("attack player");
				if (targetPlayer.Center.X>NPC.Center.X)
					playerySpeed = -playerySpeed;
				switch (Main.rand.Next(5))
                    {
						case 0:
							throwPosion(xSpeed,-playerySpeed);
							break;
						case 1:
							throwFragile(xSpeed, -playerySpeed);
							break;
						case 2:
							throwSlow(xSpeed, -playerySpeed);
							break;
						case 3:
							throwWeak(xSpeed, -playerySpeed);
							break;
					case 4:
						throwDamage(xSpeed, -playerySpeed);
						break;
                    }
				posionCount = 0;
				return;
            }

			if (targetPlayer.Center.X > NPC.Center.X)
				dir = 1;
			else
				dir = -1;
			if (targetPlayer.position.Distance(NPC.position)>600)
            {
				noMove = false;
				close(dir);
            }
			else if (targetPlayer.position.Distance(NPC.position) < 300)
            {
				noMove = false;
				close(-dir);
            }
			else
            {
				noMove = true;
				NPC.velocity.X = 0;
            }
				

			//switch (AI_State)
			//{
			//	case (float)ActionState.Judge:
			//		judge();
			//		break;
			//	case (float)ActionState.Close:
			//		close(dir);
			//		break;
			//}
		}

		private NPC targetNPC;
		private Player targetPlayer;

		bool heal = false;
		bool posion = false;
		bool slow = false;
		bool weak = false;
		bool fragile = false;
		bool recover = false;
		bool froze = false;
		bool burn = false;
		bool stone = false;
		bool iron = false;
		bool fast = false;
		bool damage = false;
		bool luck = false;
		bool explosion = false;
		bool trace = false;
		bool nogravity = false;
		bool golden = false;
		bool echo = false;
		bool born = false;
		bool spread = false;
		bool immune = false;
		bool ghost = false;
		bool longtime = false;
		bool clear = false;
		private int posionEncoder()
        {
			string bin = "1";
			if (heal)
				bin = bin + "1";
			else
				bin = bin + "0";
			if (posion)
				bin = bin + "1";
			else
				bin= bin + "0";
			if (slow)
				bin = bin + "1";
			else
				bin=(bin + "0");
			if (weak)
				bin = bin + "1";
			else
				bin =(bin + "0");
			if (fragile)
				bin = bin + "1";
			else
				bin=(bin + "0");
			if (recover)
				bin = bin + "1";
			else
				bin = (bin + "0");
			if (froze)
				bin = bin + "1";
			else
				bin = bin +"0";
			if (burn)
				bin = bin + "1";
			else
				bin = bin+"0";
			if (stone)
				bin = bin + "1";
			else
				bin = bin + "0";
			if (iron)
				bin = bin + "1";
			else
				bin = bin + "0";
			if (fast)
				bin = bin + "1";
			else
				bin = bin + "0";
			if (damage)
				bin = bin + "1";
			else
				bin = bin + "0";
			if (luck)
				bin = bin + "1";
			else
				bin = bin + "0";
			if (explosion)
				bin = bin + "1";
			else
				bin= bin + "0";
			if (trace)
				bin = bin + "1";
			else
				bin = bin + "0";
			if (nogravity)
				bin = bin + "1";
			else
				bin = bin + "0";
			if (golden)
				bin = bin + "1";
			else
				bin = bin + "0";
			if (echo)
				bin = bin + "1";
			else
				bin = bin + "0";
			if (born)
				bin = bin + "1";
			else
				bin = bin+"0";
			if (spread)
				bin = bin + "1";
			else
				bin = bin + "0";
			if (immune)
				bin = bin + "1";
			else
				bin = bin + "0";
			if (ghost)
				bin = bin + "1";
			else
				bin = bin + "0";
			if (longtime)
				bin = bin + "1";
			else
				bin = bin + "0";
			if (clear)
				bin = bin + "1";
			else
				bin = bin + "0";


			return Convert.ToInt32(bin, 2);
		}
		private void throwDamage(float xSpeed, float ySpeed)
		{
			damage = true;
			int temp = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(xSpeed, ySpeed), ModContent.ProjectileType<WitchPotion>(), 0, 0f);
			Projectile projectile = Main.projectile[temp];
			projectile.ai[0] = posionEncoder();
			damage = false;
		}

		private void throwFragile(float xSpeed, float ySpeed)
        {
			fragile = true;
			int temp = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center,new Vector2(xSpeed,ySpeed), ModContent.ProjectileType<WitchPotion>(), 0, 0f);
			Projectile projectile = Main.projectile[temp];
			projectile.ai[0] = posionEncoder();
			fragile = false;
		}
		private void throwWeak(float xSpeed, float ySpeed)
        {
			weak = true;
			int temp = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(xSpeed, ySpeed), ModContent.ProjectileType<WitchPotion>(), 0, 0f);
			Projectile projectile = Main.projectile[temp];
			projectile.ai[0] = posionEncoder();
			weak = false;
		}
		private void throwSlow(float xSpeed, float ySpeed)
        {
			slow = true;
			int temp = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(xSpeed,ySpeed), ModContent.ProjectileType<WitchPotion>(), 0, 0f);
			Projectile projectile = Main.projectile[temp];
			projectile.ai[0] = posionEncoder();
			slow = false;
		}
		private void throwHeal(float xSpeed,float ySpeed)
        {
			heal = true;
			int temp = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(xSpeed,ySpeed), ModContent.ProjectileType<WitchPotion>(), 0, 0f);
			Projectile projectile = Main.projectile[temp];
			projectile.ai[0] = posionEncoder();
			heal = false;
		}

		private void throwPosion(float xSpeed,float ySpeed)
        {
			posion = true;
			int temp = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(xSpeed,ySpeed), ModContent.ProjectileType<WitchPotion>(), 0, 0f);
			Projectile projectile = Main.projectile[temp];
			projectile.ai[0] = posionEncoder();
			posion = false;
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
				//AI_State = (float)ActionState.Ready;
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

        public override void OnKill()
        {
			switch (Main.rand.Next(3))
            {
				case 0:
					SoundEngine.PlaySound(death1);
					break;
				case 1:
					SoundEngine.PlaySound(death2);
					break;
				case 2:
					SoundEngine.PlaySound(death3);
					break;
            }

			if (Main.rand.NextBool())
				explosion = false;
			if (Main.rand.Next(10) == 0)
            {
				int i = Item.NewItem(NPC.GetSource_FromThis(), NPC.Center, ModContent.ItemType<WitchPotionItem>());
				Item item = Main.item[i];
				string name = "posion";
				string bin = "1";
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "heal " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "posion " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "slow " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "weak " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "fragile " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "recover " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "froze " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "burn " + name;
				}
				else
				{
					bin += "0";
				}

				{
					bin += "0";
				}

				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "iron " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "fast " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "damage " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "luck " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "explosion " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "trace " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "nogravity " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "golden " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "echo " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "born " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "spread " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "immue " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "ghost " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "longtime " + name;
				}
				else
				{
					bin += "0";
				}
				if (Main.rand.NextBool())
				{
					bin += "1";
					name = "clear " + name;
				}
				else
				{
					bin += "0";
				}

				item.stringColor = Convert.ToInt32(bin, 2);
				//item.buffTime = Convert.ToInt32(bin, 2);
				item.SetNameOverride(name);
			}
            
            base.OnKill();
        }



        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime

			});
		}

		
		public override void HitEffect(int hitDirection, double damage) {
            switch (Main.rand.Next(3))
            {
				case 0:
					SoundEngine.PlaySound(hurt1);
					break;
				case 1:
					SoundEngine.PlaySound(hurt2);
					break;
				case 2:
					SoundEngine.PlaySound(hurt3);
					break;
            }
		}

		SoundStyle ambient1 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Witch/Mob.witch.ambient1")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle ambient2 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Witch/Mob.witch.ambient2")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle ambient3 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Witch/Mob.witch.ambient3")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle ambient4 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Witch/Mob.witch.ambient4")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle ambient5 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Witch/Mob.witch.ambient5")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle death1 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Witch/Mob.witch.death1")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle death2 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Witch/Mob.witch.death2")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle death3 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Witch/Mob.witch.death3")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle hurt1 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Witch/Mob.witch.hurt1")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle hurt2 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Witch/Mob.witch.hurt2")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle hurt3 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Witch/Mob.witch.hurt3")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle throw1 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Witch/Mob.witch.throw1")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle throw2 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Witch/Mob.witch.throw2")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle throw3 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Witch/Mob.witch.throw3")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
	}
}
