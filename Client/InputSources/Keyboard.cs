using System;
using Microsoft.Xna.Framework;
using XnaKeyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace NetGameClient.InputSources
{
    public class Keyboard
    {
        public enum KeyState
        {
            Pressed, Unheld, Held, Released
        }

        public Dictionary<KeyState, HashSet<Keys>> keysByState;
        public Dictionary<Keys, TimeSpan> keyStartTimes;
        public static readonly TimeSpan preHeldTimeSpan = TimeSpan.FromMilliseconds(500);

        public class KeyAction : IEquatable<KeyAction>
        {
            public Keys key;
            public KeyState state;

            public override int GetHashCode() {
                // The max value of key is 255
                return (int)key + ((int)state * 256);
            }

            public override bool Equals(object obj) {
                return Equals(obj as KeyAction);
            }

            public bool Equals(KeyAction keyAction) {
                return
                    keyAction != null &&
                    keyAction.key == this.key &&
                    keyAction.state == this.state;
            }
        }

        public Dictionary<KeyAction, HashSet<Input>> keyActionDict;
        public Queue<Input> inputsReceived;

        private KeyboardState prevState;

        public Keyboard()
        {
            keysByState = new Dictionary<KeyState, HashSet<Keys>>();
            foreach (KeyState keyState in Enum.GetValues(typeof(KeyState))) {
                keysByState[keyState] = new HashSet<Keys>();
            }

            keyStartTimes = new Dictionary<Keys, TimeSpan>();

            keyActionDict = new Dictionary<KeyAction, HashSet<Input>>();
            inputsReceived = new Queue<Input>();
        }

        public void AddKeyAction(KeyAction keyAction, Input input)
        {
            if (keyActionDict.ContainsKey(keyAction)) {
                keyActionDict[keyAction].Add(input);
            } else {
                keyActionDict[keyAction] = new HashSet<Input> {
                    input
                };
            }
        }
    
        public void RemoveKeyAction(KeyAction keyAction, Input input)
        {
            keyActionDict[keyAction].Remove(input);
        }

        public void ClearKeyActions()
        {
            keyActionDict.Clear();
        }

        private void OnKeyDown(Keys key, GameTime gameTime)
        {
            if (keysByState[KeyState.Pressed].Contains(key)) {
                var keyStartTime = keyStartTimes[key];
                var elasped = gameTime.TotalGameTime - keyStartTime;
                if (elasped > preHeldTimeSpan) {
                    keysByState[KeyState.Held].Add(key);
                }
            } else {
                keysByState[KeyState.Pressed].Add(key);
                keysByState[KeyState.Unheld].Add(key);
                keyStartTimes[key] = gameTime.TotalGameTime;
            }

            keysByState[KeyState.Released].Remove(key);
        }

        public void OnKeyUp(Keys key)
        {
            keysByState[KeyState.Released].Add(key);
            keysByState[KeyState.Pressed].Remove(key);
            keysByState[KeyState.Unheld].Remove(key);
            keysByState[KeyState.Held].Remove(key);
        }

        public void PreUpdate()
        {
            keysByState[KeyState.Unheld].Clear();
            keysByState[KeyState.Released].Clear();
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState state = XnaKeyboard.GetState();

            var keysToCheck = new HashSet<Keys>(state.GetPressedKeys());
            keysToCheck.UnionWith(new HashSet<Keys>(prevState.GetPressedKeys()));

            foreach(Keys key in keysToCheck) {
                if (state.IsKeyDown(key)) {
                    OnKeyDown(key, gameTime);
                } else if (prevState.IsKeyDown(key)) {
                    OnKeyUp(key);
                }
            }

            foreach (KeyValuePair<KeyState, HashSet<Keys>> pair in keysByState) {
                KeyState keyState = pair.Key;
                HashSet<Keys> keys = pair.Value;
                foreach (Keys key in keys) {
                    var keyAction = new KeyAction {
                        key = key,
                        state = keyState
                    };

                    if (keyActionDict.ContainsKey(keyAction)) {
                        HashSet<Input> inputs = keyActionDict[keyAction];
                        foreach (Input input in inputs) {
                            inputsReceived.Enqueue(input);
                        }
                    }
                }
            }

            prevState = state;
        }
    }
}
