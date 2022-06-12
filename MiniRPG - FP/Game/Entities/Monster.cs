namespace MiniRPG.Game.Entities
{
    class Monster
    {
        public string Name { get; set; }
        public int MaxHp { get; set; }
        public int Hp { get; set; }
        public int Power { get; set; }
        public int Gold { get; set; }
        public int Xp { get; set; }

        public bool IsDead = false;

        public Monster(Structs.Monster template)
        {
            Name = template.Name;
            Hp = MaxHp = template.Hp;
            Power = template.Power;
            Gold = template.Gold;
            Xp = template.Xp;
        }
    }
}
