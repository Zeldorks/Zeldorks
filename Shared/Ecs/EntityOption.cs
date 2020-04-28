using System;
using Optional;

namespace NetGameShared.Ecs
{
    public static class EntityOption
    {
        public enum State { Added, JustRemoved, Removed }

        public static State Update(
            this ref Option<Entity> entityOption,
            Registry registry
        ) {
            State result = entityOption.Match(
                some: entity => {
                    if (registry.Contains(entity)) {
                        return State.Added;
                    } else {
                        return State.JustRemoved;
                    }
                },
                none: () => { return State.Removed; }
            );

            switch (result) {
                case State.JustRemoved:
                    entityOption = Option.None<Entity>();
                    break;
            }

            return result;
        }

        public static State Update(
            this ref Option<Entity> entityOption,
            Registry registry,
            Action<Entity> some
        ) {
            State result = entityOption.Match(
                some: entity => {
                    if (registry.Contains(entity)) {
                        some(entity);
                        return State.Added;
                    } else {
                        return State.JustRemoved;
                    }
                },
                none: () => { return State.Removed; }
            );

            switch (result) {
                case State.JustRemoved:
                    entityOption = Option.None<Entity>();
                    break;
            }

            return result;
        }
    }
}
