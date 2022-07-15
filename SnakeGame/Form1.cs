using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();//Creating a list array for snake;
        private Circle food = new Circle();
        int highScore;
        public Form1()
        {
            InitializeComponent();
            new Settings();//linking the setting class to this form
            gameTimer.Interval = 1000 / Settings.Speed;//changing game time to setting speed
            gameTimer.Tick += updateScreen; //linking a updateScreen function to the time
            gameTimer.Start();//starting the timer
            startGame();
        }
        private void updateScreen(object sender,EventArgs e)
        {
            
            if(Settings.GameOver==true)
            {
                //if game over is true and player press enter we run the start game function
                if(Input.KeyPress(Keys.Enter))
                {
                    startGame();
                }
            }
            else
            {
                //if game not over than the following commands will be executed
                if(Input.KeyPress(Keys.Right) && Settings.direction !=Directions.Left)
                {
                    Settings.direction = Directions.Right;
                }
                else if (Input.KeyPress(Keys.Left) && Settings.direction != Directions.Right)
                {
                    Settings.direction = Directions.Left;
                }
                else if (Input.KeyPress(Keys.Up) && Settings.direction != Directions.Down)
                {
                    Settings.direction = Directions.Up;
                }
                else if (Input.KeyPress(Keys.Down) && Settings.direction != Directions.Up)
                {
                    Settings.direction = Directions.Down;
                }
                movePlayer();//run move player function
            }
            pbCanvas.Invalidate(); //refersh the picture box and update the graphics on it
        }
        private void movePlayer()
        {
            //the main loop for snake head and parts
            for(int i=Snake.Count-1;i>=0;i--)
            {
                //if snake head is active
                if(i==0)
                {
                    //move rest of the body according to which way the head is moving
                    switch(Settings.direction)
                    {
                        case Directions.Right:
                            Snake[i].X++;break;

                        case Directions.Left:
                            Snake[i].X--;break;

                        case Directions.Up:
                            Snake[i].Y--; break;

                        case Directions.Down:
                            Snake[i].Y++; break;
                    }
                    //restrict the snake from leaving the canvas;
                    int maxXpos = pbCanvas.Size.Width / Settings.Width;
                    int maxYpos = pbCanvas.Size.Height / Settings.Height;

                    /*if (Snake[i].X < 0 || Snake[i].X > maxXpos || Snake[i].Y < 0 || Snake[i].Y > maxYpos)
                    {
                        //end the game either reaches the edge
                        die();

                    }*/
                    if (Snake[i].X < 0)
                    {
                        Snake[i].X = maxXpos;
                    }
                    if (Snake[i].X > maxXpos)
                    {
                        Snake[i].X = 0;
                    }
                    if (Snake[i].Y < 0)
                    {
                        Snake[i].Y = maxYpos;
                    }
                    if (Snake[i].Y > maxYpos)
                    {
                        Snake[i].Y = 0;
                    }


                    //detect collision with body
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            die();
                        }
                    }

                    //detect collision with food
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        eat();
                    }
                }
                else
                {
                    //if there is not collision then we continue moving the snake 
                    Snake[i].X = Snake[i-1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            //the key down event will trigger the change state from the input class
            Input.changeState(e.KeyCode, true);
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            //the key down event will trigger the change state from the input class
            Input.changeState(e.KeyCode, false);
        }

        private void updateGraphics(object sender, PaintEventArgs e)
        {
            //this is where we will see the snake
            Graphics canvas = e.Graphics;//create new graphics from canvas
            if(Settings.GameOver==false)
            {
                //if the game is not over then we do the following
                Brush snakeColour; //create new brush class;

                //run the loop to check the snake parts
                for(int i=0;i<Snake.Count;i++)
                {
                    if(i==0)
                    {
                        //Snake head
                        snakeColour = Brushes.DarkGreen;
                    }
                    else
                    {
                        //Snake body parts
                        snakeColour = Brushes.Green;
                    }
                    //draw snake body and head
                    canvas.FillEllipse(snakeColour, new Rectangle(
                        Snake[i].X * Settings.Width, Snake[i].Y * Settings.Height,
                        Settings.Width, Settings.Height
                        ));

                    //draw food
                    canvas.FillEllipse(Brushes.Red, new Rectangle(
                        food.X * Settings.Width, food.Y * Settings.Height,
                        Settings.Width, Settings.Height
                        ));
                }
            }
            else
            {
                //this part will run when the game is over
                string gameOver = "Game Over \n" + "Final Score is: " + Settings.Score + "\nPress enter to restart\n";
                label3.Text = gameOver;
                label3.Visible = true;
            }
        }
        private void startGame()
        {
            //this is start game function
            label3.Visible = false;
            new Settings();
            gameTimer.Interval = 1000 / Settings.Speed;
            Snake.Clear();
            Circle head = new Circle { X = 10, Y = 5 };//cretae new head for snake
            Snake.Add(head);//add the head to the snake array
            label2.Text = Settings.Score.ToString();
            generateFood();
        }
        private void generateFood()
        {
            int maxXpos = pbCanvas.Size.Width / Settings.Width;
            int maxYpos = pbCanvas.Size.Height / Settings.Height;
            Random rnd = new Random();
            food = new Circle { X = rnd.Next(0, maxXpos), Y = rnd.Next(0, maxYpos) };
            //create a new food with random X and Y
        }
        private void eat()
        {
            //add a part to body
            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(body);
            Settings.Score += Settings.Points;
            Settings.Speed += 1;
            gameTimer.Interval = 1000 / Settings.Speed;
            label2.Text = Settings.Score.ToString();
            generateFood();
        }
        private void die()
        {
            Settings.GameOver = true;
            if (Settings.Score > highScore)
            {
                highScore = Settings.Score;

                label5.Text = highScore.ToString();
                label5.ForeColor = Color.Maroon;
                //label5.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
