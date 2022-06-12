using System;
namespace MiniRPG.Game.Structs
{
    struct Input
    {
        public ConsoleKey Key { get; set; }
        public string Label { get => Key.ToString().ToLower(); }
        public string Description { get; set; }

        public Input(ConsoleKey key, string description)
        {
            Key = key;
            Description = description;
        }

        public override string ToString()
        {
            return Label + " -> " + Description;
        }
    }
}
