/*********************************************************************************
 * Maze Algorithm NEA Project                                                    *
 * Author: Nathan Cox                                                            *
 * Date: 03/04/2023                                                              *
 * Version: 1.0                                                                  *
 *                                                                               *
 * Description:                                                                  *
 *   A program to build a maze which navigates starts from a random point in a   *
 *   grid of predetermined frame size, using a recursive backtracker algorithm.  *
 *                                                                               *
 *   As is recurses through the maze it gives a commentary of the route it is    *
 *   taking and the tasks it is performing such as, removing walls, looking in   *
 *   a direction.  Once it has visited every cell in the grid it prints out      *
 *   each cell in a 4x4 matrix using ASCII characters.  ASCII was used for       *
 *   portability between different operating system compilers but graphics would *
 *   look better!                                                                *
 *                                                                               *
 * Improvements:                                                                 *
 *   1) Add input to allow the user to define the maze size                      *
 *   2) Improve the logic checking when backtracking as sometimes it exceeds     *
 *      the maze boundary and throws an error                                    *
 *   3) Add a index number to each cell so that I can plot the actual route      *
 *      on the screen for the user to see where it went.                         *
 *   4) Change to a graphical interface such as WPF, although I think just       *
 *      tweaking the ASCII grid size would improve it, 5x4 or 4x3 maybe?         *
 *                                                                               *
 *********************************************************************************/

using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;

class Maze
{
    private int width;
    private int height;
    private int[,] grid;

    private Random random = new Random();

    private int[,] notvisited;

    public Maze(int width, int height)
    {
        this.width = width;
        this.height = height;
        grid = new int[width, height];

        int xvisited = width;
        int yvisited = height;
        notvisited = new int[xvisited, yvisited];

        for (int x = 0; x < width; x++) // Recurse through the maze marking all cells with four walls.
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = 15; // 1 (West) + 2 (East) + 4 (North) + 8 (South) = 15
            }
        }

        for (int x = 0; x < width; x++) // Recurse through the maze marking all cells as not visited, i.e. set them to 1.
        {
            for (int y = 0; y < height; y++)
            {
                notvisited[x, y] = 1;
            }
        }

        Console.WriteLine("Backtracking");
        backtrack(random.Next(0, width), random.Next(0, height)); //Start backtracking from a random part of the maze.
    }

    private void backtrack(int x, int y)
    {
        // Original
        // 0 = West
        // 2 = North
        // 4 = East
        // 6 = South
        int[] neighbours = new int[] { 0, 2, 4, 6 }; // Creates an array of integers called neighbours that contains the values 0, 2, 4, and 6. These values represent the directions East, West, North, and South respectively.
        int[] walls = new int[] { 1, 0, 2, 0, 4, 0, 8 }; // Add walls where 1 = West, 2 = North, 4 = East, 8 =South
        string[] compass = new string[] { "West", "Null", "North", "Null", "East", "Null", "South" }; // Labels
        int shuffleconfirmed = 1; // Used for dialog to know if it is iterating through the neighbours!
        Shuffle(neighbours, x, y); // Shuffle / random the neighbours using the method below.
        foreach (int direction in neighbours)
        {
            // nx = new x position
            // ny = new y position
            int nx = x + (direction == 4 ? 1 : direction == 0 ? -1 : 0); //The expression direction == 4 ? 1 : direction == 0 ? -1 : 0 checks if direction is East. If it is, then add 1 to the x axis.
                                                                         //Otherwise, it checks if direction is West. If it is, then subtract 1 from the x axis. Otherwise, the value of the expression is 0, don't move on the x axis.
            int ny = y + (direction == 6 ? 1 : direction == 2 ? -1 : 0); //The expression direction == 6 ? 1 : direction == 0 ? -1 : 0 checks if direction is South.  If it is, then add 1 to the y axis.
                                                                         //Otherwise, it checks if direction is North.  If it is, then subtract 1 from the y axis. Otherwise, the value of the expression is 0, don't move on the y axis.
            if (nx >= 0 && nx < width && ny >= 0 && ny < height && notvisited[nx, ny] == 1) // Check if the new position is within the bounds of the maze and cell has not been visited before (grid[x,y] = 1 equals not visited).
            {
                notvisited[nx, ny] = 0; // Mark the new grid position as visited. I think this would be better as a boolean, but I'll leave it for now!
                grid[x, y] = grid[x, y] - walls[direction]; // Mark the current grid position with the direction travelled.
                grid[nx, ny] = grid[nx, ny] - walls[(direction ^ 4)];   // XOR the direction and record it in the new position.  The purpose of this operation is to create a path between the current position and
                                                                        // the new position in the maze. The value of direction represents the direction from the current position to the new position (i.e., East, West, North, or South).
                                                                        // The expression (direction ^ 4) represents the opposite direction (i.e., if direction is East (4), then (direction ^ 4) is West (0)).

                Console.WriteLine("          MARKED " + x + "," + y + " VISITED: Heading " + compass[direction] + " to cell (" + nx + "," + ny + ") because that is not visited - removing " + compass[direction] + " and " + compass[direction ^ 4] + " walls");

                backtrack(nx, ny);
                if (shuffleconfirmed > 0)
                {
                    Console.WriteLine("         DEADEND " + nx + "," + ny + " BACKTRACKING TO " + x + "," + y);
                    //                    Console.WriteLine("     GRID BEFORE " + grid[nx, ny]);
                    //                  grid[nx, ny] = grid[nx, ny] + walls[direction]; // Add the wall in the direction we were going and then backtrack.
                    //                    grid[x, y] = grid[x, y] + walls[direction ^ 4]; // Add the wall in the direction we were going and then backtrack.
                    //                    grid[nx, ny] = 0;
                    //                    Console.WriteLine("      GRID AFTER " + grid[nx, ny]);
                }
            }
            else if (nx >= 0 && nx < width && ny >= 0 && ny < height && notvisited[nx, ny] == 0 && shuffleconfirmed > 1)
            {
                notvisited[x, y] = 0; // Mark the current grid position as visited. I think this would be better as a boolean, but I'll leave it for now!
                                      //                grid[x, y] = grid[x, y] - walls[direction]; // Mark the current grid position with the direction travelled.
                Console.WriteLine("          MARKED " + x + "," + y + " VISITED: Facing " + compass[direction] + " to cell (" + nx + "," + ny + ") but need to try new neighbour as I've been there");
                //                Console.Write("                     ");
            }
            shuffleconfirmed++;
        }
    }

    private void Shuffle(int[] array, int x, int y) // Pick a random direction from the cell coordinates.
                                                    // It will only come back with the values for North, East, South and West from the neighbours array.
    {
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            int r = i + random.Next(n - i);
            int t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }

    public void Print()
    {
        // The console builds a four by four cell using ASCII characters.  Console instead of WPF give me a little more portability, so in theory I should be able to compile on Linux.
        string[] cellwallsrow1 = new string[] { "    ", "█   ", "████", "████", "   █", "█  █", "████", "████", "    ", "█   ", "████", "████", "   █", "█  █", "████", "████" };
        string[] cellwallsrow2 = new string[] { "    ", "█   ", "    ", "█   ", "   █", "█  █", "   █", "█  █", "    ", "█   ", "    ", "█   ", "   █", "█  █", "   █", "█  █" };
        string[] cellwallsrow3 = new string[] { "    ", "█   ", "    ", "█   ", "   █", "█  █", "   █", "█  █", "    ", "█   ", "    ", "█   ", "   █", "█  █", "   █", "█  █" };
        string[] cellwallsrow4 = new string[] { "    ", "█   ", "    ", "█   ", "   █", "█  █", "   █", "█  █", "████", "████", "████", "████", "████", "████", "████", "████" };

        Console.WriteLine("THE WALL CALCULATIONS FOR THE GRID ARE:");
        Console.Write(" ");
        for (int x = 0; x < width; x++)
        {
            Console.Write(string.Format("{0,5}", x));
        }
        Console.Write("\r\n  ┌────");
        for (int x = 0; x < width - 1; x++)
        {
            Console.Write("┬────");
        }
        Console.WriteLine("┐");
        for (int y = 0; y < height; y++)
        {
            Console.Write(y + " ");
            for (int x = 0; x < width; x++)
            {
                Console.Write("│ " + string.Format("{0,2}", grid[x, y]) + " ");
            }
            Console.WriteLine("│");
            if (y != height)
            {
                Console.Write("  ├────");
                for (int x = 0; x < width - 1; x++)
                {
                    Console.Write("┼────");
                }
                Console.WriteLine("┤");
            }
        }
        Console.Write("  └────");
        for (int x = 0; x < width - 1; x++)
        {
            Console.Write("┴────");
        }
        Console.WriteLine("┘");

        Console.WriteLine("WHICH LOOK LIKE THIS WHEN DRAWN AS A MAZE:");
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(cellwallsrow1[grid[x, y]]);
            }
            Console.WriteLine();
            for (int x = 0; x < width; x++)
            {
                Console.Write(cellwallsrow2[grid[x, y]]);
            }
            Console.WriteLine();
            for (int x = 0; x < width; x++)
            {
                Console.Write(cellwallsrow3[grid[x, y]]);
            }
            Console.WriteLine();
            for (int x = 0; x < width; x++)
            {
                Console.Write(cellwallsrow4[grid[x, y]]);
            }
            Console.WriteLine();
        }
    }

}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the maze size: ");
        int size = Convert.ToInt32(Console.ReadLine());
        if (size < 10)
            size = 10;
        Maze maze = new(size, size);
        maze.Print();
    }
}
