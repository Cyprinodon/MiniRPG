using System;
using System.Collections.Generic;
using MiniRPG.Entities;
using MiniRPG.Places;
using MiniRPG.Structs;
using MiniRPG.TextInterfaces.Prompts;

namespace MiniRPG
{
    class Game
    {
        static void Main(string[] args)
        {
            bool running = true;

            Data.Phase phase = Data.Phase.Combat;

            Console.Title = $"{Data.GAME_TITLE} - Un jeu par {Data.AUTHOR}";

            Hero hero = introduceHero(new Hero());
            DungeonPlace dungeon = new DungeonPlace(hero, new List<Input>() { });
            HamletPlace hamlet = new HamletPlace(hero, new List<Input>() { });

            do
            {
                switch (phase)
                {
                    case Data.Phase.Combat:
                        dungeon.Enter();
                        dungeon.Resolve();
                        dungeon.Exit();

                        phase = Data.Phase.Rest;
                        break;
                    case Data.Phase.Rest:
                        hamlet.Enter();
                        hamlet.Resolve();
                        hamlet.Exit();

                        phase = Data.Phase.Combat;
                        break;
                }
                hero = dungeon.GetHero();

                if (hero.GaveUp)
                {
                    running = false;
                }
                else if (hero.IsDead)
                {
                    closeConsole("=DEFAITE==================================" +
                                 "\nVous n'avez pas su garder votre héros en vie, c'est la FIN DE LA PARTIE." +
                                 "\n(Ce jeu est INJUSTE, je sais...)");
                }
                else if (hero.HasWon)
                {
                    closeConsole("=VICTOIRE================================= " +
                                 "\nBravo ! Vous avez atteint le dernier niveau et complété le jeu, c'est la FIN DE LA PARTIE.\nMerci d'avoir joué.");
                }
            } while (running);
        }

        public static void closeConsole(string message)
        {
            Console.WriteLine(message + "\nAppuyez sur une touche pour fermer la console...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        public static int askForItemCount(string message, int maxCount)
        {
            int itemCount;
            bool confirmed;
            
            IntPrompt prompt = new IntPrompt(message, 0, maxCount);
            ConfirmationPrompt confirmation = new ConfirmationPrompt();
            do
            {
                itemCount = prompt.AskForInput();
                confirmed = (bool)confirmation.AskForInput();
            } while (!confirmed);

            return itemCount;
        }

        /* Demande au joueur de saisir un nom pour son héros :
            - defaultName -> Le nom par défaut à utiliser si le joueur décide de ne rien saisir
            - gender -> un indice symbolisant le sexe du joueur. (voir l'énumération Gender associée)
           Valeur de retour :
            - playerName -> Le nom saisi par le joueur*/
        public static string askForHeroName(string defaultName, int gender)
        {
            string playerName;
            string playerTitle;
            bool confirmed;

            StringPrompt prompt = new StringPrompt("Entrez votre nom", 25);
            ConfirmationPrompt confirmation = new ConfirmationPrompt();

            if (gender == (int)Data.Gender.Female)
            {
                playerTitle = "héroïne";
            }
            else
            {
                playerTitle = "héros";
            }

            do
            {
                playerName = prompt.AskForInput();
                playerName = playerName.Trim();
                confirmed = (bool)confirmation.AskForInput();

            } while (!confirmed);

            if (playerName == "")
            {
                Console.WriteLine($"Notre {playerTitle} ne connait pas son nom, nous l'appellerons donc : {defaultName}.");
                playerName = defaultName;
            }

            return playerName;
        }

        /* Demande au joueur de saisir les informations relatives à son héros :
           Valeur de retour :
            - string[] ->
               - playerName -> Nom saisi par le joueur
               - playerTitle -> Titre honorifique du joueur
               - playerGender -> Sexe choisi par le joueur */
        public static Hero introduceHero(Hero hero)
        {
            int genderIndex;
            string defaultName;
            bool confirmed;

            ChoicePrompt prompt = new ChoicePrompt("Êtes-vous", Data.GENDER_CHOICES);
            ConfirmationPrompt confirmation = new ConfirmationPrompt();
            do
            {
                genderIndex = prompt.AskForInput();
                confirmed = (bool)confirmation.AskForInput();
            } while (!confirmed);

            switch (genderIndex)
            {
                case (int)Data.Gender.Female:
                    defaultName = Data.DEFAULT_FEMALE_NAME;
                    hero.Title = Data.DEFAULT_FEMALE_TITLE;
                    break;
                case (int)Data.Gender.Male:
                default:
                    defaultName = Data.DEFAULT_MALE_NAME;
                    hero.Title = Data.DEFAULT_MALE_TITLE;
                    break;
            }

            Console.WriteLine($"Vous êtes un(e) {hero.Title} en quête d'aventures.");

            hero.Name = askForHeroName(defaultName, genderIndex);
            hero.Gender = Data.GENDERS[genderIndex];
            hero.Level = Data.HERO_STARTING_LEVEL;
            hero.Gold = Data.HERO_STARTING_GOLD;
            hero.Health = Data.HERO_STARTING_HP;
            hero.Potions = Data.HERO_STARTING_POTIONS;
            hero.Power = Data.HERO_STARTING_POWER;
            hero.Xp = Data.HERO_STARTING_XP;

            return hero;
        }

        /* Retourne la feuille de personnage à afficher :
            - heroText -> liste des informations textuelles sur le héros
            - hero -> liste des charactéristiques du héros
           Valeur de retour :
               - feuille de personnage sous forme de message */
        public static string getHeroSheet(Hero state)
        {
            Hero hero = state;
            string levelOutput;
            int nextLevel;

            if (hero.Level == Data.HERO_MAX_LEVEL)
            {
                levelOutput = $"\n    *°*A atteint le niveau maximum*°*";
            }
            else
            {
                nextLevel = hero.Level + 1;
                levelOutput = $"\n    Expérience accumulée pour le niveau suivant ({nextLevel}) : {hero.Xp}/{hero.LevelThreshold}";
            }

            return $"{hero.Name} regarde sa feuille de personnage :" +
                $"\n={hero.Name.ToUpper()}=====================" +
                $"\n    Un(e) {hero.Title} de niveau {hero.Level}." +
                levelOutput +
                $"\n    Puissance d'attaque : {hero.Power}" +
                $"\n    Points de vie : {hero.Hp}/{hero.MaxHp}" +
                $"\n    Inventaire : {hero.Potions} potions." +
                $"\n    Bourse : {hero.Gold} pièces d'Or.";
        }

        public static Hero resolveHeroSleeping(Hero state)
        {
            Hero hero = new Hero(state);

            if (hero.Hp < hero.MaxHp)
            {
                Structs.HealResult healResult = hero.Heal(Data.SLEEP_HEALING_RATIO);
                hero = healResult.Actor;
                Console.WriteLine($"{hero.Name} récupère {healResult.HealAmount} points de vie pour avoir dormi dans un lit confortable.");
            }
            else
            {
                Console.WriteLine($"{hero.Name} a dormi dans un lit confortable.");
            }

            return hero;
        }

        public static Hero resolveHeroLevelingUp(Hero state)
        {
            Hero hero = new Hero(state);

            if (hero.Xp >= hero.LevelThreshold && hero.Level < Data.HERO_MAX_LEVEL)
            {
                hero.Level++;
                hero.Xp = 0;
                hero.Health = hero.MaxHp + Data.HERO_MAX_HP_INCREASE;
                hero.Power += Data.HERO_POWER_INCREASE;
                Console.WriteLine($"Avec {hero.Xp} points d'expérience accumulés, {hero.Name} passe au niveau {hero.Level}." +
                    $"\nLes points de vie de {hero.Name} passent à {hero.MaxHp}(+{Data.HERO_MAX_HP_INCREASE}) et " +
                    $"sa puissance monte à {hero.Power}(+{Data.HERO_POWER_INCREASE}).");
            }

            return hero;
        }
    }
}

