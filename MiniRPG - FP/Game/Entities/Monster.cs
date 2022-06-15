namespace MiniRPG.Game.Entities
{
    /// <summary>
    /// Une entité capable d'attaquer et de lâcher un butin à sa mort.
    /// </summary>
    class Monster:Fighter
    {
        public Monster():base() { }

        public Monster(Structs.Monster template):base(template.Name, template.Hp, template.Hp, template.Power, template.Xp, template.Gold) { }

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

        public Monster Update(Fighter state)
        {
            return new Monster(this)
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
