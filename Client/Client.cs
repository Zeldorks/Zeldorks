using System;
using System.Collections.Generic;
using Optional;

using Monogame = Microsoft.Xna.Framework.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NetClient = NetGameClient.Net.Client;
using Packets = NetGameShared.Net.Protocol.Packets;

using static NetGameClient.InputSources.Keyboard;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace NetGameClient
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Client : Monogame
    {
        private GraphicsDeviceManager graphics;
        private Sound sound;
        private SpriteBatch spriteBatch;
        private NetClient netClient;
        private InputSources.Keyboard keyboard;
        private Option<Game> game;
        private Texture2D Titlescreen;
        private Texture2D prologue;
        private int mAlphaValue = 0;
        private int mFadeIncrement = 5;
        private double mFadeDelay = .055;
        private int track = 0;
        private int currentTitle = 0;

        private SpriteFont debugFont;

        public enum State
        {
            Offline,
            Connecting,
            Loading,
            Online,
            Quitting
        }

        public State state;

        private void SetKeyActions(State state)
        {
            keyboard.ClearKeyActions();

            keyboard.AddKeyAction(
                new KeyAction {
                    key = Keys.Escape,
                    state = KeyState.Unheld
                },
                Input.Quit
            );

            keyboard.AddKeyAction(
                new KeyAction {
                    key = Keys.Q,
                    state = KeyState.Unheld
                },
                Input.DebugPrint
            );

            keyboard.AddKeyAction(
                new KeyAction {
                    key = Keys.R,
                    state = KeyState.Unheld
                },
                Input.DebugRender
            );

            keyboard.AddKeyAction(
                new KeyAction {
                    key = Keys.R,
                    state = KeyState.Released
                },
                Input.DebugUnRender
            );

            keyboard.AddKeyAction(
                new KeyAction {
                    key = Keys.E,
                    state = KeyState.Unheld
                },
                Input.DebugDisconnect
            );

            keyboard.AddKeyAction(
                new KeyAction {
                    key = Keys.E,
                    state = KeyState.Released
                },
                Input.DebugUnDisconnect
            );

            switch (state) {
                case State.Offline:
                    keyboard.AddKeyAction(
                        new KeyAction {
                            key = Keys.C,
                            state = KeyState.Unheld
                        },
                        Input.Connect
                    );
                    break;
                case State.Connecting:
                case State.Loading:
                    keyboard.AddKeyAction(
                        new KeyAction {
                            key = Keys.V,
                            state = KeyState.Unheld
                        },
                        Input.Disconnect
                    );
                    break;
                case State.Online:
                    keyboard.AddKeyAction(
                        new KeyAction {
                            key = Keys.W,
                            state = KeyState.Pressed
                        },
                        Input.MoveUp
                    );
                    keyboard.AddKeyAction(
                        new KeyAction { key = Keys.A,
                            state = KeyState.Pressed
                        },
                        Input.MoveLeft
                    );
                    keyboard.AddKeyAction(
                        new KeyAction {
                            key = Keys.S,
                            state = KeyState.Pressed
                        },
                        Input.MoveDown
                    );
                    keyboard.AddKeyAction(
                        new KeyAction {
                            key = Keys.D,
                            state = KeyState.Pressed
                        },
                        Input.MoveRight
                    );

                    keyboard.AddKeyAction(
                        new KeyAction {
                            key = Keys.J,
                            state = KeyState.Unheld
                        },
                        Input.UseSlotA
                    );
                    keyboard.AddKeyAction(
                        new KeyAction {
                            key = Keys.K,
                            state = KeyState.Unheld
                        },
                        Input.UseSlotB
                    );

                    keyboard.AddKeyAction(
                        new KeyAction {
                            key = Keys.L,
                            state = KeyState.Unheld
                        },
                        Input.NextItemSlotA
                    );
                    keyboard.AddKeyAction(
                        new KeyAction {
                            key = Keys.OemSemicolon,
                            state = KeyState.Unheld
                        },
                        Input.NextItemSlotB
                    );

                    keyboard.AddKeyAction(
                        new KeyAction {
                            key = Keys.O,
                            state = KeyState.Unheld
                        },
                        Input.PrevItemSlotA
                    );
                    keyboard.AddKeyAction(
                        new KeyAction {
                            key = Keys.P,
                            state = KeyState.Unheld
                        },
                        Input.PrevItemSlotB
                    );

                    goto case State.Loading;
            }
        }

        public void SetState(State state, bool force = false)
        {
            if (!force && this.state == state) {
                return;
            }

            this.state = state;

            SetKeyActions(state);
        }

        public Client()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += (Object sender, EventArgs e) => {
                game.MatchSome(game => {
                    game.world.camera.SetViewport(GraphicsDevice);
                });
            };

            IsFixedTimeStep = true;
            TargetElapsedTime = System.TimeSpan.FromMilliseconds(20);

            Global.config = new Config();
        }

        protected override void Initialize()
        {
            sound = new Sound(Content);

            netClient = new NetClient();
            netClient.Start();

            keyboard = new InputSources.Keyboard();

            // Need to force because `state` already equals `State.Offline`, but
            // this is the first time we set the state
            SetState(State.Offline, true);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            debugFont = Content.Load<SpriteFont>("Fonts/Debug");
            Titlescreen = Content.Load<Texture2D>("UI/titlescreenWtext");
            prologue = Content.Load<Texture2D>("UI/Prologue");
        }

        private void ProcessState()
        {
            NetClient.State netState = netClient.state;

            if (netState == NetClient.State.Connecting &&
                state != State.Connecting
            ) {
                System.Console.WriteLine("[DEBUG] Connecting");
                SetState(State.Connecting);
            }

            if (netState == NetClient.State.Online &&
                (state != State.Loading && state != State.Online)
            ) {
                System.Console.WriteLine("[DEBUG] Created new game");
                game = new Game(GraphicsDevice).Some();
            }

            if (netState == NetClient.State.Offline &&
                state != State.Offline
            ) {
                System.Console.WriteLine("[DEBUG] Offline");
                game = Option.None<Game>();
                SetState(State.Offline);
            }

            game.MatchSome(game => {
                Game.State gameState = game.state;
                if (gameState == Game.State.Loading &&
                    state != State.Loading
                ) {
                    System.Console.WriteLine("[DEBUG] Loading");
                    SetState(State.Loading);
                }

                if (gameState == Game.State.Ready &&
                    state != State.Online
                ) {
                    System.Console.WriteLine("[DEBUG] Online");
                    SetState(State.Online);
                }
            });
        }

        private void Process(Queue<Input> inputs)
        {
            var gameInput = new Packets.GameInput();

            while (inputs.Count > 0) {
                Input input = inputs.Dequeue();

                switch (input) {
                    case Input.Quit:
                        Exit();
                        break;
                    case Input.Connect:
                        netClient.Connect();
                        sound.Play(Sound.Background.Dungeon);
                        break;
                    case Input.Disconnect:
                        netClient.Disconnect();
                        sound.Stop();
                        break;
                    case Input.MoveUp:
                    case Input.MoveLeft:
                    case Input.MoveDown:
                    case Input.MoveRight:
                        gameInput.Movement = (int)input.GetGameInputMovement();
                        break;
                    case Input.UseSlotA:
                        gameInput.UseSlotA = true;
                        sound.Play(Sound.Effect.SwordSlash);
                        break;
                    case Input.UseSlotB:
                        gameInput.UseSlotB = true;
                        sound.Play(Sound.Effect.SwordSlash);
                        break;
                    case Input.NextItemSlotA:
                        gameInput.NextItemSlotA = true;
                        break;
                    case Input.NextItemSlotB:
                        gameInput.NextItemSlotB = true;
                        break;
                    case Input.PrevItemSlotA:
                        gameInput.PrevItemSlotA = true;
                        break;
                    case Input.PrevItemSlotB:
                        gameInput.PrevItemSlotB = true;
                        break;
                    case Input.DebugPrint:
                        game.MatchSome(game => {
                            game.world.ecsRegistry.DebugPrint();

                            // This is commented out because it's very slow
                            // game.world.map.DebugPrint();
                        });
                        break;
                    case Input.DebugRender:
                        GameNS.WorldNS.MapsExt.Renderer.renderGameLayer = true;
                        break;
                    case Input.DebugUnRender:
                        GameNS.WorldNS.MapsExt.Renderer.renderGameLayer = false;
                        break;
                    case Input.DebugDisconnect:
                        netClient.DebugDisconnect = true;
                        break;
                    case Input.DebugUnDisconnect:
                        netClient.DebugDisconnect = false;
                        break;
                    default:
                        System.Console.WriteLine(
                            "[WARN] Unhandled input: {0}",
                            input
                        );
                        break;
                }
            }

            // If `gameInput` has been modified to be different from its default
            // value, send it
            if (!gameInput.Equals(new Packets.GameInput())) {
                netClient.Send(gameInput);
            }
        }

        private void Process(NetClient.PacketsReceived packets)
        {
            game.MatchSome(game => {
                var welcomes = packets.welcomes;
                while (!welcomes.IsEmpty) {
                    Packets.Welcome welcome;
                    if (welcomes.TryDequeue(out welcome)) {
                        game.ProcessWelcome(welcome);
                    }
                }

                var gameSnapshots = packets.gameSnapshots;
                while (!gameSnapshots.IsEmpty) {
                    Packets.GameSnapshot gameSnapshot;
                    if (gameSnapshots.TryDequeue(out gameSnapshot)) {
                        game.world.Process(gameSnapshot);
                        netClient.SendSnapshotAck(gameSnapshot.Tick);
                    }
                }

                var gameSnapshotDeltas = packets.gameSnapshotDeltas;
                while (!gameSnapshotDeltas.IsEmpty) {
                    Packets.GameSnapshotDelta gameSnapshotDelta;
                    if (gameSnapshotDeltas.TryDequeue(out gameSnapshotDelta)) {
                        game.world.Process(gameSnapshotDelta);
                        netClient.SendSnapshotAck(gameSnapshotDelta.Tick);
                    }
                }
            });
        }

        protected override void Update(GameTime gameTime)
        {
            keyboard.Update(gameTime);
            netClient.Update();

            ProcessState();
            Process(keyboard.inputsReceived);
            Process(netClient.packetsReceived);

            game.MatchSome(game => game.Update());

            // Decrement the delay by the number of seconds that have elapsed since
            // the last time that the Update method was called
            mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;

            // If the Fade delays has dropped below zero, then it is time to 
            // fade out the image a little bit more.
            if (mFadeDelay <= 0)
            {
                // Reset the Fade delay
                mFadeDelay = .5;

                //Decrement the fade value for the image
                mAlphaValue -= mFadeIncrement;
                track++;

                mFadeDelay -= 0.1;
                mFadeIncrement += (1 / 2);
                if (track <= 5)
                {
                    mFadeDelay = 1.5;
                    mFadeIncrement = 100;
                    mAlphaValue = 255;
                }
            }

            base.Update(gameTime);

            keyboard.PreUpdate();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, null
			);

            switch (state)
            {
                case State.Offline:
                    spriteBatch.Draw(
                        Titlescreen,
                        new Rectangle(
                        0, 0, GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height),
                        Color.White
                    );
                    spriteBatch.Draw(prologue,
                        new Rectangle(0, 0,
                        GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height),
                    new Color(
                        Math.Clamp(mAlphaValue, 0, 255),
                        Math.Clamp(mAlphaValue, 0, 255),
                        Math.Clamp(mAlphaValue, 0, 255),
                        Math.Clamp(mAlphaValue, 0, 255))
                    );
                    break;
                case State.Connecting:
                case State.Loading:
                    break;
                case State.Online:
                    game.MatchSome(game => game.world.Draw(spriteBatch, Content)
                    );
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, System.EventArgs args)
        {
            netClient.Stop();
            base.OnExiting(sender, args);
        }
    }
}
