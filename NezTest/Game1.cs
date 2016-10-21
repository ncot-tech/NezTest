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
        Texture2D whiteRectangle;
        SpriteBatch spriteBatch;

        delegate int genCoord(int xy);
        delegate int genCoord2(int xy, int off);
        genCoord makeGridCoord;
        genCoord2 makeDoorCoord;

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
            makeGridCoord = xy => 32 + (xy * (64 + 2));

            makeDoorCoord = (xy,off) => off + (xy * (64 + 2));

            spriteBatch = new SpriteBatch(GraphicsDevice);
            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
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
            //var beeTexture = myScene.content.Load<Texture2D>("atariBee");
            //var noise = myScene.content.Load<SoundEffect>("Randomize103");
            //noise.Play(0.5f, 1.0f, 0.0f);
            // setup our Scene by adding some Entities
            //var entityOne = createEntity(new Vector2(200, 200), 15f, friction, elasticity, Vector2.Zero, beeTexture);
            //createWallEntity(new Vector2(512, 700), 1024, 64);// floor
            //createWallEntity(new Vector2(32, 320), 64, 768);
            //createWallEntity(new Vector2(1020, 320), 64, 768);
            //createWallEntity(new Vector2(512, 32), 1024, 64);//roof
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

            base.Draw(gameTime);

            DrawGrid();
            // TODO: Add your drawing code here
            

            
        }

        private void DrawGrid()
        {
            spriteBatch.Begin();
            for (int y = 0; y < 10; y ++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (roomManager.rooms[y, x] != null)
                    {
                        // Draw a rect to represent the room
                        spriteBatch.Draw(whiteRectangle, new Rectangle(makeGridCoord(x), makeGridCoord(y), 64, 64), Color.White);
                        // Draw a little rect to show a link for each exit
                        for (int i = 0; i < 4; i++)
                        {
                            int ex = roomManager.rooms[y, x].Exits[i, 1];
                            int ey = roomManager.rooms[y, x].Exits[i, 0];
                            if (ex != -1)
                            {
                                // Work out the exit direction
                                ex -= x;
                                ey -= y;
                                if (ex == 0 && ey == -1)            // N
                                {
                                    spriteBatch.Draw(whiteRectangle, new Rectangle(makeDoorCoord(x,32+16), makeDoorCoord(y,32-2), 32, 2), Color.White);
                                } else if (ex == 1 && ey == 0)      // E                                                            
                                {                                                                                                      
                                    spriteBatch.Draw(whiteRectangle, new Rectangle(makeDoorCoord(x,32+64), makeDoorCoord(y,32+16), 2, 32), Color.White);
                                } else if (ex == 0 && ey == 1)      // S                                                            
                                {                                                                                                     
                                    spriteBatch.Draw(whiteRectangle, new Rectangle(makeDoorCoord(x,32+16), makeDoorCoord(y,32+64), 32, 2), Color.White);
                                } else if (ex == 0 && ey == -1)     // W                                                            
                                {                                                                                                      
                                    spriteBatch.Draw(whiteRectangle, new Rectangle(makeDoorCoord(x,32-2), makeDoorCoord(y,32+16), 2, 32), Color.White);
                                }
                            }
                        }
                    }
                }
            }
            spriteBatch.End();          
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
