using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Topic_1_5_Assigment
{
    enum Screen
    {
        Intro,
        Game
    }
    //enum AnimationStage
    //{
    //    One,
    //    Two,
    //    Three,
    //    Four,
    //    Five
    //}

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Screen screen;
        SoundEffect cheer;
        SoundEffectInstance cheerInstance;
        SoundEffect endCheer;
        SoundEffectInstance endCheerInstance;
        //AnimationStage stage;

        List<Texture2D> cheerleaders;
        int cheerFrame;
        float cheerSpeed, cheerTime;

        Texture2D retroCov, field, qbTexture, oLine, oLine2, dLine, dLine2, ball, pickTexture, pressTexture, qbDown, touchdown;
        Rectangle window, qbRect, oLineRect, oLineRect2, dLineRect, dLineRect2, pickRect, pressRect, qbDownRect, cheerUpRect, cheerDownRect;
        Vector2 pickVect;
        MouseState mouseState;
        KeyboardState keyboardState;
        bool spacePressed;
        bool done, finish, page2, deadq, spaceDown;
        float seconds;
       
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            cheerTime = 0;
            cheerFrame = 0;
            cheerSpeed = 0.8f;
            cheerleaders = new List<Texture2D>();
            //stage = AnimationStage.One;
            spacePressed = false;
            done = false;
            seconds = 0f;
            finish = false;
            page2 = false;
            // TODO: Add your initialization logic here
            screen = Screen.Intro;
            deadq = true;
            spaceDown = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            pickRect = new Rectangle(320,200,40,40);
            window = new Rectangle (0, 0, 1067, 600);
            qbRect = new Rectangle(200, 200, 80, 40);
            oLineRect = new Rectangle(190, 145, 210, 105);
            oLineRect2 = new Rectangle(190, 205, 210, 105);
            dLineRect2 = new Rectangle(100, 180, 325, 165);
            dLineRect = new Rectangle(100, 110, 325, 165);
            pressRect = new Rectangle(10,300,200,200);
            pickVect = new Vector2(2, 0);
            qbDownRect = new Rectangle(195, 165, 40, 80);
            cheerDownRect = new Rectangle(100, 100, 400, 200);
            //cheerUpRect = new Rectangle(120, 120, 80, 40);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();
            retroCov = Content.Load<Texture2D>("RetroCov");
            field = Content.Load<Texture2D>("Field");
            qbTexture = Content.Load<Texture2D>("QBthrow2");
            oLine = Content.Load<Texture2D>("OL");
            oLine2 = Content.Load<Texture2D>("OL");
            dLine = Content.Load<Texture2D>("DL");
            dLine2 = Content.Load<Texture2D>("DL");
            ball = Content.Load<Texture2D>("ball");
            pickTexture = Content.Load<Texture2D>("pick2");
            pressTexture = Content.Load<Texture2D>("pressBegin");
            qbDown = Content.Load<Texture2D>("QBdown");
            touchdown = Content.Load<Texture2D>("touchdown");
            cheer = Content.Load<SoundEffect>("chearcrowd");
            cheerInstance = cheer.CreateInstance();
            endCheer = Content.Load<SoundEffect>("EndCheer");
            endCheerInstance = endCheer.CreateInstance();
            cheerleaders.Add(Content.Load<Texture2D>("cheerDown"));
            cheerleaders.Add(Content.Load<Texture2D>("cheerUp3"));

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            this.Window.Title = $"x = {mouseState.X}, y = {mouseState.Y}";

            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            cheerTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (cheerTime > cheerSpeed)
            {
                cheerTime = 0;
                cheerFrame = (cheerFrame + 1) % 2;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (screen == Screen.Intro)
            {
                
                if (keyboardState.GetPressedKeyCount() > 0)
                    screen = Screen.Game;

            }
            else if (screen == Screen.Game)
            {
                cheerInstance.Play();
                if (spaceDown)
                    pickRect.X += -1;

                if (keyboardState.IsKeyDown(Keys.Space))
                {

                    spacePressed = true;
                    finish = true;
                    


                }
                if (pickRect.X == 256)
                {
                    page2 = true;
                    endCheerInstance.Play();
                }
                if (seconds >= 7 && finish)
                {
                    Exit();
                }
                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            if (screen == Screen.Intro)
            {
                _spriteBatch.Draw(retroCov, window, Color.White);
            }
            else if (screen == Screen.Game)
            {
                
                _spriteBatch.Draw(field, window, Color.White);
                _spriteBatch.Draw(cheerleaders[cheerFrame], cheerDownRect, Color.White);
                
                if (deadq)
                {
                    _spriteBatch.Draw(qbTexture, qbRect, Color.White);
                }
                
                _spriteBatch.Draw(oLine, oLineRect, Color.White);
                _spriteBatch.Draw(oLine2, oLineRect2, Color.White);
                _spriteBatch.Draw(dLine2, dLineRect2, Color.White);
                _spriteBatch.Draw(dLine, dLineRect, Color.White);
                
                if (!done)
                _spriteBatch.Draw(pressTexture, pressRect, Color.White);
                _spriteBatch.Draw(pickTexture, pickRect, Color.White);
                if (spacePressed)
                {
                    spaceDown = true;
                    
                }
                if (spaceDown)
                {

                    if (!page2)
                    {
                        _spriteBatch.Draw(ball, qbRect, Color.White);
                    }
                    _spriteBatch.Draw(pickTexture, pickRect, Color.White);

                    done = true;
                    if (pickRect.X <= 200)
                    {
                        _spriteBatch.Draw(qbDown, qbDownRect, Color.White);
                        deadq = false;
                        
                    }
                    if (pickRect.X <= -10)
                    {
                        _spriteBatch.Draw(touchdown, window, Color.White);
                        
                    }
                    
                }
                
            }
           

                _spriteBatch.End();
            

            base.Draw(gameTime);
        }
    }
}
