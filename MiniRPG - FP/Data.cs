using System.Collections.Generic;
using Key = System.ConsoleKey;

namespace MiniRPG
{
    static class Data
    {
        public enum Gender { Male, Female };
        public enum Phase { Combat, Rest };
        public enum HeroText { Name, Title, Genre };
        public enum Hero { Level, Hp, MaxHp, Power, Gold, Potions, Xp, LevelThreshold };
        public enum Monster { Name, Hp, Power, GoldLoot, XpLoot };
        public enum Combat { SeeStats, Attack, DrinkPotion, Flee, Quit };
        public enum Rest { SeeStats, Sleep, BuyPotion, Quit };
        //-------------------------------
        public static readonly string GAME_TITLE = "MiniRPG";
        public static readonly string AUTHOR = "Dimitri GRABETTE";
        public static readonly string DEFAULT_MALE_NAME = "Le Vaillant Prince";
        public static readonly string DEFAULT_MALE_TITLE = "vaillant prince";
        public static readonly string DEFAULT_FEMALE_NAME = "La Vaillante Princesse";
        public static readonly string DEFAULT_FEMALE_TITLE = "vaillante princesse";
        public static readonly string DEFAULT_MONSTER_NAME = "Monstre Indéterminé";
        //-------------------------------
        public static readonly int ALLOWED_NAME_LENGTH = 25;
        public static readonly int HERO_STARTING_LEVEL = 1;
        public static readonly int HERO_STARTING_HP = 70;
        public static readonly int HERO_STARTING_POWER = 10;
        public static readonly int HERO_STARTING_GOLD = 0;
        public static readonly int HERO_STARTING_POTIONS = 1;
        public static readonly int HERO_STARTING_XP = 0;
        public static readonly int HERO_MAX_LEVEL = 10;
        public static readonly int HERO_MAX_HP_INCREASE = 15;
        public static readonly int HERO_POWER_INCREASE = 5;
        public static readonly int XP_LOSS_STRENGTH = 6;
        public static readonly int POTION_PRICE = 50;
        public static readonly int MAX_CONSECUTIVE_BATTLES = 5;
        //--------------------------------
        public static readonly double SLEEP_HEALING_RATIO = 0.33f;
        public static readonly double POTION_POTENCY = 1.0f;

        //L'expérience à accumuler pour atteindre chaque niveau
        public static readonly int[] LEVEL_THRESHOLDS = new int[]
        {
                30, 60, 90, 120, 150, 180, 210, 240, 300
        };

        public static Structs.Gender[] GENDERS = new Structs.Gender[]
        {
            new Structs.Gender("H", "Un", "Homme"),
            new Structs.Gender("F", "Une", "Femme")
        };

        public static Structs.Input[] GENDER_CHOICES = new Structs.Input[]
        {
            new Structs.Input(Key.H, GENDERS[0].Article + " " + GENDERS[0].Name),
            new Structs.Input(Key.F, GENDERS[1].Article + " " + GENDERS[1].Name)
        };

        public static Dictionary<string, Structs.Monster> MONSTERS = new Dictionary<string, Structs.Monster>()
        {
            { "worm", new Structs.Monster("Ver de terre", 5, 0, 2, 0) },
            { "rat", new Structs.Monster("Rat", 15, 5, 8, 2) },
            { "goblin", new Structs.Monster("Gobelin", 45, 10, 16, 5) },
            { "skeleton", new Structs.Monster("Squelette", 80, 15, 32, 12) },
            { "demon", new Structs.Monster("Démon", 120, 30, 64, 28) },
            { "dragon", new Structs.Monster("Dragon", 250, 50, 128, 55) }
        };

        public static Structs.Input[] COMBAT_CHOICES = new Structs.Input[]
        {
            new Structs.Input(Key.I,"Informations sur le personnage"),
            new Structs.Input(Key.A, "Attaquer"),
            new Structs.Input(Key.P, "Boire une Potion"),
            new Structs.Input(Key.F, "Fuir"),
            new Structs.Input(Key.Escape, "Quitter")
        };

        public static Structs.Input[] REST_CHOICES = new Structs.Input[]
        {
            COMBAT_CHOICES[0],
            new Structs.Input(Key.D, "Dormir"),
            new Structs.Input(Key.P, "Acheter des Potions"),
            COMBAT_CHOICES[4]
        };
    }
}
