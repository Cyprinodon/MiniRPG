using MiniRPG.Game.Structs;

namespace MiniRPG.Game.Entities
{
    class Hero
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public Gender Gender { get; set; }
        protected int InnerLevel;
        public int Level 
        {
            get => InnerLevel;
            set 
            {
                int levelIndex = Level - 1 <= 0 ? 0 : Level - 1;
                InnerLevel = value;
                LevelThreshold = Data.LEVEL_THRESHOLDS[levelIndex];
            }
        }
        protected int InnerHp;
        public int Hp
        {
            get => InnerHp;
            set
            {
                InnerHp = value;
                IsDead = Hp <= 0;
            }
        }
        public int MaxHp { get; set; }
        public int Power { get; set; }
        public int Xp { get; set; }
        public int Gold { get; set; }
        public int Potions { get; set; }
        public int LevelThreshold { get; protected set; }
        public bool GaveUp { get; set; } = false;
        public bool HasWon { get; set; } = false;
        public bool IsDead { get; protected set; } = false;
        public bool HasFled { get; set; } = false;

        public Hero(){ }

        public Hero(Hero state)
        {
            Name = state.Name;
            Title = state.Title;
            Gender = state.Gender;
            Level = state.Level;
            Hp = state.Hp;
            MaxHp = state.MaxHp;
            Power = state.Power;
            Xp = state.Xp;
            Gold = state.Gold;
            Potions = state.Potions;
            GaveUp = state.GaveUp;
            HasWon = state.HasWon;
            HasFled = state.HasFled;
        }
    }
}
