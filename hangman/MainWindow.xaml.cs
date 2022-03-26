using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace hangman
{
    public partial class MainWindow : Window
    {
        private List<Label> ListofLabel;
        private List<Canvas> ListofBar;
        /* The above is for the word display */
        private List<UIElement> TheHangMan; /*The hangman as a whole*/
        private readonly TextBlock WrongBox;
        private string WordNow; /* store the current word to be guessed */
        private int WrongGuesses; /* number of wrong guesses now */
        private readonly int MaximumGuess = 7;
        private readonly Word WordInstance = Word.WordPack;
        private List<object> alreadyUsed = new List<object>();

        private bool startIsDisabled = false;
        private bool guessIsDisabled = true;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStyle = WindowStyle.None;
            WrongBox = new TextBlock
            {
                Margin = new Thickness(250, 30, 0, 0),
                FontSize = 20,
                TextWrapping = TextWrapping.Wrap
            };
            /* We hold wrong characters guessed in the WrongBox */
            _ = Grid.Children.Add(WrongBox);

        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            if (startIsDisabled)
            {
                _ = MessageBox.Show("Ce boutton est désactivé!");
                return;
            }

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Confirmation de choix", "Ne pas jouer", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                return;
            }

            ListofLabel = new List<Label>();
            ListofBar = new List<Canvas>();
            SwapButtonState();

            CharList.Items.Clear();
            for (char i = 'A'; i <= 'Z'; ++i)
            {
                _ = CharList.Items.Add(i);
            }
            Grid.SetRow(WrongBox, 0);
            Grid.SetColumn(WrongBox, 1);
            WrongBox.Text = "";

            int Space = 0;
            WordInstance.LoadWord(); /* LoadANewWord */
            WordNow = WordInstance.TheWord;
            foreach (char c in WordNow)
            {
                /*We make the word display in run time in this loop.*/
                Label lbl = new Label();
                Canvas can = new Canvas
                {
                    Background = Brushes.Black,
                    Width = 15,
                    Height = 3,
                    Margin = new Thickness(-220 + Space, 0, 0, 0)
                };
                Grid.SetRow(can, 1);
                Grid.SetColumn(can, 1);
                _ = Grid.Children.Add(can);

                lbl.Margin = new Thickness(-220 + Space, 28, 0, 0);
                lbl.Visibility = Visibility.Hidden;
                lbl.Width = 60;
                lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                lbl.FontSize = 20;
                lbl.Content = c;

                Grid.SetRow(lbl, 1);
                Grid.SetColumn(lbl, 1);
                _ = Grid.Children.Add(lbl);

                ListofLabel.Add(lbl);
                ListofBar.Add(can);
                Space += 60;
            }

            TheHangMan = new List<UIElement>() { HangGround, HangBar, HangHead, HangBody, HangHands, HangLegs, HangRope };
            foreach (UIElement ele in TheHangMan)
            {
                ele.Visibility = Visibility.Hidden;
            }
            WrongGuesses = 0;
            ShowWrongGuesses.Content = MaximumGuess - WrongGuesses;
        }

        private bool HasWon(List<Label> Lol)
        {
            bool HasHidden = false;
            foreach (Label l in Lol)
            {
                if (l.Visibility == Visibility.Hidden)
                {
                    HasHidden = true;
                }
            }
            return !HasHidden;
        }

        private void SwapButtonState()
        {
            startIsDisabled = !startIsDisabled;
            guessIsDisabled = !guessIsDisabled;
        }

        private void ClearTable()
        {
            alreadyUsed.Clear();
            foreach (Label u in ListofLabel)
            {
                u.Content = "";
            }

            foreach (Canvas c in ListofBar)
            {
                c.Visibility = Visibility.Hidden;
            }
        }

        private void GuessButtonClick(object sender, RoutedEventArgs e)
        {
            if (guessIsDisabled)
            {
                _ = MessageBox.Show("Ce boutton est désactivé!");
                return;
            }
            bool flag = false;

            if (CharList.SelectedItem != null && WrongGuesses < MaximumGuess)
            {
                foreach (Label l in ListofLabel)
                {
                    if ((char)l.Content == (char)CharList.SelectedItem)
                    {
                        l.Visibility = Visibility.Visible;
                        flag = true;
                    }
                }

                if (!flag && !alreadyUsed.Contains(CharList.SelectedItem))
                {
                    WrongBox.Text += " " + (char)CharList.SelectedItem;
                    DrawOneStep();
                    //System.Media.SystemSounds.Asterisk.Play();
                }
                if (CharList.SelectedItem != null)
                {
                    if (alreadyUsed.Contains(CharList.SelectedItem))
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show("Vous avez déjà utilisé cette lettre", "Recommencer la partie?", MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            SwapButtonState();
                            ClearTable();
                        }
                    }
                    else
                    {
                        alreadyUsed.Add(CharList.SelectedItem);
                    }
                    //CharList.Items.Remove(CharList.SelectedItem);
                }
            }

            if (HasWon(ListofLabel))
            {
                _ = MessageBox.Show("Fin de la partie! Appuyer sur Début pour rejouer!");
                SwapButtonState();
                ClearTable();
            }
        }


        private void DrawOneStep()
        {
            if (WrongGuesses < MaximumGuess)
            {
                if (TheHangMan[WrongGuesses].Visibility == Visibility.Hidden)
                {
                    TheHangMan[WrongGuesses].Visibility = Visibility.Visible;
                    WrongGuesses++; /* We add one wrong guess when we draw. */
                    ShowWrongGuesses.Content = MaximumGuess - WrongGuesses;
                    if (WrongGuesses >= MaximumGuess)
                    {
                        _ = MessageBox.Show("Vous avez perdu!");
                        _ = MessageBox.Show("Dommage!");
                        _ = MessageBox.Show("Le mot était " + WordNow);
                        _ = MessageBox.Show("Mais vous avez utilisé les lettres :");
                        var continueLoop = true;
                        while (continueLoop) {
                            DisplayWrongLetters();
                            MessageBoxResult messageBoxResult = MessageBox.Show("Confirmation de choix", "Revoir les lettres?", MessageBoxButton.YesNo);
                            continueLoop = messageBoxResult == MessageBoxResult.Yes;
                        }
                        
                        _ = MessageBox.Show("Appuyez sur Début pour rejouer!");
                        SwapButtonState();
                        ClearTable();
                    }
                }
            }
        }

        private void DisplayWrongLetters()
        {
            foreach (var letter in WrongBox.Text.Split())
            {
                if (!string.IsNullOrEmpty(letter))
                    _ = MessageBox.Show(letter);
            }
        }

        private void AboutButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Confirmation de choix", "Ne pas ouvrir la fenêtre", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                return;
            }
            AboutWindow abw = new AboutWindow();
            abw.Show();
            About.Visibility = Visibility.Hidden;

        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
