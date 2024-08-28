using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WordleWpfApp
{
    public partial class MainWindow : Window
    {
        ////////////////////////////////////////// 
        // class enumerations
        private enum GameState { Active, Inactive, Complete };

        ////////////////////////////////////////// 
        // class readonly variables
        private readonly string SETTINGS_FULL_PATH;

        ////////////////////////////////////////// 
        // class constants
        private const int MAX_GUESSES = 6;

        ////////////////////////////////////////// 
        // default colors
        internal static SolidColorBrush DF_BUTTON_BG = Brushes.LightGray;
        internal static SolidColorBrush DF_TEXTBOX_BG = Brushes.WhiteSmoke;
        internal static SolidColorBrush DF_NOT_FOUND = Brushes.DimGray;
        internal static SolidColorBrush DF_WRONG_SPOT = Brushes.Goldenrod;
        internal static SolidColorBrush DF_RIGHT_SPOT = Brushes.ForestGreen;

        ////////////////////////////////////////// 
        // high contrast colors
        internal static SolidColorBrush HC_BUTTON_BG = Brushes.LightGray;
        internal static SolidColorBrush HC_TEXTBOX_BG = Brushes.WhiteSmoke;
        internal static SolidColorBrush HC_NOT_FOUND = Brushes.DimGray;
        internal static SolidColorBrush HC_WRONG_SPOT = Brushes.LightSkyBlue;
        internal static SolidColorBrush HC_RIGHT_SPOT = Brushes.OrangeRed;

        ////////////////////////////////////////// 
        // private fields 
        private List<string> _possibleAnswers;
        private List<string> _validWordGuesses;
        private TextBox[,] _textBoxes;
        private Dictionary<Key, Button> _buttons;
        private Dictionary<string, SolidColorBrush> _gameColors;
        private Dictionary<string, bool> _userSettings;
        private Dictionary<string, int> _userStats;
        private Random _rng;

        ////////////////////////////////////////// 
        // automatic properties
        private GameState CurrentGameState { get; set; }
        private string CurrentAnswer { get; set; }
        private int GuessCount { get; set; }
        private int LetterCount { get; set; }

        ////////////////////////////////////////// 
        // constructor
        public MainWindow()
        {
            InitializeComponent();

            // initialize the path to the settings file
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = System.IO.Path.Combine(path, "WordleGame");
            System.IO.Directory.CreateDirectory(path);
            SETTINGS_FULL_PATH = System.IO.Path.Combine(path, "Settings.txt");

            // default game values
            GuessCount = 0;
            LetterCount = 0;
            CurrentAnswer = String.Empty;

            // create the game color dictionary
            _gameColors = new Dictionary<string, SolidColorBrush>();

            // load up user settings (or use defaults)
            _userSettings = new Dictionary<string, bool>();
            LoadSettings();

            // load up user statistics
            _userStats = new Dictionary<string, int>();
            LoadStats();

            // create a 2D array of the TextBox elements
            _textBoxes = new TextBox[,]
                { { textBox_0_0, textBox_0_1, textBox_0_2, textBox_0_3, textBox_0_4 },
                  { textBox_1_0, textBox_1_1, textBox_1_2, textBox_1_3, textBox_1_4 },
                  { textBox_2_0, textBox_2_1, textBox_2_2, textBox_2_3, textBox_2_4 },
                  { textBox_3_0, textBox_3_1, textBox_3_2, textBox_3_3, textBox_3_4 },
                  { textBox_4_0, textBox_4_1, textBox_4_2, textBox_4_3, textBox_4_4 },
                  { textBox_5_0, textBox_5_1, textBox_5_2, textBox_5_3, textBox_5_4 } };
            // create a map of keys to buttons
            _buttons = new Dictionary<Key, Button>()
                { { Key.A, button_A }, { Key.B, button_B }, { Key.C, button_C },
                  { Key.D, button_D }, { Key.E, button_E }, { Key.F, button_F },
                  { Key.G, button_G }, { Key.H, button_H }, { Key.I, button_I },
                  { Key.J, button_J }, { Key.K, button_K }, { Key.L, button_L },
                  { Key.M, button_M }, { Key.N, button_N }, { Key.O, button_O },
                  { Key.P, button_P }, { Key.Q, button_Q }, { Key.R, button_R },
                  { Key.S, button_S }, { Key.T, button_T }, { Key.U, button_U },
                  { Key.V, button_V }, { Key.W, button_W }, { Key.X, button_X },
                  { Key.Y, button_Y }, { Key.Z, button_Z },
                  { Key.Back, button_Back }, { Key.Enter, button_Enter } };
            // word lists are separated by line
            string[] separators = new string[] { "\r\n", "\r", "\n" };
            // load up the valid word guesses
            string words = Properties.Resources.ValidWordList;
            string[] wordArray = words.Split(separators, StringSplitOptions.None);
            _validWordGuesses = new List<string>(wordArray);
            // load up the possible answer words 
            words = Properties.Resources.AnswerWordList;
            wordArray = words.Split(separators, StringSplitOptions.None);
            _possibleAnswers = new List<string>(wordArray);
            // seed the random number generator
            _rng = new Random();
            // set up the game
            ResetGame();
        }

        ////////////////////////////////////////// 
        // event handlers
        ////////////////////////////////////////// 

        private void buttonAchievements_Click(object sender, RoutedEventArgs e)
        {
            // show player achievements
            bool dark = _userSettings["DarkMode"];
            bool hc = _userSettings["HighContrastMode"];
            GameAchievementsWindow gaw = new GameAchievementsWindow(_userStats, dark, hc);
            gaw.ShowDialog();
            // restore focus to the enter button
            button_Enter.Focus();
        }

        private void buttonRestart_Click(object sender, RoutedEventArgs e)
        {
            // restart the game?
            if (CurrentGameState == GameState.Active)
            {
                if (!ConfirmGameEnd("restart"))
                {
                    return;
                }
            }
            ResetGame();
            // restore focus to the enter button
            button_Enter.Focus();
        }

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            // show player settings
            GameSettingsWindow gsw = new GameSettingsWindow(_userSettings);
            gsw.ShowDialog();
            if (gsw.DialogResult == true)
            {
                _userSettings["DarkMode"] = gsw.DarkMode;
                _userSettings["HighContrastMode"] = gsw.HighContrastMode;
                _userSettings["HardMode"] = gsw.HardMode;
                // save new settings
                SaveSettings();
                // set game colors
                SetGameColors();
            }
            // restore focus to the enter button
            button_Enter.Focus();
        }

        private void buttonStats_Click(object sender, RoutedEventArgs e)
        {
            // show player stats
            bool dark = _userSettings["DarkMode"];
            bool hc = _userSettings["HighContrastMode"];
            GameStatsWindow gaw = new GameStatsWindow(_userStats, dark, hc);
            gaw.ShowDialog();
            // restore focus to the enter button
            button_Enter.Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // close the game?
            if (CurrentGameState == GameState.Active)
            {
                e.Cancel = !ConfirmGameEnd("exit");
            }
        }

        private void Wordle_KeyDown(object sender, KeyEventArgs e)
        {
            if (_buttons.ContainsKey(e.Key))
            {
                Button b = _buttons[e.Key];
                Wordle_HandleLetterEntryEvent(b.Content.ToString());
            }
        }

        private void Wordle_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                Wordle_HandleLetterEntryEvent(b.Content.ToString());
            }
            // restore focus to the enter button
            button_Enter.Focus();
        }

        private void Wordle_HandleLetterEntryEvent(string letter)
        {
            if (CurrentGameState != GameState.Complete)
            {
                if (letter == button_Enter.Content.ToString())
                {
                    // handle an enter key event
                    if (LetterCount == 5)
                    {
                        string guess = string.Empty;
                        for (int i = 0; i < 5; i++)
                        {
                            guess += _textBoxes[GuessCount, i].Text;
                        }
                        guess = guess.ToLower();
                        // check if it's a valid guess
                        if (_validWordGuesses.Contains(guess))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                TextBox TB = _textBoxes[GuessCount, i];
                                string letterB = TB.Text.ToLower();
                                if (CurrentAnswer.Substring(i, 1).Equals(letterB))
                                {
                                    TB.Background = _gameColors["RightSpot"];
                                }
                                else if (CurrentAnswer.Contains(letterB))
                                {
                                    TB.Background = _gameColors["WrongSpot"];
                                }
                                else
                                {
                                    TB.Background = _gameColors["NotFound"];
                                }
                            }

                            if (CurrentAnswer.Equals(guess))
                            {
                                MessageBox.Show("Won");
                                _userStats["TotalGames"]++;
                                _userStats["TotalWins"]++;
                                _userStats["CurrentStreak"]++;
                                SaveStats();
                                CurrentGameState = GameState.Complete;
                            }

                            GuessCount++;
                            LetterCount = 0;
                            if (GuessCount == 6 && CurrentGameState != GameState.Complete)
                            {
                                MessageBox.Show("L");
                                _userStats["TotalGames"]++;
                                _userStats["TotalLosses"]++;
                                _userStats["CurrentStreak"] = 0;
                                SaveStats();
                                CurrentGameState = GameState.Complete;
                            }
                        }
                    }
                }
                else if (letter == button_Back.Content.ToString())
                {
                    // handle a backspace key event
                    if (LetterCount > 0)
                    {
                        TextBox TB = _textBoxes[GuessCount, LetterCount - 1];
                        TB.Text = string.Empty;
                        TB.Text = string.Empty;
                        LetterCount--;
                    }
                }
                else
                {
                    if (CurrentGameState == GameState.Inactive)
                    {
                        CurrentGameState = GameState.Active;
                    }
                    // handle an alpha key event
                    if (LetterCount < 5)
                    {
                        TextBox TB = _textBoxes[GuessCount, LetterCount];
                        TB.Text = letter;
                        LetterCount++;
                    }
                }
            }
        }

        ////////////////////////////////////////// 
        // instance methods
        ////////////////////////////////////////// 

        private bool ConfirmGameEnd(string type)
        {
            string message = $"Are you sure you want to {type}?\n\n(This action will ";
            message += "force a 'loss' and break any active winning streak.)";
            var choice = MessageBox.Show(message, "Warning", MessageBoxButton.YesNo);
            if (choice == MessageBoxResult.Yes)
            {
                // force end game and write out stats before closing
                _userStats["TotalGames"]++;
                _userStats["TotalLosses"]++;
                _userStats["CurrentStreak"] = 0;
                SaveStats();
                return true;
            }
            else
            {
                return false;
            }
        }

        private Button GetButtonFromLetter(string letter)
        {
            // get the upper case character
            char c = char.Parse(letter.ToUpper());
            // map the ASCII value to a key value
            Key k = (Key)(c - 21);
            // return the button from the dictionary
            return _buttons[k];
        }

        private void LoadSettings()
        {
            // defaults
            _userSettings["DarkMode"] = false;
            _userSettings["HighContrastMode"] = false;
            _userSettings["HardMode"] = false;

            // load user settings from external file
            try
            {
                StreamReader reader = new StreamReader(SETTINGS_FULL_PATH);
                using (reader)
                {
                    // read the settings line by line
                    string curline = reader.ReadLine();
                    while (curline != null)
                    {
                        string[] parts = curline.Split(':');
                        string key = parts[0];
                        if (bool.TryParse(parts[1], out bool value))
                        {
                            _userSettings[key] = value;
                        }
                        curline = reader.ReadLine();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                // ignore
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadStats()
        {
            // defaults
            _userStats["TotalGames"] = 0;
            _userStats["TotalWins"] = 0;
            _userStats["TotalLosses"] = 0;
            _userStats["CurrentStreak"] = 0;
            _userStats["BestStreak"] = 0;
            _userStats["1GuessWins"] = 0;
            _userStats["2GuessWins"] = 0;
            _userStats["3GuessWins"] = 0;
            _userStats["4GuessWins"] = 0;
            _userStats["5GuessWins"] = 0;
            _userStats["6GuessWins"] = 0;

            // load user statistics from registry
            RegistryKey subkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\QuordleGame");
            if (subkey != null)
            {
                List<string> keys = _userStats.Keys.ToList<string>();
                foreach (string key in keys)
                {
                    string number = subkey.GetValue(key).ToString();
                    if (int.TryParse(number, out int value))
                    {
                        _userStats[key] = value;
                    }
                }
                subkey.Close();
            }
        }

        private void ResetGame()
        {
            // reset the guess count
            GuessCount = 0;

            // reset the letter count
            LetterCount = 0;

            // reset the game state
            CurrentGameState = GameState.Inactive;

            // pick a random answer for current game and remove it from list
            int index = _rng.Next(_possibleAnswers.Count);
            CurrentAnswer = _possibleAnswers[index];
            _possibleAnswers.Remove(CurrentAnswer);

            // reset textbox text 
            foreach (TextBox tb in _textBoxes)
            {
                tb.Text = String.Empty;
            }

            // set game colors
            SetGameColors();
        }

        private void SaveSettings()
        {
            try
            {
                StreamWriter writer = new StreamWriter(SETTINGS_FULL_PATH);
                using (writer)
                {
                    foreach (string setting in _userSettings.Keys)
                    {
                        writer.WriteLine($"{setting}:{_userSettings[setting]}");
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SaveStats()
        {
            RegistryKey subkey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\QuordleGame");
            foreach (string key in _userStats.Keys)
            {
                subkey.SetValue(key, _userStats[key]);
            }
            subkey.Close();
        }

        private void SetGameColors()
        {
            // set game color dictionary entries based on user pref
            if (_userSettings["HighContrastMode"] == true)
            {
                _gameColors["ButtonBG"] = HC_BUTTON_BG;
                _gameColors["TextboxBG"] = HC_TEXTBOX_BG;
                _gameColors["WrongSpot"] = HC_WRONG_SPOT;
                _gameColors["RightSpot"] = HC_RIGHT_SPOT;
                _gameColors["NotFound"] = HC_NOT_FOUND;
            }
            else
            {
                _gameColors["ButtonBG"] = DF_BUTTON_BG;
                _gameColors["TextboxBG"] = DF_TEXTBOX_BG;
                _gameColors["WrongSpot"] = DF_WRONG_SPOT;
                _gameColors["RightSpot"] = DF_RIGHT_SPOT;
                _gameColors["NotFound"] = DF_NOT_FOUND;
            }

            // set window background color
            if (_userSettings["DarkMode"] == true)
            {
                Background = Brushes.Black;
                labelTitle.Foreground = Brushes.White;
                buttonStats.Foreground = Brushes.White;
                buttonAchievements.Foreground = Brushes.White;
                buttonSettings.Foreground = Brushes.White;
                buttonRestart.Foreground = Brushes.White;
            }
            else
            {
                Background = Brushes.White;
                labelTitle.Foreground = Brushes.Black;
                buttonStats.Foreground = Brushes.Black;
                buttonAchievements.Foreground = Brushes.Black;
                buttonSettings.Foreground = Brushes.Black;
                buttonRestart.Foreground = Brushes.Black;
            }

            // set background color for textboxes
            foreach (TextBox tb in _textBoxes)
            {
                if (_userSettings["DarkMode"] == true)
                {
                    tb.Background = Brushes.Black;
                    tb.Foreground = Brushes.White;
                }
                else
                {
                    tb.Background = _gameColors["TextboxBG"];
                    tb.Foreground = Brushes.Black;
                }
            }

            // set background color for buttons
            foreach (Button b in _buttons.Values)
            {
                b.Background = _gameColors["ButtonBG"];
            }
        }

    }
}
