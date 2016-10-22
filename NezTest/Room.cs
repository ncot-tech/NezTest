using System;

namespace NezTest
{
    public class Room
    {
        public int[,] Exits { get; private set; }
        public int numExits { get; private set; }
        int _x, _y;
        public Room(int x, int y)
        {
            _x = x;
            _y = y;
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

        public int[] GetExit(int exitIndex)
        {
            return new int[] { Exits[exitIndex, 1], Exits[exitIndex, 0] };
        }

        public int CheckValidExit(int direction)
        {
            int exit = -1;

            for (int i = 0; i < numExits; i++)
            {
                int edx = Exits[i, 1] - _x;
                int edy = Exits[i, 0] - _y;
                if (edx == 0 && edy == -1 && direction == 0)            // N
                {
                    exit = i;
                    break;
                }
                else if (edx == 1 && edy == 0 && direction == 1)      // E                                                            
                {
                    exit = i;
                    break;
                }
                else if (edx == 0 && edy == 1 && direction == 2)      // S  x                                                         
                {
                    exit = i;
                    break;
                }
                else if (edx == -1 && edy == 0 && direction == 3)     // W                                                            
                {
                    exit = i;
                    break;
                }
            }

            return exit;
        }
    }
}
