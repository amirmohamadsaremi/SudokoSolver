using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;

public class SudokuSolver
{
    private int n;
    private int[,] board;
    private bool[,] initial;
    private List<int>[,] domain;
    private List<(int, int)> variables;

    public SudokuSolver(int n, int[,] board)
    {
        this.n = n;
        this.board = board;
        this.initial = new bool[n, n];
        this.domain = new List<int>[n, n];
        this.variables = new List<(int, int)>();

        int i = 0;
        int j = 0;
        for (i = 0; i < n; i++)
        {
            for (j = 0; j < n; j++)
            {
                if (board[i, j] != 0)
                {
                    initial[i, j] = true;
                }
                domain[i, j] = new List<int>();
                variables.Add((i, j));
            }
        }

        for ( i = 1; i <= n; i++)
        {
            for (j = 1; j <= n; j++)
            {
                domain[i - 1, j - 1].Add(i);
            }
        }
    }

    public bool Solve()
    {
        if (variables.Count == 0)
        {
            return true;
        }

        // Find variable with minimum remaining values
        (int, int) variable = FindMRVVariable();
        List<int> values = domain[variable.Item1, variable.Item2];

        foreach (int value in values)
        {
            // Check if value satisfies constraints
            if (IsValid(variable, value))
            {
                // Assign value to variable
                board[variable.Item1, variable.Item2] = value;
                variables.Remove(variable);

                // Apply forward checking
                List<(int, int)> removed = ForwardCheck(variable);

                // Recursive call
                if (Solve())
                {
                    return true;
                }

                // Undo assignment and restore domain
                board[variable.Item1, variable.Item2] = 0;
                variables.Add(variable);
                RestoreDomain(removed);
            }
        }

        // No solution found
        return false;
    }

    private (int, int) FindMRVVariable()
    {
        (int, int) result = variables[0];
        int minSize = domain[result.Item1, result.Item2].Count;

        foreach ((int, int) variable in variables)
        {
            int size = domain[variable.Item1, variable.Item2].Count;

            if (size < minSize)
            {
                result = variable;
                minSize = size;
            }
        }

        return result;
    }

    private bool IsValid((int, int) variable, int value)
    {
        // Check row and column constraints
        for (int i = 0; i < n; i++)
        {
            if (board[i, variable.Item2] == value || board[variable.Item1, i] == value)
            {
                return false;
            }
        }

        // Check box constraint
        int boxSize = (int)Math.Sqrt(n);
        int boxRow = variable.Item1 / boxSize;
        int boxCol = variable.Item2 / boxSize;

        for (int i = boxRow * boxSize; i < (boxRow + 1) * boxSize; i++)
        {
            for (int j = boxCol * boxSize; j < (boxCol + 1) * boxSize; j++)
            {
                if (board[i, j] == value)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private List<(int, int)> ForwardCheck((int, int) variable)
    {
        List<(int, int)> removed = new List<(int, int)>();
        // Remove conflicting values from neighbor domains
        foreach ((int, int) neighbor in GetNeighbors(variable))
        {
            if (!initial[neighbor.Item1, neighbor.Item2])
            {
                List<int> values = domain[neighbor.Item1, neighbor.Item2];
                if (values.Remove(board[variable.Item1, variable.Item2]))
                {
                    removed.Add(neighbor);
                }
            }
        }

        return removed;
    }

    private void RestoreDomain(List<(int, int)> removed)
    {
        foreach ((int, int) variable in removed)
        {
            domain[variable.Item1, variable.Item2].Add(board[variable.Item1, variable.Item2]);
        }
    }

    private List<(int, int)> GetNeighbors((int, int) variable)
    {
        List<(int, int)> result = new List<(int, int)>();

        // Row and column neighbors
        for (int i = 0; i < n; i++)
        {
            if (i != variable.Item1)
            {
                result.Add((i, variable.Item2));
            }
            if (i != variable.Item2)
            {
                result.Add((variable.Item1, i));
            }
        }

        // Box neighbors
        int boxSize = (int)Math.Sqrt(n);
        int boxRow = variable.Item1 / boxSize;
        int boxCol = variable.Item2 / boxSize;

        for (int i = boxRow * boxSize; i < (boxRow + 1) * boxSize; i++)
        {
            for (int j = boxCol * boxSize; j < (boxCol + 1) * boxSize; j++)
            {
                if (i != variable.Item1 || j != variable.Item2)
                {
                    result.Add((i, j));
                }
            }
        }

        return result;
    }

    public void PrintBoard()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write(board[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}



