using MiniRPG.Entities;

namespace MiniRPG.Structs
{
    struct LoseXpResult
    {
        public Hero Actor;
        public int XpLost;

        public LoseXpResult(Hero actor, int xpLost)
        {
            Actor = actor;
            XpLost = xpLost;
        }
    }
}
