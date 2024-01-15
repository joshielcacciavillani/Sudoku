using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sudoku.Pages
{
    public class SudokuModel : PageModel
    {
        public int[][] SudokuPuzzle { get; set; }

        public void OnGet()
        {
            // Initialize a Random object
            Random random = new Random();

            // Generate a valid Sudoku puzzle using backtracking with optimizations
            SudokuPuzzle = new int[9][];
            for (int i = 0; i < 9; i++)
            {
                SudokuPuzzle[i] = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    SudokuPuzzle[i][j] = 0; // Initialize with zeros
                }
            }

            GenerateSudoku(SudokuPuzzle, random);
        }

        private bool GenerateSudoku(int[][] puzzle, Random random)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Shuffle(nums, random);

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (puzzle[row][col] == 0)
                    {
                        foreach (int num in nums)
                        {
                            if (IsSafe(puzzle, row, col, num))
                            {
                                puzzle[row][col] = num;

                                if (GenerateSudoku(puzzle, random))
                                    return true;

                                puzzle[row][col] = 0; // Backtrack if current num doesn't lead to a solution
                            }
                        }

                        // If no number is valid, backtrack to the previous cell
                        return false;
                    }
                }
            }

            return true; // Successfully filled the puzzle
        }

        private bool IsSafe(int[][] puzzle, int row, int col, int num)
        {
            // Check if num is not already present in the current row, current column, and current 3x3 subgrid
            return !UsedInRow(puzzle, row, num) && !UsedInCol(puzzle, col, num) && !UsedInSubgrid(puzzle, row - row % 3, col - col % 3, num);
        }

        private bool UsedInRow(int[][] puzzle, int row, int num)
        {
            for (int col = 0; col < 9; col++)
            {
                if (puzzle[row][col] == num)
                    return true;
            }
            return false;
        }

        private bool UsedInCol(int[][] puzzle, int col, int num)
        {
            for (int row = 0; row < 9; row++)
            {
                if (puzzle[row][col] == num)
                    return true;
            }
            return false;
        }

        private bool UsedInSubgrid(int[][] puzzle, int startRow, int startCol, int num)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (puzzle[row + startRow][col + startCol] == num)
                        return true;
                }
            }
            return false;
        }

        private void Shuffle(int[] array, Random random)
        {
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
    }
}



