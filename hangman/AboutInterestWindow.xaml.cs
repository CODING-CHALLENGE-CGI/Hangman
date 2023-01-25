using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace hangman
{

    public partial class AboutInterestWindow : Window
    {
        MainMenuWindow Parent { get; set; }
        List<CheckBox> checkBoxes { get; set; } = new List<CheckBox>();

        public AboutInterestWindow(MainMenuWindow parent)
        {
            InitializeComponent();
            checkBoxes = new List<CheckBox>() { Check2, Check3, Check4, Check5, Check6, Check7, Check8, Check9 };
            Parent = parent;
            this.WindowStyle = WindowStyle.None;
        }

        private void AboutButtonClick(object sender, RoutedEventArgs e)
        {
            Parent.FormCompleted = true;
            this.Close();
        }

        private void AboutButtonFullScreenClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void HandleLinkClick(object sender, RoutedEventArgs e)
        {
            Hyperlink hl = (Hyperlink)sender;
            string navigateUri = hl.NavigateUri.ToString();
            Process.Start(new ProcessStartInfo(navigateUri));
            e.Handled = true;
        }

        private void Reinit_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox cb in checkBoxes)
            {
                cb.IsChecked = true;
            }
        }

        private void Envoyer_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxes.Count(m => m.IsChecked == true) == 3)
            {
                TBoxClose.Visibility = Visibility.Visible;
                _ = MessageBox.Show("Bravo vous pouvez fermer cette page");
            }
            else
            {
                MessageBox.Show("Vous devez selectionner 3 préférences.");
            }
        }

        private void Check1_Checked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox cb in checkBoxes)
            {
                cb.IsChecked = true;
            }
        }

        private void Check10_Checked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox cb in checkBoxes)
            {
                cb.IsChecked = false;
            }
        }
    }
}
