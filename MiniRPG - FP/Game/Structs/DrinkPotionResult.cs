using MiniRPG.Game.Entities;

namespace MiniRPG.Game.Structs
{
    struct DrinkPotionResult
    {
        public Hero Drinker;
        public bool UsedPotion;

        public DrinkPotionResult(Hero drinker, bool usedPotion)
        {
            Drinker = drinker;
            UsedPotion = usedPotion;
        }
    }
}
