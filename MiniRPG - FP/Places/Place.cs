using MiniRPG.Entities;
using MiniRPG.Structs;
using System;
using System.Collections.Generic;

namespace MiniRPG.Places
{
    abstract class Place
    {
        public string Name { get; set; }
        protected Hero Hero;
        protected List<Input> Actions;

        public Place(string name, Hero hero, IEnumerable<Input> actions)
        {
            Name = name;
            Hero = hero;
            Actions = new(actions);
        }

        public virtual void Enter()
        {
            Console.WriteLine($"{Hero.Name} entre dans le {Name}.");
        }

        protected abstract bool DoTurn();

        public Hero Resolve()
        {
            bool done;

            do
            {
                done = DoTurn();
            } while (!done);

            return Hero;
        }

        public virtual void Exit()
        {
            Console.WriteLine($"{Hero.Name} quitte le {Name}");
        }
    }
}
