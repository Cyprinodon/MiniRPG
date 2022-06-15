using MiniRPG.Game.Entities;

namespace MiniRPG.Game.Structs
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
