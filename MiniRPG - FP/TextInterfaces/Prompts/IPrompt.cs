using System;

namespace MiniRPG.TextInterfaces.Prompts
{
    interface IPrompt<T>
    {
        string Message { get; }
        string Warning { get; }

        T AskForInput();
    }
}
