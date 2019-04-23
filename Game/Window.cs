using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Game
{
    class Window : Form
    {
        public static int width = 700, height = 700;
        private int m_FPS = 60; // 0 for unlimited
        private string m_FPSText;
        private int m_offX = 0, offY = 0;
        private Game m_game;

        public static void Main()
        {
            Window window = new Window();
        }

        public Window()
        {
            m_game = new Game();

            this.Size = new Size(width, height);
            this.DoubleBuffered = true;
            
            // Handlers
            this.Paint += PaintGraphics;
            this.MouseClick += MouseClickEvent;
            this.KeyDown += KeyDownEvent;
            this.KeyUp += KeyUpEvent;
            this.FormClosed += FormClosedEvent;

            new Thread(DisplayForm).Start();

            // Wait for form to initialize
            Thread.Sleep(500);

            UpdateDisplay();
        }

        private void DisplayForm()
        {
            this.ShowDialog();
        }

        private void UpdateDisplay()
        {
            // FPS variables
            m_FPSText = "FPS: 0";
            int FPSCount = 0;
            int FPSTime = DateTime.Now.Second;
            int now;


            while (true)
            {
                if (DateTime.Now.Second > FPSTime || (FPSTime == 59 && DateTime.Now.Second == 0))
                {
                    m_FPSText = "FPS: " + FPSCount;
                    FPSCount = 0;
                    FPSTime = DateTime.Now.Second;
                }
                this.Invoke((MethodInvoker)delegate
                {
                    Refresh();
                });
                FPSCount++;
                now = DateTime.Now.Millisecond;
                if (m_FPS != 0)
                {
                    while (!(DateTime.Now.Second > FPSTime || (FPSTime == 59 && DateTime.Now.Second == 0)) &&
                                        (now < (1000 / m_FPS) * (FPSCount + 1) ||
                                        (m_FPS <= FPSCount && now > 900)))
                    {
                        Thread.Sleep(1);
                        now = DateTime.Now.Millisecond;
                    }
                }
            }
        }

        public void PaintGraphics(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            m_game.Draw(g);

            /*SolidBrush color = new SolidBrush(Color.Green);
            if (m_offX != 250 && offY == 0)
                m_offX++;
            else if (m_offX == 250 && offY != 250)
                offY++;
            else if (m_offX != 0 && offY == 250)
                m_offX--;
            else if (m_offX == 0 && offY != 0)
                offY--;



            g.FillRectangle(color, 50 + m_offX, 50 + offY, 50, 50);*/

            //display FPS
            g.DrawString(m_FPSText, new Font("Arial", 10), new SolidBrush(Color.Green), new PointF(Width - 100, 10));
        }

        private void MouseClickEvent(object sender, MouseEventArgs e)
        {
            m_game.MouseEvent(e);
        }

        private void FormClosedEvent(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void KeyDownEvent(object sender, KeyEventArgs e)
        {
            m_game.KeyEvent(true, e);
        }

        private void KeyUpEvent(object sender, KeyEventArgs e)
        {
            m_game.KeyEvent(false, e);
        }
    }
}
