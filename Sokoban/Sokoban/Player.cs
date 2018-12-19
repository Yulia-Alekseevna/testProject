using Microsoft.Xna.Framework;

namespace Sokoban
{
    class Player
    {
        public Sprite Sprite { get; set; }
        public Point PositionMap { get; set; }

        public Player(Sprite sprite, Point pos)
        {
            Sprite = sprite;
            PositionMap = pos;
        }
    }
}
