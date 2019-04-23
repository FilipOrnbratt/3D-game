using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game
{
    class Block
    {
        public struct Angle
        {
            int primaryPercentage;
            bool right;

        }
        public int x, y, z;
        Tuple<int, int> size;
        Tuple<int, int> position;
        public int distance = 0;
        public Block(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Draw(Graphics g)
        {
            SolidBrush greenColor = new SolidBrush(Color.Green);
            Pen blackColor = new Pen(Color.Black);
            g.FillRectangle(greenColor, new Rectangle(position.Item1, position.Item2, size.Item1, size.Item2));
            g.DrawRectangle(blackColor, new Rectangle(position.Item1, position.Item2, size.Item1, size.Item2));
        }

        public void Update(Player.Direction direction)
        {
            distance = 0;
            int xpos = 0, ypos = 0;
            switch (direction)
            {
                case Player.Direction.NORTH:
                    distance = Math.Abs(y);
                    xpos = x;
                    ypos = z;
                    break;
                case Player.Direction.SOUTH:
                    distance = Math.Abs(y);
                    xpos = -x;
                    ypos = z;
                    break;
                case Player.Direction.WEST:
                    distance = Math.Abs(x);
                    xpos = -y;
                    ypos = z;
                    break;
                case Player.Direction.EAST:
                    distance = Math.Abs(x);
                    xpos = y;
                    ypos = z;
                    break;
            }
            size = CalculateSize(distance);
            position = CalculatePosition(xpos, ypos, size);

            Console.WriteLine("x: " + x + " y: " + y + " z: " + z);
            Console.WriteLine("x: " + position.Item1 + " y: " + position.Item2 + " w: " + size.Item1 + " h: " + size.Item2);
        }

        private Tuple<int, int> CalculateSize(int distance)
        {
            if (distance == 0)
                return new Tuple<int, int>(0, 0);
            return new Tuple<int, int>((int)(Window.width / (1.0 + ((distance - 1.0) / 2.0)) - Window.width / 10), 
                (int)(Window.height / (1.0 + ((distance - 1.0) / 2.0)) - Window.height / 10));
        }

        private Tuple<int, int> CalculatePosition(int xpos, int ypos, Tuple<int, int> size)
        {
            int calcX = (Window.width / 2) - (size.Item1 / 2) + (size.Item1 * xpos);
            int calcY = (Window.height / 2) - (size.Item2 / 2) + (size.Item2 * ypos);

            return new Tuple<int, int>(calcX, calcY);
        }

        public bool InFrame(Player.Direction direction)
        {
            return ((direction == Player.Direction.NORTH && y < 0) ||
                    (direction == Player.Direction.SOUTH && y > 0) ||
                    (direction == Player.Direction.WEST && x < 0) ||
                    (direction == Player.Direction.EAST && x > 0));
        }
    }
}
