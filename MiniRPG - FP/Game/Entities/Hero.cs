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

        /// <summary>
        /// Méthode permettant au <see cref="Hero"/> de récupérer le contenu d'un objet <see cref="Loot"/>.
        /// </summary>
        /// <param name="loot">Le butin à ramasser.</param>
        /// <returns><see cref="PickupResult"/> : Un <see langword="struct"/> contenant le nouvel état 
        /// de <see cref="Hero"/> ainsi que le <see cref="Loot"/> effectivement obtenu.</returns>
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

        /// <summary>
        /// Méthode permettant au <see cref="Hero"/> de boire une potion.
        /// </summary>
        /// <returns><see cref="DrinkPotionResult"/></returns>
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

        /// <summary>
        /// Méthode permettant de calculer le nombre points d'expérience perdu.
        /// <para>
        /// <remark><i>Calcule le taux à l'aide de la variable <see cref="Data.XP_LOSS_STRENGTH"/>. 
        /// Contrainte entre <c>0</c> et <c>1</c>, la variable détermine le pourcentage de la barre d'expérience 
        /// qui sera retiré.</i></remark>
        /// </para>
        /// </summary>
        /// <returns><see cref="LoseXpResult"/></returns>
        public LoseXpResult LoseXp()
        {
            Hero hero = new Hero(this);

            int maxXpLost = hero.LevelThreshold / Data.XP_LOSS_STRENGTH;
            int xpLost = hero.Xp - maxXpLost;

            if (xpLost < 0)
            {
                xpLost = maxXpLost - Math.Abs(xpLost);
            }

            hero.Xp -= xpLost;

            return new LoseXpResult(hero, xpLost);
        }

        /// <summary>
        /// Méthode permettant de soigner le héros et calculer ne nombre de points de vie rendus.
        /// </summary>
        /// <param name="healingRatio">Taux de soin appliqué à la barre de vie. 
        /// <para><remark>La valeur doit être comprise entre <c>0</c> et <c>1</c>.</remark></para></param>
        /// <returns><see cref="HealResult"/></returns>
        public HealResult Heal(double healingRatio)
        {
            Hero hero = new Hero(this);
            int healAmount = (int)Math.Floor(hero.MaxHp * healingRatio);
            hero.Hp += healAmount;
            if (hero.Hp > hero.MaxHp)
            {
                int rest = hero.Hp - hero.MaxHp;
                hero.Hp = hero.MaxHp;
                healAmount -= rest;
            }
            return new HealResult(hero, healAmount);
        }

        /// <summary>
        /// Méthode permettant de construire un nouvel objet héros et de le mettre à jour sur la base d'un objet fighter et du héros d'origine.
        /// </summary>
        /// <param name="state">instance de <see cref="Fighter"/> dont le nouveau <see cref="Hero"/> doit récupérer les valeurs.</param>
        /// <returns><see cref="Hero"/></returns>
        public Hero Update(Fighter state)
        {
            return new Hero(this)
            {
                Name = state.Name,
                MaxHp = state.MaxHp,
                Hp = state.Hp,
                Power = state.Power,
                Xp = state.Xp,
                Gold = state.Gold
            };
        }
    }
}
