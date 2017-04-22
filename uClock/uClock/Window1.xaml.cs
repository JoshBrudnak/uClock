using System.ComponentModel;
using System.Windows;

namespace uClock
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        public bool remUser;

        public Window1()
        {
            InitializeComponent();
        }

        public void closeWin1()
        {
            this.Close();
        }

        public bool remUsr()
        {
            return remUser;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;

            this.Visibility = Visibility.Collapsed;
        }

        private void canBtn_Click(object sender, RoutedEventArgs e)
        {
            remUser = false;

            this.Visibility = Visibility.Hidden;
        }

        private void yesBtn_Click(object sender, RoutedEventArgs e)
        {
            remUser = true;

            this.Visibility = Visibility.Hidden;
        }
    }
}
