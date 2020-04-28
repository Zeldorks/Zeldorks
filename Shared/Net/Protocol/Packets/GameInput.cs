using System;

namespace NetGameShared.Net.Protocol.Packets
{
    // TODO: Split into `Move` and `Attack` inputs
    public class GameInput : IEquatable<GameInput>
    {
        // Type: `NetGameShared.Protocol.GameInput.Movement`
        public int Movement { get; set; }

        public bool UseSlotA { get; set; }
        public bool UseSlotB { get; set; }

        public bool NextItemSlotA { get; set; }
        public bool NextItemSlotB { get; set; }

        public bool PrevItemSlotA { get; set; }
        public bool PrevItemSlotB { get; set; }

        public override int GetHashCode() {
            return
                Movement ^
                UseSlotA.GetHashCode() ^ UseSlotB.GetHashCode() ^
                NextItemSlotA.GetHashCode() ^ NextItemSlotB.GetHashCode() ^
                PrevItemSlotA.GetHashCode() ^ PrevItemSlotB.GetHashCode();
        }

        public override bool Equals(object obj) {
            return Equals(obj as GameInput);
        }

        public bool Equals(GameInput gameInput) {
            return
                gameInput != null &&
                gameInput.Movement == this.Movement &&
                gameInput.UseSlotA == this.UseSlotA &&
                gameInput.UseSlotB == this.UseSlotB &&
                gameInput.NextItemSlotA == this.NextItemSlotA &&
                gameInput.NextItemSlotB == this.NextItemSlotB &&
                gameInput.PrevItemSlotA == this.PrevItemSlotA &&
                gameInput.PrevItemSlotB == this.PrevItemSlotB;
        }

        public GameInput Clone()
        {
            return new GameInput {
                Movement = this.Movement,
                UseSlotA = this.UseSlotA,
                UseSlotB = this.UseSlotB,
                NextItemSlotA = this.NextItemSlotA,
                NextItemSlotB = this.NextItemSlotB,
                PrevItemSlotA = this.PrevItemSlotA,
                PrevItemSlotB = this.PrevItemSlotB
            };
        }
    }
}
