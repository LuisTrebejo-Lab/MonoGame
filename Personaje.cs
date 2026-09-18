using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System; //libreria que nos permite usar Random()

namespace probandoEstructura
{
    /// <summary>
    /// Clase que administra un personaje grafico 2D
    /// </summary>
    class Personaje
    {
        private Texture2D textura;
        private Rectangle rectangulo;
        private Microsoft.Xna.Framework.Vector2 velocidad;
        private bool activo;
        private int anchoVentana;
        private int altoVentana;

        /// <summary>
        /// Contructor que inicializa todos los parametros del Personaje
        /// </summary>
        /// <param name="sprite">Mapa de bits que se va a dibujar</param>
        /// <param name="rectX">posicion inicial en X</param>
        /// <param name="rectY">posicion inicial en Y</param>
        /// <param name="velX">velocidad en X</param>
        /// <param name="velY">velocidad en Y</param>
        /// /// <param name="anchoVentana">ancho de la ventana del juego</param>
        /// <param name="altoVentana">alto de la ventana del juego</param>
        public Personaje(Texture2D sprite, int rectX, int rectY, int velX, int velY, int anchoVentana, int altoVentana)
        {
            textura=sprite;
            rectangulo = new Rectangle(rectX, rectY, sprite.Width, sprite.Height);
            velocidad = new Microsoft.Xna.Framework.Vector2((float)velX, (float)velY);
            activo = true;
            this.anchoVentana = anchoVentana;
            this.altoVentana = altoVentana;
        }


        //Mueve el personaje segun su velocidad, rebota dentro de los limites de la ventana
        public void update()
        {
            //mover el rectagulo
            rectangulo.X += (int)velocidad.X;
            rectangulo.Y += (int)velocidad.Y;

            //Rebota contra los limmites de la ventana
            //Rebotar sprite(vertical)
            if(rectangulo.Y+rectangulo.Height > altoVentana || rectangulo.Y < 0)
            {
                velocidad.Y = -velocidad.Y;
            }
            //Rebotar sprite(horizontal)
            if(rectangulo.X+rectangulo.Width > anchoVentana || rectangulo.X < 0)
            {
                velocidad.X = -velocidad.X;
            }
        }

        /// <summary>
        /// Dibuja el personaje en la ventana
        /// </summary>
        /// <param name="sb">SpriteBatch </param>
        public void draw(SpriteBatch sb)
        {
            //Dibujo el sprite solo si esta activo
            if(activo)
            {
                sb.Draw(textura,rectangulo, Color.White);
            }
            

        }
        
    }
}