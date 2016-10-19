using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NezTest
{
    class RoomManager
    {
        Room[,] rooms;
        Random rng;

        public RoomManager()
        {
            rooms = new Room[10, 10];
            rng = new Random();
        }

        public void Generate()
        {
            bool finished = false;

            int currentX = 0, currentY = 0;
            int nextX, nextY;

            while (!finished)
            {
                ChooseNextCell(currentX, currentY, out nextX, out nextY);
                currentX += nextX;
                currentY += nextY;
            }

            

        }

        /// <summary>
        /// Works out the next cell to use
        /// </summary>
        /// <param name="curX"></param>
        /// <param name="curY"></param>
        /// <param name="nextX"></param>
        /// <param name="nextY"></param>
        private bool ChooseNextCell(int curX, int curY, out int nextX, out int nextY)
        {
            int dir = rng.Next(0, 3);

            switch (dir)
            {
                case 0:         // North
                    nextX = 0;
                    nextY = -1;
                    break;
                case 1:         // East
                    nextX = 1;
                    nextY = 0;
                    break;
                case 2:         // South
                    nextX = 0;
                    nextY = 1;
                    break;
                case 3:         // West
                    nextX = 0;
                    nextY = -1;
                    break;
                // This is only here to shut the compiler up
                // Logically we'll never get here
                default:
                    nextX = 0;
                    nextY = 0;
                    break;

            }

            // Check if going there is out of bounds

            // Check if going there has already been used



            return false;
        }
    }
}
