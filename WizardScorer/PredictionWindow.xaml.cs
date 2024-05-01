using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WizardScorer.Models;

namespace WizardScorer
{
    /// <summary>
    /// Interaction logic for PredictionWindow.xaml
    /// </summary>
    public partial class PredictionWindow : Window
    {
        private List<Tuple<Player, TextBox>> Players = [];
        private List<Label> PlayerLabels = [];
        private List<TextBox> PredictionBoxes = [];
        int round = 1;
        public PredictionWindow()
        {
            InitializeComponent();
            WizardContext context = new();
            int currentGame = context.Games.OrderByDescending(games => games.GameId).First().GameId;
            var selected = context.Players.FromSql($"SELECT Player_Name, Players.PlayerID FROM Players INNER JOIN Players_Games ON Players.PlayerID = Players_Games.PlayerID INNER JOIN Games ON Games.GameID = Players_Games.GameID WHERE Games.GameID = {currentGame} ORDER BY Games.GameID DESC").ToList();
            
            PredictionBoxes = [Player1Pred, Player2Pred, Player3Pred, Player4Pred, Player5Pred, Player6Pred];
            PlayerLabels = [Player1Name, Player2Name, Player3Name, Player4Name, Player5Name, Player6Name];

            for (int i = 0; i < selected.Count; i++)
            {
                PlayerLabels[i].Content = selected[i].PlayerName;
                PredictionBoxes[i].Text = selected[i].PlayerName + " jóslata";
                Players.Add(new Tuple<Player, TextBox>(selected[i], PredictionBoxes[i]));
            }
            if(selected.Count == 6)
            {
                return;
            }
            for (int i = selected.Count; i< PlayerLabels.Count;i++)
            {
                if (PlayerLabels[i].Content == null)
                {
                    PlayerLabels[i].Visibility = Visibility.Collapsed;
                    PredictionBoxes[i].Visibility = Visibility.Collapsed;
                }
            }
            PlayerLabels.RemoveRange(selected.Count, PlayerLabels.Count - selected.Count);
            PredictionBoxes.RemoveRange(selected.Count, PredictionBoxes.Count - selected.Count);
        }

        private void Register_Predictions(object sender, RoutedEventArgs e)
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
                Score playerScore = new()
                {
                    GamePlayerId = currentGamePlayerId,
                    RoundNumber = round,
                    Prediction = int.Parse(player.Item2.Text)
                };
                context.Add(playerScore);
            }
            context.SaveChanges();
            TricksWindow tricksWindow = new();
            tricksWindow.Show();
            this.Hide();
        }

        private void Go_To_Points(object sender, RoutedEventArgs e)
        {
            PointsWindow pointsWindow = new();
            pointsWindow.Show();
            this.Hide();
        }
    }
}