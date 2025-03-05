using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace MobiileApp;

public partial class TicTacToePage : ContentPage
{
    private string currentPlayer = "X"; // Начальный игрок
    private string[,] gameBoard;
    private bool gameInProgress = true;
    private Random random = new Random();
    private int gridSize = 3; // Начальный размер поля
    private bool isBotTurn = false; // Для отслеживания хода бота
    private int playerWins = 0;
    private int botWins = 0;
    private int draws = 0;
    private Stack<(int row, int col, string player)> moveHistory = new Stack<(int, int, string)>();

    public TicTacToePage(int k)
    {
        InitializeComponent();
        ResetGameBoard();
        

    }

    private void NewGameButton_Click(object sender, EventArgs e)
    {
        ResetGameBoard(); // Сбрасываем игровое поле
    }

    // Обработчик изменения размера поля
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

    // Обработчик выбора режима игры
    private void GameModePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedMode = gameModePicker.SelectedItem.ToString();
       
        if (selectedMode == "Mängija vs Mängija")
        {
            isBotTurn = false;  // Указываем, что ходит бот
        }
        else
        {
            isBotTurn = true; // Для игры "Игрок против игрока"
        }
        
        ResetGameBoard();
    }

    // Сброс игры и создание новой сетки
    private void ResetGameBoard()
    {
        gameBoard = new string[gridSize, gridSize];
        gameInProgress = true;
        currentPlayer = "X";  // Начинаем с игрока X
        currentPlayerLabel.Text = $"Hetkel mängib: {currentPlayer}";

        // Очистить старые кнопки и пересоздать сетку
        gameGrid.Children.Clear();
        gameGrid.RowDefinitions.Clear();
        gameGrid.ColumnDefinitions.Clear();

        for (int i = 0; i < gridSize; i++)
        {
            gameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            gameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        // Создание новых кнопок для клеток
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                var button = new Button
                {
                    FontSize = 24,
                    WidthRequest = 80,
                    HeightRequest = 80,
                    BackgroundColor = Colors.LightGray,
                    StyleId = $"Cell{i}{j}"
                };
                button.Clicked += Cell_Click;
                gameGrid.Children.Add(button); // Добавить кнопку
                Grid.SetRow(button, i);         // Установить строку
                Grid.SetColumn(button, j);      // Установить колонку

                // Добавление разделительных линий между ячейками
                button.BorderColor = Colors.Black;
                button.BorderWidth = 1;
            }
        }

        // Если выбран режим с ботом, то бот должен сделать первый ход
        if (isBotTurn)
        {
            BotMove(); // Бот делает первый ход
        }
    }

    // Обработчик клика по клетке
    private void Cell_Click(object sender, EventArgs e)
    {
        if (!gameInProgress) return;

        var button = sender as Button;
        var cellName = button?.StyleId;
        int row = int.Parse(cellName[4].ToString());
        int col = int.Parse(cellName[5].ToString());

        // Если клетка уже занята, выходим
        if (gameBoard[row, col] != null) return;

        // Размещение символа и обновление игрового поля
        gameBoard[row, col] = currentPlayer;
        button.Text = currentPlayer;
        button.TextColor = currentPlayer == "X" ? Colors.Blue : Colors.Red;

        // Сохранение хода для отмены
        moveHistory.Push((row, col, currentPlayer));

        // Проверка на победителя
        if (CheckWinner())
        {
            DisplayAlert("Võitja", $"{currentPlayer} võitis!", "OK");
            gameInProgress = false;
            UpdateScore(currentPlayer);
            AskForNewGame();
            return;
        }

        // Проверка на ничью
        if (IsBoardFull())
        {
            DisplayAlert("Tulemused", "Mäng lõppes viigiga!", "OK");
            gameInProgress = false;
            draws++; // Увеличиваем статистику ничьей
            UpdateScore("Draw");
            AskForNewGame();
            return;
        }

        // Смена игрока
        currentPlayer = currentPlayer == "X" ? "O" : "X";
        currentPlayerLabel.Text = $"Hetkel mängib: {currentPlayer}";

        // Если ходит бот, делаем его ход
        if (currentPlayer == "O")
        {
            BotMove();
        }
    }


    // Проверка ничьей
    private bool CheckDraw()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (gameBoard[i, j] == null)
                {
                    return false; // Если есть хотя бы одна пустая клетка, ничья невозможна
                }
            }
        }
        return true; // Если пустых клеток нет, значит ничья
    }

    // Логика хода бота (случайный ход)
    private void BotMove()
    {
        isBotTurn = true;
        int row, col;

        // Пока очередь бота, он продолжает ходить
        do
        {
            row = random.Next(gridSize);
            col = random.Next(gridSize);
        } while (gameBoard[row, col] != null); // Убедитесь, что клетка пуста

        // Бот ставит O
        gameBoard[row, col] = "O";
        var button = gameGrid.Children[row * gridSize + col] as Button;
        button.Text = "O";
        button.TextColor = Colors.Red;

        // Проверка на победителя
        if (CheckWinner())
        {
            DisplayAlert("Võitja", "Bot võitis!", "OK");
            gameInProgress = false;
            UpdateScore("O");
            AskForNewGame();
        }
        else if (IsBoardFull()) // Если поле заполнено и нет победителя, ничья
        {
            DisplayAlert("Tulemused", "Mäng lõppes viigiga!", "OK");
            gameInProgress = false;
            draws++; // Увеличиваем статистику ничьей
            UpdateScore("Draw");
            AskForNewGame();
        }
        else
        {
            // После хода бота, переключаем очередь на игрока
            currentPlayer = "X"; // Теперь ход игрока X
            currentPlayerLabel.Text = $"Hetkel mängib: {currentPlayer}";
        }

        isBotTurn = false; // Завершаем ход бота
    }

    private bool IsBoardFull()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (gameBoard[i, j] == null)
                {
                    return false; // Если есть пустая клетка, то не ничья
                }
            }
        }
        return true; // Если нет пустых клеток, то ничья
    }

    // Проверка победителя
    private bool CheckWinner()
    {
        for (int i = 0; i < gridSize; i++)
        {
            if (CheckLine(i, 0, 0, 1)) return true; // Проверка по строкам
            if (CheckLine(0, i, 1, 0)) return true; // Проверка по колонкам
        }

        if (CheckLine(0, 0, 1, 1)) return true; // Проверка по диагонали
        if (CheckLine(0, gridSize - 1, 1, -1)) return true; // Проверка по обратной диагонали

        return false;
    }

    // Проверка линии (строка, колонка, диагональ)
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

    // Обновление статистики побед
    private void UpdateScore(string winner)
    {
        if (winner == "X") playerWins++;
        else if (winner == "O") botWins++;
        else draws++;

        // Обновление UI
        playerWinsLabel.Text = $"Mängija X: {playerWins}";
        botWinsLabel.Text = $"Bot O: {botWins}";
        drawsLabel.Text = $"Nõrk: {draws}";
    }

    // Запрос на новую игру
    private async void AskForNewGame()
    {
        var result = await DisplayAlert("Mäng Lõppenud", "Kas soovite mängida uuesti?", "Jah", "Ei");
        if (result)
        {
            ResetGameBoard();
        }
    }

    // Смена фона
    private void ChangeBackgroundButton_Click(object sender, EventArgs e)
    {
        this.BackgroundColor = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
    }

    // Показать правила игры
    private async void ShowRulesButton_Click(object sender, EventArgs e)
    {
        await DisplayAlert("Mängureeglid", "Eesmärk on saada kolm järjestikust markeeringut. Mängijad vahetavad käike. Esimene, kes saab kolm järjestikust markeeringut, võidab!", "OK");
    }

    // Выбор первого игрока
    private async void ChooseFirstPlayerButton_Click(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Kes alustab?", "Tühista", null, "Mängija X", "Bot (O)");
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

    // Отмена последнего хода
    private void UndoMoveButton_Click(object sender, EventArgs e)
    {
        if (moveHistory.Count == 0) return;

        // Восстановление последнего хода
        var lastMove = moveHistory.Pop();
        int row = lastMove.row;
        int col = lastMove.col;
        gameBoard[row, col] = null;

        var button = gameGrid.Children[row * gridSize + col] as Button;
        button.Text = "";
        button.TextColor = Colors.Black;

        // Смена игрока
        currentPlayer = currentPlayer == "X" ? "O" : "X";
        currentPlayerLabel.Text = $"Hetkel mängib: {currentPlayer}";
    }
}
