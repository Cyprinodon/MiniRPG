using System;
using System.Collections.Generic;
using MiniRPG.Entities;

namespace MiniRPG
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
            state.Monster = state.Monster.Update(attackResult.Target);

            //Conception du rapport d'événement.
            string attackLog = $"{state.Hero.Name} décide d'attaquer le {state.Monster.Name} en le frappant de son épée." +
                            $"\nLe {state.Monster.Name} perds {attackResult.Dammage} points de vie.";

            if (state.Monster.IsDead) //Le monstre a t-il été tué ?
            {
                attackLog += $"\nLe {state.Monster.Name} meurt de ses blessures.";
                state.IsDone = true; //Le combat est terminé.
            }
            else
            {
                attackLog += $" Il lui en reste {state.Monster.Hp}." +
                             $"\nLe {state.Monster.Name} est toujours en vie !";

                attackResult = state.Monster.Attack(state.Hero);
                state.Hero = state.Hero.Update(attackResult.Target);

                attackLog += $"\n\nLe {state.Monster.Name} ripose vicieusement ! " +
                             $"\n{state.Hero.Name} perds {attackResult.Dammage} points de vie.";
            }

            if (state.Hero.IsDead)
            {
                attackLog += $"\n{state.Hero.Name} meurt de ses blessures.";
            }
            else
            {
                attackLog += $" Il lui en reste {state.Hero.Hp}.";
            }

            state.Turn++; //Mise à jour du compteur de tours.

            //Ecriture du rapport dans la console.
            Console.WriteLine(attackLog);

            return state;
        }

        public Battle DoPlayerFlee()
        {
            //Création des combattants à partir de leur état précédent.
            Battle state = new Battle(this);

            string actionLog = "";

            double flightChances;
            double flightAttempt;

            Random randomizer = new Random();

            if (state.Monster.Hp >= state.Monster.MaxHp * 0.75)
            {
                flightChances = 0.5;
            }
            else if (state.Monster.Hp >= state.Monster.MaxHp * 0.5)
            {
                flightChances = 0.75;
            }
            else
            {
                flightChances = 1;
            }

            flightAttempt = randomizer.NextDouble();

            if (flightAttempt <= flightChances)
            {
                string partialText = "";
                Structs.LoseXpResult loseXpResult = state.Hero.LoseXp();
                state.Hero = loseXpResult.Actor;
                state.Hero.HasFled = true;

                if (loseXpResult.XpLost > 0)
                {
                    partialText = $" et perds {loseXpResult.XpLost} points d'expérience";
                }

                actionLog = $"{state.Hero.Name} fuit le combat" + partialText + ".";
            }
            else
            {
                Structs.AttackResult attackResult = state.Monster.Attack(state.Hero);
                state.Hero = state.Hero.Update(attackResult.Target);

                actionLog = $"{state.Hero.Name} tente de fuir le combat mais le {state.Monster.Name} l'en empêche." +
                            $"\n\nLe {state.Monster.Name} ripose vicieusement ! " +
                            $"\n{state.Hero.Name} perds {attackResult.Dammage} points de vie.";

                if (state.Hero.IsDead)
                {
                    actionLog += $"\n{state.Hero.Name} meurt de ses blessures.";
                }
                else
                {
                    actionLog += $" Il lui en reste {state.Hero.Hp}.";
                }
            }
            Console.WriteLine(actionLog);

            return state;
        }

        public Battle DoPlayerDrinkPotion()
        {
            Battle state = new Battle(this);

            if (state.Hero.IsFullHealth)
            {
                Console.WriteLine($"{state.Hero.Name} n'est pas blessé(e), boire une potion serait du pur gâchis !");
            }
            else if (state.Hero.Potions <= 0)
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
