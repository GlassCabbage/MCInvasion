using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;
using MCInvasion.Projectiles.WitherBossProjectile;

namespace MCInvasion.Items.WitherBossItem
{
	public class WitherSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault(".");
			// DisplayName.SetDefault("Wither Sword");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 40; // 物品贴图的宽度(像素)
			Item.height = 40; // 物品贴图的高度(像素)
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 20;
			Item.useAnimation = 20; // 物品使用动画播放一次所需的时间，这里建议和 useTime 设置成一样的值
			Item.autoReuse = true;


			Item.DamageType = DamageClass.Melee;
			Item.damage = 50; // 物品基础伤害
			Item.knockBack = 6; // 物品击退力，最大值为20，详见Wiki: https://terraria.wiki.gg/zh/wiki/%E5%87%BB%E9%80%80
			Item.crit = 6; // 物品本身所具有的基础暴击率，玩家的默认基础暴击率为 4%
			Item.scale = 3f;

			// 物品的价格，这里使用 buyPrice 也就是买入价，gold: 1 也就是一金的买入价
			// 而物品出售价=买入价/5，1金=100银，所以这个物品的出售价就是20银
			Item.value = Item.buyPrice(gold: 1);
			// 用 sellPrice 可以直接设置物品的出售价，也就是说上面这行等价于下面这行:
			// Item.value = Item.sellPrice(silver: 20);

			Item.rare = ItemRarityID.Yellow; // 给予这个物品自定义稀有度
			Item.UseSound = SoundID.Item1; // 物品使用时发出的声音
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			Vector2 pos = player.Center + new Vector2(Main.rand.Next(2000) - 1000, Main.rand.Next(2000) - 1000);
			Projectile.NewProjectile(player.GetSource_FromThis(), pos, 30*pos.DirectionTo(target.Center), ModContent.ProjectileType<WitherHeadFriend>(), 100, 2);
		}

		// 这里写的是合成配方，合成配方在 Content/ExampleRecipes.cs 有更详尽的介绍
		public override void AddRecipes()
		{
			
		}
	}
}
