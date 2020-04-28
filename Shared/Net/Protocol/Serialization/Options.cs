using LiteNetLib.Utils;
using Optional;

namespace NetGameShared.Net.Protocol.Serialization
{
    // TODO: This class is unused, but may be used in the future.
    public static class Options
    {
        // TODO: Use `NetPacketProcessor.RegisterNestedType` on these?

        public static void Serialize<T>(
            NetDataWriter writer,
            Option<T> option
        ) {
            option.Match(
                some: t => {
                    writer.Put(true);
                    // More cases can be added as needed
                    switch (t) {
                        case INetSerializable tx:
                            tx.Serialize(writer);
                            break;
                        case int tx:
                            writer.Put(tx);
                            break;
                    }
                },
                none: () => {
                    writer.Put(false);
                }
            );
        }

        public static Option<T> Deserialize<T>(NetDataReader reader)
            where T : INetSerializable, new()
        {
            bool hasValue = reader.GetBool();
            if (hasValue) {
                T t = new T();
                t.Deserialize(reader);
                return t.Some();
            } else {
                return Option.None<T>();
            }
        }

        // TODO: Figure a way to reduce code duplication
        // More overloaded methods can be added as needed

        public static Option<int> DeserializeInt(NetDataReader reader)
        {
            bool hasValue = reader.GetBool();
            if (hasValue) {
                int t = reader.GetInt();
                return t.Some();
            } else {
                return Option.None<int>();
            }
        }
    }
}
