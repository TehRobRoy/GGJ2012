using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Nebula
{
    class Button
    {
        private Game game;
        private Texture2D actualTextureToDisplay;
        private Texture2D defaultTexture;
        private Texture2D hoveringTexture;
        private Texture2D clickedTexture;
        private Vector2 position;
        public delegate void EventHandler(System.Object sender, System.EventArgs e);
        public event EventHandler Clicked;

        public bool clicked = false;

        /// <summary>  
        ///   
        /// </summary>  
        /// <param name="game">The Game object</param>  
        /// <param name="defaultTexture">The path to the default texture for the button</param>  
        /// <param name="hoveringTexture">The path to the texture for the button when the mouse is within its area</param>  
        /// <param name="clickedTexture">The path to the texture for the button when the mouse is within its area and the left mouse button is clicked</param>  
        /// <param name="position">The position of the button in the window, note you must provide the center position of the button</param>  
        /// <param name="delegateClick">The method that will be called when the left mouse button is within the button and that you release the left button</param>  
        public Button(Microsoft.Xna.Framework.Game game,
                        string defaultTexture, string hoveringTexture, string clickedTexture,
                        Microsoft.Xna.Framework.Vector2 position)
        {
            this.game = game;
            
            this.defaultTexture = this.game.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(defaultTexture);
            this.hoveringTexture = this.game.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(hoveringTexture);
            this.clickedTexture = this.game.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(clickedTexture);
            this.actualTextureToDisplay = this.defaultTexture;
            this.position = position;
        }


        public void Update(MouseState mouse, MouseState lastMouse)
        {
            clicked = false;
            // is the mouse within the button?  
            if ((mouse.X >= (this.position.X - this.actualTextureToDisplay.Width / 2)) &&
                (mouse.X <= (this.position.X + this.actualTextureToDisplay.Width / 2)) &&
                (mouse.Y >= (this.position.Y - this.actualTextureToDisplay.Height / 2)) &&
                (mouse.Y <= (this.position.Y + this.actualTextureToDisplay.Height / 2)))
            {
                this.actualTextureToDisplay = this.hoveringTexture;
                if (mouse.LeftButton == ButtonState.Released &&
                    lastMouse.LeftButton == ButtonState.Pressed)
                {
                    if (this.Clicked != null)
                    {
                        this.Clicked(this, new System.EventArgs());
                    }
                }
                else if (mouse.LeftButton == ButtonState.Pressed)
                {
                    
                    this.actualTextureToDisplay = this.clickedTexture;
                    clicked = true;
                }
                
            }
            else
            {
                this.actualTextureToDisplay = this.defaultTexture;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.actualTextureToDisplay, new Microsoft.Xna.Framework.Vector2(this.position.X - this.actualTextureToDisplay.Width / 2, this.position.Y - this.actualTextureToDisplay.Height / 2), Color.White);
        }
    }
}
