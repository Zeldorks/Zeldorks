namespace NetGameShared.Util
{
    public static class Math
    {
        public static int Mod(int n, int m)
        {
            int r = n % m;
            return r < 0 ? r + m : r;
        }
    }
}
