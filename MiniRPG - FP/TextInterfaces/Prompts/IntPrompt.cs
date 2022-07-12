using System;

namespace MiniRPG.TextInterfaces.Prompts
{
    class IntPrompt : LinePrompt<int>
    {
        public int Min = 0;
        public int Max = 0;
        public override string Message {
            get { return Min == 0 && Max == 0 ? _Message + ":" : $"{_Message}(min. {Min} max. {Max}) :"; }
        }

        public IntPrompt(string message, int min, int max) : base(message)
        {
            Min = min;
            Max = max;
            Warning = base.Warning + $"Saisissez un nombre compris entre {Min} et {Max} inclus.";
        }

        public IntPrompt(string message, string warning, int min, int max) : this(message, min, max)
        {
            Warning = warning;
        }

        protected override bool CheckInput(out int result)
        {
            result = UserInput;

            if (result < Min || result > Max)
            {
                return false;    
            }
            return true;
        }
    }
}
