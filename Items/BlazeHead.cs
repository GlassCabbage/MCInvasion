
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MCInvasion.Projectiles;
using Terraria.Audio;

namespace MCInvasion.Items
{
	public class BlazeHead : ModItem
	{
		public override void SetStaticDefaults()
		{
		}

		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.DamageType = DamageClass.Magic;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<JetFlame>();
			Item.noMelee = true;
			Item.shootSpeed = 10;
			Item.mana = 5;
			Item.scale = 0.7f;
		}

		int counter = 0;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);

			counter++;
			if (counter==1 || counter==100)
            {
				switch (Main.rand.Next(3))
				{
					case 0:
						SoundEngine.PlaySound(hit1, player.Center);
						break;
					case 1:
						SoundEngine.PlaySound(hit2, player.Center);
						break;
					case 2:
						SoundEngine.PlaySound(hit3, player.Center);
						break;
					default:
						SoundEngine.PlaySound(hit4, player.Center);
						break;
				}
			}
            else
            {
				switch (Main.rand.Next(3))
				{
					case 0:
						SoundEngine.PlaySound(breath1, player.Center);
						break;
					case 1:
						SoundEngine.PlaySound(breath2, player.Center);
						break;
					case 2:
						SoundEngine.PlaySound(breath3, player.Center);
						break;
					default:
						SoundEngine.PlaySound(breath4, player.Center);
						break;
				}
			}
			if (counter <= 100) { 
				for (int i = 0; i < 10; i++)
				{
					Vector2 ran = new Vector2(Main.rand.Next(20), Main.rand.Next(20)) - Vector2.One * 10;
					Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, -5f * player.DirectionFrom(target) + ran / 10, ModContent.ProjectileType<JetFlame>(), Item.damage, Item.knockBack, Main.myPlayer);
				}
			}
			else
				for (int i = 0; i < 10; i++)
				{
					Vector2 ran = new Vector2(Main.rand.Next(20), Main.rand.Next(20)) - Vector2.One * 10;
					Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, -5f * player.DirectionFrom(target) + ran / 10, ModContent.ProjectileType<StrongJetFlame>(), Item.damage, Item.knockBack, Main.myPlayer);
				}
			return false;
		}

		public override void HoldItem(Player player)
		{
			if (player.controlUseItem==false)
				counter = 0;
		}




		SoundStyle hit1 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Blaze/Blaze_hit1")
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
		SoundStyle breath1 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Blaze/Blaze_breathe1")
		{
			Volume = 0.4f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle breath2 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Blaze/Blaze_breathe2")
		{
			Volume = 0.4f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle breath3 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Blaze/Blaze_breathe3")
		{
			Volume = 0.4f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		SoundStyle breath4 = new SoundStyle($"{nameof(MCInvasion)}/Assets/Sounds/Blaze/Blaze_breathe4")
		{
			Volume = 0.4f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
	}
}