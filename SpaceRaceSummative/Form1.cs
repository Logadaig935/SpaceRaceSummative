using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace SpaceRaceSummative
{
    public partial class Form1 : Form
    {
        SoundPlayer ping = new SoundPlayer(Properties.Resources.Point_Get);
        SoundPlayer boom = new SoundPlayer(Properties.Resources.Funny_Boom);

        Rectangle hero = new Rectangle(150, 300, 10, 20);
        Rectangle hero2 = new Rectangle(450, 300, 10, 20);
        int heroSpeed = 2;

        List<Rectangle> obstaclesL = new List<Rectangle>();
        List<Rectangle> obstaclesR = new List<Rectangle>();
        List<int> obstacleSpeeds = new List<int>();
        List<string> obstacleColours = new List<string>();
        int obstacleSizeW = 5;
        int obstacleSizeH = 5;

        int scoreP1 = 0;
        int scoreP2 = 0;
        int time = 50000;

        bool wDown = false;
        bool sDown = false;
        bool iDown = false;
        bool kDown = false;
        bool spaceDown = false;
        bool escDown = false;

        string gameState = "waiting";

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush slateGrayBrush = new SolidBrush(Color.SlateGray);

        int wallL = 0;
        int wallR = 0;
        int YL;
        int YR;


        public Form1()
        {
            InitializeComponent();
            outcomeLabel.Visible = true;
            subTitleLabel.Visible = true;
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.I:
                    iDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.K:
                    kDown = false;
                    break;
                case Keys.Escape:
                    escDown = false;
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.I:
                    iDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.K:
                    kDown = true;
                    break;
                case Keys.Space:
                    spaceDown = true;
                    if (gameState == "waiting" || gameState == "over")
                    {
                        GameInitialize();
                    }
                    break;
                case Keys.Escape:
                    escDown = true;
                    if (gameState == "waiting" || gameState == "over")
                    {
                        Application.Exit();
                    }
                    break;

            }
        }
        public void GameInitialize()
        {
            outcomeLabel.Visible = false;
            subTitleLabel.Visible = false;

            gameTimer.Enabled = true;
            gameState = "running";
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")
            {
                timeLabel.Text = "";
                p1ScoreLabel.Text = "";
                p2ScoreLabel.Text = "";

                outcomeLabel.Text = "SPACE RACE 1V1";

                subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
            }
            else if (gameState == "running")
            {
                // draw text at top 

                timeLabel.Text = $"Time Left: {time}";
                p1ScoreLabel.Text = $"{scoreP1}";
                p2ScoreLabel.Text = $"{scoreP2}";

                e.Graphics.FillRectangle(whiteBrush, hero);
                e.Graphics.FillRectangle(whiteBrush, hero2);

                //draw balls 
                for (int i = 0; i < obstaclesL.Count(); i++)
                {
                    if (obstacleColours[i] == "slateGray")
                    {
                        e.Graphics.FillRectangle(slateGrayBrush, obstaclesL[i]);
                    }
                }
                for (int i = 0; i < obstaclesR.Count(); i++)
                {
                    if (obstacleColours[i] == "slateGray")
                    {
                        e.Graphics.FillRectangle(slateGrayBrush, obstaclesR[i]);
                    }
                }
            }
            else if (gameState == "over")
            {
                timeLabel.Text = "";
                p1ScoreLabel.Text = "";
                p2ScoreLabel.Text = "";

                subTitleLabel.Text += "\nPress Space Bar to Start or Escape to Exit";
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

            Random randGen = new Random();
            YL = randGen.Next(40, 276);
            YR = randGen.Next(40, 276);

            if (wDown == true && hero.Y > 0 && time < 49900)
            {
                hero.Y -= heroSpeed;
            }

            if (iDown == true && hero.Y < this.Width - hero.Width && time < 49900)
            {
                hero2.Y -= heroSpeed;
            }
            if (sDown == true && hero.Y > 0 && time < 49900)
            {
                hero.Y += heroSpeed;
            }

            if (kDown == true && hero.Y < this.Width - hero.Width && time < 49900)
            {
                hero2.Y += heroSpeed;
            }
            // move balls 
            for (int i = 0; i < obstaclesR.Count(); i++)
            {
                //find the new postion of y based on speed 
                int x2 = obstaclesR[i].X - obstacleSpeeds[i];

                //replace the rectangle in the list with updated one using new y 
                obstaclesR[i] = new Rectangle(x2, obstaclesR[i].Y, obstacleSizeW, obstacleSizeH);
            }

            for (int i = 0; i < obstaclesL.Count(); i++)
            {
                //find the new postion of y based on speed 
                int x = obstaclesL[i].X + obstacleSpeeds[i];

                //replace the rectangle in the list with updated one using new y 
                obstaclesL[i] = new Rectangle(x, obstaclesL[i].Y, obstacleSizeW, obstacleSizeH);
            }

            if (time % 8 == 0)
            {
                obstaclesL.Add(new Rectangle(0, YL, obstacleSizeW, obstacleSizeH));

                obstacleSpeeds.Add(5);

                obstacleColours.Add("slateGray");
            }

            if (time % 8 == 0)
            {
                obstaclesR.Add(new Rectangle(855, YR, obstacleSizeW, obstacleSizeH));

                obstacleSpeeds.Add(7);

                obstacleColours.Add("slateGray");
            }
            //check if ball is below play area and remove it if it is 
            for (int i = 0; i < obstaclesL.Count(); i++)
            {
                if (obstaclesL[i].X > this.Height - obstacleSizeH - wallL)
                {
                    //obstaclesL.RemoveAt(i);
                    //obstacleSpeeds.RemoveAt(i);
                    //obstacleColours.RemoveAt(i);
                }
            }

            for (int i = 0; i < obstaclesR.Count(); i++)
            {
                if (obstaclesR[i].X > this.Height - obstacleSizeH - wallR)
                {
                    //obstaclesR.RemoveAt(i);
                    //obstacleSpeeds.RemoveAt(i);
                    //obstacleColours.RemoveAt(i);
                }
            }

            //check collision of ball and paddle 

            for (int i = 0; i < obstaclesL.Count(); i++)
            {
                if (hero.IntersectsWith(obstaclesL[i]))
                {
                    hero.Y = 300;
                    boom.Play();
                }
            }

            for (int i = 0; i < obstaclesR.Count(); i++)
            {
                if (hero2.IntersectsWith(obstaclesR[i]))
                {
                    hero2.Y = 300;
                    boom.Play();
                }
            }

            for (int i = 0; i < obstaclesL.Count(); i++)
            {
                if (hero2.IntersectsWith(obstaclesL[i]))
                {
                    hero2.Y = 300;
                    boom.Play();
                }
            }

            for (int i = 0; i < obstaclesR.Count(); i++)
            {
                if (hero.IntersectsWith(obstaclesR[i]))
                {
                    hero.Y = 300;
                    boom.Play();
                }
            }
            for (int i = 0; i < obstaclesR.Count(); i++)
            {
                if (hero.Y <= 0)
                {
                    scoreP1++;
                    hero.Y = 300;

                    ping.Play();

                    //obstaclesR.RemoveAt(i);
                    //obstacleSpeeds.RemoveAt(i);
                    //obstacleColours.RemoveAt(i);
                }
            }
            for (int i = 0; i < obstaclesR.Count(); i++)
            {
                if (hero2.Y <= 0)
                {
                    scoreP2++;
                    hero2.Y = 300;

                    ping.Play();

                    //obstaclesR.RemoveAt(i);
                    //obstacleSpeeds.RemoveAt(i);
                    //obstacleColours.RemoveAt(i);
                }
            }
            for (int i = 0; i < obstaclesL.Count(); i++)
            {
                if (hero.Y <= 0)
                {
                    scoreP1++;
                    hero.Y = 300;

                    ping.Play();

                    //obstaclesR.RemoveAt(i);
                    //obstacleSpeeds.RemoveAt(i);
                    //obstacleColours.RemoveAt(i);
                }
            }
            for (int i = 0; i < obstaclesL.Count(); i++)
            {
                if (hero2.Y <= 0)
                {
                    scoreP2++;
                    hero2.Y = 300;

                    ping.Play();

                    //obstaclesR.RemoveAt(i);
                    //obstacleSpeeds.RemoveAt(i);
                    //obstacleColours.RemoveAt(i);
                }
            }

            /// stops the players from going below the starting point
            for (int i = 0; i < obstaclesR.Count(); i++)
            {
                if (hero.Y >= 299)
                {
                    hero.Y = 300;
                }
            }
            for (int i = 0; i < obstaclesR.Count(); i++)
            {
                if (hero2.Y >= 299)
                {
                    hero2.Y = 300;
                }
            }
            time--;
            for (int i = 0; i < obstaclesR.Count(); i++)
            {
                if (scoreP1 == 3)
                {
                    gameTimer.Enabled = false;
                    outcomeLabel.Visible = true;
                    subTitleLabel.Visible = true;

                    outcomeLabel.Text = "Player 1 Wins!";
                    subTitleLabel.Text = "Press Esc to exit.";

                    obstaclesR.RemoveAt(i);
                    obstacleSpeeds.RemoveAt(i);
                    obstacleColours.RemoveAt(i);
                }
            }

            for (int i = 0; i < obstaclesR.Count(); i++)
            {
                if (scoreP2 == 3)
                {
                    gameTimer.Enabled = false;
                    outcomeLabel.Visible = true;
                    outcomeLabel.Text = "Player 2 Wins!";

                    obstaclesR.RemoveAt(i);
                    obstacleSpeeds.RemoveAt(i);
                    obstacleColours.RemoveAt(i);
                }
            }
            for (int i = 0; i < obstaclesR.Count(); i++)
            {
                if (time <= 0)
                {
                    gameTimer.Enabled = false;
                    outcomeLabel.Visible = true;
                    outcomeLabel.Text = "No Winner!";

                    obstaclesR.RemoveAt(i);
                    obstacleSpeeds.RemoveAt(i);
                    obstacleColours.RemoveAt(i);
                }
            }
            Refresh();

            //decrease time counter and check to see if time is up 

            if (time == 0)
            {
                gameTimer.Enabled = false;
                gameState = "over";

            }
        }
    }
}
