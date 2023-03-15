using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace MCInvasion
{
	public class MCInvasion : Mod
	{
        public static Effect GrayEffect;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                //GrayEffect = ModContent.Request<Effect>("MCInvasion/Assets/Effect/GrayEffect").Value;
                //Filters.Scene["MCInvasion:GrayEffect"] = new Filter(new GrayScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("MCInvasion/Assets/Effect/GrayEffect").Value), "GrayEffect"), EffectPriority.Medium);
                //Filters.Scene["MCInvasion:GrayEffect"].Load();
            }


        }

        public override void PostSetupContent()
        {
            if (!Main.dedServ)
                GrayEffect = ModContent.Request<Effect>("MCInvasion/Assets/Effect/GrayEffect").Value;
        }

        public override void Unload()
        {
            GrayEffect = null;
        }
    }
}