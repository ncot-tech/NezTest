using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;


namespace NezTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Core
    {

        Scene myScene;
        RoomManager roomManager;

        public Game1() : base(width: 1024, height: 768, isFullScreen: false, enableEntitySystems: false)
        { }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            var friction =0.8f;
            var elasticity = 0.2f;

            base.Initialize();
            
            Window.AllowUserResizing = true;

            roomManager = new RoomManager();
            roomManager.Generate();

            // create our Scene with the DefaultRenderer and a clear color of CornflowerBlue
            Physics.gravity = Vector2.Zero;
            myScene = Scene.createWithDefaultRenderer(Color.CornflowerBlue);
            Input.gamePads[0].isLeftStickVertcialInverted = true;
            // load a Texture. Note that the Texture is loaded via the scene.content class. This works just like the standard MonoGame Content class
            // with the big difference being that it is tied to a Scene. When the Scene is unloaded so too is all the content loaded via myScene.content.
            var beeTexture = myScene.content.Load<Texture2D>("atariBee");
            var noise = myScene.content.Load<SoundEffect>("Randomize103");
            noise.Play(0.5f, 1.0f, 0.0f);
            // setup our Scene by adding some Entities
            var entityOne = createEntity(new Vector2(200, 200), 15f, friction, elasticity, Vector2.Zero, beeTexture);
            createWallEntity(new Vector2(512, 700), 1024, 64);// floor
            createWallEntity(new Vector2(32, 320), 64, 768);
            createWallEntity(new Vector2(1020, 320), 64, 768);
            createWallEntity(new Vector2(512, 32), 1024, 64);//roof
            // set the scene so Nez can take over
            scene = myScene;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        ArcadeRigidbody createWallEntity(Vector2 position, float width, float height)
        {
            var rigidbody = new ArcadeRigidbody()
                .setMass(0)
                .setFriction(0)
                .setElasticity(1.0f);

            var entity = myScene.createEntity(Utils.randomString(3));
            entity.transform.position = position;
            var spr = new PrototypeSprite(width, height);
            spr.color = Color.DarkGoldenrod;
            entity.addComponent(spr);
            entity.addComponent(rigidbody);
            entity.addCollider(new BoxCollider());

            return rigidbody;
        }

        ArcadeRigidbody createEntity(Vector2 position, float mass, float friction, float elasticity, Vector2 velocity, Texture2D texture)
        {
            var rigidbody = new ArcadeRigidbody()
                .setMass(mass)
                .setFriction(friction)
                .setElasticity(elasticity)
                .setVelocity(velocity);



            var entity = myScene.createEntity(Utils.randomString(3));
            entity.transform.position = position;
            entity.addComponent(new Sprite(texture));
            entity.addComponent(rigidbody);
            entity.addComponent(new ImpulseMover());
            entity.addCollider(new CircleCollider());

            return rigidbody;
        }
    }
}
