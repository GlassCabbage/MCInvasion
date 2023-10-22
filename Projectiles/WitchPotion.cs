using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MCInvasion.Projectiles
{
	public class WitchPotion : ModProjectile
	{

		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Potion");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0; 
		}

		public override void SetDefaults() {
			Projectile.width = 8; 
			Projectile.height = 8; 
			Projectile.aiStyle = -1; 
			Projectile.friendly = false; 
			Projectile.hostile = false; 
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.penetrate = 1; 
			Projectile.timeLeft = 6000; 
			Projectile.alpha = 0; 
			Projectile.light = 0f; 
			Projectile.ignoreWater = true; 
			Projectile.tileCollide = true; 
			Projectile.extraUpdates = 1; 
		}

		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}

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

		enum EFFECT
        {
		heal,
		posion,
		slow,
		weak,
		fragile,
		 recover,
		froze,
		 burn,
		 stone,
		 iron,
		 fast,
		damage,
		luck,
		explosion,
		trace,
		nogravity,
		golden,
		echo,
		born,
		spread,
		immune,
		ghost,
		longtime,
		clear,
	}
		private void posionDecoder()
        {
			string a = Convert.ToString((int)Projectile.ai[0], 2);
			string b = "1";
			bool[] temp = new bool[(int)EFFECT.clear+1];
			for (int i =1;i<(int)EFFECT.clear+2;i++)
            {
				if (a[i] == b[0])
					temp[i - 1] = true;
				else
					temp[i - 1] = false;
            }
		heal =temp[0];
		posion=temp[1];
		slow =temp[2];
		weak = temp[3];
		fragile =temp[4];
		recover =temp[5];
		froze =temp[6];
		burn =temp[7];
		stone =temp[8];
		iron =temp[9];
		fast =temp[10];
		damage = temp[11];
		luck = temp[12];
		explosion =temp[13];
		trace =temp[14];
		nogravity =temp[15];
		golden =temp[16];
		echo =temp[17];
		born = temp[18];
		spread =temp[19];
		immune =temp[20];
		ghost =temp[21];
		longtime =temp[22];
		clear =temp[23];
		}


		public int EffectRadius = 100;
		public int healPower = 200;
		bool isKilled = false;
		public override void OnKill(int timeLeft) {
			if (isKilled)
				return;
			isKilled = true;
            switch (Main.rand.Next(3))
            {
				case 0:
					SoundEngine.PlaySound(break1);
					break;
				case 1:
					SoundEngine.PlaySound(break2);
					break;
				case 2:
					SoundEngine.PlaySound(break3);
					break;
            }
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			int dustType = 21;
			for (int i = 0; i <= 120; i++)
			{
				if (i <= 30)
					Dust.NewDust(Projectile.Center + new Vector2(0, 16), 10, -10, dustType, 5 / (float)Math.Tan(3 * i - 45), -5);
				else if (i > 30 && i <= 60)
					Dust.NewDust(Projectile.Center + new Vector2(0, 16), 10, -10, dustType, 5, -5 * (float)Math.Tan(3 * i - 45));
				else if (i > 60 && i <= 90)
					Dust.NewDust(Projectile.Center + new Vector2(0, 16), 10, -10, dustType, -5 / (float)Math.Tan(3 * i - 45), 5);
				else
					Dust.NewDust(Projectile.Center + new Vector2(0, 16), 10, -10, dustType, -5, 5 * (float)Math.Tan(3 * i - 45));
			}
			if (heal)
			{
				dealHeal();
			}
			if (damage)
				dealDamage();
			dealBuff();

			if (born)
				foreach(NPC npc in Main.npc)
                {
					if (npc.type != NPCID.Bunny  && checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y - npc.height / 2), new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y + npc.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y - npc.height / 2), new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y + npc.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y - npc.height / 2), new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y - npc.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y + npc.height / 2), new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y + npc.height / 2)))
						NPC.NewNPC(Projectile.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, 46);
				}

			Projectile.timeLeft = 0;
		}

		float clock = 1;
		private void dealBuff()
        { 
			foreach (NPC npc in Main.npc)
			{
				if (checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y - npc.height / 2), new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y + npc.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y - npc.height / 2), new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y + npc.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y - npc.height / 2), new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y - npc.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y + npc.height / 2), new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y + npc.height / 2)))
				{
					if (posion)
						npc.AddBuff(20, (int)(600*clock));
					if (slow)
						npc.AddBuff(32, (int)(600 * clock));
					if (weak)
						npc.AddBuff(33, (int)(600 * clock));
					if (fragile)
						npc.AddBuff(36, (int)(600 * clock));
					if (recover)
						npc.AddBuff(2, (int)(600 * clock));
					if (froze)
						npc.AddBuff(44, (int)(600 * clock));
					if (burn)
						npc.AddBuff(24, (int)(600 * clock));
					if (stone)
						npc.AddBuff(156, (int)(600 * clock));
					if (iron)
						npc.AddBuff(5, (int)(600 * clock));
					if (fast)
						npc.AddBuff(3, (int)(600 * clock));
					if (luck)
						npc.AddBuff(257, (int)(600 * clock));

					if (golden)
						npc.AddBuff(72, (int)(600 * clock));

					if (immune)
						npc.GetImmuneTime(0, 120);
				}
			}
			foreach (Player player in Main.player)
			{
				if (checkLine(Projectile.Center, EffectRadius, new Vector2(player.Center.X - player.width / 2, player.Center.Y - player.height / 2), new Vector2(player.Center.X - player.width / 2, player.Center.Y + player.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(player.Center.X + player.width / 2, player.Center.Y - player.height / 2), new Vector2(player.Center.X + player.width / 2, player.Center.Y + player.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(player.Center.X - player.width / 2, player.Center.Y - player.height / 2), new Vector2(player.Center.X + player.width / 2, player.Center.Y - player.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(player.Center.X - player.width / 2, player.Center.Y + player.height / 2), new Vector2(player.Center.X + player.width / 2, player.Center.Y + player.height / 2)))
				{
					if (immune)
						player.immuneTime += 120;

					if (posion)
						player.AddBuff(20, (int)(1200 * clock));
					if (slow)
						player.AddBuff(32, (int)(1200 * clock));
					if (weak)
						player.AddBuff(33, (int)(1200 * clock));
					if (fragile)
						player.AddBuff(36, (int)(1200 * clock));
					if (recover)
						player.AddBuff(2, (int)(3000 * clock));

					Player npc = player;
					if (froze)
						npc.AddBuff(44, (int)(1200 * clock));
					if (burn)
						npc.AddBuff(24, (int)(1200 * clock));
					if (stone)
						npc.AddBuff(156, (int)(600 * clock));
					if (iron)
						npc.AddBuff(5, (int)(3000 * clock));
					if (fast)
						npc.AddBuff(3, (int)(3000 * clock));
					if (luck)
						npc.AddBuff(257, (int)(3000 * clock));

					if (clear)
                    {
						npc.ClearBuff(20);
						npc.ClearBuff(20);
						npc.ClearBuff(32);
						npc.ClearBuff(33);
						npc.ClearBuff(36);
						npc.ClearBuff(44);
						npc.ClearBuff(24);
						npc.ClearBuff(156);
                    }
				}
			}
		}

		private void dealDamage()
		{
			foreach (NPC npc in Main.npc)
			{
				if (checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y - npc.height / 2), new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y + npc.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y - npc.height / 2), new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y + npc.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y - npc.height / 2), new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y - npc.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y + npc.height / 2), new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y + npc.height / 2)))
				{
					npc.GetAttackDamage_ForProjectiles(50f, 100f);
				}
			}
			foreach (Player player in Main.player)
			{
				if (checkLine(Projectile.Center, EffectRadius, new Vector2(player.Center.X - player.width / 2, player.Center.Y - player.height / 2), new Vector2(player.Center.X - player.width / 2, player.Center.Y + player.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(player.Center.X + player.width / 2, player.Center.Y - player.height / 2), new Vector2(player.Center.X + player.width / 2, player.Center.Y + player.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(player.Center.X - player.width / 2, player.Center.Y - player.height / 2), new Vector2(player.Center.X + player.width / 2, player.Center.Y - player.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(player.Center.X - player.width / 2, player.Center.Y + player.height / 2), new Vector2(player.Center.X + player.width / 2, player.Center.Y + player.height / 2)))
				{
					int playerindex =0;
					for (int i=0;i<Main.player.Length;i++)
                    {
						if (Main.player[i]==player)
                        {
							playerindex = i;
                        }
                    }
					player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByProjectile(playerindex,0), 50, 0);
				}
			}
		}

		private void dealHeal()
        {
			foreach (NPC npc in Main.npc)
			{
				if (checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y - npc.height / 2), new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y + npc.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y - npc.height / 2), new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y + npc.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y - npc.height / 2), new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y - npc.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(npc.Center.X - npc.width / 2, npc.Center.Y + npc.height / 2), new Vector2(npc.Center.X + npc.width / 2, npc.Center.Y + npc.height / 2)))
				{
					if (npc.life + healPower > npc.lifeMax)
						npc.life = npc.lifeMax;
					else
					{
						npc.life += healPower;
					}
				}
			}
			foreach (Player player in Main.player)
			{
				if (checkLine(Projectile.Center, EffectRadius, new Vector2(player.Center.X - player.width / 2, player.Center.Y - player.height / 2), new Vector2(player.Center.X - player.width / 2, player.Center.Y + player.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(player.Center.X + player.width / 2, player.Center.Y - player.height / 2), new Vector2(player.Center.X + player.width / 2, player.Center.Y + player.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(player.Center.X - player.width / 2, player.Center.Y - player.height / 2), new Vector2(player.Center.X + player.width / 2, player.Center.Y - player.height / 2)) || checkLine(Projectile.Center, EffectRadius, new Vector2(player.Center.X - player.width / 2, player.Center.Y + player.height / 2), new Vector2(player.Center.X + player.width / 2, player.Center.Y + player.height / 2)))
				{
					if (player.HasBuff(21))
						player.Heal(healPower / 10);
					else
					{
						player.Heal(healPower);
						player.AddBuff(21, 600);
					}
				}
			}
		}

		private bool checkLine(Vector2 center, int radius,Vector2 firstPoint, Vector2 secondPoint)
        {
			if (firstPoint.X == secondPoint.X)
            {
				if ((center.Y<=firstPoint.Y && center.Y>=secondPoint.Y) || (center.Y>=firstPoint.Y && center.Y <= secondPoint.Y))
                {
					if (Math.Abs(center.X - firstPoint.X) < radius)
						return true;
                }
            }
            else
            {
				if ((center.X <= firstPoint.X && center.X >= secondPoint.X) || (center.X >= firstPoint.X && center.X <= secondPoint.X))
				{
					if (Math.Abs(center.Y - firstPoint.Y) < radius)
						return true;
				}
			}
			if (Vector2.Distance(firstPoint, center) < radius)
				return true;
			if (Vector2.Distance(secondPoint, center) < radius)
				return true;
			
			return false;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
		}

		NPC target;
		public override void AI()
		{
			if (Projectile.timeLeft == 5999)
            {
				posionDecoder();
				if (spread)
					EffectRadius = 150;
				if (ghost)
				{
					Projectile.tileCollide = false;
					Projectile.alpha -= 100;
				}
				if (longtime)
					clock = 1.5f;
            }

			if (explosion)
				OnKill(0);


			Projectile.rotation += 0.03f;


			if (!trace && !nogravity)
				Projectile.velocity = Projectile.velocity + new Vector2(0, 0.02f);
			bool hasNPC = false;
			if (trace)
            {	
				float minDis = 999;
				foreach(NPC npc in Main.npc)
                {
					if (Projectile.Center.Distance(npc.Center)<=100 && npc.friendly == false)
                    {
						hasNPC = true;
						minDis = Projectile.Center.Distance(npc.Center);
					}
						
                }
				if (hasNPC)
				{
					
					foreach(NPC npc in Main.npc)
                    {
						if (Projectile.Center.Distance(npc.Center) <= minDis && npc.friendly == false)
                        {
							target = npc;
							minDis=Projectile.Center.Distance(npc.Center);
                        }
							
					}
					Projectile.velocity = Projectile.velocity*0.75f + Projectile.Center.DirectionTo(target.Center)*1.5f;		
				}					

            }


			if (Projectile.timeLeft < 5997) { 
				foreach (NPC i in Main.npc)
				{
					if (Projectile.Center.X > i.Center.X - i.width / 2 && Projectile.Center.X < i.Center.X + i.width / 2 && Projectile.Center.Y < i.Center.Y + i.height / 2 && Projectile.Center.Y > i.Center.Y - i.height / 2)
						OnKill(10);
				}
				foreach (Player i in Main.player)
				{
					if (Projectile.Center.X > i.Center.X - i.width / 2 && Projectile.Center.X < i.Center.X + i.width / 2 && Projectile.Center.Y < i.Center.Y + i.height / 2 && Projectile.Center.Y > i.Center.Y - i.height / 2)
						OnKill(10);
				}
			}
		}



		SoundStyle break1 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/PotionProjectile/Glass_break1")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle break2 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/PotionProjectile/Glass_break2")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle break3 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/PotionProjectile/Glass_break3")
		{
			Volume = 0.9f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
	}
}