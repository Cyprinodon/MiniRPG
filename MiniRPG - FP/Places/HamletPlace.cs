using MiniRPG.Entities;
using MiniRPG.Structs;
using MiniRPG.TextInterfaces.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniRPG.Places
{
    internal class HamletPlace : Place
    {
        public HamletPlace(Hero hero, IEnumerable<Input> actions) : base("hameau", hero, actions)
        {
        }

        protected override bool DoTurn()
        {

            ChoicePrompt prompt = new ChoicePrompt(null, Data.REST_CHOICES);
            ConfirmationPrompt quitPrompt = new ConfirmationPrompt();

            //Variables du héros
            int playerChoice;

                prompt.Message = $"====================================\n" +
                            $"Enfin au calme ! {Hero.Name} peut effectuer une de ces actions";
                playerChoice = prompt.AskForInput();

                // Résoudre l'action du joueur
                switch (playerChoice)
                {
                    case (int)Data.Rest.SeeStats:
                        Console.WriteLine(Game.getHeroSheet(Hero));
                        break;

                    case (int)Data.Rest.Sleep:
                        Hero = Game.resolveHeroLevelingUp(Hero);
                        Hero = Game.resolveHeroSleeping(Hero);
                        if (Hero.Level == Data.HERO_MAX_LEVEL)
                        {
                            Hero.HasWon = true;
                        }
                        return true;

                    case (int)Data.Rest.BuyPotion:
                        int potionsCount;
                        int price;
                        bool tooExpensive;
                        do
                        {
                            Console.WriteLine($"{Hero.Name} possède {Hero.Gold} pièces d'or.");
                            potionsCount = Game.askForItemCount($"Combien de potions ? ({Data.POTION_PRICE} pièces d'or par article)", 10);
                            price = Data.POTION_PRICE * potionsCount;
                            tooExpensive = Hero.Gold < price;

                            if (tooExpensive)
                            {
                                Console.WriteLine($"{Hero.Name} possède {Hero.Gold} pièces d'or et" +
                                    $"ne peut pas se permettre cet achat de {price} pièces d'or.");
                            }
                        } while (tooExpensive);

                        if (potionsCount == 0)
                        {
                            break;
                        }

                        Hero.Gold -= price;
                        Hero.Potions += potionsCount;
                        Console.WriteLine($"{Hero.Name} a acheté {potionsCount} potions pour la modique somme de {price} pièces d'or.");
                        return true;

                    case (int)Data.Rest.Quit:
                        Hero.GaveUp = (bool)quitPrompt.AskForInput();
                        break;
                }

            if (Hero.GaveUp) return true;

            return false;
        }

        public override void Exit()
        {
            if(!Hero.GaveUp)
            {
                Console.WriteLine($"{Hero.Name} a bien profité de ce répit mais il est temps de reprendre le combat.");
                base.Exit();
            }
        }
    }
}
