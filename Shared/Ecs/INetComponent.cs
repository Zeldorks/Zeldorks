using System;
using LiteNetLib.Utils;

namespace NetGameShared.Ecs
{
    // `T` will generally refer to the implementing type
    public interface INetComponent<T> :
        IComponent,
        INetSerializable,
        ICloneable,
        IEquatable<T>
    {
        // Empty
    }
}
