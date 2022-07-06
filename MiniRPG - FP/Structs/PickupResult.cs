using MiniRPG.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniRPG.Structs
{
    struct PickupResult
    {
        public Hero Actor;
        public Loot LootGained;
        public PickupResult(Hero actor, Loot lootGained) 
        {
            Actor = actor;
            LootGained = lootGained;
        }
    }
}
