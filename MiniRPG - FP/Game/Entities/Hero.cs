using MiniRPG.Game.Structs;

namespace MiniRPG.Game.Entities
{
    /// <summary>
    /// Une entité capable de monter en niveau, attaquer, fuir, boire des potions et se reposer.
    /// </summary>
    class Hero
    {
        /// <value>
        /// <see cref="Name"/> est une propriété de type <see langword="string"/> représentant le nom du héros.
        /// <example>(ex: Valérie)</example>
        /// </value>
        public string Name { get; set; } = "";

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
        /// <see cref="Hp"/> est une propriété de type <see langword="int"/> représentant les points de vie restant du héros.
        /// </value>
        public int Hp { get; set; } = 1;

        /// <value>
        /// <see cref="MaxHp"/> est une propriété de type <see langword="int"/> représentant les points de vie maximum 
        /// du héros.
        /// </value>
        public int MaxHp { get; set; } = 1;

        /// <value>
        /// <see cref="Power"/> est une propriété de type <see langword="int"/> représentant la puissance d'attaque brute 
        /// du héros.
        /// </value>
        public int Power { get; set; } = 0;

        /// <value>
        /// <see cref="Xp"/> est une propriété de type <see langword="int"/> représentant la quantité de points d'expérience 
        /// accumulés par le héros.
        /// </value>
        public int Xp { get; set; } = 0;

        /// <value>
        /// <see cref="Gold"/> est une propriété de type <see langword="int"/> représentant la quantité d'argent 
        /// accumulée par le héros.
        /// </value>
        public int Gold { get; set; } = 0;

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
        /// <see cref="IsDead"/> est un accesseur de type <see langword="bool"/> permettant de savoir si le héros est mort.
        /// <para></para>
        /// <remark>
        /// <i>Cette variable prends la valeur <c>true</c> lorsque les points de vie du héros sont inférieur à <c>1</c>.</i>
        /// </remark>
        /// </value>
        public bool IsDead { get => Hp <= 0; }

        /// <value>
        /// <see cref="HasFled"/> est une propriété de type <see langword="bool"/> permettant de savoir si le héros 
        /// a réussi sa tentative de fuite.
        /// </value>
        public bool HasFled { get; set; } = false;

        /// <summary>
        /// Constructeur par défaut de <see cref="Hero"/>.
        /// </summary>
        public Hero(){ }

        /// <summary>
        /// Constructeur capable de construire une copie d'un objet <see cref="Hero"/> existant.
        /// </summary>
        /// <param name="state"><see cref="Hero"/> à copier.</param>
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
