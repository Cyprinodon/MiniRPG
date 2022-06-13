using MiniRPG.Game.Structs;
using System;

namespace MiniRPG.Game.Entities
{
    /// <summary>
    /// Une entité capable de monter en niveau, attaquer, fuir, boire des potions et se reposer.
    /// </summary>
    class Hero:Fighter
    {
        /// <value>
        /// <see cref="Title"/> est une propriété de type <see langword="string"/> représentant l'appellation courante du héros.
        /// <example><i>(ex: vaillant prince)</i></example>
        /// </value>
        public string Title { get; set; } = "";

        /// <value>
        /// <see cref="Gender"/> est une propriété de type <see cref="Structs.Gender"/> représentant le genre du héros.
        /// </value>
        public Gender Gender { get; set; } = Data.GENDER_CHOICES[0];

        /// <value>
        /// <see cref="Level"/> est une propriété de type <see langword="int"/> représentant le niveau du héros. 
        /// </value>
        public int Level { get; set; } = 1;

        /// <value>
        /// <see cref="Potion"/> est une propriété de type <see langword="int"/> représentant la quantité de potions 
        /// détenues par le héros.
        /// </value>
        public int Potions { get; set; } = 0;

        /// <value>
        /// <see cref="LevelThreshold"/> est un accesseur de type <see langword="int"/> permettant d'obtenir 
        /// la quantité d'expérience que le héros doit accumuler à son niveau pour passer au niveau suivant.
        /// </value>
        public int LevelThreshold
        {
            get
            {
                int levelIndex = Level - 1 <= 0 ? 0 : Level - 1;
                return Data.LEVEL_THRESHOLDS[levelIndex];
            }
        }

        /// <value>
        /// <see cref="GaveUp"/> est une propriété de type <see langword="bool"/> permettant de savoir si 
        /// le joueur a demandé à quitter la partie.
        /// </value>
        public bool GaveUp { get; set; } = false;

        /// <value>
        /// <see cref="HasWon"/> est une propriété de type <see langword="bool"/> permettant de savoir si le joueur 
        /// a gagné la partie.
        /// </value>
        public bool HasWon { get; set; } = false;

        /// <value>
        /// <see cref="HasFled"/> est une propriété de type <see langword="bool"/> permettant de savoir si le héros 
        /// a réussi sa tentative de fuite.
        /// </value>
        public bool HasFled { get; set; } = false;

        /// <summary>
        /// Constructeur par défaut de <see cref="Hero"/>.
        /// </summary>
        public Hero():base() { }

        /// <summary>
        /// Constructeur capable de construire une copie d'un objet <see cref="Hero"/> existant.
        /// </summary>
        /// <param name="state"><see cref="Hero"/> à copier.</param>
        public Hero(Hero state):base(state.Name, state.Hp, state.MaxHp, state.Power, state.Xp, state.Gold)
        {
            Title = state.Title;
            Gender = state.Gender;
            Level = state.Level;
            Potions = state.Potions;
            GaveUp = state.GaveUp;
            HasWon = state.HasWon;
            HasFled = state.HasFled;
        }

        public PickupResult PickUp(Loot loot)
        {
            Hero hero = new Hero(this);
            Loot gains = loot;

            if (hero.Xp + loot.Xp > hero.LevelThreshold)
            {
                int xpGains = hero.LevelThreshold - hero.Xp;
                gains = new Loot(loot.Gold, xpGains);
            }

            hero.Xp += gains.Xp;
            hero.Gold += gains.Gold;

            return new PickupResult(hero, gains);
        }

        public DrinkPotionResult DrinkPotion()
        {
            Hero hero = new Hero(this);
            bool usedPotion = false;

            if (hero.Potions > 0)
            {
                hero.Potions--;
                hero.Hp = hero.MaxHp;
                usedPotion = true;
            }

            return new DrinkPotionResult(hero, usedPotion);
        }
    }
}
