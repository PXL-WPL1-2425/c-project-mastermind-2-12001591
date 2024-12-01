using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace c_project_mastermind_1_12001591
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private string color1, color2, color3, color4;
        private int attempts; 
        private DateTime startTime;
        private bool gameEnded = false;
        private bool isDebugMode = false;

        public MainWindow()
        {
            InitializeComponent();
            attempts = 0; 
            Title = $"Poging {attempts + 1}/10"; 

            
            GenerateNewCode();

           
            timer.Interval = TimeSpan.FromSeconds(1); 
            timer.Tick += Timer_Tick; 

            
            StartCountdown();

            this.KeyDown += MainWindow_KeyDown;
            comboBox1.SelectionChanged += ComboBox_SelectionChanged;
            comboBox2.SelectionChanged += ComboBox_SelectionChanged;
            comboBox3.SelectionChanged += ComboBox_SelectionChanged;
            comboBox4.SelectionChanged += ComboBox_SelectionChanged;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12 && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                ToggleDebug(); 
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            Label correspondingLabel = null;

           
            if (comboBox == comboBox1)
                correspondingLabel = colorLabel1;
            else if (comboBox == comboBox2)
                correspondingLabel = colorLabel2;
            else if (comboBox == comboBox3)
                correspondingLabel = colorLabel3;
            else if (comboBox == comboBox4)
                correspondingLabel = colorLabel4;

            
            if (comboBox?.SelectedItem is ComboBoxItem selectedItem)
            {
                correspondingLabel.Background = new SolidColorBrush(GetColorFromString(selectedItem.Content.ToString()));
            }
        }

       
        private void GenerateNewCode()
        {
            Random rnd = new Random();
            color1 = RandomColor(rnd);
            color2 = RandomColor(rnd);
            color3 = RandomColor(rnd);
            color4 = RandomColor(rnd);
        }

        
        private string RandomColor(Random rnd)
        {
            int randomNumber = rnd.Next(1, 7);
            switch (randomNumber)
            {
                case 1: return "Blue";
                case 2: return "Red";
                case 3: return "White";
                case 4: return "Yellow";
                case 5: return "Orange";
                case 6: return "Green";
                default: return "";
            }
        }

        /// <summary> 
        /// Deze methode wordt aangeroepen wanneer het spel begint of wanneer een nieuwe beurt wordt gestart.
        /// De timer geeft de tijd weer tijdens de poging.
        /// </summary>
        private void StartCountdown()
        {
            startTime = DateTime.Now; 
            timer.Start(); 
            timer.Interval = TimeSpan.FromMilliseconds(1); 
        }

        
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan interval = DateTime.Now.Subtract(startTime); 
            timerTextBox.Text = interval.ToString(@"ss\:fff"); 

            
            if (interval.TotalSeconds >= 10)
            {
                StopCountdown(); 
                MessageBox.Show("Time's up! You lost your turn.");

                
                attempts++; 
                Title = $"Poging {attempts + 1}/10"; 

                
                if (attempts >= 10)
                {
                    MessageBox.Show("Game over! You've reached the maximum number of attempts.");
                    gameEnded = true; 
                    timer.Stop(); 
                }
            }
        }

        /// <summary> 
        /// Deze methode stopt het aftellen van de timer
        /// </summary>
        private void StopCountdown()
        {
            timer.Stop(); 
        }
       
        
        private string GetComboBoxColor(ComboBox comboBox)
        {
            
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
            return selectedItem?.Content.ToString(); 
        }

        
        private Color GetColorFromString(string color)
        {
            switch (color)
            {
                case "Blue": return Colors.Blue;
                case "Red": return Colors.Red;
                case "White": return Colors.White;
                case "Yellow": return Colors.Yellow;
                case "Orange": return Colors.Orange;
                case "Green": return Colors.Green;
                default: return Colors.Gray; 
            }
        }

        
        private void CompareCodeWithLabels(string input1, string input2, string input3, string input4)
        {
            SameColor(colorLabel1, input1, color1, 1); 
            SameColor(colorLabel2, input2, color2, 2); 
            SameColor(colorLabel3, input3, color3, 3); 
            SameColor(colorLabel4, input4, color4, 4); 
        }

        private void SameColor(Label label, string chosenColor, string correctColor, int position)
        {
            
            Color chosenColorActual = GetColorFromString(chosenColor);
            Color correctColorActual = GetColorFromString(correctColor);

            if (chosenColorActual == correctColorActual)
            {
                
                label.Background = new SolidColorBrush(chosenColorActual); 
                label.BorderThickness = new Thickness(5); 
                label.BorderBrush = new SolidColorBrush(Colors.Red); 
            }
            else if (IsPartialMatch(chosenColorActual, position)) 
            {
                label.Background = new SolidColorBrush(chosenColorActual); 
                label.BorderBrush = new SolidColorBrush(Colors.Wheat); 
                label.BorderThickness = new Thickness(3);
            }
            
        }

        
        private bool IsPartialMatch(Color chosenColorActual, int position)
        {
            
            switch (position)
            {
                case 1:
                    return color2 == GetStringFromColor(chosenColorActual) || color3 == GetStringFromColor(chosenColorActual) || color4 == GetStringFromColor(chosenColorActual);
                case 2:
                    return color1 == GetStringFromColor(chosenColorActual) || color3 == GetStringFromColor(chosenColorActual) || color4 == GetStringFromColor(chosenColorActual);
                case 3:
                    return color1 == GetStringFromColor(chosenColorActual) || color2 == GetStringFromColor(chosenColorActual) || color4 == GetStringFromColor(chosenColorActual);
                case 4:
                    return color1 == GetStringFromColor(chosenColorActual) || color2 == GetStringFromColor(chosenColorActual) || color3 == GetStringFromColor(chosenColorActual);
                default:
                    return false;
            }
        }
        private string GetStringFromColor(Color color)
        {
            if (color == Colors.Blue) return "Blue";
            if (color == Colors.Red) return "Red";
            if (color == Colors.White) return "White";
            if (color == Colors.Yellow) return "Yellow";
            if (color == Colors.Orange) return "Orange";
            if (color == Colors.Green) return "Green";
            return string.Empty;
        }
        private void ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            if (gameEnded) return; 

            
            timer.Stop(); 
            StartCountdown(); 

           
            string comboBox1Color = GetComboBoxColor(comboBox1);
            string comboBox2Color = GetComboBoxColor(comboBox2);
            string comboBox3Color = GetComboBoxColor(comboBox3);
            string comboBox4Color = GetComboBoxColor(comboBox4);

            if (comboBox1Color == null || comboBox2Color == null || comboBox3Color == null || comboBox4Color == null)
            {
                MessageBox.Show("Please select colors from all ComboBoxes.");
                return; 
            }

            
            CompareCodeWithLabels(comboBox1Color, comboBox2Color, comboBox3Color, comboBox4Color);

           
            attempts++;
            Title = $"Poging {attempts + 1}/10";

            
            if (comboBox1Color == color1 && comboBox2Color == color2 && comboBox3Color == color3 && comboBox4Color == color4)
            {
                MessageBox.Show("Congratulations! You guessed the code correctly!");
                gameEnded = true; 
                timer.Stop(); 
            }
            
            else if (attempts >= 10)
            {
                MessageBox.Show("Game over! You've reached the maximum number of attempts.");
                gameEnded = true; 
                timer.Stop(); 
            }
            else
            {
                
            }
        }
        /// <summary>
        /// Schakelt de debugmodus in of uit. Wanneer deze actief is, wordt de code getoond in de textBox.
        /// </summary>
        private void ToggleDebug()
        {
            isDebugMode = !isDebugMode; 
            if (isDebugMode)
            {
                debugCodeTextBox.Visibility = Visibility.Visible;
                debugCodeTextBox.Text = $"{color1}, {color2}, {color3}, {color4}"; 
            }
            else
            {
                debugCodeTextBox.Visibility = Visibility.Collapsed;
            }
        }
    }
}
