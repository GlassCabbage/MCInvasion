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
using MCInvasion.Common.Players;
using MCInvasion.Items.weapon;

namespace MCInvasion.NPCs
{
	public class SmallGhost : ModNPC
	{
		private enum ActionState
		{
			Judge,
			Close,
			Shoot,
		}

		private enum Frame
		{
			Close1,
			Close2,
			Close3,
			Close4,
			Shoot1,
			Shoot2,
			Shoot3,
			Shoot4,
		}

		public ref float AI_State => ref NPC.ai[0];
		public ref float AI_Timer => ref NPC.ai[1];
		public ref float AI_Time2 => ref NPC.ai[2];

		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Ghost");
			Main.npcFrameCount[NPC.type] = 8;
		}

		public override void SetDefaults() {
			NPC.width = 50;
			NPC.height = 50;
			NPC.damage = 10;
			NPC.defense = 10;
			NPC.lifeMax = 100;
			NPC.DeathSound = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Ghost/Death")
			{
				Volume = 0.9f,
				PitchVariance = 0.2f,
				MaxInstances = 3,
			};
			NPC.value = 300f;
			NPC.knockBackResist = 0.2f;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
		}

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<powder>(), 5,2,5));
			//npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GhostHead>(), 5));
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (NPC.AnyNPCs(ModContent.NPCType<Ghost>()) && spawnInfo.Player.ZoneUnderworldHeight)
				return 1f;
			if (spawnInfo.Player.GetModPlayer<MinecraftPlayer>().hasMinecraftEffect == true && spawnInfo.Player.ZoneUnderworldHeight)
				return 0.3f;
			else
				return 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.spriteDirection = NPC.direction;

			switch (AI_State)
			{
				case (float)ActionState.Shoot:
					NPC.frameCounter++;

					if (NPC.frameCounter < 10)
					{
						NPC.frame.Y = (int)Frame.Shoot1 * frameHeight;
					}
					else if (NPC.frameCounter < 20)
					{
						NPC.frame.Y = (int)Frame.Shoot2 * frameHeight;
					}
					else if (NPC.frameCounter < 30)
						NPC.frame.Y = (int)Frame.Shoot3 * frameHeight;
					else if (NPC.frameCounter < 40)
						NPC.frame.Y = (int)Frame.Shoot4 * frameHeight;
					else if (NPC.frameCounter < 50)
						NPC.frame.Y = (int)Frame.Shoot3 * frameHeight;
					else if (NPC.frameCounter < 60)
						NPC.frame.Y = (int)Frame.Shoot2 * frameHeight;
					else
					{
						NPC.frameCounter = 0;
					}
					break;
				default:
					NPC.frameCounter++;

					if (NPC.frameCounter < 10)
					{
						NPC.frame.Y = (int)Frame.Close1 * frameHeight;
					}
					else if (NPC.frameCounter < 20)
					{
						NPC.frame.Y = (int)Frame.Close2 * frameHeight;
					}
					else if (NPC.frameCounter < 30)
						NPC.frame.Y = (int)Frame.Close3 * frameHeight;
					else if (NPC.frameCounter < 40)
						NPC.frame.Y = (int)Frame.Close4 * frameHeight;
					else if (NPC.frameCounter < 50)
						NPC.frame.Y = (int)Frame.Close3 * frameHeight;
					else if (NPC.frameCounter < 60)
						NPC.frame.Y = (int)Frame.Close2 * frameHeight;
					else
					{
						NPC.frameCounter = 0;
					}
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
				case (float)ActionState.Shoot:
					shoot();
					break;
			}
		}

		private void judge()
		{
			AI_Time2 --;
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

		float highestSpeed = 5;
		float maxRotation = 0.25F;
		float closeDistance;
		float leaveDistance;
		float fixx = Main.rand.Next(20) - 10;
		float fixy = Main.rand.Next(20) - 10;
		private void close(int dir)
		{
			AI_Timer++;
			AI_Time2--;
			if (AI_Time2>120 || (NPC.Center.Y - Main.player[NPC.target].Center.Y > -250 + fixy || NPC.Center.Y - Main.player[NPC.target].Center.Y < -300 + fixy))
            {
				closeDistance = 250;
				leaveDistance = 200;
            }
			else
            {
				closeDistance = 200;
				leaveDistance = 150;
            }

			if (AI_Timer%60==0 && Main.rand.Next(100) == 0)
            {
				switch (Main.rand.Next(7)) {
					case 1:
						SoundEngine.PlaySound(RandSound1,NPC.Center);
						break;
						case 2:
						SoundEngine.PlaySound(RandSound2,NPC.Center);
						break;
					case 3:
						SoundEngine.PlaySound(RandSound3,NPC.Center);
						break;
					case 4:
						SoundEngine.PlaySound(RandSound4,NPC.Center);
						break;
					case 5:
						SoundEngine.PlaySound(RandSound5,NPC.Center);
						break;
					case 6:
						SoundEngine.PlaySound(RandSound6,NPC.Center);
						break;
					default:
						SoundEngine.PlaySound(RandSound7,NPC.Center);
						break;
				}
            }

			NPC.TargetClosest(true);
			if (NPC.Center.Y - Main.player[NPC.target].Center.Y > -100+fixy)
				NPC.velocity.Y = -2;
			else if (NPC.Center.Y - Main.player[NPC.target].Center.Y < -150+fixy)
				NPC.velocity.Y = 2;
			else
				NPC.velocity.Y = 0;

			if (AI_Time2 < 20 && (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) < closeDistance + fixx) && Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) >= leaveDistance)
				NPC.velocity.X = NPC.velocity.X / (float)1.1;
			else
			{
				if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) >= closeDistance + fixx && ((dir == 1 && NPC.velocity.X < highestSpeed) || (dir == -1 && NPC.velocity.X > -highestSpeed)))
				{
					NPC.velocity.X = NPC.velocity.X + (float)0.1 * dir;
				}
				else if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) < leaveDistance + fixx && ((dir == 1 && NPC.velocity.X > -highestSpeed) || (dir == -1 && NPC.velocity.X < highestSpeed)))
				{
					NPC.velocity.X = NPC.velocity.X - (float)0.1 * dir;
				}
				else
				{
					NPC.velocity.X = NPC.velocity.X / (float)1.1;
				}
			}

			if (NPC.velocity.X > 1 && NPC.rotation < maxRotation)
				NPC.rotation = NPC.rotation + (float)0.01;
			else if (NPC.velocity.X < -1 && NPC.rotation > -maxRotation)
				NPC.rotation = NPC.rotation - (float)0.01;
            if (Math.Abs(NPC.velocity.X) < 1 && Math.Abs(NPC.rotation) > 0)
                NPC.rotation = NPC.rotation /(float)1.2;


            if (AI_Timer>20)
            {
				AI_Timer = 0;
				AI_State= (float)ActionState.Judge;
            }
			if (AI_Time2<=0 && Math.Abs(NPC.rotation)<0.01 && (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) <= closeDistance + fixx) && Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) >= leaveDistance && NPC.Center.Y - Main.player[NPC.target].Center.Y <= -100 + fixy && NPC.Center.Y - Main.player[NPC.target].Center.Y >= -150 + fixy)
			{
				NPC.rotation=0;
				AI_Timer = 0;
				NPC.velocity.X = 0;
				AI_State = (float)ActionState.Shoot;
			}

		}

		SoundStyle shootsound= new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Blaze/Ghast_fireball4")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		private void shoot()
		{
			int hold = 60;
			NPC.velocity.Y = -0.1f;
			AI_Timer++;
			if (AI_Timer < hold) { 
				if (AI_Timer % 5 == 0)
					for (int i = -15; i < 15; i++)
					{
						Vector2 face = new Vector2(16 * NPC.direction, 0);
						if (Main.rand.Next(10) == 0)
						{
							Vector2 proPosi = new Vector2(NPC.Center.X + 10 * i, NPC.Center.Y + (float)Math.Sqrt(22500 - Math.Pow(10 * i, 2)));
							Projectile.NewProjectile(NPC.GetSource_FromThis(), proPosi + face, NPC.DirectionFrom(proPosi) / 5, ModContent.ProjectileType<GhostFire>(), 0, 0);
						}
						if (Main.rand.Next(10) == 0)
						{
							Vector2 negproPosi = new Vector2(NPC.Center.X + 10 * i, NPC.Center.Y - (float)Math.Sqrt(22500 - Math.Pow(10 * i, 2)));
							Projectile.NewProjectile(NPC.GetSource_FromThis(), negproPosi + face, NPC.DirectionFrom(negproPosi) / 5, ModContent.ProjectileType<GhostFire>(), 0, 0);
						}
					}
			}
			NPC.velocity.X = 0;
			if (AI_Timer == hold+93)
				SoundEngine.PlaySound(Ready, NPC.Center);

			if (AI_Timer > hold+120)
			{
				if (AI_Timer < hold+140)
					NPC.rotation -= 0.01f*NPC.direction*(1-(AI_Timer-(hold+120))/20);
				else if (AI_Timer < hold+155 && AI_Timer>=hold+150)
				{
					NPC.rotation += 0.2f*NPC.direction;
					NPC.velocity.Y++;
				}
				if (AI_Timer == hold+153)
				{
					SoundEngine.PlaySound(shootsound, NPC.Center);
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, 5F * Main.player[NPC.target].DirectionFrom(NPC.Center), ModContent.ProjectileType<Fireball>(), (int)NPC.damage/3, 0f, Main.myPlayer);
				}
				if (AI_Timer > hold+155)
				{
					AI_Timer = 0;
					AI_Time2 = 800;
					//if (Main.getGoodWorld)
					//	AI_Time2 = 300;
					AI_State = (float)ActionState.Judge;
				}
			}
			
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld
			});
		}

		SoundStyle hit1= new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Ghost/Hit1")
		{
			Volume = 0.7f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle hit2 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Ghost/Hit2")
		{
			Volume = 0.7f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle hit3 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Ghost/Hit3")
		{
			Volume = 0.7f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle hit4 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Ghost/Hit4")
		{
			Volume = 0.7f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle hit5 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Ghost/Hit5")
		{
			Volume = 0.7f,
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
			int wichsound = Main.rand.Next(5);
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
					case 3:
					SoundEngine.PlaySound(hit4, NPC.Center);
					break;
					default:
					SoundEngine.PlaySound(hit5, NPC.Center);
					break;
            }
		}
		SoundStyle RandSound1 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Ghost/Rand1")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle RandSound2 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Ghost/Rand2")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle RandSound3 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Ghost/Rand3")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle RandSound4 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Ghost/Rand4")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle RandSound5 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Ghost/Rand5")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle RandSound6 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Ghost/Rand6")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle RandSound7 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Ghost/Rand7")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle Ready = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Ghost/Ready")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
	}
}
