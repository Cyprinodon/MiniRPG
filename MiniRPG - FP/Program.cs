using System;
using System.Collections.Generic;
using MiniRPG.Game;
using MiniRPG.Game.Entities;
using Structs = MiniRPG.Game.Structs;

namespace MiniRPG
{
    class GameFP
    {
        static void Main(string[] args)
        {
            bool running = true;

            Data.Phase phase = Data.Phase.Combat;

            Console.Title = $"{Data.GAME_TITLE} - Un jeu par {Data.AUTHOR}";

            Hero hero = introduceHero(new Hero());

            do
            {
                switch (phase)
                {
                    case Data.Phase.Combat:
                        // Gestion des combats :
                        hero = manageCombatPhase(hero);
                        // Si le tableau récupéré ne contient qu'une valeur, c'est qu'il s'est passé quelque chose.
                        phase = Data.Phase.Rest;
                        break;
                    case Data.Phase.Rest:
                        hero = manageRestPhase(hero);
                        phase = Data.Phase.Combat;
                        break;
                }

                if (hero.GaveUp)
                {
                    running = false;
                }
                else if (hero.IsDead)
                {
                    closeConsole("==========================================" +
                                 "\nVous n'avez pas su garder votre héros en vie, c'est la FIN DE LA PARTIE." +
                                 "\n(Ce jeu est INJUSTE, je sais...)");
                }
                else if (hero.HasWon)
                {
                    closeConsole("========================================== " +
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

        /* Demande une confirmation à l'utilisateur :
            - CustomMessage -> le message à afficher
            - yesKey -> la touche à enfoncer pour accepter
            - noKey -> la touche à enfoncer pour refuser
           Valeur de retour :
            - yesOrNo -> 'oui' vaut 'true', 'non' vaut 'false'*/
        public static bool? askForConfirmation(string customMessage = "En êtes-vous sûr(e) ?", string yesKey = "o", string noKey = "n")
        {
            string warning = null;
            string userInput;
            bool? yesOrNo = null;
            bool valid;

            // On compose le message à afficher
            customMessage = $"{customMessage}\n {yesKey} - Oui\n {noKey} - Non";

            do
            {
                Console.WriteLine(warning + customMessage);

                userInput = Console.ReadKey(true).Key.ToString();
                userInput = userInput.ToLower();

                Console.WriteLine(userInput);

                if (userInput == yesKey)
                {
                    valid = true;
                    yesOrNo = true;
                }
                else if (userInput == noKey)
                {
                    valid = true;
                    yesOrNo = false;
                }
                else
                {
                    valid = false;
                    warning = $"'{userInput}' n'est pas la saisie attendue. Saisissez '{yesKey}' ou '{noKey}'.\n";
                }
            } while (!valid);

            return yesOrNo;
        }

        /* Demande à l'utilisateur de saisir une réponse :
            - CustomMessage -> le message à afficher
            - maxLength -> le nombre maximum de caractères autorisé. renseigner une valeur inférieure à 1 pour une saisie illimitée
           Valeur de retour : 
            - userInput -> la chaîne saisie par l'utilisateur*/
        public static string askForInput(string customMessage = "Saisissez votre réponse", int maxLength = 0)
        {
            string warning = null;
            string userInput;
            bool valid;

            // On ajoute l'indicateur de nombre de caractères si applicable
            if (maxLength > 0)
            {
                customMessage = $"{customMessage}(max. {maxLength})";
            }

            customMessage += " :";

            do
            {
                Console.WriteLine(warning + customMessage);
                userInput = Console.ReadLine();

                //S'il n'y a pas de contrainte sur la saisie, il est inutile d'évaluer le reste de la boucle
                if (maxLength <= 0) break;

                if (userInput.Length > maxLength)
                {
                    valid = false;
                    warning = $"Votre saisie ne doit pas dépasser {maxLength} caractères. (Celle-ci en fait {userInput.Length})\n";
                }
                else
                {
                    valid = true;
                }
            } while (!valid);

            return userInput;
        }

        public static int askForInput(string customMessage = "Saisissez un nombre", int min = 1, int max = 5)
        {
            int integerValue;
            string warning = null;
            string userInput;
            bool valid;

            customMessage = $"{customMessage}(min. {min} max. {max}) :";

            do
            {
                Console.WriteLine(warning + customMessage);
                userInput = Console.ReadLine();
                if (Int32.TryParse(userInput, out integerValue) == false)
                {
                    warning = "votre saisie doit être un nombre ou un chiffre.\n";
                    valid = false;
                }
                else if (integerValue < min || integerValue > max)
                {
                    valid = false;
                    warning = $"Votre saisie doit être un nombre ou un chiffre compris entre {min} et {max} inclus.\n";
                }
                else
                {
                    valid = true;
                }
            } while (!valid);

            return integerValue;
        }

        public static int askForItemCount(string message, int maxCount)
        {
            int itemCount;
            bool confirmed;
            do
            {
                itemCount = askForInput(message, 0, maxCount);
                confirmed = (bool)askForConfirmation();
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
                playerName = askForInput("Entrez votre nom", 25);
                playerName = playerName.Trim();
                confirmed = (bool)askForConfirmation();

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

            do
            {
                genderIndex = askForChoice(Data.GENDER_CHOICES, "Êtes-vous");
                confirmed = (bool)askForConfirmation();
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

        /* Demande à l'utilisateur de choisir une option :
            - choices -> liste de choix 
               - keyInput -> La touche à saisir pour sélectionner ce choix
               - choiceLabel -> L'intitulé du choix
            - customMessage -> Le message demandant de faire un choix
           Valeur de retour :
            - userChoice -> un nombre entier symbolisant le choix de l'utilisateur*/
        public static int askForChoice(Structs.Input[] choices, string customMessage = "Faites un choix")
        {
            int userChoice = -1;
            bool valid = false;
            ConsoleKey userInput;
            string warning = null;

            customMessage += " :\n";
            
            // On génère l'affichage du choix
            foreach (var choice in choices)
            {
                customMessage += $"    {choice.Label} -> {choice.Description}\n";
            }

            do
            {
                Console.Write(warning + customMessage);
                userInput = Console.ReadKey(true).Key;
                string input = userInput.ToString().ToLower();
                Console.WriteLine(input);

                //On cherche une correspondance dans la liste des choix
                for (int i = 0; i < choices.Length; i++)
                {
                    ConsoleKey choiceKey = choices[i].Key;
                    if (userInput == choiceKey)
                    {
                        valid = true;
                        userChoice = i;
                        break;
                    }
                }
                // Si on ne la trouve pas après voir parcouru le tableau, c'est que la saisie n'est pas bonne
                if (!valid)
                {
                    warning = $"'{input}' n'est pas la saisie attendue. Saisissez une lettre parmi celles proposées.\n";
                }
            } while (!valid);

            return userChoice;
        }

        /* Effectue toute la logique liée à la phase de combat
            - heroText -> Liste des informations textuelles du héros
            - hero -> Liste des statistiques du héros
           Valeur de retour :
            - hero -> La liste mise à jour des statistiques du héros
            - -1 si le joueur choisit de quitter le jeu
            - -2 si le héros meurt */
        public static Hero manageCombatPhase(Hero state)
        {
            Hero hero = new Hero(state);

            //Variables du héros
            int playerChoice;

            bool inBattle = true;

            int battlesCount = 0;

            Console.WriteLine($"{hero.Name} entre dans le Donjon.");
            do
            {
                Battle fight = new Battle(hero);
                Console.WriteLine($"Un nouveau combat se prépare... ({battlesCount + 1}/{Data.MAX_CONSECUTIVE_BATTLES})");

                // Créer un monstre
                Monster monster = fight.SpawnRandomMonster(Data.MONSTERS);

                // Afficher les actions disponibles pour le joueur
                do
                {
                    playerChoice = askForChoice(Data.COMBAT_CHOICES, $"====================================\n" +
                        $"Face à ce dangereux {monster.Name}, {hero.Name} doit déterminer sa prochaine action");
                    // Résoudre l'action du joueur
                    switch (playerChoice)
                    {
                        case (int)Data.Combat.SeeStats:
                            string statisticsSheet = getHeroSheet(hero);
                            Console.WriteLine(statisticsSheet);
                            break;

                        case (int)Data.Combat.Attack:
                            fight = fight.DoPlayerAttack();
                            hero = fight.Hero;
                            monster = fight.Monster;
                            break;

                        case (int)Data.Combat.DrinkPotion:
                            fight = fight.DoPlayerDrinkPotion();
                            hero = fight.Hero;
                            break;

                        case (int)Data.Combat.Flee:
                            fight = fight.DoPlayerFlee();
                            hero = fight.Hero;
                            break;

                        case (int)Data.Combat.Quit:
                            hero.GaveUp = (bool)askForConfirmation("Êtes-vous sûr(e) de vouloir quitter ?");
                            break;
                    }

                    if (fight.IsDone)
                    {
                        Structs.Loot monsterLoot = new Structs.Loot(monster.Xp, monster.Gold);

                        Structs.PickupResult pickupResult = hero.PickUp(monsterLoot);
                        hero = pickupResult.Actor;
                        Console.WriteLine($"{hero.Name} a gagné {pickupResult.LootGained.Gold} pièces d'or et {pickupResult.LootGained.Xp} points d'expérience.");

                        if (hero.Xp == hero.LevelThreshold)
                        {
                            Console.WriteLine($"{hero.Name} a atteint la limite d'expérience autorisée ({hero.LevelThreshold} exp.). Dormez une fois en ville pour gagner un niveau.");
                        }
                    }

                    if(hero.IsDead)
                    {
                        return hero;
                    }

                } while (!hero.GaveUp && !monster.IsDead && !hero.HasFled);

                battlesCount++;
                hero.HasFled = false;

                if (battlesCount >= Data.MAX_CONSECUTIVE_BATTLES || hero.GaveUp)
                {
                    Console.WriteLine($"Tous les occupant de cette partie du donjon ont été tués ou évités.\n{hero.Name} sort du donjon.");
                    inBattle = false;
                }
            } while (inBattle);

            return hero;

        }

        public static Hero manageRestPhase(Hero state)
        {
            Hero hero = new Hero(state);

            bool done = false;

            //Variables du héros
            int playerChoice;

            Console.WriteLine($"{hero.Name} entre en ville.");
            do
            {
                playerChoice = askForChoice(Data.REST_CHOICES, $"====================================\n" +
                            $"Enfin au calme ! {hero.Name} peut effectuer une de ces actions");
                // Résoudre l'action du joueur
                switch (playerChoice)
                {
                    case (int)Data.Rest.SeeStats:
                        Console.WriteLine(getHeroSheet(hero));
                        break;

                    case (int)Data.Rest.Sleep:
                        hero = resolveHeroLevelingUp(hero);
                        hero = resolveHeroSleeping(hero);
                        if (hero.Level == Data.HERO_MAX_LEVEL)
                        {
                            hero.HasWon = true;
                        }
                        done = true;
                        break;

                    case (int)Data.Rest.BuyPotion:
                        int potionsCount;
                        int price;
                        bool tooExpensive;
                        do
                        {
                            Console.WriteLine($"{hero.Name} possède {hero.Gold} pièces d'or.");
                            potionsCount = askForItemCount($"Combien de potions ? ({Data.POTION_PRICE} pièces d'or par article)", 10);
                            price = Data.POTION_PRICE * potionsCount;
                            tooExpensive = hero.Gold < price;

                            if (tooExpensive)
                            {
                                Console.WriteLine($"{hero.Name} possède {hero.Gold} pièces d'or et" +
                                    $"ne peut pas se permettre cet achat de {price} pièces d'or.");
                            }
                        } while (tooExpensive);

                        if (potionsCount == 0)
                        {
                            break;
                        }

                        hero.Gold -= price;
                        hero.Potions += potionsCount;
                        Console.WriteLine($"{hero.Name} a acheté {potionsCount} potions pour la modique somme de {price} pièces d'or.");
                        done = true;
                        break;
                    case (int)Data.Rest.Quit:
                        hero.GaveUp = (bool)askForConfirmation("Êtes-vous sûr(e) de vouloir quitter ?");
                        break;
                }
            } while (!hero.GaveUp && !done);

            Console.WriteLine($"{hero.Name} a bien profité de ce répit mais il est temps de reprendre le combat.");

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
                $"\n____{hero.Name.ToUpper()}____" +
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

