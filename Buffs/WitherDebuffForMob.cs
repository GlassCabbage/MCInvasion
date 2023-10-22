using Terraria;
using Terraria.ModLoader;

namespace MCInvasion.Buffs
{
    public class WitherDebuffForMob : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 60;
            npc.defense -= 30;
            if (npc.lifeRegen > 0)
            {
                npc.lifeRegen = 0;
            }
            if (npc.defense < 0)
            {
                npc.defense = 0;
            }
        }
    }
}
		


  