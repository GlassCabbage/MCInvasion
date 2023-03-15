
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MCInvasion.Projectiles;
using Terraria.Audio;
using MCInvasion.Projectiles.GhostHeadProjectiles;
using Terraria.GameContent.Creative;
using System;

namespace MCInvasion.Items.weapon
{
	public class GhostHead : ModItem
	{
		public override void SetStaticDefaults()
		{

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
		}

		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.knockBack = 10f;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 30;
			Item.height = 26;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<GhostHeadLargeFireBall>();
			Item.noMelee = true;
			Item.shootSpeed = 10;
			Item.useAmmo = ModContent.ItemType<powder>();
		}

		int counter = 0;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (counter<=19)
				counter++;
			return false;
		}

		public override bool CanUseItem(Player player)
		{
				return true;
		}

		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			if (counter<=19)
				return Main.rand.NextFloat() >= 0.80f;
			else
				return Main.rand.NextFloat() >= 1f;
		}

		Vector2 target;
		public override void HoldItem(Player player)
		{
			if (counter > 0)
				Dust.NewDust(player.position, 10, 10, DustID.Torch);
			if (counter>10)
				Dust.NewDust(player.position, 10, 10, DustID.Pixie);
			if (counter > 20)
				for (int i = 0; i < 15; i++)
					Dust.NewDust(player.position, 10, 10, DustID.GoldFlame);
			if (player.controlUseItem==false && counter>0)
            {

				target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
				if (player.gravDir == -1)
                {
					float dis = target.Y - player.Center.Y;
					target.Y=target.Y - 2*dis;
                }
				if (counter > 0 && counter <= 10)
					Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, (counter+3)*player.DirectionTo(target), ModContent.ProjectileType<GhostHeadFireball>(), counter * Item.damage,0f,player.whoAmI);
				if (counter > 10)
					Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, (counter+3)*player.DirectionTo(target), ModContent.ProjectileType<GhostHeadLargeFireBall>(), counter * Item.damage, 0f,player.whoAmI);
				counter = 0;
            }
			
				
		}




		
	}
}