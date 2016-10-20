using System;

namespace NezTest
{
    public class Room
    {
        public int[,] Exits;
        int numExits;

        public Room()
        {
            numExits = 0;
            Exits = new int[4,2];
            for (int i = 0; i < 4; i++)
            {
                Exits[i, 0] = -1;
                Exits[i, 1] = -1;
            }
        }

        public void AddExit(int x, int y)
        {
            if (numExits == 4)
                return;
            Exits[numExits, 0] = y;
            Exits[numExits, 1] = x;
            numExits++;
        }
    }
}
