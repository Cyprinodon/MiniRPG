using System;
using MiniRPG.Utilities;

namespace MiniRPG.TextInterfaces.Prompts
{
    abstract class LinePrompt<T> : Prompt<T> where T : IConvertible
    {
        protected T UserInput;

        public LinePrompt(string message) : base(message) { }

        protected LinePrompt(string message, string warning) : base(message, warning) { }

        protected override void ReadInput()
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            UserInput = (T)Convert.ChangeType(input, typeof(T));
        }
    }
}
