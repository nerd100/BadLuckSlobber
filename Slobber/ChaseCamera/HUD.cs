using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;



namespace ChaseCameraSample
{
    public class HUD
    {
        public int playerScore, screenWidth, screenHeight;
        public SpriteFont playerScoreFont, playerGoalFont, playerTimeFont;
        public Vector2 playerScorePos, playerTimePos, playerGoalPos;
        public bool showHud;
        public int i = 0;
        public float j = 100;
        //Constructor
        public HUD()
        {
            playerScore = 0;
            showHud = true;
            screenHeight = 0;
            screenWidth = 100;
            playerScoreFont = null;
            playerTimeFont = null;
            playerScorePos = new Vector2(50, 50);
            playerTimePos = new Vector2(500, 50);
            playerGoalPos = new Vector2(350, 50);

        }

        //Load Content
        public void LoadContent(ContentManager Content)
        {
            playerScoreFont = Content.Load<SpriteFont>("georgia");
            playerTimeFont = Content.Load<SpriteFont>("georgia");
            playerGoalFont = Content.Load<SpriteFont>("georgia");
        }

        //Update
        public void Update(GameTime gameTime)
        {

            //Get keyboard state
            KeyboardState keyState = Keyboard.GetState();

        }


        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            KeyboardState keyState2 = Keyboard.GetState();
            if (keyState2.IsKeyDown(Keys.W))
            {
                i += 1;
            }
            if (keyState2.IsKeyDown(Keys.S))
            {
                i += 1;
            }
            if (keyState2.IsKeyDown(Keys.A))
            {
                i -= 1;
            }
            if (keyState2.IsKeyDown(Keys.D))
            {
                i -= 1;
            }

            j = j - 0.05f;

            //if we are showing our HUD (if showHud == true) then display our HUD
            if (showHud)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(playerScoreFont, "Score = " + i / 10, playerScorePos, Color.Red);
                spriteBatch.DrawString(playerTimeFont, "Time = " + (int)j, playerTimePos, Color.Blue);
                spriteBatch.DrawString(playerGoalFont, "Time", playerGoalPos, Color.Green);
                spriteBatch.End();


            }


        }

    }
}
