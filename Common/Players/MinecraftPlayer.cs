using Terraria;
using Terraria.ModLoader;

namespace MCInvasion.Common.Players
{
	public class MinecraftPlayer : ModPlayer
	{
		public bool hasMinecraftEffect;
		public override void ResetEffects()
		{
			hasMinecraftEffect = false;
		}


	}
}
