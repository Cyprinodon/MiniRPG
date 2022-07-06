using System;

namespace MiniRPG.Structs
{
    struct Monster
    {
        public readonly string Name;
        public readonly int Hp;
        public readonly int Gold;
        public readonly int Power;
        public readonly int Xp;

        public Monster(string name, int hp, int power, int gold, int xp)
        {
            Name = name;
            Hp = hp;
            Gold = gold;
            Power = power;
            Xp = xp;
        }
    }
}
