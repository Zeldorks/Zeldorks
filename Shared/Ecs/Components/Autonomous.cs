namespace NetGameShared.Ecs.Components
{
    class Autonomous : IComponent
    {
        public enum Kind
        {
            Basic,
            Flying,
            Charging,
            Shuffling
        }

        public Kind kind = Kind.Basic;
        public Projectile.Kind attackType = Projectile.Kind.Arrow;
        public bool stopWhileAttacking = true;
        public float attackChance = 0;
        public float changePeriod = 1;
        public float moveSpeed = 1;
        public bool spreadShots = false;
    }
}
