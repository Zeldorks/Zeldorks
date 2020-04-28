using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Optional;

namespace NetGameShared.Ecs
{
    public class Registry
    {
        protected Dictionary<Entity, Dictionary<Type, IComponent>> data;
        protected Dictionary<Type, HashSet<Entity>> entitiesWithType;
        protected uint nextEntityId = 0;

        public Registry()
        {
            entitiesWithType = new Dictionary<Type, HashSet<Entity>>();
            data = new Dictionary<Entity, Dictionary<Type, IComponent>>();
        }

        public Entity CreateEntity()
        {
            Entity result = new Entity(nextEntityId);
            nextEntityId++;
            return result;
        }

        // Add component to entity.
        // If the entity doesn't exist, it is created.
        // If the entity already contains a component of the same type, then the
        // previous component is overwritten.
        public void AssignComponent(Entity entity, IComponent component)
        {
            Type compType = component.GetType();

            if (data.ContainsKey(entity)) {
                data[entity][compType] = component;
            } else {
                data[entity] = new Dictionary<Type, IComponent> {
                    { compType, component }
                };

                nextEntityId = Math.Max(nextEntityId, entity.id + 1);
            }

            if (entitiesWithType.ContainsKey(compType)) {
                entitiesWithType[compType].Add(entity);
            } else {
                entitiesWithType[compType] = new HashSet<Entity> {
                    entity
                };
            }
        }

        // Get entities that have components of the specified type
        public HashSet<Entity> GetEntities(Type compType)
        {
            if (entitiesWithType.ContainsKey(compType)) {
                // We return a shallow copy of the HashSet so the caller cannot
                // inadvertently mutate `entitiesWithType`
                return new HashSet<Entity>(entitiesWithType[compType]);
            } else {
                // Return empty
                return new HashSet<Entity>();
            }
        }

        // Using a `List` can result in faster access and iteration
        public List<Entity> GetEntitiesList(Type compType)
        {
            if (entitiesWithType.ContainsKey(compType)) {
                // We return a shallow copy of the HashSet so the caller cannot
                // inadvertently mutate `entitiesWithType`
                return new List<Entity>(entitiesWithType[compType]);
            } else {
                // Return empty
                return new List<Entity>();
            }
        }

        // Get entities that have components of all specified types
        public HashSet<Entity> GetEntities(params Type[] compTypes)
        {
            HashSet<Entity> result = GetEntities(compTypes[0]);

            for (int i = 1; i < compTypes.Length; i++) {
                HashSet<Entity> matches = GetEntities(compTypes[i]);
                result.IntersectWith(matches);
            }

            return result;
        }

        // Get entities that have components of any of the specified types
        public HashSet<Entity> GetEntitiesWithAny(params Type[] compTypes)
        {
            HashSet<Entity> result = new HashSet<Entity>();

            for (int i = 0; i < compTypes.Length; i++) {
                HashSet<Entity> matches = GetEntities(compTypes[i]);
                result.UnionWith(matches);
            }

            return result;
        }

        // TODO: Implement function that gets entities that have components of
        // any sub-types of the specified type
        // E.g. `Shape -> { Shapes.Rectangles, Shapes.Circle }

        // Get components belonging to entity
        public Dictionary<Type, IComponent> GetComponents(Entity entity)
        {
            if (data.ContainsKey(entity)) {
                return data[entity];
            } else {
                // Return empty
                return new Dictionary<Type, IComponent>();
            }
        }

        // Get component of the specified type belonging to entity
        public Option<Comp> GetComponent<Comp>(Entity entity)
            where Comp : IComponent
        {
            var ecDict = GetComponents(entity);
            if (ecDict.ContainsKey(typeof(Comp))) {
                var comp = (Comp)ecDict[typeof(Comp)];
                return comp.Some();
            } else {
                return Option.None<Comp>();
            }
        }

        // Like `GetComponentOfEntity`, but fail if entity doesn't have the
        // component
        public Comp GetComponentUnsafe<Comp>(Entity entity)
            where Comp : IComponent
        {
            Option<Comp> compOpt = GetComponent<Comp>(entity);
            return compOpt.Match(
                some: comp => { return comp; },
                none: () => {
                    throw new ArgumentException(
                        "Entity doesn't contain specified component"
                    );
                }
            );
        }

        public int CountComponents(Entity entity)
        {
            return GetComponents(entity).Count;
        }

        protected virtual void RemoveIfEmpty(Entity entity)
        {
            if (CountComponents(entity) == 0) {
                data.Remove(entity);
            }
        }

        public void RemoveComponent(Entity entity, Type compType)
        {
            if (data.ContainsKey(entity)) {
                data[entity].Remove(compType);
            }

            if (entitiesWithType.ContainsKey(compType)) {
                entitiesWithType[compType].Remove(entity);
            }

            RemoveIfEmpty(entity);
        }

        // Remove all components with type
        public void ClearComponent(Type compType)
        {
            if (entitiesWithType.ContainsKey(compType)) {
                var entities = new Queue<Entity>(entitiesWithType[compType]);
                while (entities.Count > 0) {
                    var entity = entities.Dequeue();
                    RemoveComponent(entity, compType);
                }
            }
        }

        public void Clear()
        {
            data.Clear();
            entitiesWithType.Clear();
            nextEntityId = 0;
        }

        public void Remove(Entity entity)
        {
            Dictionary<Type, IComponent> compTable = GetComponents(entity);
            foreach (Type compType in compTable.Keys) {
                entitiesWithType[compType].Remove(entity);
            }

            data.Remove(entity);
        }

        public bool Contains(Entity entity)
        {
            return data.ContainsKey(entity);
        }

        // ---
        // Debug methods

        public static void DebugPrint(Type compType, IComponent iComp)
        {
            Debug.Assert(compType == iComp.GetType());
            dynamic comp = Convert.ChangeType(iComp, compType);
            Console.WriteLine( "    Component: {0}", comp);
        }

        public static void DebugPrint(Dictionary<Type, IComponent> compDict)
        {
            foreach (KeyValuePair<Type, IComponent> pair in compDict) {
                Type compType = pair.Key;
                IComponent iComp = pair.Value;
                DebugPrint(compType, iComp);
            }
        }

        public void DebugPrint()
        {
            foreach (KeyValuePair<Entity, Dictionary<Type, IComponent>> pair in data) {
                Entity entity = pair.Key;
                Dictionary<Type, IComponent> compDict = pair.Value;
                Console.WriteLine("Entity ID: {0}", entity.id);
                DebugPrint(compDict);
            }
        }
    }
}
