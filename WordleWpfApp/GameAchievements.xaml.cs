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
    /// Interaction logic for GameAchievements.xaml
    /// </summary>
    public partial class GameAchievementsWindow : Window
    {
        //////////////////////////////////////////
        // class constants
        private const string TROPHY_SYMBOL = "🏆";

        ////////////////////////////////////////// 
        // properties
        private bool DarkMode { get; set; }
        private bool HighContrastMode { get; set; }

        //////////////////////////////////////////
        // constuctor
        public GameAchievementsWindow(Dictionary<string, int> stats, bool dark, bool hc)
        {
            InitializeComponent();

            // initialize properties
            DarkMode = dark;
            HighContrastMode = hc;

            // set window and widget colors
            SetWindowColors();

            // activate the achievements
            ActivateAchievement("LowHangingFruit");
            if (stats["TotalWins"] >= 1)
            {
                ActivateAchievement("N00b");
            }
            if (stats["TotalWins"] >= 10)
            {
                ActivateAchievement("SlowSteady");
            }
            if (stats["TotalWins"] >= 100)
            {
                ActivateAchievement("Relentless");
            }
            if (stats["TotalWins"] >= 1000)
            {
                ActivateAchievement("Pwned");
            }
            if (stats["BestStreak"] >= 5)
            {
                ActivateAchievement("GettingHangOfIt");
            }
            if (stats["BestStreak"] >= 10)
            {
                ActivateAchievement("LookMaNoHands");
            }
            if (stats["BestStreak"] >= 100)
            {
                ActivateAchievement("EyesClosed");
            }
            if (stats["BestStreak"] >= 500)
            {
                ActivateAchievement("Unbreakable");
            }
            if (stats["1GuessWins"] > 0)
            {
                ActivateAchievement("GodMode");
            }
            if (stats["2GuessWins"] > 0)
            {
                ActivateAchievement("Eagle");
            }
            if (stats["3GuessWins"] > 0)
            {
                ActivateAchievement("Birdie");
            }
            if (stats["6GuessWins"] > 0)
            {
                ActivateAchievement("DoubleBogey");
            }
        }

        ////////////////////////////////////////// 
        // private helper methods

        private void ActivateAchievement(string achievement)
        {
            string rect = achievement + "Rect";
            Rectangle shape = (Rectangle)FindName(rect);
            shape.Fill = HighContrastMode ? MainWindow.HC_RIGHT_SPOT : MainWindow.DF_RIGHT_SPOT;
            string symbol = achievement + "Symbol";
            Label sym = (Label)FindName(symbol);
            sym.Content = TROPHY_SYMBOL;
            sym.Foreground = Brushes.Gold;
            string title = achievement + "Title";
            Label ttl = (Label)FindName(title);
            ttl.Foreground = Brushes.White;
            string desc = achievement + "Desc";
            Label dsc = (Label)FindName(desc);
            dsc.Foreground = Brushes.White;
        }

        private void SetWindowColors()
        {
            // window background color
            Background = DarkMode ? Brushes.Black : Brushes.White;

            // element background color
            Grid[] grids = { gridGameplay, gridWins, gridStreaks, gridGuesses };
            foreach (Grid grid in grids)
            {
                grid.Background = DarkMode ? Brushes.Black : Brushes.White;
                foreach (Rectangle rect in grid.Children.OfType<Rectangle>())
                {
                    rect.Fill = DarkMode ? Brushes.Black : Brushes.White;
                }
                foreach (Label label in grid.Children.OfType<Label>())
                {
                    label.Foreground = DarkMode ? Brushes.DimGray : Brushes.LightGray;
                }
            }
        }
    }
}
