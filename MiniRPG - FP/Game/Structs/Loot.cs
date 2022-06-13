namespace MiniRPG.Game.Structs
{
    /// <summary>
    /// Représente le butin lâché par un monstre vaincu.
    /// </summary>
    struct Loot
    {
        /// <value>
        /// Quantité d'or lâché par le monstre à sa mort.
        /// </value>
        public readonly int Gold;

        /// <value>
        /// Quantité d'de points d'expérience lâché par le monstre à sa mort.
        /// </value>
        public readonly int Xp;

        /// <summary>
        /// Constructeur par défaut du <see langword="struct"/> <see cref="Loot"/>.
        /// </summary>
        /// <param name="gold">Or du butin.</param>
        /// <param name="xp">Expérience du butin.</param>
        public Loot(int gold = 0, int xp = 0)
        {
            Gold = gold;
            Xp = xp;
        }
    }
}
