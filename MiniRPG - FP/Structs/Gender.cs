namespace MiniRPG.Structs
{
    struct Gender
    {
        public string Index;
        public string Article;
        public string Name;

        public Gender(string index, string article, string name)
        {
            Index = index;
            Article = article;
            Name = name;
        }
    }
}
