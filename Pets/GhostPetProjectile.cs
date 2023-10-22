using Microsoft.Xna.Framework;
using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MCInvasion.Pets
{
	// 能跑
	public class GhostPetProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Ghast");

			Main.projFrames[Projectile.type] = 16;
			Main.projPet[Projectile.type] = true;
		}
		public ref float AI_State => ref Projectile.ai[0];
		int cd_round = 600;
		int cd_play = 600;

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);


			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
		}

		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];

			player.zephyrfish = false;

			return true;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];


			if (!player.dead && player.HasBuff(ModContent.BuffType<GhostPetBuff>())) {
				Projectile.timeLeft = 2;
			}
			cd_round--;
			cd_play--;

			if (player.miscDyes[0].type == 4778)
				AI_State = (float)ActionState.Disco;
			

			switch (AI_State)
			{
				case (float)ActionState.Judge:
					judge();
					break;
				case (float)ActionState.Close:
					close();
					break;
				case (float)ActionState.Round:
					round();
					break;
				case (float)ActionState.Play:
					play();
					break;
				case (float)ActionState.Stay:
					stay();
					break;
				case (float)ActionState.Disco:
					disco();
					break;
			}
			ChangeFrame();
		}
		private enum ActionState
		{
			Judge,
			Close,
			Round,
			Play,
			Stay,
			Disco,
		}

		private void judge()
        {
			Player player = Main.player[Projectile.owner];
			if (Math.Sqrt(Math.Pow(Math.Abs(Projectile.Center.X - player.Center.X), 2) + Math.Pow(Math.Abs(Projectile.Center.Y - player.Center.Y), 2)) > 150)
				AI_State = (float)ActionState.Close;
			else if (cd_round<0 && Main.rand.Next(3)==0)
				AI_State = (float)ActionState.Round;
			else if (cd_play<0 && Main.rand.Next(3)==0)
				AI_State =(float)ActionState.Play;
			else
				AI_State = (float)ActionState.Stay;
			Projectile.ai[1] = 0;


        }


		float highestSpeed = 10;
		float maxRotation = 0.25F;
		int dir;
		private void close()
		{


			Player player = Main.player[Projectile.owner];

			if (player.Center.X - Projectile.position.X > 0)
			{
				dir = 1;
			}
			else if (player.Center.X - Projectile.position.X < 0)
			{
				dir = -1;
			}


			if (Projectile.Center.Y - player.Center.Y > -50)
				Projectile.velocity.Y = -3;
			else if (Projectile.Center.Y - player.Center.Y < -100)
				Projectile.velocity.Y = 3;
			else
				Projectile.velocity.Y = 0;


			int closeDistance = 50;
			
				if (Math.Abs(Projectile.Center.X - player.Center.X) >= closeDistance && ((dir == 1 && Projectile.velocity.X < highestSpeed) || (dir == -1 && Projectile.velocity.X > -highestSpeed)))
				{
					Projectile.velocity.X = Projectile.velocity.X + (float)0.1 * dir;
				}
				else
				{
					Projectile.velocity.X = Projectile.velocity.X / (float)1.1;
				}

			if (Projectile.velocity.X > 1 && Projectile.rotation < maxRotation)
				Projectile.rotation = Projectile.rotation + (float)0.1;
			else if (Projectile.velocity.X < -1 && Projectile.rotation > -maxRotation)
				Projectile.rotation = Projectile.rotation - (float)0.1;
			if (Math.Abs(Projectile.velocity.X) < 0.1 && Math.Abs(Projectile.rotation) > 0)
				Projectile.rotation = Projectile.rotation / (float)1.2;

			if (Math.Sqrt(Math.Pow(Math.Abs(Projectile.Center.X - player.Center.X), 2) + Math.Pow(Math.Abs(Projectile.Center.Y - player.Center.Y), 2)) < 150)
			{ 
				AI_State = (float)ActionState.Judge;
				Projectile.ai[1] = 0;
			}
		}

		bool IsPositive;
		int whenToEnd;
		private void round()
        {
			Projectile.ai[1]++;
			if (Projectile.ai[1] == 1)
			{
				IsPositive = Main.rand.NextBool();
				whenToEnd = (3 + Main.rand.Next(3));
			}

			if (IsPositive)
				Projectile.velocity.X = 1;
			Projectile.velocity.Y = 0;
			Projectile.rotation = Projectile.rotation + (float)Math.PI/60;
			if (Projectile.ai[1]>whenToEnd && Projectile.rotation%(2*Math.PI)<0.1)
            {
				AI_State= (float)ActionState.Judge;
				Projectile.ai[1] = 0;
				cd_round = 2000;
				Projectile.rotation = 0;
            }

        }

		Vector2 WhereToGo;
		bool IsArrive;
		int ArriveCount;
		private void play()
        {
			Projectile.ai[1]++;
			if (Projectile.ai[1]==1)
            {
				ArriveCount = 3;
				Player player = Main.player[Projectile.owner];
				WhereToGo.X = player.Center.X + Main.rand.Next(1000)-500;
				WhereToGo.Y= player.Center.Y + Main.rand.Next(800)-400;
			}
			if (WhereToGo.X - Projectile.position.X > 0)
			{
				dir = 1;
			}
			else if (WhereToGo.X - Projectile.position.X < 0)
			{
				dir = -1;
			}
			if (Projectile.Center.Y - WhereToGo.Y > -50)
				Projectile.velocity.Y = -3;
			else if (Projectile.Center.Y - WhereToGo.Y < -100)
				Projectile.velocity.Y = 3;
			else
				Projectile.velocity.Y = 0;


			if (Math.Abs(Projectile.Center.X - WhereToGo.X) >= 100 && ((dir == 1 && Projectile.velocity.X < highestSpeed) || (dir == -1 && Projectile.velocity.X > -highestSpeed)))
			{
				Projectile.velocity.X = Projectile.velocity.X + (float)0.1 * dir;
			}
			else
			{
				Projectile.velocity.X = Projectile.velocity.X / (float)1.1;
				if (Math.Abs(Projectile.velocity.X)<0.1&&Projectile.velocity.Y==0)
					IsArrive = true;
			}

			if (IsArrive)
            {
				ArriveCount--;
				for (int i = 0; i < 15; i++)
					Dust.NewDust(Projectile.position, 10, 10, DustID.Torch);
				IsArrive=false;
				Player player = Main.player[Projectile.owner];
				WhereToGo.X = player.Center.X + Main.rand.Next(1000) - 500;
				WhereToGo.Y = player.Center.Y + Main.rand.Next(800) - 400;

			}

			if (!IsArrive)
			{
				if (Projectile.velocity.X > 1 && Projectile.rotation < maxRotation)
					Projectile.rotation = Projectile.rotation + (float)0.01;
				else if (Projectile.velocity.X < -1 && Projectile.rotation > -maxRotation)
					Projectile.rotation = Projectile.rotation - (float)0.01;
				if (Math.Abs(Projectile.velocity.X) < 1 && Math.Abs(Projectile.rotation) > 0)
					Projectile.rotation = Projectile.rotation / (float)1.2;
				if (Math.Abs(Projectile.rotation) < 0.01)
					Projectile.rotation = 0;
			}

			if (Projectile.ai[1]>300 && Projectile.rotation%(2*Math.PI)<0.1 && ArriveCount<=0)
            {
				cd_play = 600;
				AI_State = (float)ActionState.Judge;
				Projectile.rotation = 0;
				Projectile.ai[1] = 0;
            }
		}

		private void stay()
        {
			Projectile.ai[1]++;
			Projectile.velocity = Projectile.velocity / (float)1.1;
			if (Math.Abs(Projectile.velocity.X) < 1 && Math.Abs(Projectile.rotation) > 0)
				Projectile.rotation = Projectile.rotation / (float)1.2;
			if (Math.Abs(Projectile.rotation) < 0.01)
				Projectile.rotation = 0;

			if (Projectile.ai[1]>30)
            {
				Projectile.ai[1] = 0;
				AI_State = (float)ActionState.Judge;
            }

		}

		int WhichSideClose;
		int DiscoTarget=1;
		int calmdown;
		Vector2 DustPosition;
		private void disco()
        {
			calmdown--;
			Player player = Main.player[Projectile.owner];
			if (DiscoTarget == 1)
			{
				WhereToGo.X = player.Center.X + 608;
				WhereToGo.Y = player.Center.Y - 300;
			}
			else if (DiscoTarget==2)
            {
				WhereToGo.X = player.Center.X + 608;
				WhereToGo.Y= player.Center.Y + 300;
			}
			else if (DiscoTarget==3)
            {
				WhereToGo.X = player.Center.X - 608;
				WhereToGo.Y = player.Center.Y + 300;
			}
			else
            {
				WhereToGo.X = player.Center.X - 608;
				WhereToGo.Y = player.Center.Y - 300;
			}

			if (WhereToGo.X - Projectile.position.X > 0)
			{
				dir = 1;
			}
			else if (WhereToGo.X - Projectile.position.X < 0)
			{
				dir = -1;
			}
			if (Projectile.Center.Y - WhereToGo.Y > 10)
				Projectile.velocity.Y = -15;
			else if (Projectile.Center.Y - WhereToGo.Y < -10)
				Projectile.velocity.Y = 15;
			else
				Projectile.velocity.Y = 0;


			if (Math.Abs(Projectile.Center.X - WhereToGo.X) > 15)
			{
				Projectile.velocity.X = 15 * dir;
			}
			else
            {

				Projectile.velocity.X = 0;
				if (Math.Abs(Projectile.Center.X - WhereToGo.X)<20 && Math.Abs(Projectile.Center.Y - WhereToGo.Y)<20 && calmdown<0) 
				{
					calmdown = 20;
					DiscoTarget++;
					if (DiscoTarget > 4)
						DiscoTarget = 1;
				}	
            }

			DustPosition.X = Projectile.position.X - 40;
			DustPosition.Y = Projectile.position.Y - 40;
            {
				Dust.NewDust(DustPosition, 80, 80, DustID.SilverFlame);
				Dust.NewDust(DustPosition, 80, 80, DustID.GoldFlame);
				Dust.NewDust(DustPosition, 80, 80, DustID.Firefly);
			}

			Projectile.rotation = Projectile.rotation + (float)Math.PI / 30;
			if (Projectile.rotation % ((float)Math.PI * 2) < 0.1)
				Projectile.rotation = 0;
			





			if (player.miscDyes[0].type != 4778)
				AI_State = (float)ActionState.Judge;

		}


















		int WhatType=Main.rand.Next(3);
		private enum Frame
		{
			Glass1_1,
			Glass1_2,
			Glass1_3,
			Glass1_4,
			Glass2_1,
			Glass2_2,
			Glass2_3,
			Glass2_4,
			Tape1,
			Tape2,
			Tape3,
			Tape4,
		}
		private void ChangeFrame()
        {
			Projectile.spriteDirection = -Projectile.direction;
			Projectile.frameCounter++;
			if (AI_State != (float)ActionState.Disco)
			{
				if (Projectile.frameCounter < 10)
				{
					Projectile.frame = 4 * WhatType;
				}
				else if (Projectile.frameCounter < 20)
				{
					Projectile.frame = 4 * WhatType + 1;
				}
				else if (Projectile.frameCounter < 30)
					Projectile.frame = 4 * WhatType + 2;
				else if (Projectile.frameCounter < 40)
					Projectile.frame = 4 * WhatType + 3;
				else if (Projectile.frameCounter < 50)
					Projectile.frame = 4 * WhatType + 2;
				else if (Projectile.frameCounter < 60)
					Projectile.frame = 4 * WhatType + 1;
				else
				{
					Projectile.frameCounter = 0;
				}
			}
			else
            {
				if (Projectile.frameCounter < 10)
				{
					Projectile.frame = 4 * 3;
				}
				else if (Projectile.frameCounter < 20)
				{
					Projectile.frame = 4 * 3 + 1;
				}
				else if (Projectile.frameCounter < 30)
					Projectile.frame = 4 * 3 + 2;
				else if (Projectile.frameCounter < 40)
					Projectile.frame = 4 * 3 + 3;
				else
					Projectile.frameCounter = 0;
			}

		}















		}
}
