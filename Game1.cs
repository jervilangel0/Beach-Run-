using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


//NOT THESE
namespace FINALPROJ
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private Texture2D _walkTextureRight;
        private Texture2D _walkTextureLeft;
        private Texture2D _attackTexture;
        private Texture2D _idleTexture;
        private int currentFrame;
        private int totalFrames;
        private int frameWidth;
        private int frameHeight;
        private Vector2 _position;
        private bool isWalking;
        private bool isFacingRight;
        private bool isJumping;
        private float jumpVelocity;
        private const float Gravity = 0.5f;
        private const int AnimationDelay = 200; // fixed anim delay between frames 
        private int animationTimer;
        private bool isAttacking;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _walkTextureRight = Content.Load<Texture2D>("char1right");
            _walkTextureLeft = Content.Load<Texture2D>("char1left");
            _attackTexture = Content.Load<Texture2D>("char1attack");
            _idleTexture = Content.Load<Texture2D>("char1idle");


            _position = new Vector2(100, 200); // Starting Pos
            currentFrame = 0;
            totalFrames = 8;
            frameWidth = _idleTexture.Width / totalFrames;
            frameHeight = _idleTexture.Height;
            isWalking = false;
            isFacingRight = true;
            isJumping = false;
            jumpVelocity = 0f;
            animationTimer = 0;
            isAttacking = false;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboardState = Keyboard.GetState();
            var movementSpeed = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) ? 5 : 3; // Check if Shift key is held down

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                _position.X -= movementSpeed;
                isFacingRight = false;
                isWalking = true;
                animationTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (animationTimer >= AnimationDelay)
                {
                    animationTimer = 0;
                    currentFrame++;
                    if (currentFrame >= totalFrames)
                        currentFrame = 0;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                _position.X += movementSpeed;
                isFacingRight = true;
                isWalking = true;
                animationTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (animationTimer >= AnimationDelay)
                {
                    animationTimer = 0;
                    currentFrame++;
                    if (currentFrame >= totalFrames)
                        currentFrame = 0;
                }
            }
            else
            {
                isWalking = false;
                animationTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (animationTimer >= AnimationDelay)
                {
                    animationTimer = 0;
                    currentFrame++;
                    if (currentFrame >= totalFrames)
                        currentFrame = 0;
                }
            }

            if ((keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up)) && !isJumping)
            {
                jumpVelocity = -10f; // Changeable velocity
                isJumping = true;
            }

            // Applying gravity
            if (isJumping)
            {
                jumpVelocity += Gravity;
                _position.Y += jumpVelocity;

                if (_position.Y >= 200)
                {
                    _position.Y = 200;
                    jumpVelocity = 0f;
                    isJumping = false;
                }
            }

            // If left click is triggered
            var mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                isAttacking = true;
                currentFrame = 0; // Return to current frame after
            }
            else
            {
                isAttacking = false;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (isWalking)
            {
                Texture2D currentTexture;

                if (isFacingRight)
                    currentTexture = _walkTextureRight;
                else
                    currentTexture = _walkTextureLeft;

                // x, y coordinates of the frame
                int frameX = currentFrame * frameWidth;
                int frameY = 0;

                _spriteBatch.Draw(currentTexture, _position, new Rectangle(frameX, frameY, frameWidth, frameHeight), Color.White);
            }
            else if (isAttacking)
            {
                int frameX = currentFrame * frameWidth;
                int frameY = 0;

                _spriteBatch.Draw(_attackTexture, _position, new Rectangle(frameX, frameY, frameWidth, frameHeight), Color.White);

                currentFrame++;
                if (currentFrame > 3)
                {
                    currentFrame = 0;
                    isAttacking = false;
                }
            }
            else
            {
                int frameX = currentFrame * frameWidth;
                int frameY = 0;

                _spriteBatch.Draw(_idleTexture, _position, new Rectangle(frameX, frameY, frameWidth, frameHeight), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
