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
        }
        private void CreatePointsTable()
        {
            int numPlayers = ((SelectPlayersWindow)Application.Current.MainWindow).NumPlayers;
            for (int i = 0; i < numPlayers; i++)
            {
                ColumnDefinition columnDef = new()
                {
                    Width = new GridLength(100, GridUnitType.Auto)
                };
                ColumnDefinition columnDef2 = new()
                {
                    Width = new GridLength(100, GridUnitType.Auto)
                };
                PointsGrid.ColumnDefinitions.Add(columnDef);
                PointsGrid.ColumnDefinitions.Add(columnDef2);
                TextBlock playerName = new()
                {
                    Text = ((SelectPlayersWindow)Application.Current.MainWindow).SelectedPlayers[i].PlayerName,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(15, 0, 15, 0)
                };
                Grid.SetColumn(playerName, i * 2);
                Grid.SetColumnSpan(playerName, 2);
                PointsGrid.Children.Add(playerName);
                TextBlock point = new()
                {
                    Text = "Pont",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(15, 0, 15, 0)
                };
                TextBlock trick = new()
                {
                    Text = "Ütés",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(15, 0, 15, 0)
                };

                Grid.SetRow(point, 1);
                Grid.SetRow(trick, 1);
                Grid.SetColumn(point, i * 2);
                Grid.SetColumn(trick, i * 2 + 1);
                PointsGrid.Children.Add(point);
                PointsGrid.Children.Add(trick);
            }

            for (int j = 1; j <= (60 / numPlayers); j++)
            {
                RowDefinition rowDef = new();
                PointsGrid.RowDefinitions.Add(rowDef);
                //Border border = new()
                //{
                //    BorderThickness = new Thickness(0.1),
                //    BorderBrush=new SolidColorBrush(Colors.Black)
                //};
                TextBlock RowNum = new()
                {
                    Text = j.ToString()
                };
                Grid.SetRow(RowNum, j + 1);
                //Grid.SetRow(border, j + 1);
                PointsGrid.Children.Add(RowNum);
                //PointsGrid.Children.Add(border);
            }
        }



        private void To_predictions_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
