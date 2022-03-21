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

    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            this.WindowStyle = WindowStyle.None;
        }

        private void AboutButtonClick(object sender, RoutedEventArgs e)
        {
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
            TboxComment.Text = TboxMail.Text = TboxName.Text = "";
        }

        private void Envoyer_Click(object sender, RoutedEventArgs e)
        {
            var errorMsg = "";
            if (TboxName.Text.Length < 5)
            {
                errorMsg = "Le nom doit faire plus de 5 caractères";
            }
            if (TboxMail.Text.Length < 5)
            {
                errorMsg = "Le mail doit faire plus de 5 caractères";
            }
            if (!TboxMail.Text.Contains("@gmail.com"))
            {
                errorMsg = "Le mail doit être un gmail.com";
            }
            if (!TboxMail.Text.Contains("@"))
            {
                errorMsg = "Le mail doit contenir le symbole @";
            }
            if (TboxComment.Text.Length < 30)
            {
                errorMsg = "Le commentaire doit faire plus de 30 caractères";
            }
            if (!TboxComment.Text.Contains(TboxName.Text))
            {
                errorMsg = "Le commentaire doit contenir le nom";
            }
            if (!TboxMail.Text.Contains(TboxName.Text))
            {
                errorMsg = "Le mail doit contenir le nom";
            }
            if (errorMsg != "")
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(errorMsg, "Il y a une erreur, voulez vous réinitialiser la totalité?", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    Reinit_Click(sender, e);
                    return;
                }
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(errorMsg, "Il y a pas d'erreur, voulez vous réinitialiser la totalité quand même?", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    Reinit_Click(sender, e);
                    return;
                }
                else
                {
                    _ = MessageBox.Show(errorMsg, "Message envoyé avec succès!");
                }
            }
        }
    }
}
