using System;

namespace MiniRPG.Game.Entities
{
    /// <summary>
    /// La classe de base de toute entité capable d'attaquer et recevoir des dégats.
    /// </summary>
    class Fighter
    {
        /// <value>
        /// <see cref="Name"/> est une propriété de type <see langword="string"/> représentant le nom du combattant.
        /// <example>(ex: Valérie)</example>
        /// </value>
        public string Name { get; set; } = "";

        /// <value>
        /// <see cref="Hp"/> est une propriété de type <see langword="int"/> représentant les points de vie restant du combattant.
        /// </value>
        public int Hp { get; protected set; } = 1;

        /// <value>
        /// <see cref="MaxHp"/> est une propriété de type <see langword="int"/> représentant les points de vie maximum 
        /// du combattants.
        /// </value>
        public int MaxHp { get; set; } = 1;

        /// <value>
        /// <see cref="Power"/> est une propriété de type <see langword="int"/> représentant la puissance d'attaque brute 
        /// du combattant.
        /// </value>
        public int Power { get; set; } = 0;

        /// <value>
        /// <see cref="Xp"/> est une propriété de type <see langword="int"/> représentant la quantité de points d'expérience 
        /// possédés ou accumulés par le combattant.
        /// </value>
        public int Xp { get; set; } = 0;

        /// <value>
        /// <see cref="Gold"/> est une propriété de type <see langword="int"/> représentant la quantité d'argent possédé 
        /// ou accumulé par le combattant.
        /// </value>
        public int Gold { get; set; } = 0;

        /// <value>
        /// <see cref="IsDead"/> est un accesseur de type <see langword="bool"/> permettant de savoir si le combattant est mort.
        /// <para>
        /// <remark>
        /// <i>Cette variable prends la valeur <c>true</c> lorsque les points de vie du combattant sont inférieur à <c>1</c>.</i>
        /// </remark>
        /// </para>
        /// </value>
        public bool IsDead { get => Hp <= 0; }

        /// <value>
        /// <see cref="IsFullHealth"/> est un accesseur de type <see langword="bool"/> permettant de savoir si le combattant est en pleine santé.
        /// <para>
        /// <remark>
        /// <i>Cette variable prends la valeur <c>false</c> lorsque les points de vie du combattant sont inférieur à ceux de <see cref="MaxHp"/>.</i>
        /// </remark>
        /// </para>
        /// </value>
        public bool IsFullHealth { get => Hp == MaxHp; }

        /// <summary>
        /// Constructeur par défaut de <see cref="Fighter"/>.
        /// </summary>
        public Fighter() { }

        /// <summary>
        /// Constructeur capable de construire une copie d'un objet <see cref="Fighter"/> existant.
        /// </summary>
        /// <param name="state"><see cref="Fighter"/> à copier.</param>
        public Fighter(Fighter state)
        {
            Name = state.Name;
            Hp = state.Hp;
            MaxHp = state.MaxHp;
            Power = state.Power;
            Xp = state.Xp;
            Gold = state.Gold;
        }

        /// <summary>
        /// Constructeur  permettant d'instancier un nouveau <see cref="Fighter"/> à partir de valeurs brutes.
        /// </summary>
        /// <param name="name">Le nom du combattant.</param>
        /// <param name="hp">Les points de vie restant du combattant.</param>
        /// <param name="maxHp">Les points de vie maximum du combattant.</param>
        /// <param name="power">La puissance d'attaque brute du combattant.</param>
        /// <param name="xp">Les points d'expérience du combattant.</param>
        /// <param name="gold">L'or du combattant.</param>
        public Fighter(string name, int hp, int maxHp, int power, int xp, int gold)
        {
            Name = name;
            Hp = hp;
            MaxHp = maxHp;
            Power = power;
            Xp = xp;
            Gold = gold;
        }

        /// <summary>
        /// Méthode permettant au combattant d'attaquer un autre combattant.
        /// </summary>
        /// <param name="state">Le <see cref="Fighter"/> cible de l'attaque.</param>
        /// <returns><see cref="Structs.AttackResult"/></returns>
        public Structs.AttackResult Attack(Fighter state)
        {
            Fighter target = new Fighter(state);
            int dammage = target.LoseHp(Power);

            return new Structs.AttackResult(target, dammage);
        }

        /// <summary>
        /// Retire des points de vie et renvoie le nombre de points de vie retiré, corrigé de la différence 
        /// si la puissance est supérieure au montant restant de points de vie.
        /// <para>
        /// <exemple><i>(ex: Si la puissance est de 10 et le nombre de Points de vie restant de 4, cette fonction 
        /// retirera 4 points de vie et retournera la valeur.)</i>
        /// </exemple>
        /// </para>
        /// </summary>
        /// <param name="attackPower"></param>
        /// <returns>Dégâts(<see langword="int"/>) infligés au combattant.</returns>
        public int LoseHp(int attackPower)
        {
            int dammage = attackPower - Math.Abs(Hp - attackPower);
            Hp -= dammage;

            return dammage; 
        }
    }
}
