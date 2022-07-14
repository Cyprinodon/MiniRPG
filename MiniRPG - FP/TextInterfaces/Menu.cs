using MiniRPG.Structs;
using System.Collections;
using System.Collections.Generic;

namespace MiniRPG.TextInterfaces
{
    class Menu : IEnumerable<Input>
    {
        public List<Input> Entries { get; protected set; }

        public Menu(IEnumerable<Input> entries)
        {
            Entries = new List<Input>(entries);
        }

        public void Add(Input entry)
        {
            if (Entries.Contains(entry)) return;

            Entries.Add(entry);
        }

        public void Remove(Input entry)
        {
            if (!Entries.Contains(entry)) return;

            Entries.Remove(entry);
        }

        public override string ToString()
        {
            string output = "\n";

            foreach (var entry in Entries)
            {
                output += $"    {entry}\n";
            }

            return output;
        }
        public IEnumerator<Input> GetEnumerator()
        {
            return ((IEnumerable<Input>)Entries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Entries).GetEnumerator();
        }
    }
}
