using System;
using MiniRPG.Utilities;
using Key = System.ConsoleKey;

namespace MiniRPG.TextInterfaces.Prompts
{
    abstract class KeyPrompt<T> : Prompt<T>
    {

        protected Key UserInput;

        public KeyPrompt(string message) : base(message) { }

        public KeyPrompt(string message, string warning) : base(message, warning) { }

        protected override void ReadInput()
        {
            UserInput = Console.ReadKey(true).Key;

            string label = Converter.KeyToString(UserInput);

            Console.WriteLine(label);
        }
    }
}
