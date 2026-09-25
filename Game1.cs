using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace esqueletoMonoGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    //textSonic es una variable que va ser el sprite de la imagen
    Texture2D texSonic;

    //recSonic va a ser un rectangulo que va a contener a texSonic, pensalo como una hitbox
    Rectangle recSonic;

    //movimiento es una varible que tiene una posicion en X y Y
    //conveniente asi tenemos en una sola varible posicion en X e Y
    Microsoft.Xna.Framework.Vector2 movimiento;

    //Variable que toma el estado de la tecla
    KeyboardState teclaActual;

    //dos constantes que definimos para que sean el temaño de la ventana
    const int alto_pantalla = 600;
    const int ancho_pantalla = 800;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        //Definimos tamaño de la ventana
        _graphics.PreferredBackBufferWidth = ancho_pantalla;
        _graphics.PreferredBackBufferHeight = alto_pantalla;

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        //le asignamos a movimiento la cantidad de pixeles en X e Y que se va a mover el rectangulo 
        movimiento = new Microsoft.Xna.Framework.Vector2(5,5);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        //asignamos a texSonic la imagen que cargamos en content
        texSonic = Content.Load<Texture2D>("sonic");

        //definimos la posicion en X e Y inicial, definimos la anchura y altura del rectangulo
        recSonic = new Rectangle(((ancho_pantalla/2)-75), ((alto_pantalla/2)-75), 150, 150);
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        //teclaActual lee el estado actual de la tecla, osea que tecla se esta presionando
        teclaActual = Keyboard.GetState();

        //si presionas la tecla esc cierra el juego
        if(teclaActual.IsKeyDown(Keys.Escape))
        {
            this.Exit();
        }

        //si presionas la tecla left se mueve a la izquierda sobre el eje X
        if(teclaActual.IsKeyDown(Keys.Left))
        {
            recSonic.X -= (int)movimiento.X;
        }

        //si presionas la tecla rigt se mueve a la derecha sobre el eje X
        if(teclaActual.IsKeyDown(Keys.Right))
        {
            recSonic.X += (int)movimiento.X;
        }

        //si presionas la tecla Up se mueve para arriba sobre el eje Y
        if(teclaActual.IsKeyDown(Keys.Up))
        {
            recSonic.Y -= (int)movimiento.Y;
        }

        //si presionas la tecla Down se mueve para abajo sobre el eje Y
        if(teclaActual.IsKeyDown(Keys.Down))
        {
            recSonic.Y += (int)movimiento.Y;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        _spriteBatch.Draw(texSonic, recSonic, Color.White);
        _spriteBatch.End();
        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
