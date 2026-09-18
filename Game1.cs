using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System; //libreria que nos permite usar Random()

namespace probandoEstructura
{
    public class Game1 : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Declaro objetos del programa
        Personaje haru_urara;
        
        //Textura del personaje
        Texture2D texMomia;

        //Rectangulo que define donde y de que tamaño dibujar al personaje
        Rectangle recDibuja;

        //Almaceno la velocidad al que se movera la imagen por la ventana
        Microsoft.Xna.Framework.Vector2 velocidad; //vector2 estructura
        
        //Declaro constantes de la altura y ancho de la ventana
        const int alto_pantalla = 600;
        const int ancho_pantalla = 800;
        
        //velocidad en x e y constante
        //int velocidadX = 5;
        //int velocidadY = 0;

        //Declaro una variable random
        Random random = new Random();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //cambio el tamaño de la ventana
            _graphics.PreferredBackBufferWidth = ancho_pantalla;
            _graphics.PreferredBackBufferHeight = alto_pantalla;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            
            //Creo una textura temporal
            Texture2D tempHaru = Content.Load<Texture2D>("haru");
            //Inicializo al personaje
            haru_urara = new Personaje(tempHaru, 
            random.Next(0, ancho_pantalla-tempHaru.Width),
            random.Next(0, alto_pantalla-tempHaru.Height),
            random.Next(-2,2), random.Next(-3,3), ancho_pantalla, alto_pantalla);

            //Cargo la imagen a mi textura 2D
            texMomia = Content.Load<Texture2D>("momia");

            //Creo el rectangulo
            //recDibuja = new Rectangle(150,100,texMomia.Width,texMomia.Height);
            //Asignamos ejes random en x e y, el rectangulo no comienza en una posicion fija
            recDibuja = new Rectangle(random.Next(0, ancho_pantalla-texMomia.Width),
            random.Next(0, alto_pantalla-texMomia.Height),
            texMomia.Width,texMomia.Height);

            //asignamos la velocidad
            //velocidad = new Microsoft.Xna.Framework.Vector2(3,3);
            //si queremos que la velocidad sea "random"
            velocidad = new Microsoft.Xna.Framework.Vector2(random.Next(-2,2),random.Next(-3,3));
        
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
            haru_urara.update();
            //mover el rectangulo //vector es de tipo flotante, por eso lo pasamos a int porque recDibuja es entero
            recDibuja.X += /*velocidadX*/(int)velocidad.X;
            recDibuja.Y += /*velocidadY*/(int)velocidad.Y;

            //Rebotar sprite(vertical)
            if(recDibuja.Y+recDibuja.Height > alto_pantalla || recDibuja.Y < 0)
            {
                velocidad.Y = -velocidad.Y;
                //velocidadY = -velocidadY;
            }
            //Rebotar sprite(horizontal)
            if(recDibuja.X+recDibuja.Width > ancho_pantalla || recDibuja.X < 0)
            {
                velocidad.X = -velocidad.X;
                //velocidadX = -velocidadX;
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            //Dibujo el personaje
            _spriteBatch.Begin(); //aqui empiezo a dibujar
            
            _spriteBatch.Draw(texMomia, recDibuja, Color.White);
            haru_urara.draw(_spriteBatch);

            _spriteBatch.End(); //en el momento que llega End dibuja todo lo copia el buffer a la pantalla principal

            base.Draw(gameTime);
        }
    }
}