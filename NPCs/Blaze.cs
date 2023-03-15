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


namespace MCInvasion.NPCs
{
	public class Blaze : ModNPC
	{
		private enum ActionState
		{
			Judge,
			Close,
			Shoot
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
			DisplayName.SetDefault("Blaze");
			Main.npcFrameCount[NPC.type] = 10;
		}

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.height = 56;
			NPC.damage = 10;
			NPC.defense = 10;
			NPC.lifeMax = 200;
			NPC.DeathSound = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Blaze/Blaze_death")
			{
				Volume = 0.9f,
				PitchVariance = 0.2f,
				MaxInstances = 3,
			};
			NPC.value = 530f;
			NPC.knockBackResist = 0.2f;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.lavaImmune = true;
			NPC.buffImmune[BuffID.OnFire] = true;
		}

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(2701, 10,2,10));
			npcLoot.Add(ItemDropRule.Common(174, 20));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BlazeHead>(), 50));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if (spawnInfo.Player.GetModPlayer<MinecraftPlayer>().hasMinecraftEffect == true && spawnInfo.Player.ZoneUnderworldHeight && Main.hardMode)
                return 0.3f;
            else
                return 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.spriteDirection = NPC.direction;

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

		float highestSpeed = 10;
		float maxRotation = 0.25F;
		float fixx = Main.rand.Next(100)-50;
		float fixy = Main.rand.Next(100) - 50;

		private void close(int dir)
		{
			AI_Timer++;
			AI_Time2--;
			NPC.TargetClosest(true);
			if (NPC.Center.Y - Main.player[NPC.target].Center.Y > -200+fixy)
				NPC.velocity.Y = -2;
			else if (NPC.Center.Y - Main.player[NPC.target].Center.Y < -250+fixy)
				NPC.velocity.Y = 2;
			else
				NPC.velocity.Y = 0;

			if (AI_Time2 < 20)
				NPC.velocity.X = NPC.velocity.X / (float)1.1;
			else
			{
				if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) >= 425f + fixx && ((dir == 1 && NPC.velocity.X < highestSpeed) || (dir == -1 && NPC.velocity.X > -highestSpeed)))
				{
					NPC.velocity.X = NPC.velocity.X + (float)0.1 * dir;
				}
				else if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) < 300f + fixx && ((dir == 1 && NPC.velocity.X > -highestSpeed) || (dir == -1 && NPC.velocity.X < highestSpeed)))
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
			if (AI_Time2<=0 && Math.Abs(NPC.rotation)<0.01)
			{
				NPC.rotation=0;
				AI_Timer = 0;
				NPC.velocity.X = 0;
				NPC.velocity.Y = (float)0.3;
				AI_Time2 = 300;
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
			AI_Timer++;
			if (AI_Timer%5==0)
			Dust.NewDust(NPC.position+new Vector2(0,16),10, 10, DustID.Torch);

			NPC.velocity.X = 0;
			Dust.NewDust(NPC.position + new Vector2(0, 16), 10, 10, DustID.Torch);

			if (AI_Timer > 60)
			{
				if (AI_Timer % 20 == 0)
				{
					SoundEngine.PlaySound(shootsound, NPC.Center);
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, 4F * Main.player[NPC.target].DirectionFrom(NPC.Center), ModContent.ProjectileType<BlazeFireball>(), 35, 10f, Main.myPlayer);
				}
				if (AI_Timer > 121)
				{
					AI_Timer = 0;
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

		SoundStyle hit1= new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Blaze/Blaze_hit1")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle hit2 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Blaze/Blaze_hit2")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle hit3 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Blaze/Blaze_hit3")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle hit4 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Blaze/Blaze_hit4")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		public override void HitEffect(int hitDirection, double damage) {
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
