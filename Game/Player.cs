using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game
{
    class Player
    {
        public enum Direction { NORTH, WEST, EAST, SOUTH }
        public struct Position
        {
            public int x, y, z;
            public Position(int x, int y, int z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        };

        public int x, y, z;
        public Direction direction = Direction.NORTH;
        private Position lastPosition;

        public Player(int x, int y, int z, Direction direction)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.direction = direction;
        }

        public void Draw(Graphics g)
        {
            
        }

        public void Move(bool forward, int[,,] map)
        {
            lastPosition = new Position(x, y, z);
            switch (direction)
            {
                case Direction.NORTH:
                    y -= forward ? 1 : -1;
                    break;
                case Direction.SOUTH:
                    y += forward ? 1 : -1;
                    break;
                case Direction.WEST:
                    x -= forward ? 1 : -1;
                    break;
                case Direction.EAST:
                    x += forward ? 1 : -1;
                    break;
            }
            ValidatePosition(map);
        }

        private void RevertToPosition(Position position)
        {
            x = position.x;
            y = position.y;
            z = position.z;
        }

        private void ValidatePosition(int[,,] map)
        {
            int maxX = map.GetLength(1);
            int maxY = map.GetLength(0);
            if (y >= maxY || y < 0 || x >= maxX || x < 0 || map[y, x, 2] == 1)
            {
                RevertToPosition(lastPosition);
            }
        }

        public void Rotate(bool left)
        {
            if((direction == Direction.EAST && left) || (direction == Direction.WEST && !left))
            {
                direction = Direction.NORTH;
            }
            else if ((direction == Direction.EAST && !left) || (direction == Direction.WEST && left))
            {
                direction = Direction.SOUTH;
            }
            else if ((direction == Direction.NORTH && left) || (direction == Direction.SOUTH && !left))
            {
                direction = Direction.WEST;
            }
            else if ((direction == Direction.NORTH && !left) || (direction == Direction.SOUTH && left))
            {
                direction = Direction.EAST;
            }
        }
    }
}
