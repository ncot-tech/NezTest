using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NezTest
{
    class RoomManager
    {
        private Room[,] rooms;
        public Point currentRoomCoords { get; private set; }
        Random rng;
        
        public RoomManager()
        {
            rooms = new Room[10, 10];
            rng = new Random();
            rooms[0, 0] = new Room(0,0);
            currentRoomCoords = new Point(0, 0);
        }

        private void GetExitCoord(int exit, int _x, int _y, out int x, out int y)
        {
            switch (exit)
            {
                case 0:         // N
                    x = _x;
                    y = _y - 1;
                    break;
                case 1:         // E
                    x = _x + 1;
                    y = _y;
                    break;
                case 2:         // S
                    x = _x;
                    y = _y + 1;
                    break;
                case 3:         // W
                    x = _x - 1;
                    y = _y;
                    break;
                default:        // Error
                    x = 0;
                    y = 0;
                    break;
            }
        }

        /// <summary>
        /// Returns a bit pattern to represent which exits exist
        /// They go N, E, S, W
        /// </summary>
        /// <returns>a 4 bool array, True means the exit exists</returns>
        public bool[] GetCurrentExits()
        {
            bool[] exits = new bool[] { false, false, false, false };

            for (int i = 0; i < 4; i++)
            {
                int exitDir = rooms[currentRoomCoords.Y, currentRoomCoords.X].CheckValidExit((Exit)i);
                if (exitDir != -1)
                {
                    exits[i] = true;
                }
            }

            return exits;
        }

        public bool ExitRoomToThe(Exit exit)
        {
            bool exitSuccess = false;

            int exitDir = rooms[currentRoomCoords.Y, currentRoomCoords.X].CheckValidExit(exit);
            if (exitDir != -1)
            {
                Point newExits = rooms[currentRoomCoords.Y, currentRoomCoords.X].GetExitEntry(exitDir);
                currentRoomCoords = newExits;
                exitSuccess = true;
            }

            return exitSuccess;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Generate()
        {
            int cellCount = 1;

            int currentX = 0, currentY = 0;
            int nextX = 0, nextY = 0;
            List<int> validCells = new List<int>();

            Debug.WriteLine("Generating map...");

            while (cellCount < 75)
            {
                validCells.Clear();

                // Go through each exit
                for (int i = 0; i < 4; i++)
                {
                    // if it's unused and in bounds, add its co-ords to valid cells
                    GetExitCoord(i, currentX, currentY, out nextX, out nextY);
                    if (nextX > -1 && nextX < 10 && nextY > -1 && nextY < 10)
                    {
                        if (rooms[nextY,nextX] == null)
                        {
                            validCells.Add(nextY * 10 + nextX);
                        }
                    }
                }

                Debug.WriteLine("At co-ords (" + currentX.ToString() + "," + currentY.ToString() + ")");
                Debug.Indent();
                Debug.WriteLine("Found" + validCells.Count.ToString() + " valid exits.");

                if (validCells.Count == 0)
                {
                    // set currentxy to be currentxy.exits[0] (backtrack)
                    int tx = rooms[currentY, currentX].Exits[0].X;
                    int ty = rooms[currentY, currentX].Exits[0].Y;
                    currentX = tx;
                    currentY = ty;
                    Debug.WriteLine("Backtracking to (" + currentX.ToString() + "," + currentY.ToString() + ")");
                    // if currentxy is zero, we are full, break;
                    if (currentX == 0 && currentY == 0)
                    {
                        Debug.WriteLine("Map full, exiting");
                        break;
                    } else
                    {
                        Debug.Unindent();
                        continue;
                    }
                }

                // pick random number between 0 and validcells length
                int dir = rng.Next(0, validCells.Count);
                // add validcells[rand] to currentxy exits
                nextX = validCells[dir] % 10;
                nextY = validCells[dir] / 10;
                rooms[currentY, currentX].AddExit(nextX, nextY);
                // add currentxy to validcells[rand] exits
                rooms[nextY, nextX] = new Room(nextX, nextY);
                rooms[nextY, nextX].AddExit(currentX, currentY);

                Debug.WriteLine("Adding new room at (" + nextX.ToString() + "," + nextY.ToString() + ")");
                Debug.Unindent();

                currentY = nextY;
                currentX = nextX;

                cellCount++;
            }
        }       
    }
}
