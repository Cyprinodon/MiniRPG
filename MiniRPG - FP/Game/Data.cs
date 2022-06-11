namespace MiniRPG.Game
{
    class Data
    {
        public enum Gender { Male, Female };
        public enum Phase { Combat, Rest };
        public enum HeroText { Name, Title, Genre };
        public enum Hero { Level, Hp, MaxHp, Power, Gold, Potions, Xp, LevelThreshold };
        public enum Monster { Name, Hp, Power, GoldLoot, XpLoot };
        public enum Combat { SeeStats, Attack, DrinkPotion, Flee, Quit };
        public enum Rest { SeeStats, Sleep, BuyPotion, Quit };
        //-------------------------------
        public string GAME_TITLE { get; }
        public string AUTHOR { get; }
        public string DEFAULT_MALE_NAME { get; }
        public string DEFAULT_MALE_TITLE { get; }
        public string DEFAULT_FEMALE_NAME { get; }
        public string DEFAULT_FEMALE_TITLE { get; }
        //-------------------------------
        public int HERO_STARTING_LEVEL { get; }
        public int HERO_STARTING_HP { get; }
        public int HERO_STARTING_POWER { get; }
        public int HERO_STARTING_GOLD { get; }
        public int HERO_STARTING_POTIONS { get; }
        public int HERO_STARTING_XP { get; }
        public int HERO_MAX_LEVEL { get; }
        public int HERO_MAX_HP_INCREASE { get; }
        public int HERO_POWER_INCREASE { get; }
        public int XP_LOSS_STRENGTH { get; }
        public int POTION_PRICE { get; }
        public int MAX_CONSECUTIVE_BATTLES { get; }
        //--------------------------------
        public double SLEEP_HEALING_RATIO { get; }
        public double POTION_POTENCY { get; }


        public Data()
        {
            GAME_TITLE = "MiniRPG";
            AUTHOR = "Dimitri GRABETTE";

            DEFAULT_MALE_NAME = "Le Vaillant Prince";
            DEFAULT_MALE_TITLE = "vaillant prince";

            DEFAULT_FEMALE_NAME = "La Vaillante Princesse";
            DEFAULT_FEMALE_TITLE = "vaillante princesse";

            HERO_STARTING_LEVEL = 1;
            HERO_STARTING_HP = 70;
            HERO_STARTING_POWER = 10;
            HERO_STARTING_GOLD = 0;
            HERO_STARTING_POTIONS = 1;
            HERO_STARTING_XP = 0;
            HERO_MAX_LEVEL = 10;

            HERO_MAX_HP_INCREASE = 15;
            HERO_POWER_INCREASE = 5;

            MAX_CONSECUTIVE_BATTLES = 5;
            XP_LOSS_STRENGTH = 6;
            SLEEP_HEALING_RATIO = 0.33f;

            POTION_PRICE = 50;
            POTION_POTENCY = 1; // 0 = aucun soin, 1 = soin total
        }
    }
}
