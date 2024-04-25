using System.Text;
using System.Windows;
using System.Windows.Controls;
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
        private List<Label> PlayerLabels = [];
        private List<TextBox> PredictionBoxes = [];
        public List<int> PlayerPredictions = [];
        public PredictionWindow()
        {
            InitializeComponent();
            var selected = ((SelectPlayersWindow)Application.Current.MainWindow).SelectedPlayers;
            PredictionBoxes = [ Player1Pred, Player2Pred, Player3Pred, Player4Pred, Player5Pred, Player6Pred ];
            PlayerLabels = [ Player1Name, Player2Name,Player3Name, Player4Name, Player5Name, Player6Name ];
            for (int i = 0; i < selected.Count; i++)
            {
                PlayerLabels[i].Content = selected[i].PlayerName;
                PredictionBoxes[i].Text = selected[i].PlayerName + " jóslata";
            }
            for (int i = 0; i< PlayerLabels.Count;i++)
            {
                if (PlayerLabels[i].Content == null)
                {
                    PlayerLabels[i].Visibility = Visibility.Collapsed;
                    PredictionBoxes[i].Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Register_Predictions(object sender, RoutedEventArgs e)
        {

        }

        private void Go_To_Points(object sender, RoutedEventArgs e)
        {
            PointsWindow pointsWindow = new();
            pointsWindow.Show();
            this.Hide();
        }
    }
}