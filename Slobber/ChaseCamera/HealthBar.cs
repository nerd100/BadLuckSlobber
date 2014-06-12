using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ChaseCameraSample
{
    class HealthBar
    {
        private Texture2D container, lifeBar;
        private Vector2 position,postion2;
        private int fullHealth;
        private int currentHealth;
        private int rateOfChange = 1;
        public HealthBar(ContentManager content)
        {
            position = new Vector2(50, 10);
            postion2 = new Vector2(52, 12);
            LoadContent(content);
            fullHealth = lifeBar.Width;
            currentHealth = fullHealth;
        }

        private void LoadContent(ContentManager content)
        {
            container = content.Load<Texture2D>("HealtBar");
            lifeBar = content.Load<Texture2D>("Health");
            
        }

        public void Update()
        {
           
            if (currentHealth >= 0)
                currentHealth -= rateOfChange;
          
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
       
            spriteBatch.Draw(container, position, Color.White);
            spriteBatch.Draw(lifeBar, postion2, new Rectangle((int)position.X, (int)position.Y, currentHealth, lifeBar.Height), Color.Red); 
           // spriteBatch.End();
        }
        
    }
}
