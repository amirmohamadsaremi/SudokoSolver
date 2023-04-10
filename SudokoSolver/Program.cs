// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


int n = 9;
int[,] board = {
    {0, 2, 6, 0, 0, 0, 8, 1, 0},
    {3, 0, 0, 7, 0, 8, 0, 0, 6},
    {4, 0, 0, 0, 5, 0, 0, 0, 7},
    {0, 5, 0, 1, 0, 7, 0, 9, 0},
    {0, 0, 3, 9, 0, 5, 1, 0, 0},
    {0, 4, 0, 3, 0, 2, 0, 5, 0},
    {1, 0, 0, 0, 3, 0, 0, 0, 2},
    {5, 0, 0, 2, 0, 4, 0, 0, 9} ,
    {0, 3, 8, 0, 0, 0, 4, 6, 0}
};

SudokuSolver solver = new SudokuSolver(n, board);
if (solver.Solve())
{
    solver.PrintBoard();
}
else
{
    Console.WriteLine("No solution found.");
}
