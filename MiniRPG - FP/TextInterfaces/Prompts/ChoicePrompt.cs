using System;
using System.Collections.Generic;
using MiniRPG.Structs;

namespace MiniRPG.TextInterfaces.Prompts
{
    class ChoicePrompt : KeyPrompt<int>
    {
        public override string Message
        {
            get { return $"{_Message}:{ChoiceList}"; }
        }

        public override string Warning { get; set; } = "Saisie Incorrecte. Saisissez une touche parmis les choix proposés.";

        protected List<Input> Choices;
        protected string ChoiceList;

        public ChoicePrompt(string message = "Faites un choix", params Input[] choices) : base(message)
        {
            Choices = new List<Input>(choices);
            ChoiceList = GetChoiceList();
        }

        protected string GetChoiceList()
        {
            string output = "\n";

            foreach (var choice in Choices)
            {
                output += $"    {choice.Label} -> {choice.Description}\n";
            }

            return output;
        }

        public void AddChoice(Input choice)
        {
            if (Choices.Contains(choice)) return;
            
            Choices.Add(choice);
            ChoiceList = GetChoiceList();
        }

        public void RemoveChoice(Input choice)
        {
            if (!Choices.Contains(choice)) return;

            Choices.Remove(choice);
            ChoiceList = GetChoiceList();
        }

        protected override bool CheckInput(out int result)
        {
            result = -1;

            foreach (var choice in Choices)
            {
                ConsoleKey choiceKey = choice.Key;
                if (UserInput == choiceKey)
                {
                    result = Choices.IndexOf(choice);
                    return true;
                }
            }
            return false;
        }
    }
}
