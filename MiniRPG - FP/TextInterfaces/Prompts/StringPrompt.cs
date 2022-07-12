using System;

namespace MiniRPG.TextInterfaces.Prompts
{
    class StringPrompt : LinePrompt<string>
    {
        public override string Message
        {
            get => InputLimit > 0 ? $"{_Message}(max. {InputLimit}):" : Message + ":";
            set => _Message = value;
        }

        public int InputLimit { get; set; } = 0;

        public StringPrompt(string message, int inputLimit) : base(message) 
        {
            InputLimit = inputLimit;
        }

        protected override void ReadInput()
        {
            UserInput = Console.ReadLine();
        }

        protected override bool CheckInput(out string result)
        {
            result = UserInput;

            if (InputLimit <= 0) return true;

            if (UserInput.Length > InputLimit)
            {
                Warning = $"Votre saisie ne doit pas dépasser {InputLimit} caractères. (Celle-ci en fait {UserInput.Length})\n";
                return false;
            }
            return true;
        }
    }
}
