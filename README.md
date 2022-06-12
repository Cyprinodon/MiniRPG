# MiniRPG
Mon premier projet C# : Un mini RPG sur la console de commande.

## Détails du projet
Le but du jeu est de survivre et amener son héros au niveau maximum (niveau 10).
Pour ce faire, le joueur doit combattre des monstres apparaissant les uns après les autres dans un donjon afin d'accumuler de l'or et de l'expérience qu'il pourra ensuite utiliser en ville.

### Combats
Au début d'un combat, un monstre est généré aléatoirement à partir d'une liste des monstres disponibles.

Durant un combat, le joueur peut:
1. Consulter sa feuille de personnage
2. Attaquer le monstre
3. Boire une potion (et regagner la totalité de sa vie)
4. Tenter de fuir (et se prendre une attaque du monstre en cas d'échec)

Pour fuir, le héros dispose de plus de chances si le monstre a perdu des points de vie (75% contre 50% initialement).

Le héros peut posséder jusqu'à 10 potions en même temps. Boire une potion diminue son stock de 1 et lui rends l'intégralité de ses Points de vie.

Si le joueur décide d'attaquer, il inflige des dégâts équivalent à sa valeur de puissance et le monstre, s'il est toujours en vie, ripostera de la même manière.

Si les points de vie du héros atteignent 0, le héros meurt et le jeu prends fin.

A la mort du monstre, l'expérience du héros augmente du montant possédé par le monstre. Il gagne également de l'or si celui-ci en possédait.

Si l'expérience du héros atteint le montant requis pour passer au niveau supérieur, le héros ne pourra pas en gagner plus et devra attendre de se reposer en ville pour passer au niveau suivant.

L'expérience requise pour monter de niveau est proportionelle au niveau (le niveau 3 coûte plus cher que le niveau 2).

### Ville

Après avoir survécu à 5 combats d'affilée, le joueur retourne automatiquement en ville.

En ville, le joueur peut:
1. Consulter sa feuille de personnage
2. Acheter des potions
3. Se reposer

Si le joueur décide d'acheter des potions, il indique la quantité souhaité (0 pour annuler; jusqu'à 10). S'il possède suffisamment d'or et d'inventaire disponible, il recevra le nombre voulu.
Si le joueur décide de se reposer, il monte de niveau s'il dispose de suffisamment d'expérience, et/ou récupère 30% de points de vie. Il retourne ensuite automatiquement au donjon pour y recommencer un set de 5 combats.
