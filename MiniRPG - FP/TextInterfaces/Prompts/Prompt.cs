using System;
using MiniRPG.Utilities;
using Key = System.ConsoleKey;

namespace MiniRPG.TextInterfaces.Prompts
{
    abstract class Prompt<T> : IPrompt<T>
    {
        public virtual string Message
        {
            get => Message + ":";
            set => _Message = value;
        }

        public virtual string Warning { get; set; } = "Saisie incorrecte.";

        protected string _Message = "Saisissez une réponse";

        public Prompt(string message)
        {
            _Message = message;
        }

        protected Prompt(string message, string warning) : this(message)
        {
            Warning = warning;
            _Message = message;
        }

        protected abstract bool CheckInput(out T result);
        protected abstract void ReadInput();

        public T AskForInput()
        {
            string prepend = null;
            bool valid;
            T result;

            do
            {
                Console.WriteLine(prepend + Message);

                ReadInput();

                valid = CheckInput(out result);

                prepend = !valid ? Warning + "\n" : null;

            } while (!valid);

            return result;
        }
    }
}
