namespace MiniRPG.Game.Entities
{
    /// <summary>
    /// Une entité capable d'attaquer et de lâcher un butin à sa mort.
    /// </summary>
    class Monster
    {
        /// <value>
        /// <see cref="Name"/> est une propriété de type <see langword="string"/> représentant le type de monstre.
        /// <example><i>(ex: Gobelin)</i></example>
        /// </value>
        public string Name { get; set; }

        /// <value>
        /// <see cref="MaxHp"/> est une propriété de type <see langword="int"/> représentant les points de vie maximum 
        /// du monstre.
        /// </value>
        public int MaxHp { get; set; }

        /// <value>
        /// <see cref="Hp"/> est une propriété de type <see langword="int"/> représentant les points de vie restant du monstre.
        /// </value>
        public int Hp { get; set; }

        /// <value>
        /// <see cref="MaxHp"/> est une propriété de type <see langword="int"/> représentant les points de vie maximum 
        /// du monstre.
        /// </value>
        /// 
        /// <value>
        /// <see cref="Power"/> est une propriété de type <see langword="int"/> représentant la puissance d'attaque brute 
        /// du monstre.
        /// </value>
        public int Power { get; set; }

        /// <value>
        /// <see cref="Gold"/> est une propriété de type <see langword="int"/> représentant la quantité d'argent 
        /// lâché par le monstre à sa mort.
        /// </value>
        public int Gold { get; set; }

        /// <value>
        /// <see cref="Xp"/> est une propriété de type <see langword="int"/> représentant la quantité de points d'expérience 
        /// lâchés par le monstre à sa mort.
        /// </value>
        public int Xp { get; set; }

        /// <value>
        /// <see cref="IsDead"/> est un accesseur de type <see langword="bool"/> permettant de savoir si le monstre est mort.
        /// <para>
        /// <remark>
        /// <i>Cette variable prends la valeur <c>true</c> lorsque les points de vie du monstre sont inférieur à <c>1</c>.</i>
        /// </remark>
        /// </para>
        /// </value>
        public bool IsDead { get => Hp <= 0; }

        /// <summary>
        /// Constructeur par défaut de <see cref="Monster"/> capable d'instancier un monstre à partir de son modèle de données.
        /// <para>
        /// Voir aussi:
        /// <seealso cref="Structs.Monster"/>
        /// </para>
        /// </summary>
        /// <param name="template">Le modèle de données du monstre.</param>
        public Monster(Structs.Monster template)
        {
            Name = template.Name;
            Hp = MaxHp = template.Hp;
            Power = template.Power;
            Gold = template.Gold;
            Xp = template.Xp;
        }

        /// <summary>
        /// Constructeur de <see cref="Monster"/> capable d'instancier une copie d'un autre objet <see cref="Monster"/>.
        /// </summary>
        /// <param name="state">L'objet <see cref="Monster"/> à copier.</param>
        public Monster(Monster state)
        {
            Name = state.Name;
            MaxHp = state.MaxHp;
            Hp = state.Hp;
            Power = state.Power;
            Gold = state.Gold;
            Xp = state.Xp;
        }
    }
}
