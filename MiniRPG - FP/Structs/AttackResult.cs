using MiniRPG.Entities;

namespace MiniRPG.Structs
{
    /// <summary>
    /// Le résultat d'une attaque d'un combattant sur un autre.
    /// </summary>
    struct AttackResult
    {
        /// <value>
        /// La cible victime de l'attaquant.
        /// </value>
        public Fighter Target;

        /// <value>
        /// La quantité de dégâts réels infligés à la cible.
        /// </value>
        public int Dammage;

        /// <summary>
        /// Le constructeur par défaut de <see cref="AttackResult"/>.
        /// </summary>
        /// <param name="target">La cible de l'attaque.</param>
        /// <param name="dammage">Les dégats reçus par la cible.</param>
        public AttackResult(Fighter target, int dammage)
        {
            Target = target;
            Dammage = dammage;
        }
    }
}
