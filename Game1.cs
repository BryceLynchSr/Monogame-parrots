using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Papegojor
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D parrot;
        List<Vector2> parrotHastighet = new List<Vector2>();
        List<Rectangle> parrotRect = new List<Rectangle>();
        SpriteFont nedräkning;
        int räknare = 9;
        String meddelande = "test";
        Vector2 meddelandePosition = new Vector2(780, 0);
       
        MouseState mus;
        MouseState musNyss;
        Random slump = new Random();
        KeyboardState keyboard = Keyboard.GetState();
        SoundEffect applause;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            for(int x = 0; x < 10; x++)
            {
                parrotRect.Add(new Rectangle(slump.Next(0, 700), slump.Next (0, 380), 50, 50));
                parrotHastighet.Add(new Vector2(slump.Next(-5, 6), slump.Next(-5, 6)));
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            parrot = Content.Load<Texture2D>("parrot");
            applause = Content.Load<SoundEffect>("applause_y");
            nedräkning = Content.Load<SpriteFont>("papegojspel");


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            mus = Mouse.GetState();
            Vector2 tempVect;
            Rectangle tempRect;
            Rectangle tempRect2;

            for (int i = 1; i < parrotRect.Count; i++)
            {
                tempRect = parrotRect[i];
                tempRect.X += (int)parrotHastighet[i].X;
                tempRect.Y += (int)parrotHastighet[i].Y;

                // krockar dem med fönstrets kanter

                if (tempRect.X < 0 || tempRect.X > 750)
                {
                    tempVect = parrotHastighet[i];
                    tempVect.X *= -1;
                    parrotHastighet[i] = tempVect;
                }

                if (tempRect.Y < 0 || tempRect.Y > 430)
                {
                    tempVect = parrotHastighet[i];
                    tempVect.Y *= -1;
                    parrotHastighet[i] = tempVect;
                }

                parrotRect[i] = tempRect;

            }

            // ta bort bilder där musen klickar
            for (int i = parrotRect.Count - 1; i >= 0; i--)
            {
                if (parrotRect[i].Contains(mus.Position)== true && mus.LeftButton == ButtonState.Pressed)
                {
                    parrotRect.RemoveAt(i);
                    räknare--;
                    
                }
            }

            for (int j = parrotRect.Count - 1; j > 0; j--)
            {
                if (parrotRect[j].Intersects(parrotRect[0]))
                {
                    parrotRect.RemoveAt(j);
                    räknare--;
                }
            }

            // styr en av bilderna med tangenter
            
                tempRect2 = parrotRect[0];


                if (keyboard.IsKeyDown(Keys.Up))
                {
                    tempRect2.Y = tempRect2.Y - 2;
                }
                if (keyboard.IsKeyDown(Keys.Down))
                {
                    tempRect2.Y = tempRect2.Y + 2;
                }
                if (keyboard.IsKeyDown(Keys.Left))
                {
                    tempRect2.X = tempRect2.X - 2;
                }
                if (keyboard.IsKeyDown(Keys.Right))
                {
                    tempRect2.X = tempRect2.X + 2; ;
                }
            parrotRect[0] = tempRect2;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MonoGameOrange);

            

            _spriteBatch.Begin();
            _spriteBatch.DrawString(nedräkning, räknare.ToString(), meddelandePosition, Color.Black);
            foreach (Rectangle pos in parrotRect)
            {
                _spriteBatch.Draw(parrot, pos, Color.White);
            }
            

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
