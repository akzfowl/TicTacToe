using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
namespace TicTacToe
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        int circleWin, crossWin;
        Vector2 circleWinPos, crossWinPos;
        bool end;
        string results;
        Vector2 resultsPos;
        Texture2D circleTex, crossTex, emptyTex;
        Cell[,] cellMat = new Cell[3, 3];
        int playerIndex = 1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 480;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
        }

        protected override void Initialize()
        {
            base.Initialize();
            TouchPanel.EnabledGestures = GestureType.Tap;
            crossWin = circleWin = 0;
            end = false;
            results = "";
            resultsPos = new Vector2(240, 52);
            circleWinPos = new Vector2(64, 748);
            crossWinPos = new Vector2(192, 748);

            //screen offsets for placing cells
            int iX = 24;
            int iY = 104;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    cellMat[i, j] = new Cell(new Vector2(iX, iY), Content);
                    iY += 124 + 104;
                }
                iY = 104;
                iX += 128 + 24;
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>(@"default");
            emptyTex = Content.Load<Texture2D>(@"empty");
            crossTex = Content.Load<Texture2D>(@"cross");
            circleTex = Content.Load<Texture2D>(@"circle");
        }

        protected override void UnloadContent()
        {
        }

        public bool checkForEntryWin(Entry entry) //did the passed entry (X or O) win?
        {
            bool finish = false;
            for (int i = 0; i < 3; i++)
            {
                if (finish)
                    break;
                for (int j = 0; j < 3; j++)
                {
                    if (cellMat[i, j].entry == entry)
                    {
                        //check for vertical wins
                        switch (j)
                        {
                            case 0:
                                if (cellMat[i, j + 1].entry == entry && cellMat[i, j + 2].entry == entry)
                                    finish = true;
                                break;
                            case 1:
                                if (cellMat[i, j + 1].entry == entry && cellMat[i, j - 1].entry == entry)
                                    finish = true;
                                break;
                            case 2:
                                if (cellMat[i, j - 1].entry == entry && cellMat[i, j - 2].entry == entry)
                                    finish = true;
                                break;
                        }
                        //check for horizontal wins
                        switch (i)
                        {
                            case 0:
                                if (cellMat[i + 1, j].entry == entry && cellMat[i + 2, j].entry == entry)
                                    finish = true;
                                break;
                            case 1:
                                if (cellMat[i + 1, j].entry == entry && cellMat[i - 1, j].entry == entry)
                                    finish = true;
                                break;
                            case 2:
                                if (cellMat[i - 1, j].entry == entry && cellMat[i - 2, j].entry == entry)
                                    finish = true;
                                break;
                        }
                        //check for diagonal wins
                        if (i == 0)
                        {
                            switch (j)
                            {
                                case 0:
                                    if (cellMat[i + 1, j + 1].entry == entry && cellMat[i + 2, j + 2].entry == entry)
                                        finish = true;
                                break;
                                case 2:
                                    if (cellMat[i + 1, j - 1].entry == entry && cellMat[i + 2, j - 2].entry == entry)
                                        finish = true;
                                break;

                            }   
                        }
                        
                        if (finish)
                            break;
                    }
                }
            }
            return finish;
        }

        public void checkForEnd()
        {
            if (checkForEntryWin(Entry.Circle))
            {
                results = "Circle Wins!";
                circleWin++;
                end = true;
            }
            else if (checkForEntryWin(Entry.Cross))
            {
                results = "Cross wins!";
                end = true;
                crossWin++;
            }
            else
            {
                int countSelectedEntries = 0;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (cellMat[i, j].set)
                            countSelectedEntries++;
                    }
                }
                if (countSelectedEntries == 9)
                {
                    results = "Tie!";
                    end = true;
                }
            }
        }

        public void setCell(int i, int j)
        {
            if (playerIndex == 1)
            {
                cellMat[i, j].buttonTex = crossTex;
                cellMat[i, j].entry = Entry.Cross;
            }
            else
            {
                cellMat[i, j].buttonTex = circleTex;
                cellMat[i, j].entry = Entry.Circle;
            }

            playerIndex = 1 - playerIndex; //change active player
            cellMat[i, j].set = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (!end)
            {
                if (TouchPanel.IsGestureAvailable)
                {
                    GestureSample gesture = TouchPanel.ReadGesture();
                    if (gesture.GestureType == GestureType.Tap)
                    {
                        //Set cells to X or O if they're tapped
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                if (gesture.Position.X >= cellMat[i, j].position.X && gesture.Position.X <= cellMat[i, j].position.X + 128 &&
                            gesture.Position.Y >= cellMat[i, j].position.Y && gesture.Position.Y <= cellMat[i, j].position.Y + 128 && !cellMat[i, j].set)
                                {
                                    setCell(i, j);
                                    checkForEnd();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (TouchPanel.IsGestureAvailable)
                {
                    GestureSample gesture = TouchPanel.ReadGesture();
                    if (gesture.GestureType == GestureType.Tap)
                    {
                        //reset everything
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                cellMat[i, j].buttonTex = emptyTex;
                                cellMat[i, j].entry = Entry.Empty;
                                cellMat[i, j].set = false;
                            }
                        }
                        results = "";
                        end = false;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    cellMat[i, j].Draw(gameTime, spriteBatch);
                }
            }
            spriteBatch.DrawString(font, "X:" + crossWin, crossWinPos, Color.White);
            spriteBatch.DrawString(font, "O:" + circleWin, circleWinPos, Color.White);
            spriteBatch.DrawString(font, results, resultsPos, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}