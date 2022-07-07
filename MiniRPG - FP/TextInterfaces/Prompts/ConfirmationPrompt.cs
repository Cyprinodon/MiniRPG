using MiniRPG.Structs;
using Key = System.ConsoleKey;

namespace MiniRPG.TextInterfaces.Prompts
{
    class ConfirmationPrompt : KeyPrompt<bool?>
    {
        public override string Message
        {
            get { return $"{_Message}\n {Yes}\n {No}"; }
        }
        public Input Yes;
        public Input No;

        public ConfirmationPrompt(string message = "En êtes-vous sûr(e) ?") : this(message, new Input(Key.O, "Oui"), new Input(Key.N, "Non")) { }

        public ConfirmationPrompt(string message, Input yes, Input no) : base(message)
        {
            Yes = yes;
            No = no;
            Warning = base.Warning + $"Saisissez '{Yes.Label}' ou '{No.Label}'.";
        }

        protected override bool CheckInput(out bool? result)
        {
            result = null;

            if (UserInput == Yes.Key)
            {
                result = true;
                return true;
            }
            else if (UserInput == No.Key)
            {
                result = false;
                return true;
            }

            return false;
        }
    }
}
