using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
    /// Interaction logic for TricksWindow.xaml
    /// </summary>
    public partial class TricksWindow : Window
    {
        private List<Tuple<Player, TextBox>> Players = [];
        private List<Label> PlayerLabels = [];
        private List<TextBox> TrickBoxes = [];
        public TricksWindow()
        {
            InitializeComponent();
            WizardContext context = new();
            int currentGame = context.Games.OrderByDescending(games => games.GameId).First().GameId;
            var selected = context.Players.FromSql($"SELECT Player_Name, Players.PlayerID FROM Players INNER JOIN Players_Games ON Players.PlayerID = Players_Games.PlayerID INNER JOIN Games ON Games.GameID = Players_Games.GameID WHERE Games.GameID = {currentGame} ORDER BY Games.GameID DESC").ToList();

            TrickBoxes = [Player1Trick, Player2Trick, Player3Trick, Player4Trick, Player5Trick, Player6Trick];
            PlayerLabels = [Player1Name, Player2Name, Player3Name, Player4Name, Player5Name, Player6Name];
            for (int i = 0; i < selected.Count; i++)
            {
                PlayerLabels[i].Content = selected[i].PlayerName;
                TrickBoxes[i].Text = selected[i].PlayerName + " ütései";
                Players.Add(new Tuple<Player, TextBox>(selected[i], TrickBoxes[i]));
            }
            if (selected.Count == 6)
            {
                return;
            }
            for (int i = selected.Count; i < PlayerLabels.Count; i++)
            {
                if (PlayerLabels[i].Content == null)
                {
                    PlayerLabels[i].Visibility = Visibility.Collapsed;
                    TrickBoxes[i].Visibility = Visibility.Collapsed;
                }
            }
            PlayerLabels.RemoveRange(selected.Count, PlayerLabels.Count - selected.Count);
            TrickBoxes.RemoveRange(selected.Count, TrickBoxes.Count - selected.Count);
        }

        private void Register_Tricks(object sender, RoutedEventArgs e)
        {
            foreach (var player in Players)
            {
                if (!int.TryParse(player.Item2.Text, out int resultpred))
                {
                    MessageBox.Show($"Adj meg egy számot {player.Item2.Text} helyett!", "Helytelen érték!", MessageBoxButton.OK);
                    return;
                }
            }
            WizardContext context = new();
            int currentGame = context.Games.OrderByDescending(games => games.GameId).First().GameId;
            foreach (var player in Players)
            {
                var currentGamePlayerId = context.PlayersGames
                    .Where(record => record.PlayerId == player.Item1.PlayerId &&
                           record.GameId == currentGame).First().GamePlayerId;
                //var currentGamePlayeradsId = context.PlayersGames.FromSql($"SELECT * FROM Players_Games WHERE Players_Games.PlayerID={player.Item1.PlayerId} AND Players_Games.GameID={currentGame}").First().GamePlayerId;
                var CurrentScoreEntity = context.Scores.FromSql($"SELECT * FROM Scores WHERE GamePlayerID = {currentGamePlayerId}").First();
                if (CurrentScoreEntity != null)
                {
                    CurrentScoreEntity.Tricks = int.Parse(player.Item2.Text);
                    CurrentScoreEntity.Score1 = Calculate_Score(CurrentScoreEntity.Tricks, CurrentScoreEntity.Prediction);
                    context.SaveChanges();
                }
                else
                {
                    MessageBox.Show($"Nincs regisztrálva pont a játékoshoz!", "Lekérdezés hiba!", MessageBoxButton.OK);
                }
            }
            context.SaveChanges();
            PointsWindow pointsWindow = new();
            pointsWindow.Show();
            this.Hide();
        }
        private int Calculate_Score(int tricks, int predictions)
        {
            int score;
            if ( predictions == tricks)
            {
                score = 20 + predictions * 10;
                return score;
            }
            score = -(int.Abs(predictions - tricks)) * 10;
            return score;
        }

        private void Go_To_Points(object sender, RoutedEventArgs e)
        {
            PointsWindow pointsWindow = new();
            pointsWindow.Show();
            this.Hide();
        }
    }
}
