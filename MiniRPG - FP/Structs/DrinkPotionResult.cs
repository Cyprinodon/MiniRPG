using MiniRPG.Entities;

namespace MiniRPG.Structs
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
