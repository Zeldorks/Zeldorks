using System;
using System.Linq;
using System.Text;
using Optional;
using LiteNetLib.Utils;
using System.Collections.Generic;
using static NetGameShared.Util.Math;
using Serialization = NetGameShared.Net.Protocol.Serialization;

namespace NetGameShared.Ecs.Components
{
    public class Inventory : INetComponent<Inventory>
    {
        public class StorageInfo :
            INetSerializable,
            IEquatable<StorageInfo>,
            ICloneable
        {
            private int count;
            public int Count
            {
                get { return count;  }
                set {
                    maxCountOpt.Match(
                        some: maxCount => {
                            count = Math.Clamp(value, 0, maxCount);
                        },
                        none: () => {
                            count = Math.Max(value, 0);
                        }
                    );
                }
            }
            public Option<int> maxCountOpt; // None means infinite count

            public bool IsFull
            {
                get {
                    return maxCountOpt.Match(
                        some: maxCount => { return Count == maxCount; },
                        none: () => { return false; }
                    );
                }
            }

            // ---

            public override int GetHashCode() {
                return count ^ maxCountOpt.GetHashCode();
            }

            public override bool Equals(object obj) {
                return Equals(obj as StorageInfo);
            }

            public bool Equals(StorageInfo storageInfo) {
                return
                    storageInfo != null &&
                    this.count == storageInfo.count &&
                    this.maxCountOpt == storageInfo.maxCountOpt;
            }

            // ---

            public void Serialize(NetDataWriter writer)
            {
                writer.Put(count);
                Serialization.Options.Serialize(writer, maxCountOpt);
            }

            public void Deserialize(NetDataReader reader)
            {
                count = reader.GetInt();
                maxCountOpt = Serialization.Options.DeserializeInt(reader);
            }

            // ---

            public object Clone()
            {
                return new StorageInfo {
                    count = this.count,
                    maxCountOpt = this.maxCountOpt
                };
            }
        }

        public Dictionary<Item.Kind, StorageInfo> data;

        // Duplicates are not allowed (i.e. two slots cannot have the same item
        // kind)
        public List<Option<Item.Kind>> slots;

        // TODO: In a pure ECS, components would have little to no logic
        // implemented. However, this component has many methods implemented.

        public Inventory()
        {
            data = new Dictionary<Item.Kind, StorageInfo>();

            // Initialize two empty slots
            slots = new List<Option<Item.Kind>> {
                Option.None<Item.Kind>(),
                Option.None<Item.Kind>()
            };

            foreach (Item.Kind itemKind in Enum.GetValues(typeof(Item.Kind))) {
                switch (itemKind) {
                    case Item.Kind.Heart:
                        data[itemKind] = new StorageInfo {
                            Count = 1,
                            maxCountOpt = 10.Some()
                        };
                        break;
                    case Item.Kind.Rupee:
                        data[itemKind] = new StorageInfo {
                            Count = 1,
                            maxCountOpt = Option.None<int>()
                        };
                        break;
                    case Item.Kind.Bow:
                    case Item.Kind.Sword:
                    case Item.Kind.RedKey:
                    case Item.Kind.BlueKey:
                    case Item.Kind.GreenKey:
                    case Item.Kind.PurpleKey:
                    case Item.Kind.YellowKey:
                        data[itemKind] = new StorageInfo {
                            Count = 0,
                            maxCountOpt = 1.Some()
                        };
                        break;
                    default:
                        data[itemKind] = new StorageInfo {
                            Count = 0,
                            maxCountOpt = Option.None<int>()
                        };
                        break;
                }
            }
        }

        public Inventory(
            Dictionary<Item.Kind, StorageInfo> data,
            List<Option<Item.Kind>> slots
        ) {
            this.data = data;
            this.slots = slots;
        }

        public bool Contains(Item.Kind itemKind)
        {
            return data[itemKind].Count > 0;
        }

        // Determine whether `itemKind` is an item that can be used (i.e. one
        // that produces an action).
        private static bool IsUsable(Item.Kind itemKind)
        {
            switch (itemKind) {
                case Item.Kind.Compass:
                case Item.Kind.Bow:
                case Item.Kind.Sword:
                case Item.Kind.Bomb:
                case Item.Kind.Boomerang:
                case Item.Kind.Clock:
                case Item.Kind.RedKey:
                case Item.Kind.BlueKey:
                case Item.Kind.GreenKey:
                case Item.Kind.PurpleKey:
                case Item.Kind.YellowKey:
                case Item.Kind.TriforceShard:
                    return true;
                default:
                    return false;
            }
        }

        // Determine whether we can use `itemKind` in a slot based on the state
        // of inventory.
        private bool IsUsableInSlots(Item.Kind itemKind)
        {
            return
                IsUsable(itemKind) &&
                Contains(itemKind) &&
                !slots.Contains(itemKind.Some());
        }

        // Pre-conditions: `IsUsable(itemKind) && Contains(itemKind)`
        private void LoadInEmptySlot(Item.Kind itemKind)
        {
            if (slots.Contains(itemKind.Some())) {
                Console.WriteLine(
                    "[DEBUG] A slot already contains item {0}",
                    itemKind
                );
                return;
            }

            for (int i = 0; i < slots.Count; i++) {
                if (!slots[i].HasValue) {
                    slots[i] = itemKind.Some();
                    Console.WriteLine(
                        "Loaded item {0} in slot {1}",
                        itemKind, i
                    );
                    return;
                }
            }

            Console.WriteLine("[DEBUG] All slots full");
        }

        private void TakeItem(Item.Kind itemKind) {
            data[itemKind].Count++;
            Console.WriteLine(
                "Stored item {0} for a total of {1}",
                itemKind,
                data[itemKind].Count
            );

            if (IsUsable(itemKind)) {
                LoadInEmptySlot(itemKind);
            } else {
                Console.WriteLine("Not usable in slot");
            }
        }

        public bool TryTakeItem(Item.Kind itemKind)
        {
            if (data[itemKind].IsFull) {
                return false;
            } else {
                TakeItem(itemKind);
                return true;
            }
        }

        // Remove from slots if present
        private void RemoveFromSlots(Item.Kind itemKind)
        {
            for (int i = 0; i < slots.Count; i++) {
                bool remove = slots[i].Match(
                    some: slot => { return slot == itemKind; },
                    none: () => { return false; }
                );

                if (remove) {
                    slots[i] = Option.None<Item.Kind>();
                    return;
                }
            }
        }

        public void Remove(Item.Kind itemKind)
        {
            data[itemKind].Count--;
            if (data[itemKind].Count == 0) {
                RemoveFromSlots(itemKind);
            }
        }

        private void ClearSlot(int slot)
        {
            slots[slot] = Option.None<Item.Kind>();
        }

        // Load item in `slot` with the next or previous item in the inventory
        // (determined by `forward`). The next or previous item is based on the
        // original item stored in the slot. If there are no other available
        // items, the original item will remain in the slot.
        //
        // If `slot` is empty, then we chose the first item available.
        //
        // Pre-condition: `this.slots` has index `slot`
        public void LoadItemInSlot(int slot, bool forward)
        {
            // Clear `slot` after saving the original item loaded there. By the
            // end of this method, we will have replaced the item in `slot`. If
            // there are no other item kinds available, the original item kind
            // can be loaded back in.
            Option<Item.Kind> oldItemKindOpt = slots[slot];
            ClearSlot(slot);

            List<Item.Kind> itemKinds = Enum.GetValues(typeof(Item.Kind))
                .Cast<Item.Kind>().ToList();

            int startIndex = oldItemKindOpt.Match(
                some: oldItemKind => {
                    int result = itemKinds.FindIndex(x => x == oldItemKind);
                    result += forward ? 1 : -1;
                    result = Mod(result, itemKinds.Count);
                    return result;
                },
                none: () => { return 0; }
            );

            // Loop through `itemKinds` starting at `startIndex`. If we find an
            // item that we can use, put it into the slot and break from the
            // loop. The boolean `forward` determines whether we loop forwards
            // or backwards.
            int i = startIndex; // Index for `itemKinds`
            for (int j = 0; j < itemKinds.Count; j++) {
                // `j` only ensures that we only loop through the list at most
                // once
                Item.Kind itemKind = itemKinds[i];
                if (IsUsableInSlots(itemKind)) {
                    slots[slot] = itemKind.Some();
                    Console.WriteLine(
                        "Loaded item {0} into slot {1}",
                        itemKind, slot
                    );
                    break;
                }

                i += forward ? 1 : -1;
                i = Mod(i, itemKinds.Count);
            }
        }

        // ---
        // TODO: Can't use `Serialization.Collections` utilities because can't
        // implement `INetSerializable` on `Item.Kind` or `Option<Item.Kind>`.
        // For now we'll just do it manually.

        private void SerializeData(NetDataWriter writer)
        {
            int count = data.Count;
            writer.Put(count);
            foreach (KeyValuePair<Item.Kind, StorageInfo> pair in data) {
                Item.Kind itemKind = pair.Key;
                StorageInfo storageInfo = pair.Value;
                writer.Put((int)pair.Key);
                storageInfo.Serialize(writer);
            }
        }

        private void DeserializeData(NetDataReader reader)
        {
            int count = reader.GetInt();
            data = new Dictionary<Item.Kind, StorageInfo>(count);
            for (int i = 0; i < count; i++) {
                var itemKind = (Item.Kind)reader.GetInt();
                StorageInfo storageInfo = new StorageInfo();
                storageInfo.Deserialize(reader);
                data.Add(itemKind, storageInfo);
            }
        }

        // ---
        // TODO: Would be nice if we could use `Serialization.Options`

        private void SerializeSlots(NetDataWriter writer)
        {
            int count = slots.Count;
            writer.Put(count);
            foreach (Option<Item.Kind> itemKindOpt in slots) {
                itemKindOpt.Match(
                    some: itemKind => {
                        writer.Put(true);
                        writer.Put((int)itemKind);
                    },
                    none: () => {
                        writer.Put(false);
                    }
                );
            }
        }

        private void DeserializeSlots(NetDataReader reader)
        {
            int count = reader.GetInt();
            slots = new List<Option<Item.Kind>>(count);
            for (int i = 0; i < count; i++) {
                bool hasValue = reader.GetBool();
                if (hasValue) {
                    var itemKind = (Item.Kind)reader.GetInt();
                    slots.Add(itemKind.Some());
                } else {
                    slots.Add(Option.None<Item.Kind>());
                }
            }
        }

        // ---

        public void Serialize(NetDataWriter writer)
        {
            SerializeData(writer);
            SerializeSlots(writer);
        }

        public void Deserialize(NetDataReader reader)
        {
            DeserializeData(reader);
            DeserializeSlots(reader);
        }

        // ---

        public override int GetHashCode() {
            return (data, slots).GetHashCode();
        }

        public override bool Equals(object obj) {
            return Equals(obj as Inventory);
        }

        public bool Equals(Inventory inventory) {
            return
                inventory != null &&
                data.SequenceEqual(inventory.data) &&
                slots.SequenceEqual(inventory.slots);
        }

        // ---

        public object Clone()
        {
            // Deep clone
            var data = this.data.ToDictionary(
                pair => pair.Key,
                pair => (StorageInfo)pair.Value.Clone()
            );

            // Deep clone
            var slots = new List<Option<Item.Kind>>(this.slots);

            return new Inventory(data, slots);
        }

        // ---

        private string DataToString() {
            StringBuilder sb = new StringBuilder("data: { ");
            foreach (KeyValuePair<Item.Kind, StorageInfo> pair in data) {
                string key = pair.Key.ToString();
                string value = pair.Value.Count.ToString();
                sb.Append(String.Format("({0}, {1}) ", key, value));
            }
            sb.Append("}");
            return sb.ToString();
        }

        private string SlotsToString() {
            StringBuilder sb = new StringBuilder("slots: [ ");
            foreach (Option<Item.Kind> itemKindOpt in slots) {
                itemKindOpt.Match(
                    some: itemKind => {
                        sb.Append(itemKind.ToString() + " ");
                    },
                    none: () => {
                        sb.Append("<none> ");
                    }
                );
            }
            sb.Append("]");
            return sb.ToString();
        }

        public override string ToString()
        {
            return base.ToString() + ": {\n" +
                "    " + DataToString() + "\n" +
                "    " + SlotsToString() + "\n}";
        }
    }
}
