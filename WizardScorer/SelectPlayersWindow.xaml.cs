using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SelectPlayersWindow.xaml
    /// </summary>
public partial class SelectPlayersWindow : Window
    {
        public int NumPlayers { get; set; }
        ObservableCollection<Player> PlayerList = [];
        public ObservableCollection<Player> SelectedPlayers { get; set; } = [];
        private List<ComboBox> playerComboBoxes = [];
        public WizardContext context = new();
        public SelectPlayersWindow()
        {
            foreach(Player player in context.Players.ToList())
            {
                PlayerList.Add(player);
            }
            
            InitializeComponent();
            this.DataContext = this;
            InitializeComboBoxList();
            List<string> SelectedPlayers = [];
        }

        private void InitializeComboBoxList()
        {
            // Initialize the list of ComboBoxes
            playerComboBoxes =
        [
            Player1,
            Player2,
            Player3,
            Player4,
            Player5,
            Player6
        ];
            foreach(var box in playerComboBoxes)
            {
                box.ItemsSource = PlayerList;
            }
        }
        private void Start_Game(object sender, RoutedEventArgs e)
        {
            foreach (ComboBox comboBox in playerComboBoxes)
            {
                if (comboBox.SelectedItem != null)
                {
                    SelectedPlayers.Add((Player)comboBox.SelectedItem);
                }
            }
            if (SelectedPlayers.Count < 3)
            {
                SelectedPlayers.Clear();
                NumPlayers = 0;
                MessageBox.Show($"Válassz legalább 3 játékost!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                NumPlayers = SelectedPlayers.Count;
                Game currentgame = new()
                {
                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                context.Add(currentgame);
                foreach(var player in SelectedPlayers)
                {
                    PlayersGame game = new()
                    {
                        Player = player,
                        Game = currentgame
                    };
                    context.Add(game);
                }
                context.SaveChanges();
                PredictionWindow predictionWindow = new ();
                predictionWindow.Show();
                this.Hide();
            }
        }

        private void AddPlayer_Click(object sender, RoutedEventArgs e)
        {
            Player player = new()
            {
                PlayerName = NewPlayerName.Text
            };
            if(context.Players.FromSql($"SELECT * FROM players where Player_Name={NewPlayerName.Text}").Any())
            {
                string errormsg = "Már szerepel az adatbázisban ilyen nevű játékos!";
                string errorcaption = "Új játékos hiba!";
                
                MessageBox.Show(errormsg, errorcaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            context.Add(player);
            PlayerList.Add(player);
            context.SaveChanges();
        }
    }
}
