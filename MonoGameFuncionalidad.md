# ¿Cómo funciona MonoGame?

Guía rápida para entender la estructura básica de un proyecto MonoGame.

Desde Visual Studio 2022
- Crear un proyecto
- Buscamos MonoGame
- Utilizamos la plantilla MonoGame Windows Desktop Application

## La idea general

Un juego, en el fondo, es un programa que repite un ciclo muchísimas veces por segundo (normalmente 60 veces por segundo):

1. Revisa qué pasó (¿se movió el mouse?, ¿se apretó una tecla?)
2. Actualiza el estado del juego (mueve personajes, revisa colisiones, etc.)
3. Dibuja todo en pantalla

Eso se llama el **game loop** (bucle del juego). MonoGame ya te arma ese bucle por vos: vos solo tenés que llenar 4 métodos y el motor se encarga de llamarlos en el orden correcto, una y otra vez, automáticamente.

Esos 4 métodos viven en la clase principal del juego (en este proyecto, [Game1.cs](Game1.cs)), y son:

```
Initialize()  →  LoadContent()  →  [ Update() ←→ Draw() se repiten en loop ]
```

## 1. El constructor `Game1()`

No es uno de los 4 métodos "famosos", pero es lo primero que se ejecuta. Acá se configuran cosas muy generales, antes de que arranque nada gráfico: el tamaño de la ventana, si el mouse se ve o no, dónde está la carpeta de contenido, etc.

Ejemplo real del proyecto ([Game1.cs:38-47](Game1.cs#L38-L47)):

```csharp
public Game1()
{
    _graphics = new GraphicsDeviceManager(this);
    Content.RootDirectory = "Content";
    IsMouseVisible = true;

    // cambio el tamaño de la ventana
    _graphics.PreferredBackBufferWidth = ancho_pantalla;
    _graphics.PreferredBackBufferHeight = alto_pantalla;
}
```

## 2. `Initialize()`

Se ejecuta **una sola vez**, al principio, después del constructor. Es para inicializar cosas que necesitan que el motor ya esté "armado" (por ejemplo, la tarjeta gráfica ya lista), pero que **todavía no dependen de imágenes, sonidos ni ningún archivo cargado**.

En un juego simple casi no se usa mucho, y muchas veces se deja casi vacío (como en este proyecto).

```csharp
protected override void Initialize()
{
    // Acá iría lógica de inicialización que no necesite imágenes/sonidos todavía
    base.Initialize();
}
```

> Regla simple: si necesitás cargar una imagen o un sonido, **no va acá**, va en `LoadContent()`.

## 3. `LoadContent()`

También se ejecuta **una sola vez**, justo después de `Initialize()`. Acá es donde se cargan todos los recursos (assets): imágenes, sonidos, fuentes de texto, etc. También es donde se suelen crear los objetos del juego, porque muchos objetos necesitan una imagen ya cargada para poder existir.

Ejemplo real ([Game1.cs:55-85](Game1.cs#L55-L85)):

```csharp
protected override void LoadContent()
{
    _spriteBatch = new SpriteBatch(GraphicsDevice);

    // Cargo una imagen desde la carpeta Content
    Texture2D tempHaru = Content.Load<Texture2D>("haru");

    // Creo el personaje usando esa imagen ya cargada
    haru_urara = new Personaje(tempHaru,
        random.Next(0, ancho_pantalla - tempHaru.Width),
        random.Next(0, alto_pantalla - tempHaru.Height),
        random.Next(-2, 2), random.Next(-3, 3),
        ancho_pantalla, alto_pantalla);

    texMomia = Content.Load<Texture2D>("momia");
}
```

Puntos clave para explicarle a alguien nuevo:

- `Content.Load<Texture2D>("haru")` busca un archivo llamado `haru` dentro de la carpeta `Content` (ese archivo se agrega antes con el **Content Pipeline**, el `Content.mgcb`).
- `SpriteBatch` es el "pincel" que se usa después en `Draw()` para dibujar imágenes en pantalla. Se crea acá porque recién acá ya existe la tarjeta gráfica lista para usarse.

## 4. `Update(GameTime gameTime)`

A partir de acá empieza el **loop real**: `Update()` y `Draw()` se llaman en bucle, muchas veces por segundo, mientras el juego esté abierto.

`Update()` es donde va **toda la lógica del juego**: mover cosas, leer el teclado/mouse/mando, detectar colisiones, revisar si el jugador ganó o perdió, etc. Acá **nunca se dibuja nada**, solo se calculan datos.

Ejemplo real ([Game1.cs:87-113](Game1.cs#L87-L113)):

```csharp
protected override void Update(GameTime gameTime)
{
    // Leer input: si se aprieta ESC (o Back del mando), cerrar el juego
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
        || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

    base.Update(gameTime);

    haru_urara.update(); // le pido al personaje que actualice su propia posición

    // Mover el rectángulo según su velocidad
    recDibuja.X += (int)velocidad.X;
    recDibuja.Y += (int)velocidad.Y;

    // Rebotar contra los bordes de la ventana
    if (recDibuja.Y + recDibuja.Height > alto_pantalla || recDibuja.Y < 0)
        velocidad.Y = -velocidad.Y;

    if (recDibuja.X + recDibuja.Width > ancho_pantalla || recDibuja.X < 0)
        velocidad.X = -velocidad.X;
}
```

El parámetro `GameTime gameTime` es importante: te dice cuánto tiempo pasó desde el frame anterior. Sirve para que el movimiento sea parejo sin importar si la compu es rápida o lenta (en este ejemplo no se usa `gameTime` todavía, pero es la herramienta correcta para eso).

## 5. `Draw(GameTime gameTime)`

Se llama justo después de cada `Update()`, y es donde se **dibuja** todo lo que se calculó. Acá no se debería cambiar la lógica del juego, solo mostrar en pantalla el estado actual.

Ejemplo real ([Game1.cs:115-129](Game1.cs#L115-L129)):

```csharp
protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.CornflowerBlue); // "borro" la pantalla pintándola de un color de fondo

    _spriteBatch.Begin();               // aviso que voy a empezar a dibujar

    _spriteBatch.Draw(texMomia, recDibuja, Color.White);
    haru_urara.draw(_spriteBatch);      // le pido al personaje que se dibuje solo

    _spriteBatch.End();                 // termino de dibujar: acá se manda todo a la pantalla

    base.Draw(gameTime);
}
```

Cosas clave para explicar:

- `GraphicsDevice.Clear(...)` limpia la pantalla del frame anterior. Si no se hace esto, quedarían "rastros" de los frames pasados.
- Todo dibujo va **entre** `_spriteBatch.Begin()` y `_spriteBatch.End()`.
- El orden en que se llama a `Draw` importa: lo que se dibuja último queda "encima" de lo anterior.

## Resumen del ciclo completo

```
Game1()          → configuración general (una sola vez)
Initialize()      → inicialización sin recursos cargados (una sola vez)
LoadContent()     → cargar imágenes/sonidos y crear objetos (una sola vez)
        │
        ▼
   ┌─────────┐
   │ Update()│  ← lógica: mover, leer input, detectar colisiones
   └────┬────┘
        │
   ┌────▼────┐
   │  Draw()  │  ← dibujar en pantalla el estado actual
   └────┬────┘
        │
        └──── se repite en loop, ~60 veces por segundo, hasta que se cierra el juego
```

## Bonus: por qué existe la clase `Personaje`

Este proyecto además tiene una clase propia, [Personaje.cs](Personaje.cs), que **no** es parte de MonoGame, sino una idea de organización: en vez de meter toda la lógica de mover y dibujar un personaje dentro de `Game1.cs`, se la separa en su propia clase con sus propios métodos `update()` y `draw()`.

Es un patrón muy común: `Game1` (el juego) le pide a cada objeto (`Personaje`, y en un juego más grande podrían ser `Enemigo`, `Bala`, `Powerup`, etc.) que se actualice y se dibuje a sí mismo, en vez de que `Game1` sepa hacer todo. Así el código se mantiene más ordenado a medida que el juego crece.

## Para empezar un proyecto nuevo desde cero

1. Crear el proyecto con la plantilla de MonoGame (`dotnet new mgdesktopgl` o desde Visual Studio).
2. En el constructor de `Game1`, configurar tamaño de ventana y opciones básicas.
3. En `LoadContent()`, agregar las imágenes a `Content.mgcb` (con el MGCB Editor) y cargarlas con `Content.Load<Texture2D>("nombre")`.
4. En `Update()`, ir agregando la lógica: mover cosas, leer teclado, detectar colisiones.
5. En `Draw()`, dibujar cada cosa con `_spriteBatch.Draw(...)` entre `Begin()` y `End()`.
6. Ejecutar con `dotnet run` y ver los cambios.
