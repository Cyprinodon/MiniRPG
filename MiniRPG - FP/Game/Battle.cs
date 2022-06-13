using System;
using System.Collections.Generic;
using MiniRPG.Game.Entities;

namespace MiniRPG.Game
{
    class Battle
    {
        public Hero Hero;
        public Monster Monster;
        public bool IsDone = false;
        protected int Turn = 1;
        public Battle(Hero state)
        {
            Hero = new Hero(state);
        }

        public Battle(Battle state)
        {
            Hero = new Hero(state.Hero);
            Monster = new Monster(state.Monster);
            Turn = state.Turn;
            IsDone = state.IsDone;
        }

        /// <summary>
        /// Instancie un monstre aléatoire à partir d'une liste de modèles de monstre.
        /// </summary>
        /// <param name="monsterTemplates">La liste de modèles de données instanciables.</param>
        /// <returns>Une instance de <see cref="Entities.Monster"/>.</returns>
        public Monster SpawnRandomMonster(Dictionary<string, Structs.Monster> monsterTemplates)
        {
            //Copie des clefs du dictionnaire dans un tableau.
            string[] keys = new string[monsterTemplates.Count];
            monsterTemplates.Keys.CopyTo(keys, 0);

            //Génération d'un index aléatoire.
            Random randomizer = new Random();
            int randomIndex = randomizer.Next(0, keys.Length);

            //Récupération de la clef associée.
            string chosenKey = keys[randomIndex];
            
            //Obtention du modèle de monstre à partir de la clef.
            Structs.Monster monsterTemplate = monsterTemplates[chosenKey];

            //Instanciation du monstre à partir du modèle.
            Monster = new Monster(monsterTemplate);
            Console.WriteLine($"Un {Monster.Name} apparait !\nIl possède {Monster.MaxHp} points de vie et ses attaques infligent {Monster.Power} points de dégâts.");

            return Monster;
        }

        public Battle DoPlayerAttack()
        {
            //Création des combattants à partir de leur état précédent.
            Battle state = new Battle(this);

            //Résolution de l'attaque et récupération du résultat.
            Structs.AttackResult attackResult = state.Hero.Attack(state.Monster);

            //Mise à jour du monstre.
            state.Monster = (Monster)attackResult.Target;

            //Conception du rapport d'événement.
            string attackLog = $"{state.Hero.Name} décide d'attaquer le {state.Monster.Name} en le frappant de son épée." +
                            $"\n{state.Monster.Name} perds {attackResult.Dammage} points de vie.";

            if (state.Monster.IsDead) //Le monstre a t-il été tué ?
            {
                attackLog += $"\n{state.Monster.Name} meurt de ses blessures.";
                state.IsDone = true; //Le combat est terminé.
            }
            else
            {
                attackLog += $" Il lui en reste {state.Monster.Hp}." +
                             $"\n{state.Monster.Name} est toujours en vie !";
            }

            state.Turn++; //Mise à jour du compteur de tours.

            //Ecriture du rapport dans la console.
            Console.WriteLine(attackLog);

            return state;
        }

        public Battle DoPlayerDrinkPotion()
        {
            Battle state = new Battle(this);

            if(state.Hero.IsFullHealth)
            {
                Console.WriteLine($"{state.Hero.Name} n'est pas blessé(e), boire une potion serait du pur gâchis !");
            }
            else if(state.Hero.Potions <= 0)
            {
                Console.WriteLine($"{state.Hero.Name} voudrait boire une potion de soin mais n'en a pas de disponible dans son inventaire. Il va falloir penser à se refaire un stock !");
            }
            else //Si le héros dispose de potions et est blessé.
            {
                //Résolution de l'action et récupération du résultat.
                Structs.DrinkPotionResult drinkPotionResult = state.Hero.DrinkPotion();
                state.Hero = drinkPotionResult.Drinker;
                Console.WriteLine($"{state.Hero.Name} boit une potion de soin. Ses blessures sont entièrement guéries !\nIl lui reste {state.Hero.Potions} potions.");
            }

            return state;
        }
    }
}
