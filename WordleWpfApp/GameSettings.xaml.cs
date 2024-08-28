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
    /// Interaction logic for GameSettings.xaml
    /// </summary>
    public partial class GameSettingsWindow : Window
    {
        ////////////////////////////////////////// 
        // properties
        public bool DarkMode { get; private set; }
        public bool HighContrastMode { get; private set; }
        public bool HardMode { get; private set; }

        ////////////////////////////////////////// 
        // constructor
        public GameSettingsWindow(Dictionary<string, bool> settings)
        {
            InitializeComponent();

            // initalize properties
            DarkMode = settings["DarkMode"];
            HighContrastMode = settings["HighContrastMode"];
            HardMode = settings["HardMode"];

            // initialize form controls
            checkBoxDarkMode.IsChecked = DarkMode;
            checkBoxHighContrastMode.IsChecked = HighContrastMode;
            checkBoxHardMode.IsChecked = HardMode;
            SetWindowColors();
        }

        ////////////////////////////////////////// 
        // event handlers
        ////////////////////////////////////////// 

        private void checkBoxDarkMode_Click(object sender, RoutedEventArgs e)
        {
            SetWindowColors();
        }

        private void checkBoxHighContrastMode_Click(object sender, RoutedEventArgs e)
        {
            SetWindowColors();
        }

        private void checkBoxHardMode_Click(object sender, RoutedEventArgs e)
        {
            SetWindowColors();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            // update the form properties
            DarkMode = checkBoxDarkMode.IsChecked == true;
            HighContrastMode = checkBoxHighContrastMode.IsChecked == true;
            HardMode = checkBoxHardMode.IsChecked == true;
            // set the DialogResult and close the window
            DialogResult = true;
            Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            // set the DialogResult and close the window
            DialogResult = false;
            Close();
        }

        ////////////////////////////////////////// 
        // instance methods
        ////////////////////////////////////////// 

        private void SetWindowColors()
        {
            // set window background color
            if (checkBoxDarkMode.IsChecked == true)
            {
                Background = Brushes.Black;
                groupBoxSettings.Foreground = Brushes.White;
                checkBoxDarkMode.Foreground = Brushes.White;
                checkBoxHighContrastMode.Foreground = Brushes.White;
                checkBoxHardMode.Foreground = Brushes.White;
            }
            else
            {
                Background = Brushes.White;
                groupBoxSettings.Foreground = Brushes.Black;
                checkBoxDarkMode.Foreground = Brushes.Black;
                checkBoxHighContrastMode.Foreground = Brushes.Black;
                checkBoxHardMode.Foreground = Brushes.Black;
            }

            // set the background color for first textbox (not found)
            if (checkBoxHighContrastMode.IsChecked == true)
            {
                textBox1.Background = MainWindow.HC_NOT_FOUND;
            }
            else
            {
                textBox1.Background = MainWindow.DF_NOT_FOUND;
            }

            // set the background color for second textbox (wrong spot)
            if (checkBoxHighContrastMode.IsChecked == true)
            {
                textBox2.Background = MainWindow.HC_WRONG_SPOT;
            }
            else
            {
                textBox2.Background = MainWindow.DF_WRONG_SPOT;
            }

            // set the background color for first textbox (right spot)
            if (checkBoxHighContrastMode.IsChecked == true)
            {
                textBox3.Background = MainWindow.HC_RIGHT_SPOT;
            }
            else
            {
                textBox3.Background = MainWindow.DF_RIGHT_SPOT;
            }
        }

    }
}
