namespace Sokoban.Menu
{
    class MenuItem
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public delegate void Click();
        public Click click;

        public MenuItem(string name, Click del)
        {
            click = del;
            Name = name;
            Active = true;
        }
    }
}
