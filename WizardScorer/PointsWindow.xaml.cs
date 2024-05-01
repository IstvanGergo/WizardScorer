using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WizardScorer.Models;

namespace WizardScorer
{
    /// <summary>
    /// Interaction logic for PointsWindow.xaml
    /// </summary>
    public partial class PointsWindow : Window
    {
        public PointsWindow()
        {
            InitializeComponent();
            CreatePointsTable();
            FillPointsTable();
        }
        private void CreatePointsTable()
        {
            int numPlayers = ((SelectPlayersWindow)Application.Current.MainWindow).NumPlayers;
            CreateColumn();
            for (int i = 0; i < numPlayers; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    CreateColumn();
                }
                AddPlayerToGrid(i);
                CreateSubColumn("Pont", i * 3,1);
                CreateSubColumn("Jóslat", i * 3 + 1,1);
                CreateSubColumn("Vitt Ütés", i * 3 + 2, 1);
            }
            for (int j = 1; j <= (60 / numPlayers); j++)
            {
                CreateNewRow(j);
            }
        }
        private void CreateColumn()
        {
            ColumnDefinition columnDef = new()
            {
                Width = new GridLength(100, GridUnitType.Auto)
            };
            PointsGrid.ColumnDefinitions.Add(columnDef);
        }
        private void AddPlayerToGrid(int Playernum)
        {
            TextBlock playerName = new()
            {
                Text = ((SelectPlayersWindow)Application.Current.MainWindow).SelectedPlayers[Playernum].PlayerName,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(30, 0, 30, 0)
            };
            Border playerborder = new()
            {
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Colors.Black)
            };
            Grid.SetColumn(playerborder, Playernum * 3 + 1);
            Grid.SetColumnSpan(playerborder, 3);
            PointsGrid.Children.Add(playerborder);
            Grid.SetColumn(playerName, Playernum * 3 + 1);
            Grid.SetColumnSpan(playerName, 3);
            PointsGrid.Children.Add(playerName);
        }
        private void CreateSubColumn(string ColumnName, int ColPosition, int RowPosition)
        {
            Border border = new()
            {
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Colors.Black)
            };
            TextBlock textblock = new()
            {
                Text = ColumnName,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(10, 0, 10, 0)
            };
            Grid.SetRow(border, RowPosition);
            Grid.SetColumn(border, ColPosition + 1);
            PointsGrid.Children.Add(border);
            Grid.SetRow(textblock, RowPosition);
            Grid.SetColumn(textblock, ColPosition + 1);
            PointsGrid.Children.Add(textblock);
        }
        private void CreateNewRow(int Position)
        {
            RowDefinition rowDef = new();
            PointsGrid.RowDefinitions.Add(rowDef);
            Border border = new()
            {
                BorderThickness = new Thickness(0.1),
                BorderBrush = new SolidColorBrush(Colors.Black)
            };
            TextBlock RowNum = new()
            {
                Text = Position.ToString()
            };
            Grid.SetRow(RowNum, Position + 1);
            Grid.SetRow(border, Position + 1);
            PointsGrid.Children.Add(RowNum);
            PointsGrid.Children.Add(border);
        }
        private void FillPointsTable()
        {
            WizardContext context = new();
            var GameID = context.PlayersGames.FromSql($"SELECT * FROM Players_Games ORDER BY GameID DESC LIMIT 1").First().GameId;
            var Players = context.PlayersGames.FromSql($"SELECT * FROM Players_Games WHERE GameID={GameID}").ToList();
            int i = 0;
            foreach (var player in Players)
            {
                var playerScore = context.Scores.FromSql($"SELECT * FROM Scores WHERE GamePlayerID = {player.GamePlayerId} ORDER BY RoundNumber ASC").ToList();
                foreach (var score in playerScore)
                {
                    CreateSubColumn(score.Score1.ToString(), i * 3 , score.RoundNumber + 1);
                    CreateSubColumn(score.Prediction.ToString(), i * 3 + 1, score.RoundNumber + 1);
                    CreateSubColumn(score.Tricks.ToString(), i * 3 + 2, score.RoundNumber + 1);
                }
                i++;
            }
        }

        private void To_predictions_Click(object sender, RoutedEventArgs e)
        {
            PredictionWindow predictionWindow = new();
            predictionWindow.Show();
            this.Hide();
        }
    }
}