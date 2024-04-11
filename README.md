First enter the Maze size:

![image](https://github.com/NateAC/Recursive-Backtracker-Algorithm/assets/97734863/d1641815-86db-456f-842a-f92b6594251e)

Then the code generates the maze and visits the current cell, picking a random direction to move each time. (Starts at 0,0 which is top-left)

![image](https://github.com/NateAC/Recursive-Backtracker-Algorithm/assets/97734863/5cb72205-7f76-40d5-b77f-1b609c731214)

Until it arrives at a cell with only visited neighbours, backtracking to a valid cell.

![image](https://github.com/NateAC/Recursive-Backtracker-Algorithm/assets/97734863/9f57eab6-517b-49c0-afb0-1129575a9182)

Each cell starts with a value of 15, full walls, and with each visit the total is reduced depending on the direction:

grid[x, y] = 15; // 1 (West) + 2 (East) + 4 (North) + 8 (South) = 15

![image](https://github.com/NateAC/Recursive-Backtracker-Algorithm/assets/97734863/9acff540-7d9c-4e9e-af41-44d447bf34de)

![image](https://github.com/NateAC/Recursive-Backtracker-Algorithm/assets/97734863/0611936e-ed63-4647-99ef-d50151c3197f)
