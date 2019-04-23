using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    class Game
    {
        private int[,,] map;
        Player player;
        List<Block> blocks;
        private bool clicked = false;
        public Game()
        {
            Init();
            new Thread(Update).Start();
        }

        private void Init()
        {
            player = new Player(2, 2, 2, Player.Direction.NORTH);
            map = new int[10,10,10];
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                map[random.Next() % map.GetLength(0), random.Next() % map.GetLength(0), 2] = 1;
            }
            blocks = new List<Block>();
            UpdateBlocks();
        }

        private void UpdateBlocks()
        {
            lock (blocks)
            {
                blocks.Clear();
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        for (int z = 0; z < map.GetLength(2); z++)
                        {
                            if (map[y, x, z] == 1)
                            {
                                Block block = new Block(x - player.x, y - player.y, z - player.z);
                                block.Update(player.direction);
                                blocks.Add(block);
                            }
                        }
                    }
                }
                blocks = blocks.OrderByDescending(b => b.distance).ToList();
            }
        }
        
        private void Update()
        {
            while (true)
            { 
                Thread.Sleep(100);
            }
        }

        public void Draw(Graphics g)
        {
            lock (blocks)
            {
                foreach (Block b in blocks)
                {
                    if (b.InFrame(player.direction))
                    {
                        b.Draw(g);
                    }
                }
            }
            SolidBrush color;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    color = new SolidBrush(Color.Black);
                    if (player.x == x && player.y == y)
                    {
                        color = new SolidBrush(Color.FromArgb(100, 255, 0, 0));
                    }
                    else if (map[y, x, 2] == 1)
                    {
                        color = new SolidBrush(Color.FromArgb(100, 0, 255, 0));
                    }
                    else
                    {
                        color = new SolidBrush(Color.FromArgb(100, 100, 100, 100));
                    }
                    int minimapTileSize = 10;
                    g.FillRectangle(color, new Rectangle(x * minimapTileSize, y * minimapTileSize, minimapTileSize, minimapTileSize));
                }
            }
            color = new SolidBrush(Color.Black);
            g.DrawString(player.direction.ToString()[0].ToString(), new Font("Arial", 24, FontStyle.Bold), color,
                new Point(Window.width / 2 - 10, 50));
        }

        public void KeyEvent(bool down, KeyEventArgs e)
        {
            if (!down)
            {
                clicked = false;
                return;
            }
                
            if (clicked)
                return;
            clicked = true;
            if (e.KeyCode.Equals(Keys.W))
            {
                player.Move(true, map);
            }
            else if (e.KeyCode.Equals(Keys.S))
            {
                player.Move(false, map);
            }
            else if (e.KeyCode.Equals(Keys.A))
            {
                player.Rotate(true);
            }
            else if (e.KeyCode.Equals(Keys.D))
            {
                player.Rotate(false);
            }
            Console.WriteLine("x: " + player.x + " y: " + player.y + " dir: " + player.direction.ToString());
            UpdateBlocks();
        }

        public void MouseEvent(MouseEventArgs e)
        {
            
        }
    }
}
