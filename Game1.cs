using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShootingGalery;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D targetSprite;
    private Texture2D backgroundSprite;

    private SpriteFont gameFont;

    private Random rand = new Random();

    private Vector2 targetPosition = new Vector2(300, 300);
    private const int targetRadius = 45;

    private MouseState mouseState;
    private bool mouseReleased = true;

    private int score = 0;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    private void NewTargetPosition()
    {
        targetPosition.X = rand.Next(targetRadius, _graphics.PreferredBackBufferWidth - targetRadius);
        targetPosition.Y = rand.Next(targetRadius, _graphics.PreferredBackBufferHeight - targetRadius);
    }

    protected override void Initialize()
    {
        NewTargetPosition();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        targetSprite = Content.Load<Texture2D>("target");
        backgroundSprite = Content.Load<Texture2D>("sky");
        gameFont = Content.Load<SpriteFont>("galleryFont");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        mouseState = Mouse.GetState();

        if (mouseState.LeftButton == ButtonState.Pressed && mouseReleased)
        {
            var distance = Vector2.Distance(targetPosition, mouseState.Position.ToVector2());
            if (distance < targetRadius)
            {
                NewTargetPosition();
                score++;
            }

            mouseReleased = false;
        }

        if (mouseState.LeftButton == ButtonState.Released)
        {
            mouseReleased = true;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);
        _spriteBatch.Draw(targetSprite, new Vector2(targetPosition.X - targetRadius, targetPosition.Y - targetRadius),
            Color.White);
        _spriteBatch.DrawString(gameFont, $"Score: {score}", new Vector2(10, 10), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}