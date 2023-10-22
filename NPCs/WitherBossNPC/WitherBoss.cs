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
using MCInvasion.Content.BossBars;
using System.Collections.Generic;
using MCInvasion.Projectiles.WitherBossProjectile;
using Terraria.DataStructures;
using MCInvasion.Buffs;

namespace MCInvasion.NPCs.WitherBossNPC
{
	[AutoloadBossHead]
	public class WitherBoss : ModNPC
	{
		public static Player target;
		private int shootDamage = 20;
		private int screenDamage = 20;
		private int trackDamage = 20;
		private int damage = 0;
		private int spawnTime = 200;
		private enum ActionState
		{
			Spawn,
			Judge,
			Appear,
			Rush,
			Shoot,
			Screen,
			ChangeState,
			Track,
			Dead
		}

		public static bool isSecondStage = false;

		private enum Frame
		{
			Close1,
			Close2,
			Close3,
			Close4,
			Dead1,
			Dead2,
			Dead3,
			Dead4,
		}

		public ref float AI_State => ref NPC.ai[0];
		public ref float AI_Timer => ref NPC.ai[1];
		public ref float ServeNum => ref NPC.ai[2];
		public ref float RemainingShields => ref NPC.localAI[2];

		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Wither Boss");
			// 凋零似乎有了新的攻击动画，不过在改动前得看看碰撞箱与贴图相对坐标原点的变化
			Main.npcFrameCount[NPC.type] = 8;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			// TODO: Make some buff more maybe
			NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
		}

		public override void SetDefaults() {
			NPC.width = 55;
			NPC.height = 108;
			NPC.damage = 0;
			NPC.defense = 70;
			NPC.lifeMax = 140000;
			NPC.knockBackResist =  0f;
			NPC.value = 0f;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.lavaImmune = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.noTileCollide = true;

			NPC.boss = true;
			NPC.npcSlots = 10f;
			NPC.dontTakeDamage = true;
			NPC.BossBar = ModContent.GetInstance<WitherBossBossBar>();
			if (!Main.dedServ)
			{
				Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/witherFight");
			}
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
				new FlavorTextBestiaryInfoElement("Wither Boss")
			});
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{

		}

		public override void OnKill()
		{

			// TODO: 进行一个原地爆炸然后在原地留下一个下界之星

			//NPC.SetEventFlagCleared(ref DownedBossSystem.downedMinionBoss, -1);

			/*
			if (Main.netMode == NetmodeID.Server) {
				NetMessage.SendData(MessageID.WorldData);
			}
			*/
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			// TODO: 整一个宝藏袋，再整一个战利品
		}



		public override void FindFrame(int frameHeight)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.frameCounter++;
			if (!isSecondStage)
			{
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
				else
				{
					NPC.frameCounter = 0;
				}
			}
			else
			{
				if (NPC.frameCounter < 10)
				{
					NPC.frame.Y = (int)Frame.Dead1 * frameHeight;
				}
				else if (NPC.frameCounter < 20)
				{
					NPC.frame.Y = (int)Frame.Dead2 * frameHeight;
				}
				else if (NPC.frameCounter < 30)
					NPC.frame.Y = (int)Frame.Dead3 * frameHeight;
				else if (NPC.frameCounter < 40)
					NPC.frame.Y = (int)Frame.Dead4 * frameHeight;
				else
				{
					NPC.frameCounter = 0;
				}
			}
		}


		private bool startChange = false;
		public override void AI()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}

			if (!isSecondStage && NPC.life <= NPC.lifeMax/2 &&  AI_State!=(float)ActionState.Spawn && !startChange)
            {
				startChange = true;
				AI_Timer = 0;
				AI_State = (float)ActionState.ChangeState;
            }

			if (isSecondStage)
            {
				secondStage();
            }

			Player player = Main.player[NPC.target];
			target = player;

			if (player.dead)
			{
				NPC.velocity.Y -= 0.04f;
				NPC.EncourageDespawn(10);
				return;
			}
			rushCD++;
			screenCD++;
			shootCD++;
			trackCD++;
			switch (AI_State)
			{
				case (float)ActionState.Spawn:
					spawn();
					break;
				case (float)ActionState.Dead:
					dead();
					break;
				case (float)ActionState.Judge:
					judge(player);
					break;
				case (float)ActionState.Appear:
					appear(player);
					break;
				case (float)ActionState.Rush:
					rush(player);
					break;
				case (float)ActionState.Screen:
					screen(player);
					break;
				case (float)ActionState.Shoot:
					shoot(player);
					break;
				case (float)ActionState.Track:
					track();
					break;
				case (float)ActionState.ChangeState:
					changeState();
					break;
			}
		}

		private Vector2 loc;
		private int trackNum = 5;
		private int trackCD = 0;
		private void track()
        {
			AI_Timer++;

			if (target.Center.X < NPC.Center.X)
				NPC.direction = -1;
			else
				NPC.direction = 1;
			if (Math.Sqrt(Math.Pow(NPC.Center.X - target.Center.X, 2) + Math.Pow(NPC.Center.Y - target.Center.Y, 2)) > 500 || isAppear)
			{
				isAppear = true;
				NPC.damage = 0;
				appearTimer++;
				NPC.dontTakeDamage = true;
				if (appearTimer <= 25)
					NPC.alpha += 10;
				if (appearTimer == 25)
				{
					NPC.Center = target.Center - getPositivePosition(300);
				}
				else if (appearTimer > 25)
				{
					NPC.alpha -= 10;
					if (NPC.alpha < 1)
					{
						appearTimer = 0;
						NPC.damage = damage;
						NPC.dontTakeDamage = false;
						NPC.alpha = 0;
						isAppear = false;
					}
				}
			}


			/*
			if (AI_Timer==1)
            {
				trackCD = 0;
				if (NPC.Center.X > target.Center.X)
                {
					loc = new Vector2(450,-200);
                }else
                {
					loc = new Vector2(-450, -200);
                }
            }
			NPC.position=target.Center+loc;
			*/
			if (AI_Timer%300 == 10)
            {
				for (int i =0;i<trackNum;i++)
                {
					float r = 2*(float)Math.PI / trackNum;

					spawnTrack(new Vector2(15*(float)Math.Sin(r * i), 15*(float)Math.Cos(r * i)));
                }
            }
			if (AI_Timer >= 1200)
            {
				trackCD = 0;
				AI_Timer = 0;
				AI_State = (float)ActionState.Judge;
				foreach (Projectile i in Main.projectile)
                {
					if (i.type == ModContent.ProjectileType<TrackHead>())
						i.timeLeft = 60;
                }
            }
        }


		private int secondTimer = 0;
		private DamageClass damageClass;
		private void secondStage()
        {
			secondTimer++;
			if (secondTimer==1)
            {
				switch(Main.rand.Next(4))
                {
					case 0:
						damageClass = DamageClass.Melee;
						break;
					case 1:
						damageClass = DamageClass.Ranged;
						break;
					case 2:
						damageClass = DamageClass.Magic;
						break;
					case 3:
						damageClass = DamageClass.Summon;
						break;
                }
            }

			if (Main.getGoodWorld)
            {
				foreach (Projectile i in Main.projectile)
				{
					if (i.friendly && i.DamageType != damageClass)
					{
						i.Kill();
					}
				}
			}
            else
            {
				foreach (Projectile i in Main.projectile)
				{
					if (i.friendly && i.DamageType == damageClass)
					{
						i.Kill();
					}
				}
			}

			if (secondTimer >= 900 && !Main.getGoodWorld)
            {
				secondTimer = 0;
            }
			else if(secondTimer >= 1800)
            {
				secondTimer = 0;
            }
			
        }

		private void changeState()
        {
			AI_Timer++;
			if (isSecondStage)
			foreach(Projectile i in Main.projectile){
				if (i.friendly)
                {
					i.Kill();
                }
            }
			if (AI_Timer == 1)
            {
				NPC.velocity = Vector2.Zero;
				NPC.rotation = 0;
				NPC.dontTakeDamage = true;
				NPC.damage = damage;
				screenDistance = 660;
				//rushSpeed = 40;
				shootDistance = 375;
				rushCount = 0;
				trackNum = 8;
				
			}
			if (AI_Timer<10)
            {
				NPC.alpha += 30;
				return;
            }
			else if(AI_Timer==10)
            {
				NPC.alpha = 255;
				NPC.Center = target.Center - getPositivePosition(50);
            }
			else if(AI_Timer<=20)
            {
				NPC.alpha -= 30;
				if (NPC.alpha<=0)
					NPC.alpha=0;
            }
			else if (AI_Timer==120)
            {
				isSecondStage = true;
			}
			else if (AI_Timer <= 600)
            {
				if (AI_Timer % 5 == 0)
                {
					NPC.alpha++;
					NPC.position.Y -= 1;
				}
            }
            else
            {
				NPC.damage = damage;
				NPC.dontTakeDamage = false;
				AI_Timer = 0;
				AI_State = (float)ActionState.Appear;
            }
			
			

			
			

        }

		private void spawn()
        {
			AI_Timer++;
			NPC.velocity = Vector2.Zero;

			if (AI_Timer==1)
            {
				isSecondStage= false;
				isAppear= false;
				NPC.position = target.position;
            }

			if (AI_Timer < spawnTime)
			{
				if ((int)(NPC.lifeMax * (AI_Timer) / spawnTime) <= 1)
				{
					NPC.life = 1;
				}
				else
				{
					NPC.life = (int)(NPC.lifeMax * (AI_Timer) / spawnTime);
					NPC.life = (int)(NPC.lifeMax * (AI_Timer) / spawnTime);
					NPC.scale = 0.7f + ((AI_Timer) / spawnTime);
				}
				NPC.dontTakeDamage = true;
				NPC.damage = 0;
			}
			else
			{
				NPC.damage = damage;
				NPC.dontTakeDamage = false;
				NPC.life = NPC.lifeMax;
			}

			if (AI_Timer>spawnTime)
            {
				int dustType = 148;
				for (int i = 0; i <= 120; i++)
				{
					if (i <= 30)
						Dust.NewDust(NPC.Center + new Vector2(0, 16), 10, -10, dustType, 5 / (float)Math.Tan(3 * i - 45), -5);
					else if (i > 30 && i <= 60)
						Dust.NewDust(NPC.Center + new Vector2(0, 16), 10, -10, dustType, 5, -5 * (float)Math.Tan(3 * i - 45));
					else if (i > 60 && i <= 90)
						Dust.NewDust(NPC.Center + new Vector2(0, 16), 10, -10, dustType, -5 / (float)Math.Tan(3 * i - 45), 5);
					else
						Dust.NewDust(NPC.Center + new Vector2(0, 16), 10, -10, dustType, -5, 5 * (float)Math.Tan(3 * i - 45));
				}
				AI_Timer = 0;
				AI_State = (float)ActionState.Rush;
            }
        }

		private void judge(Player player)
        {
			NPC.velocity = Vector2.Zero;
			if (Math.Sqrt(Math.Pow(NPC.Center.X-player.Center.X,2)+Math.Pow(NPC.Center.Y-player.Center.Y,2))>500)
            {
				AI_State = (float)ActionState.Appear;
            }
			else
            {
				
					int rushRight = Main.rand.Next(rushCD);
					int shootRight = Main.rand.Next(shootCD);
					int screenRight = Main.rand.Next(screenCD);
					int trackRight = 2*Main.rand.Next(trackCD);
				int max = rushRight;
				if (shootRight>max)
					max= shootRight;
				if (screenRight>max)
					max = screenRight;
				if (trackRight>max)
					max= trackRight;
				
					if (rushRight==max)
                {
					AI_State = (float)ActionState.Rush;
                }
					else if (shootRight == max)
                {
					AI_State = (float)ActionState.Shoot;
                }
                else if (screenRight==max)
                {
					AI_State = (float)ActionState.Screen;
                }
					else
                {
					AI_State = (float)ActionState.Track;
                }
                	/*			
				if (rushCD <= shootCD && rushCD <= screenCD)
					AI_State = (float)ActionState.Rush;
				else if (shootCD <= rushCD && shootCD <= screenCD)
					AI_State = (float)ActionState.Shoot;
				else
					AI_State = (float)ActionState.Screen;
					*/
            }
        }

		private int appearTimer=0;
		private bool isAppear;
		private void appear(Player player)
        {
			AI_Timer++;
			NPC.dontTakeDamage = true;
			if (AI_Timer<=25)
				NPC.alpha += 10;
			if (AI_Timer==25)
            {
				NPC.Center = player.Center - new Vector2(0, 150);
			}
            else if (AI_Timer > 25)
            {
				NPC.alpha -= 10;
				if (NPC.alpha<1)
                {
					NPC.dontTakeDamage=false;
					NPC.alpha = 0;
					AI_State = (float)ActionState.Judge;
					AI_Timer = 0;
                }
            }
				
            				
        }


		private int rushCount=0;
		private int rushSpeed = 40;
		private int rushCD = 0;
		private int innerCount = 0;
		private int rushMax = 0;
		private void rush(Player player)
        {
			spawnExplosion(Main.rand.NextFloat(1) + 1);
            
			AI_Timer++;
			/*
			if (isSecondStage && AI_Timer%60==0)
            {
				spawnScreen();
            }
			*/

			NPC.damage = 0;
			if (AI_Timer == 1)
            {
				rushMax = Main.rand.Next(20) + 10;
				innerCount = 0;
            }
			if (AI_Timer<=10)
            {
				NPC.alpha += 15;
				return;
            }
			innerCount++;
			if (innerCount == 1)
            {
				if (isSecondStage)
					spawnTrack();
				NPC.Center = target.Center + getPosition(500);
				if (NPC.Center.X<target.Center.X)
                {
					NPC.rotation = (target.Center-NPC.Center).ToRotation();
					NPC.direction = 1;
                }
                else
                {
					NPC.rotation = (target.Center - NPC.Center).ToRotation() + (float)Math.PI;
					NPC.direction = -1;
				}
				NPC.velocity = rushSpeed * NPC.DirectionTo(player.Center);
			}

			if (innerCount<=20 && NPC.alpha>100)
            {
				NPC.alpha -= 15;
            }
			else if (innerCount>=30)
            {
				NPC.alpha += 15;
            }
            else
            {
				NPC.alpha = 100;
            }
			if (innerCount > 40)
            {
				innerCount = 0;
				rushCount++;
            }
			if (rushCount>=rushMax)
            {
				NPC.damage = damage;
				AI_Timer = 0;
				AI_State = (float)ActionState.Judge;
				rushCD = 0;
				NPC.rotation = 0;
				rushCount = 0;
			}
        }

		private int screenCD = 0;
		private int screenDistance = 700;
		private void screen(Player player)
        {
			if (player.Center.X < NPC.Center.X)
				NPC.direction = -1;
			else
				NPC.direction = 1;
			if (Math.Sqrt(Math.Pow(NPC.Center.X - player.Center.X, 2) + Math.Pow(NPC.Center.Y - player.Center.Y, 2)) > 500 || isAppear)
			{
				isAppear = true;
				NPC.damage = 0;
				appearTimer++;
				NPC.dontTakeDamage = true;
				if (appearTimer <= 25)
					NPC.alpha += 10;
				if (appearTimer == 25)
				{
					NPC.Center = player.Center - getPositivePosition(250);
				}
				else if (appearTimer > 25)
				{
					NPC.alpha -= 10;
					if (NPC.alpha < 1)
					{
						appearTimer = 0;
						NPC.damage = damage;
						NPC.dontTakeDamage = false;
						NPC.alpha = 0;
						isAppear = false;
					}
				}
			}


			AI_Timer++;
			if (AI_Timer%8==0)
            {
				spawnScreen();
            }
			if (AI_Timer%450==0 && isSecondStage)
            {
				spawnTrack();
			}
			if (AI_Timer>900 && !isAppear)
            {
				AI_Timer = 0;
				AI_State = (float)ActionState.Judge;
				screenCD = 0;
				foreach(Projectile i in Main.projectile)
                {
					if (i.type == ModContent.ProjectileType<WitherHead>())
                    {
						i.timeLeft = 10;
                    }
                }
            }
        }

		Projectile shoot1;
		Projectile shoot2;
		Vector2 position1;
		Vector2 position2;
		Projectile shoot3;
		Projectile shoot4;
		Vector2 position3;
		Vector2 position4;
		private int shootCD = 0;
		private int shootTimer1=-1;
		private int shootTimer2=-1;
		private int temp;
		private Vector2 predicte;
		private int shootDistance = 400;
		private void shoot(Player player)
        {
			if (player.Center.X < NPC.Center.X)
				NPC.direction = -1;
			else
				NPC.direction = 1;
			if (Math.Sqrt(Math.Pow(NPC.Center.X - player.Center.X, 2) + Math.Pow(NPC.Center.Y - player.Center.Y, 2)) >500 || isAppear)
			{
				isAppear = true;
				NPC.damage = 0;
				appearTimer++;
				NPC.dontTakeDamage = true;
				if (appearTimer <= 25)
					NPC.alpha += 10;
				if (appearTimer == 25)
				{
					NPC.Center = player.Center - getPositivePosition(250);
				}
				else if (appearTimer > 25)
				{
					NPC.alpha -= 10;
					if (NPC.alpha < 1)
					{
						appearTimer = 0;
						NPC.damage = damage;
						NPC.dontTakeDamage = false;
						NPC.alpha = 0;
						isAppear = false;
					}
				}
			}
			
			AI_Timer++;
			if (AI_Timer==2)
            {
				shootTimer1 = 0;
            }
			if (shootTimer1>=0)
				shootTimer1++;
			if (shootTimer1 > 0)
			{
				if (shootTimer1 == 1)
				{
					temp = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<WitherShoot>(), shootDamage, 10f);
					shoot1 = Main.projectile[temp];
					temp = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<WitherShoot>(), shootDamage, 10f);
					shoot2 = Main.projectile[temp];
					position1 = getPosition(shootDistance);
					position2 = getPosition(shootDistance);
				}
				else if (shootTimer1 < 150)
				{
					shoot1.Center = player.Center + position1;
					shoot2.Center = player.Center + position2;
				}
				else
				{
					shootTimer1 = 0;
				}
			}


			if (AI_Timer==60)
            {
				shootTimer2 = 0;
            }
			if (shootTimer2 >= 0)
            {
				shootTimer2++;
            }
				
			if (shootTimer2 >= 1)
			{
				if (shootTimer2 == 1)
				{
					temp = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<WitherShoot>(), shootDamage, 10f);
					shoot3 = Main.projectile[temp];
					temp = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<WitherShoot>(), shootDamage, 10f);
					shoot4 = Main.projectile[temp];
					position3 = getPosition(shootDistance);
					position4 = getPosition(shootDistance);
				}
				else if (shootTimer2 <150)
				{
					shoot3.Center = player.Center + position3;
					shoot4.Center = player.Center + position4;
				}
				else
				{
					shootTimer2 = 0;
				}
			}

			if (AI_Timer>600 && !isAppear)
            {
				shootCD = 0;
				shootTimer1 = -1;
				shootTimer2 = -1;
				AI_Timer = 0;
				AI_State = (float)ActionState.Judge;
            }
        }

		private bool realDead=false;
		private int deadTimer = 0;
		private void dead()
		{
			deadTimer++;
			if (deadTimer==1)
            {
				NPC.position = target.Center + new Vector2(0, -100);
				foreach (Projectile i in Main.projectile)
				{
					if (i.type == ModContent.ProjectileType<TrackHead>() || i.type == ModContent.ProjectileType<WitherHead>())
						i.timeLeft = 60;
				}
			}
			NPC.velocity.X = 0;
			NPC.alpha += 1;
			NPC.position.Y -= 0.3f;
			if (NPC.alpha >= 255)
			{ 
			NPC.life = 0;
			realDead = true;
			}
        }

		
        public override bool CheckDead()
        {
			if (realDead)
            {
				return true;
            }
			NPC.life = 1;
			AI_State = (float)ActionState.Dead;
			NPC.dontTakeDamage = true;
			return false;
        }

		// TODO: 用下面的方法来实现劲爆尾杀，修改掉dead()里面的判定
		public override void HitEffect(NPC.HitInfo hit)
		{
			// If the NPC dies, spawn gore and play a sound
			if (Main.netMode == NetmodeID.Server)
			{
				// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
				return;
			}

			//if (NPC.life <= 0) {
			//	// These gores work by simply existing as a texture inside any folder which path contains "Gores/"
			//	int backGoreType = Mod.Find<ModGore>("MinionBossBody_Back").Type;
			//	int frontGoreType = Mod.Find<ModGore>("MinionBossBody_Front").Type;

			//	var entitySource = NPC.GetSource_Death();

			//	for (int i = 0; i < 2; i++) {
			//		Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
			//		Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);
			//	}

			//	SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
			//}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
			if (AI_State == (float)ActionState.Dead)
				return false;
			else
				return true;
        }

		private Vector2 getPosition(int r)
        {
			Vector2 position;
			position.X = r - Main.rand.Next(2*r);
			if (Main.rand.NextBool())
				position.Y = (float)Math.Sqrt(Math.Pow(r, 2) - Math.Pow(position.X, 2));
			else
				position.Y = -(float)Math.Sqrt(Math.Pow(r, 2) - Math.Pow(position.X, 2));
			return position;
		}

		private Vector2 getPositivePosition(int r)
		{
			Vector2 position;
			position.X = r - Main.rand.Next(2 * r);
			position.Y = (float)Math.Sqrt(Math.Pow(r, 2) - Math.Pow(position.X, 2));
			return position;
		}

		private int explosionDamage = 25;
		private void spawnExplosion(float sz)
        {

			int temp = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + new Vector2 (Main.rand.Next(100)-50,Main.rand.Next(100)-50), Vector2.Zero, ModContent.ProjectileType<WitherExplosion>(), explosionDamage, 100);
			Projectile ex = Main.projectile[temp];
			ex.scale = sz;
		}

		private void spawnScreen()
        {
			int id = 0;
			switch (Main.rand.Next(4))
			{
				case 0:
					id = Projectile.NewProjectile(NPC.GetSource_FromThis(), target.Center - new Vector2(screenDistance - Main.rand.Next(2 * screenDistance), screenDistance), new Vector2(0, 5), ModContent.ProjectileType<WitherHead>(), screenDamage, 0f);//最上面那条
					Main.projectile[id].rotation = -(float)(Math.PI / 2);
					break;
				case 1:
					id = Projectile.NewProjectile(NPC.GetSource_FromThis(), target.Center - new Vector2(-screenDistance, screenDistance - Main.rand.Next(2 * screenDistance)), new Vector2(-5, 0), ModContent.ProjectileType<WitherHead>(), screenDamage, 0f);//右边那条
					break;
				case 2:
					id = Projectile.NewProjectile(NPC.GetSource_FromThis(), target.Center - new Vector2(screenDistance - Main.rand.Next(2 * screenDistance), -screenDistance), new Vector2(0, -5), ModContent.ProjectileType<WitherHead>(), screenDamage, 0f);//下边那条
					Main.projectile[id].rotation = (float)(Math.PI / 2);
					break;
				case 3:
					id = Projectile.NewProjectile(NPC.GetSource_FromThis(), target.Center - new Vector2(screenDistance, screenDistance - Main.rand.Next(2 * screenDistance)), new Vector2(5, 0), ModContent.ProjectileType<WitherHead>(), screenDamage, 0f);//左边那条
					Main.projectile[id].spriteDirection = -1;
					break;
			}
			if (isSecondStage)
            {
				Main.projectile[id].frame = 1;
				Main.projectile[id].ai[1] = 1;
            }
				
		}


		private void spawnTrack()
        {
			spawnTrack(NPC.DirectionTo(15 * target.Center));
        }

		private void spawnTrack(Vector2 velocity)
        {
			int temp = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity, ModContent.ProjectileType<TrackHead>(), trackDamage, 0f);
			Projectile track = Main.projectile[temp];
			track.alpha = 255;
			if (!isSecondStage)
            {
				track.timeLeft = 600;
				if (AI_State == (float)ActionState.Track)
                {
					track.timeLeft = 1200;
                }
				track.penetrate = 3;
            }
			else
            {
				track.timeLeft = 600;
				if (AI_State == (float)ActionState.Track)
				{
					track.timeLeft = 1200;
				}
				track.penetrate = -1;
            }

		}

    }
}
