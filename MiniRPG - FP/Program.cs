using System;
using MiniRPG.Game;

namespace MiniRPG
{
    class GameFP
    {
        static void Main(string[] args)
        {
            Data data = new Data();

            //L'expérience à accumuler pour atteindre chaque niveau
            int[] levelsThresholds = new int[]
            {
                30, 60, 90, 120, 150, 180, 210, 240, 300
            };

            // Ordre des valeurs : Nom, Titre, Genre
            string[] heroText;
            // Ordre des valeurs : Niveau, Vie, Puissance, Or, Expérience, expérience requise
            int[] hero;
            int nextLevelThreshold;

            int gameEvent = 0;

            bool running = true;

            Data.Phase phase = Data.Phase.Combat;

            Console.Title = $"{data.GAME_TITLE} - Un jeu par {data.AUTHOR}";

            heroText = introduceHero(data);
            int levelIndex = data.HERO_STARTING_LEVEL - 1 == 0 ? 0 : data.HERO_STARTING_LEVEL - 1;
            nextLevelThreshold = levelsThresholds[levelIndex];

            hero = new int[8];
            hero[(int)Data.Hero.Level] = data.HERO_STARTING_LEVEL;
            hero[(int)Data.Hero.Hp] = data.HERO_STARTING_HP;
            hero[(int)Data.Hero.MaxHp] = data.HERO_STARTING_HP;
            hero[(int)Data.Hero.Power] = data.HERO_STARTING_POWER;
            hero[(int)Data.Hero.Gold] = data.HERO_STARTING_GOLD;
            hero[(int)Data.Hero.Potions] = data.HERO_STARTING_POTIONS;
            hero[(int)Data.Hero.Xp] = data.HERO_STARTING_XP;
            hero[(int)Data.Hero.LevelThreshold] = nextLevelThreshold;

            do
            {
                switch (phase)
                {
                    case Data.Phase.Combat:
                        // Gestion des combats :
                        hero = manageCombatPhase(heroText, hero, data);
                        // Si le tableau récupéré ne contient qu'une valeur, c'est qu'il s'est passé quelque chose.
                        phase = Data.Phase.Rest;
                        break;
                    case Data.Phase.Rest:
                        hero = manageRestPhase(heroText, hero, levelsThresholds, data);
                        phase = Data.Phase.Combat;
                        break;
                }

                if (hero.Length == 1)
                {
                    gameEvent = hero[0];
                }

                if (gameEvent == -2) //Le joueur désire quitter
                {
                    running = false;
                }
                else if (gameEvent == -1)// Le personnage est mort
                {
                    closeConsole("==========================================" +
                                 "\nVous n'avez pas su garder votre héros en vie, c'est la FIN DE LA PARTIE." +
                                 "\n(Ce jeu est INJUSTE, je sais...)");
                }
                else if (gameEvent == -3)
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
        public static string[] introduceHero(Data data)
        {
            string playerGender;
            int genderIndex;
            string defaultName, playerName;
            string playerTitle;
            bool confirmed;
            /* Si l'on souhaite étendre le nombre de genres, 
             * toujours ajouter l'article - soit un ou une -
             * sinon, l'extraction du genre sera erronnée. */
            string[][] genderChoices = new string[][] {
                new string[] { "h", "Un Homme" },
                new string[] { "f", "Une Femme" },
            };

            do
            {
                genderIndex = askForChoice(genderChoices, "Êtes-vous");
                confirmed = (bool)askForConfirmation();
            } while (!confirmed);

            switch (genderIndex)
            {
                case (int)Data.Gender.Female:
                    defaultName = data.DEFAULT_FEMALE_NAME;
                    playerTitle = data.DEFAULT_FEMALE_TITLE;
                    break;
                case (int)Data.Gender.Male:
                default:
                    defaultName = data.DEFAULT_MALE_NAME;
                    playerTitle = data.DEFAULT_MALE_TITLE;
                    break;
            }

            Console.WriteLine($"Vous êtes un(e) {playerTitle} en quête d'aventures.");

            playerName = askForHeroName(defaultName, genderIndex);
            //On récupère le label du genre
            playerGender = genderChoices[genderIndex][1];
            playerGender = playerGender.ToLower();

            string[] heroText = new string[3];
            heroText[(int)Data.HeroText.Name] = playerName;
            heroText[(int)Data.HeroText.Title] = playerTitle;
            heroText[(int)Data.HeroText.Genre] = playerGender;

            return heroText;
        }

        /* Demande à l'utilisateur de choisir une option :
            - choices -> liste de choix 
               - keyInput -> La touche à saisir pour sélectionner ce choix
               - choiceLabel -> L'intitulé du choix
            - customMessage -> Le message demandant de faire un choix
           Valeur de retour :
            - userChoice -> un nombre entier symbolisant le choix de l'utilisateur*/
        public static int askForChoice(string[][] choices, string customMessage = "Faites un choix")
        {
            int userChoice = -1;
            bool valid = false;
            string userInput;
            string warning = null;

            customMessage += " :\n";

            // On génère l'affichage du choix
            foreach (string[] choice in choices)
            {
                string choiceKey = choice[0];
                string choiceLabel = choice[1];

                customMessage += $"    {choiceKey} -> {choiceLabel}\n";
            }

            do
            {
                Console.Write(warning + customMessage);
                userInput = Console.ReadKey(true).Key.ToString();
                userInput = userInput.ToLower();
                Console.WriteLine(userInput);

                //On cherche une correspondance dans la liste des choix
                for (int i = 0; i < choices.Length; i++)
                {
                    string choiceKey = choices[i][0].ToLower();
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
                    warning = $"'{userInput}' n'est pas la saisie attendue. Saisissez une lettre parmi celles proposées.\n";
                }
            } while (!valid);

            return userChoice;
        }

        /* Choisis un monstre dans la liste fournie en paramètre et affiche son apparition
            - monsterList -> La liste contenant tous les monstres et leurs valeurs
           Valeur de retour :
            - monsterIndex -> l'indice du monstre dans la liste*/
        public static int spawnRandomMonster(object[][] monstersList)
        {
            int monsterIndex;
            string monsterName;
            int monsterHp;
            int monsterPower;
            Random randomizer = new Random();

            monsterIndex = randomizer.Next(0, monstersList.Length);
            monsterName = (string)monstersList[monsterIndex][0];
            monsterHp = (int)monstersList[monsterIndex][1];
            monsterPower = (int)monstersList[monsterIndex][2];
            Console.WriteLine($"Un {monsterName} apparait !\nIl possède {monsterHp} points de vie et ses attaques infligent {monsterPower} points de dégâts.");

            return monsterIndex;
        }

        /* Effectue toute la logique liée à la phase de combat
            - heroText -> Liste des informations textuelles du héros
            - hero -> Liste des statistiques du héros
           Valeur de retour :
            - hero -> La liste mise à jour des statistiques du héros
            - -1 si le joueur choisit de quitter le jeu
            - -2 si le héros meurt */
        public static int[] manageCombatPhase(string[] heroText, int[] hero, Data data)
        {
            //Signification des valeurs : Nom, vie, puissance, expérience, butin
            object[][] monsters = new object[][]
            {
                new object[] { "Ver de terre",  5,  0,   2,  0 },
                new object[] { "Rat",          15,  5,   8,  2 },
                new object[] { "Gobelin",      45, 10,  16,  5 },
                new object[] { "Squelette",    80, 15,  32, 12 },
                new object[] { "Démon",       120, 30,  64, 28 },
                new object[] { "Dragon",      250, 50, 128, 55 }
            };

            // La liste des choix que le joueur peut faire en combat
            string[][] combatChoices = new string[][]
            {
                new string[] { "i", "Informations sur le personnage"},
                new string[] { "a", "Attaquer" },
                new string[] { "p", "Boire une Potion"},
                new string[] { "f", "Fuir" },
                new string[] { "escape", "Quitter" }
            };

            //Variables du héros
            int playerChoice;
            string heroName = heroText[(int)Data.HeroText.Name];
            int heroLevel = hero[(int)Data.Hero.Level];
            int heroHp = hero[(int)Data.Hero.Hp];
            int heroMaxHp = hero[(int)Data.Hero.MaxHp];
            int heroPower = hero[(int)Data.Hero.Power];
            int heroGold = hero[(int)Data.Hero.Gold];
            int heroPotions = hero[(int)Data.Hero.Potions];
            int heroXp = hero[(int)Data.Hero.Xp];
            int nextLevelThreshold = hero[(int)Data.Hero.LevelThreshold];


            bool inBattle = true;
            bool quit = false;
            bool deadHero = false;

            int battlesCount = 0;

            Console.WriteLine($"{heroName} entre dans le Donjon.");
            do
            {
                Console.WriteLine($"Un nouveau combat se prépare... ({battlesCount + 1}/{data.MAX_CONSECUTIVE_BATTLES})");
                bool fleeing = false;

                // Créer un monstre
                int monsterIndex = spawnRandomMonster(monsters);
                object[] monster = monsters[monsterIndex];
                string monsterName = (string)monster[(int)Data.Monster.Name];
                int monsterHp = (int)monster[(int)Data.Monster.Hp];
                int monsterPower = (int)monster[(int)Data.Monster.Power];
                int monsterXpLoot = (int)monster[(int)Data.Monster.XpLoot];
                int monsterGoldLoot = (int)monster[(int)Data.Monster.GoldLoot];
                bool deadMonster = false;

                // Afficher les actions disponibles pour le joueur
                do
                {
                    playerChoice = askForChoice(combatChoices, $"====================================\n" +
                        $"Face à ce dangereux {monsterName}, {heroName} doit déterminer sa prochaine action");
                    // Résoudre l'action du joueur
                    switch (playerChoice)
                    {
                        case (int)Data.Combat.SeeStats:
                            hero = new[] { heroLevel, heroHp, heroMaxHp, heroPower, heroGold, heroPotions, heroXp, nextLevelThreshold };
                            string statisticsSheet = getHeroSheet(heroText, hero, data);
                            Console.WriteLine(statisticsSheet);
                            break;

                        case (int)Data.Combat.Attack:
                            monsterHp = resolveHeroAttack(heroName, monsterName, heroPower, monsterHp);

                            if (monsterHp == 0)
                            {
                                deadMonster = true;
                                int[] monsterLoot = new[] { monsterXpLoot, monsterGoldLoot };
                                int[] heroEssentials = new[] { heroGold, heroXp, nextLevelThreshold };

                                int[] gains = resolveHeroGains(heroName, heroEssentials, monsterLoot);

                                heroXp = gains[0];
                                heroGold = gains[1];
                            }
                            else
                            {
                                heroHp = resolveMonsterAttack(heroName, monsterName, monsterPower, heroHp);
                            }

                            if (heroHp == 0)
                            {
                                deadHero = true;
                            }
                            break;

                        case (int)Data.Combat.DrinkPotion:
                            int[] heroValues = resolveHeroDrinksPotion(heroName, heroHp, heroMaxHp, heroPotions);
                            heroHp = heroValues[0];
                            heroPotions = heroValues[1];
                            break;

                        case (int)Data.Combat.Flee:
                            int heroFlight = resolveHeroFlight(
                                new[] { heroName, monsterName },
                                new[] { heroXp, nextLevelThreshold },
                                new[] { monsterHp, (int)monster[(int)Data.Monster.Hp] }, data);

                            if (heroFlight == -1)
                            {
                                heroHp = resolveMonsterAttack(heroName, monsterName, monsterPower, heroHp);

                                if (heroHp == 0)
                                {
                                    deadHero = true;
                                }
                                break;
                            }
                            else
                            {
                                heroXp = heroFlight;
                                fleeing = true;
                            }
                            break;

                        case (int)Data.Combat.Quit:
                            quit = (bool)askForConfirmation("Êtes-vous sûr(e) de vouloir quitter ?");
                            break;
                    }

                } while (!quit && !deadHero && !deadMonster && !fleeing);

                if (deadHero) { return new int[] { -1 }; } // Game Over
                if (quit) { return new int[] { -2 }; } // Le joueur demande à quitter le jeu

                battlesCount++;

                if (battlesCount >= data.MAX_CONSECUTIVE_BATTLES || quit)
                {
                    Console.WriteLine($"Tous les occupant de cette partie du donjon ont été tués ou évités.\n{heroName} sort du donjon.");
                    inBattle = false;
                }
            } while (inBattle);
            hero[(int)Data.Hero.Level] = heroLevel;
            hero[(int)Data.Hero.Hp] = heroHp;
            hero[(int)Data.Hero.MaxHp] = heroMaxHp;
            hero[(int)Data.Hero.Power] = heroPower;
            hero[(int)Data.Hero.Gold] = heroGold;
            hero[(int)Data.Hero.Potions] = heroPotions;
            hero[(int)Data.Hero.Xp] = heroXp;
            hero[(int)Data.Hero.LevelThreshold] = nextLevelThreshold;

            return hero;

        }

        public static int[] manageRestPhase(string[] heroText, int[] hero, int[] levelThresholds, Data data)
        {
            // La liste des choix que le joueur peut faire au repos
            string[][] restChoices = new string[][]
            {
                new string[] { "i", "Informations sur le personnage"},
                new string[] { "d", "Dormir" },
                new string[] { "p", "Acheter des Potions"},
                new string[] { "escape", "Quitter" }
            };

            bool done = false, quit = false;
            bool gameWon = false;

            //Variables du héros
            int playerChoice;
            string heroName = heroText[(int)Data.HeroText.Name];
            int heroLevel = hero[(int)Data.Hero.Level];
            int heroHp = hero[(int)Data.Hero.Hp];
            int heroMaxHp = hero[(int)Data.Hero.MaxHp];
            int heroPower = hero[(int)Data.Hero.Power];
            int heroGold = hero[(int)Data.Hero.Gold];
            int heroPotions = hero[(int)Data.Hero.Potions];
            int heroXp = hero[(int)Data.Hero.Xp];
            int nextLevelThreshold = hero[(int)Data.Hero.LevelThreshold];

            Console.WriteLine($"{heroName} entre en ville.");
            do
            {
                playerChoice = askForChoice(restChoices, $"====================================\n" +
                            $"Enfin au calme ! {heroName} peut effectuer une de ces actions");
                // Résoudre l'action du joueur
                switch (playerChoice)
                {
                    case (int)Data.Rest.SeeStats:
                        Console.WriteLine(getHeroSheet(heroText, hero, data));
                        break;

                    case (int)Data.Rest.Sleep:
                        int[] heroEssentials = new int[] { heroLevel, heroHp, heroMaxHp, heroPower, heroXp, nextLevelThreshold };
                        heroEssentials = resolveHeroLevelingUp(heroName, heroEssentials, levelThresholds, data);
                        heroLevel = heroEssentials[0];
                        heroHp = heroEssentials[1];
                        heroMaxHp = heroEssentials[2];
                        heroPower = heroEssentials[3];
                        heroXp = heroEssentials[4];
                        nextLevelThreshold = heroEssentials[5];
                        heroHp = resolveHeroSleeping(heroName, heroHp, heroMaxHp, data);
                        if (heroLevel == data.HERO_MAX_LEVEL)
                        {
                            gameWon = true;
                        }
                        done = true;
                        break;

                    case (int)Data.Rest.BuyPotion:
                        int potionsCount;
                        int price;
                        bool tooExpensive;
                        do
                        {
                            Console.WriteLine($"{heroName} possède {heroGold} pièces d'or.");
                            potionsCount = askForItemCount($"Combien de potions ? ({data.POTION_PRICE} pièces d'or par article)", 10);
                            price = data.POTION_PRICE * potionsCount;
                            tooExpensive = heroGold < price;

                            if (tooExpensive)
                            {
                                Console.WriteLine($"{heroName} possède {heroGold} pièces d'or et" +
                                    $"ne peut pas se permettre cet achat de {price} pièces d'or.");
                            }
                        } while (tooExpensive);

                        if (potionsCount == 0)
                        {
                            break;
                        }

                        heroGold -= price;
                        heroPotions += potionsCount;
                        Console.WriteLine($"{heroName} a acheté {potionsCount} potions pour la modique somme de {price} pièces d'or.");
                        done = true;
                        break;
                    case (int)Data.Rest.Quit:
                        quit = (bool)askForConfirmation("Êtes-vous sûr(e) de vouloir quitter ?");
                        break;
                }
            } while (!quit && !done);

            if (quit)
            {
                return new int[] { -2 };
            }

            if (gameWon)
            {
                return new int[] { -3 };
            }

            Console.WriteLine($"{heroName} a bien profité de ce répit mais il est temps de reprendre le combat.");

            hero[(int)Data.Hero.Level] = heroLevel;
            hero[(int)Data.Hero.Hp] = heroHp;
            hero[(int)Data.Hero.MaxHp] = heroMaxHp;
            hero[(int)Data.Hero.Power] = heroPower;
            hero[(int)Data.Hero.Gold] = heroGold;
            hero[(int)Data.Hero.Potions] = heroPotions;
            hero[(int)Data.Hero.Xp] = heroXp;
            hero[(int)Data.Hero.LevelThreshold] = nextLevelThreshold;

            return hero;
        }

        /* Retourne la feuille de personnage à afficher :
            - heroText -> liste des informations textuelles sur le héros
            - hero -> liste des charactéristiques du héros
           Valeur de retour :
               - feuille de personnage sous forme de message */
        public static string getHeroSheet(string[] heroText, int[] hero, Data data)
        {
            string levelOutput;

            string heroName = heroText[(int)Data.HeroText.Name];
            string heroTitle = heroText[(int)Data.HeroText.Title];
            string heroGenre = heroText[(int)Data.HeroText.Genre];

            int heroLevel = hero[(int)Data.Hero.Level];
            int heroHp = hero[(int)Data.Hero.Hp];
            int heroMaxHp = hero[(int)Data.Hero.MaxHp];
            int heroPower = hero[(int)Data.Hero.Power];
            int heroGold = hero[(int)Data.Hero.Gold];
            int heroPotions = hero[(int)Data.Hero.Potions];
            int heroXp = hero[(int)Data.Hero.Xp];
            int nextLevelThreshold = hero[(int)Data.Hero.LevelThreshold];

            int nextLevel;

            if (heroLevel == data.HERO_MAX_LEVEL)
            {
                levelOutput = $"\n    *°*A atteint le niveau maximum*°*";
            }
            else
            {
                nextLevel = heroLevel + 1;
                levelOutput = $"\n    Expérience accumulée pour le niveau suivant ({nextLevel}) : {heroXp}/{nextLevelThreshold}";
            }

            return $"{heroName} regarde sa feuille de personnage :" +
                $"\n____{heroName.ToUpper()}____" +
                $"\n    Un(e) {heroTitle} de niveau {heroLevel}." +
                levelOutput +
                $"\n    Puissance d'attaque : {heroPower}" +
                $"\n    Points de vie : {heroHp}/{heroMaxHp}" +
                $"\n    Inventaire : {heroPotions} potions." +
                $"\n    Bourse : {heroGold} pièces d'Or.";
        }

        /* Retourne la quantité de points de vie perdue par le défenseur selon la puissance de l'attaquant.
            - attackerPower -> La puissance de l'attaquant
            - defenderHp -> Les points de vie restant du défenseur
           Valeur de retour :
            - Points de vie perdus par le défenseur (0 si le défenseur n'a plus de points de vie) */
        public static int getHpLost(int attackerPower, int defenderHp)
        {
            int hpLoss = attackerPower;

            if (defenderHp - hpLoss < 0)
            {
                hpLoss = attackerPower - Math.Abs(defenderHp - attackerPower);
            }

            return hpLoss;
        }

        public static int getHpHealed(int hp, int maxHp, Data data)
        {
            int healAmount = (int)Math.Floor(maxHp * data.SLEEP_HEALING_RATIO);
            hp += healAmount;
            if (hp > maxHp)
            {
                int rest = hp - maxHp;
                healAmount -= rest;
            }
            return healAmount;
        }

        public static int getXpLost(int xp, int nextLevelThreshold, Data data)
        {
            int maxXpLost = nextLevelThreshold / data.XP_LOSS_STRENGTH;
            int xpLost = xp - maxXpLost;

            if (xpLost < 0)
            {
                xpLost = maxXpLost - Math.Abs(xpLost);
            }
            return xpLost;
        }

        public static int resolveAttack(string[] fightersNames, int[] attackValues, string attackMean = "attaque", string deathCause = "à cause de ses blessures trop importantes")
        {
            string output;

            string attackerName = fightersNames[0]; string victimName = fightersNames[1];
            int attackerPower = attackValues[0]; int victimHp = attackValues[1];

            int hpLost = getHpLost(attackerPower, victimHp);
            victimHp -= hpLost;

            output = $"{attackerName} {attackMean}.\n{victimName} perds {hpLost} points de vie.";

            if (victimHp <= 0)
            {
                victimHp = 0;

                output += $"\n{victimName} meurt {deathCause}.";
            }
            else
            {
                output += $" Il lui en reste {victimHp}.";
                output += $"\n{victimName} est toujours en vie !";
            }
            Console.WriteLine(output);

            return victimHp;
        }

        /* Enlève des points de vie au monstre selon la puissance du héros :
            - heroName -> le Nom du héros
            - monsterName -> le nom du monstre
            - heroPower -> la puissance du héros
            - monsterHp -> les points de vie du monstre
           Valeur de retour :
            - monsterHp -> Les points de vie restant du monstre (0 s'il est mort)*/
        public static int resolveHeroAttack(string heroName, string monsterName, int heroPower, int monsterHp)
        {
            string[] names = new string[] { heroName, $"le {monsterName}" };
            int[] attackValues = new int[] { heroPower, monsterHp };
            string attack = $"décide d'attaquer le {monsterName} en le frappant de son épée";

            monsterHp = resolveAttack(names, attackValues, attack);

            return monsterHp;
        }

        public static int resolveMonsterAttack(string heroName, string monsterName, int monsterPower, int heroHp)
        {
            string[] names = new string[] { $"le {monsterName}", heroName };
            int[] attackValues = new int[] { monsterPower, heroHp };

            heroHp = resolveAttack(names, attackValues, "riposte vicieusement");

            return heroHp;
        }

        /* Donne au héros le butin du monstre qu'il est en droit de recevoir :
            - heroName -> Le nom du héros
            - heroEssentials -> les statistiques du héros (son or, son xp, et le prérequis pour accéder au niveau suivant)
            - gains -> l'or et l'expérience donnée par le monstre)
           Valeur de retour :
            - heroGains -> l'or et l'expérience que le héros va recevoir */
        public static int[] resolveHeroGains(string heroName, int[] heroEssentials, int[] gains)
        {
            int heroGold = heroEssentials[0];
            int heroXp = heroEssentials[1];
            int nextLevelThreshold = heroEssentials[2];

            int goldGain = gains[0];
            int xpGain = gains[1];

            if (heroXp + xpGain > nextLevelThreshold)
            {
                xpGain = nextLevelThreshold - heroXp;
            }

            heroGold += goldGain;
            heroXp += xpGain;

            Console.WriteLine($"{heroName} a gagné {goldGain} pièces d'or et {xpGain} points d'expérience.");

            if (heroXp >= nextLevelThreshold)
            {
                Console.WriteLine($"{heroName} a atteint la limite d'expérience autorisée ({nextLevelThreshold} exp.). Dormez une fois en ville pour gagner un niveau.");
            }

            return new int[] { heroXp, heroGold };
        }

        public static int resolveHeroSleeping(string heroName, int heroHp, int heroMaxHp, Data data)
        {
            if (heroHp < heroMaxHp)
            {
                int healAmount = getHpHealed(heroHp, heroMaxHp, data);
                Console.WriteLine($"{heroName} récupère {healAmount} points de vie pour avoir dormi dans un lit confortable.");
            }
            else
            {
                Console.WriteLine($"{heroName} a dormi dans un lit confortable.");
            }

            return heroHp;
        }

        public static int[] resolveHeroLevelingUp(string heroName, int[] heroEssentials, int[] levelThresholds, Data data)
        {
            int heroLevel = heroEssentials[0];
            int heroHp = heroEssentials[1];
            int heroMaxHp = heroEssentials[2];
            int heroPower = heroEssentials[3];
            int heroXp = heroEssentials[4];
            int levelThreshold = heroEssentials[5];

            if (heroXp >= levelThreshold && heroLevel < data.HERO_MAX_LEVEL)
            {
                heroLevel++;
                heroXp = 0;
                heroHp = heroMaxHp += data.HERO_MAX_HP_INCREASE;
                heroPower += data.HERO_POWER_INCREASE;
                levelThreshold = levelThresholds[heroLevel - 1];
                Console.WriteLine($"Avec {heroXp} points d'expérience accumulés, {heroName} passe au niveau {heroLevel}." +
                    $"\nLes points de vie de {heroName} passent à {heroMaxHp}(+{data.HERO_MAX_HP_INCREASE}) et " +
                    $"sa puissance monte à {heroPower}(+{data.HERO_POWER_INCREASE}).");
            }

            return new int[] { heroLevel, heroHp, heroMaxHp, heroPower, heroXp, levelThreshold };
        }

        public static int resolveHeroFlight(string[] names, int[] heroEssentials, int[] monsterEssentials, Data data)
        {
            string heroName = names[0];
            string monsterName = names[1];
            int monsterHp = monsterEssentials[0];
            int monsterMaxHp = monsterEssentials[1];
            int heroXp = heroEssentials[0];
            int nextLevelThreshold = heroEssentials[1];
            double flightChances;
            double flightAttempt;

            Random randomizer = new Random();

            if (monsterHp >= monsterMaxHp * 0.75)
            {
                flightChances = 0.5;
            }
            else if (monsterHp >= monsterMaxHp * 0.5)
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
                int xpLost = getXpLost(heroXp, nextLevelThreshold, data);
                heroXp -= xpLost;

                if (xpLost > 0)
                {
                    partialText = $" et perds {xpLost} points d'expérience";
                }

                Console.WriteLine($"{heroName} fuit le combat" + partialText + ".");
                return heroXp;
            }

            Console.WriteLine($"{heroName} tente de fuir le combat mais le {monsterName} l'en empêche.");
            return -1;


        }

        public static int[] resolveHeroDrinksPotion(string heroName, int heroHp, int heroMaxHp, int heroPotions)
        {
            if (heroHp == heroMaxHp)
            {
                Console.WriteLine($"{heroName} n'est pas blessé(e), boire une potion serait du pur gâchis !");
            }
            else
            {
                if (heroPotions > 0)
                {
                    heroPotions--;
                    heroHp = heroMaxHp;
                    Console.WriteLine($"{heroName} boit une potion de soin. Ses blessures sont entièrement guéries !\nIl lui reste {heroPotions} potions.");
                }
                else
                {
                    Console.WriteLine($"{heroName} voudrait boire une potion de soin mais n'en a pas de disponible dans son inventaire. Il va falloir penser à se refaire un stock !");
                }
            }

            return new int[] { heroHp, heroPotions };
        }
    }
}

