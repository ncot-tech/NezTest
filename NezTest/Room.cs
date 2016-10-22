using System;
using Microsoft.Xna.Framework;
namespace NezTest
{
    public enum Exit { NORTH, EAST, SOUTH, WEST };

    public class Room
    {
        public Point[] Exits { get; private set; }
        public int numExits { get; private set; }
        int _x, _y;
        public Room(int x, int y)
        {
            _x = x;
            _y = y;
            numExits = 0;
            Exits = new Point[4];
            for (int i = 0; i < 4; i++)
            {
                Exits[i].X = -1;
                Exits[i].Y = -1;
            }
        }

        public void AddExit(int x, int y)
        {
            if (numExits == 4)
                return;
            Exits[numExits].Y = y;
            Exits[numExits].X = x;
            numExits++;
        }

        public Point GetExitEntry(int exitIndex)
        {
            return Exits[exitIndex];
        }

        public int CheckValidExit(Exit direction)
        {
            int exit = -1;

            for (int i = 0; i < numExits; i++)
            {
                int edx = Exits[i].X - _x;
                int edy = Exits[i].Y - _y;
                if (edx == 0 && edy == -1 && direction == Exit.NORTH)            // N
                {
                    exit = i;
                    break;
                }
                else if (edx == 1 && edy == 0 && direction == Exit.EAST)      // E                                                            
                {
                    exit = i;
                    break;
                }
                else if (edx == 0 && edy == 1 && direction == Exit.SOUTH)      // S  x                                                         
                {
                    exit = i;
                    break;
                }
                else if (edx == -1 && edy == 0 && direction == Exit.WEST)     // W                                                            
                {
                    exit = i;
                    break;
                }
            }

            return exit;
        }
    }
}
