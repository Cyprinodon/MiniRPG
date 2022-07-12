using MiniRPG.Entities;
using MiniRPG.TextInterfaces.Prompts;
using MiniRPG.Utilities;
using System;
using System.Collections.Generic;

namespace MiniRPG.Places
{
    class DungeonPlace : Place
    {
        public Monster Monster;
        public ProgressCounter BattleCounter = new ProgressCounter(Data.MAX_CONSECUTIVE_BATTLES, 1);
        public bool HeroLost { get => Hero.IsDead; }

        public DungeonPlace(Hero hero, IEnumerable<Structs.Input> actions) : base("donjon", hero, actions)
        {
            
        }

        protected override bool DoTurn()
        {
            Battle fight = new Battle(Hero);
            Console.WriteLine($"Un nouveau combat se prépare... ({BattleCounter})");

            Monster = fight.SpawnRandomMonster(Data.MONSTERS);

            ChoicePrompt prompt = new ChoicePrompt(null, Data.COMBAT_CHOICES);
            ConfirmationPrompt quitPrompt = new ConfirmationPrompt("Êtes-vous sûr(e) de vouloir quitter ?");

            do
            {
                prompt.Message = $"====================================\n" +
                    $"Face à ce dangereux {Monster.Name}, {Hero.Name} doit déterminer sa prochaine action";

                int playerChoice = prompt.AskForInput();
                // Résoudre l'action du joueur
                switch (playerChoice)
                {
                    case (int)Data.Combat.SeeStats:
                        string statisticsSheet = Game.getHeroSheet(Hero);
                        Console.WriteLine(statisticsSheet);
                        break;

                    case (int)Data.Combat.Attack:
                        fight = fight.DoPlayerAttack();
                        Hero = fight.Hero;
                        Monster = fight.Monster;
                        break;

                    case (int)Data.Combat.DrinkPotion:
                        fight = fight.DoPlayerDrinkPotion();
                        Hero = fight.Hero;
                        break;

                    case (int)Data.Combat.Flee:
                        fight = fight.DoPlayerFlee();
                        Hero = fight.Hero;
                        break;

                    case (int)Data.Combat.Quit:
                        Hero.GaveUp = (bool)quitPrompt.AskForInput();
                        break;
                }

                if (Hero.IsDead) return true;

                if (fight.IsDone)
                {
                    Structs.Loot monsterLoot = new Structs.Loot(Monster.Xp, Monster.Gold);

                    Structs.PickupResult pickupResult = Hero.PickUp(monsterLoot);
                    Hero = pickupResult.Actor;
                    Console.WriteLine($"{Hero.Name} a gagné {pickupResult.LootGained.Gold} pièces d'or et {pickupResult.LootGained.Xp} points d'expérience.");

                    if (Hero.Xp == Hero.LevelThreshold)
                    {
                        Console.WriteLine($"{Hero.Name} a atteint la limite d'expérience autorisée ({Hero.LevelThreshold} exp.). Dormez une fois en ville pour gagner un niveau.");
                    }
                }

            } while (!Hero.GaveUp && !Monster.IsDead && !Hero.HasFled);

            BattleCounter.CountUp();
            Hero.HasFled = false;

            if (Hero.GaveUp) return true;
            
            if (BattleCounter.IsDone)
            {
                Console.WriteLine($"Tous les occupant de cette partie du donjon ont été tués ou évités.");
                return true;
            }

            return false;
        }

        public override void Exit()
        {
            if(!Hero.IsDead)
            {
                base.Exit();
            }
        }

        public Hero GetHero()
        {
            return Hero;
        }
    }
}
