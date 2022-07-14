using System;
using MiniRPG.Structs;

namespace MiniRPG.TextInterfaces.Prompts
{
    class ChoicePrompt : KeyPrompt<int>
    {
        public override string Message
        {
            get { return $"{_Message}:{Menu}"; }
        }

        public override string Warning { get; set; } = "Saisie Incorrecte. Saisissez une touche parmis les choix proposés.";

        protected Menu Menu;

        public ChoicePrompt(string message = "Faites un choix", params Input[] choices) : base(message)
        {
            Menu = new Menu(choices);
        }

        public void AddChoice(Input choice)
        {
            Menu.Add(choice);
        }

        public void RemoveChoice(Input choice)
        {
            Menu.Remove(choice);
        }

        protected override bool CheckInput(out int result)
        {
            result = -1;

            foreach (var choice in Menu)
            {
                ConsoleKey choiceKey = choice.Key;
                if (UserInput == choiceKey)
                {
                    result = Menu.Entries.IndexOf(choice);
                    return true;
                }
            }
            return false;
        }
    }
}
