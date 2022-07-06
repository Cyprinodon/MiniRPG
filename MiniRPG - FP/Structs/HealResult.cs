using MiniRPG.Entities;

namespace MiniRPG.Structs
{
    struct HealResult
    {
        public Hero Actor;
        public int HealAmount;

        public HealResult(Hero actor, int healAmount)
        {
            Actor = actor;
            HealAmount = healAmount;
        }
    }
}
