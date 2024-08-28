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

namespace WordleWpfApp
{
    /// <summary>
    /// Interaction logic for GameStats.xaml
    /// </summary>
    public partial class GameStatsWindow : Window
    {
        //////////////////////////////////////////
        // class constants
        private const int MAX_WIDTH = 220;

        ////////////////////////////////////////// 
        // properties
        public bool DarkMode { get; private set; }
        public bool HighContrastMode { get; private set; }

        ////////////////////////////////////////// 
        // constructor
        public GameStatsWindow(Dictionary<string, int> stats, bool dark, bool hc)
        {
            InitializeComponent();

            // initialize properties
            DarkMode = dark;
            HighContrastMode = hc;

            // set window and widget colors
            SetWindowColors();

            // display the main stats
            int games = stats["TotalGames"];
            double wins = stats["TotalWins"];
            double pct = (games > 0) ? wins / games : 0.0;
            int current = stats["CurrentStreak"];
            int best = stats["BestStreak"];
            lblTotalGames.Content = $"Total Games Played: {games}";
            lblWinPercentage.Content = $"Win Percentage: {pct*100:F1}%";
            lblCurrentStreak.Content = $"Current Streak: {current}";
            lblBestStreak.Content = $"Best Streak: {best}";

            // display the bar graphs
            double max = Math.Max(stats["1GuessWins"],
                           Math.Max(stats["2GuessWins"],
                             Math.Max(stats["3GuessWins"],
                               Math.Max(stats["4GuessWins"],
                                 Math.Max(stats["5GuessWins"],
                                          stats["6GuessWins"])))));
            SolidColorBrush brush = HighContrastMode ? MainWindow.HC_RIGHT_SPOT : MainWindow.DF_RIGHT_SPOT;
            if (stats["1GuessWins"] > 0)
            {
                int width = (int)(stats["1GuessWins"] / max * MAX_WIDTH) + (int)textBox1.Width;
                textBox1.Text = stats["1GuessWins"].ToString();
                textBox1.Width = width;
                textBox1.Background = brush;
            }
            if (stats["2GuessWins"] > 0)
            {
                int width = (int)(stats["2GuessWins"] / max * MAX_WIDTH) + (int)textBox2.Width;
                textBox2.Text = stats["2GuessWins"].ToString();
                textBox2.Width = width;
                textBox2.Background = brush;
            }
            if (stats["3GuessWins"] > 0)
            {
                int width = (int)(stats["3GuessWins"] / max * MAX_WIDTH) + (int)textBox3.Width;
                textBox3.Text = stats["3GuessWins"].ToString();
                textBox3.Width = width;
                textBox3.Background = brush;
            }
            if (stats["4GuessWins"] > 0)
            {
                int width = (int)(stats["4GuessWins"] / max * MAX_WIDTH) + (int)textBox4.Width;
                textBox4.Text = stats["4GuessWins"].ToString();
                textBox4.Width = width;
                textBox4.Background = brush;
            }
            if (stats["5GuessWins"] > 0)
            {
                int width = (int)(stats["5GuessWins"] / max * MAX_WIDTH) + (int)textBox5.Width;
                textBox5.Text = stats["5GuessWins"].ToString();
                textBox5.Width = width;
                textBox5.Background = brush;
            }
            if (stats["6GuessWins"] > 0)
            {
                int width = (int)(stats["6GuessWins"] / max * MAX_WIDTH) + (int)textBox6.Width;
                textBox6.Text = stats["6GuessWins"].ToString();
                textBox6.Width = width;
                textBox6.Background = brush;
            }
        }

        ////////////////////////////////////////// 
        // private helper methods

        private void SetWindowColors()
        {
            // window background color
            Background = DarkMode ? Brushes.Black : Brushes.White;

            // label background color
            foreach (Label label in gridStats.Children.OfType<Label>())
            {
                label.Foreground = DarkMode ? Brushes.White : Brushes.Black;
            }
        }
    }
}
