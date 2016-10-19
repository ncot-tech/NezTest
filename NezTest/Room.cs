using System;

namespace NezTest
{
    public class Room
    {
        public Tuple<int, int>[] Exits;

        public Room()
        {
            Exits = new Tuple<int, int>[4];
            for (int i = 0; i < 4; i++)
            {
                Exits[i] = new Tuple<int, int>(0, 0);
            }
        }

        public void SetExit(int exit, int x, int y)
        {
            Exits[exit] = new Tuple<int, int>(x, y);
        }

        public Tuple<int,int> GetExit(int exit)
        {
            return Exits[exit];
        }
    }
}
