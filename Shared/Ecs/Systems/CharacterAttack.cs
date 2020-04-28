using System;
using System.Collections.Generic;
using Comps = NetGameShared.Ecs.Components;

using GameInputs = NetGameShared.Net.Protocol.GameInputs;

using static NetGameShared.Constants.Server.Timing;
using Tick = System.UInt32;

namespace NetGameShared.Ecs.Systems
{
    public static class CharacterAttack
    {
        // Show the attack long enough for it to be visible
        private static Tick attackDuration =
            ToTicks(TimeSpan.FromMilliseconds(200));

        public static void Run(Registry registry, Tick tick)
        {
            HashSet<Entity> entities = registry.
                GetEntities(typeof(Comps.Character));

            foreach (Entity entity in entities)
            {
                var characterComp = registry.GetComponentUnsafe<Comps.Character>(entity);

                // Unset `attack` if expired
                Tick elapsed = tick - characterComp.attackStartTick;
                if (elapsed > attackDuration)
                {
                    characterComp.attacking = false;
                }
            }
        }

        // Pre-condition: `movement != GameInputs.Movement.None`
        public static void Process(
            GameInputs.Movement movement,
            Entity entity,
            Registry registry
        )
        {
            // Stop attacking when moving
            var characterCompOpt = registry.GetComponent<Comps.Character>(entity);
            characterCompOpt.MatchSome(characterComp => {
                characterComp.attacking = false;
            });
        }
    }
}
