using System;
using Microsoft.Maui.Controls;

namespace MobiileApp;

public partial class TicTacToePage : ContentPage
{
    private string currentPlayer = "X"; // Initially Player X
    private string[,] gameBoard;
    private bool gameInProgress = true;
    private Random random = new Random();
    private int gridSize = 3; // Default grid size
    private string playerColor = "Black"; // Default color for player X
    private string botColor = "Gray"; // Default color for bot O
    private bool isBotTurn = false; // To keep track if it's bot's turn

    public TicTacToePage(int k)
    {
        InitializeComponent();
        ResetGameBoard();
    }

    // Handler for grid size selection
    private void SizePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedSize = sizePicker.SelectedItem.ToString();
        switch (selectedSize)
        {
            case "3x3":
                gridSize = 3;
                break;
            case "4x4":
                gridSize = 4;
                break;
            case "5x5":
                gridSize = 5;
                break;
        }
        ResetGameBoard();
    }

    // Reset game logic and recreate the grid dynamically
    private void ResetGameBoard()
    {
        gameBoard = new string[gridSize, gridSize];
        gameInProgress = true;
        currentPlayer = "X";

        // Clear existing buttons and redefine grid
        gameGrid.Children.Clear();
        gameGrid.RowDefinitions.Clear();
        gameGrid.ColumnDefinitions.Clear();

        for (int i = 0; i < gridSize; i++)
        {
            gameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            gameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        // Create new buttons for the grid cells
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                var button = new Button
                {
                    FontSize = 24,
                    WidthRequest = 80,
                    HeightRequest = 80,
                    BackgroundColor = Color.LightGray,
                    StyleId = $"Cell{i}{j}"
                };
                button.Clicked += Cell_Click;
                gameGrid.Children.Add(button, j, i);
            }
        }
    }

    // Handle cell click
    private void Cell_Click(object sender, EventArgs e)
    {
        if (!gameInProgress) return;

        var button = sender as Button;
        var cellName = button?.StyleId;
        int row = int.Parse(cellName[4].ToString());
        int col = int.Parse(cellName[5].ToString());

        // Skip if the cell is already filled
        if (gameBoard[row, col] != null) return;

        // Place the player's mark and update game board
        gameBoard[row, col] = currentPlayer;
        button.Text = currentPlayer;
        button.TextColor = currentPlayer == "X" ? Color.Blue : Color.Red;

        // Check for winner
        if (CheckWinner())
        {
            DisplayAlert("Winner", $"{currentPlayer} wins!", "OK");
            gameInProgress = false;
            return;
        }

        // Switch turns
        currentPlayer = currentPlayer == "X" ? "O" : "X";

        // If it's the bot's turn, make a move
        if (currentPlayer == "O")
            BotMove();
    }

    // Bot move logic (random move)
    private void BotMove()
    {
        isBotTurn = true;
        int row, col;
        do
        {
            row = random.Next(gridSize);
            col = random.Next(gridSize);
        } while (gameBoard[row, col] != null); // Ensure the cell is empty

        gameBoard[row, col] = "O";
        var button = gameGrid.Children[row * gridSize + col] as Button;
        button.Text = "O";
        button.TextColor = Color.Red;

        if (CheckWinner())
        {
            DisplayAlert("Winner", "Bot wins!", "OK");
            gameInProgress = false;
        }
        else
        {
            currentPlayer = "X"; // Switch back to player X
        }
        isBotTurn = false;
    }

    // Check for winner
    private bool CheckWinner()
    {
        // Check rows, columns, and diagonals for a win
        for (int i = 0; i < gridSize; i++)
        {
            if (CheckLine(i, 0, 0, 1)) return true; // Check row
            if (CheckLine(0, i, 1, 0)) return true; // Check column
        }

        if (CheckLine(0, 0, 1, 1)) return true; // Check main diagonal
        if (CheckLine(0, gridSize - 1, 1, -1)) return true; // Check anti diagonal

        return false;
    }

    // Check a line (row, column, diagonal)
    private bool CheckLine(int startX, int startY, int deltaX, int deltaY)
    {
        string first = gameBoard[startX, startY];
        if (string.IsNullOrEmpty(first)) return false;

        for (int i = 1; i < gridSize; i++)
        {
            int x = startX + i * deltaX;
            int y = startY + i * deltaY;

            if (gameBoard[x, y] != first)
                return false;
        }
        return true;
    }

    // Handle "New Game" button click
    private void NewGameButton_Click(object sender, EventArgs e)
    {
        ResetGameBoard();
    }

    // Handle "Choose First Player" button click
    private async void ChooseFirstPlayerButton_Click(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Who goes first?", "Cancel", null, "Player X", "Bot (O)");
        if (action == "Bot (O)")
        {
            currentPlayer = "O";
            BotMove();
        }
        else
        {
            currentPlayer = "X";
        }
    }

    // Display game rules
    private async void ShowRulesButton_Click(object sender, EventArgs e)
    {
        await DisplayAlert("Game Rules", "The goal is to get three marks in a row. Players take turns marking their spots. The first to get three marks in a row wins!", "OK");
    }

    // Change background theme
    private void ChangeBackgroundButton_Click(object sender, EventArgs e)
    {
        this.BackgroundColor = new Color(random.NextDouble(), random.NextDouble(), random.NextDouble());
    }
}
