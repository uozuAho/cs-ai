namespace dp.GridWorld
{
    internal readonly struct GridWorldState
    {
        /// <summary>
        /// Position of the agent in the grid world. 0 is 0,0, 15 is 3,3
        /// </summary>
        public int Position1D { get; }

        public bool IsTerminal => Position1D == 0 || Position1D == 15;

        public GridWorldState(int position1D)
        {
            Position1D = position1D;
        }
    }
}