namespace uClock
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data;
    using System.Windows;
    using System.Windows.Threading;

    public partial class MainWindow : Window
    {

        private SqlCommand com = new SqlCommand();
        private DataTable dt = new DataTable();
        private Util tool = new Util();
        private ObservableCollection<Person> Names;

        private string nm;

        public MainWindow()
        {

            Names = new ObservableCollection<Person>();

            bool inConn = tool.CheckConnection();
            bool dbConn = com.useDb();

            this.InitializeComponent();

            this.Visibility = Visibility.Visible;
            this.ShowInTaskbar = true;
            this.WindowState = WindowState.Normal;

            if (dbConn == false) nserv.Visibility = Visibility.Visible;
       
            if (inConn == false) inconlab.Visibility = Visibility.Visible;

            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1);
            dtClockTime.Tick += dtClockTime_Tick;
            dtClockTime.Start();

            string date = tool.CurrDate().ToString();
            dtLabel.Content = date;

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            tiLabel.Content = DateTime.Now.ToLongTimeString();
        }

        // Initial log in button
        private void logIn_Click(object sender, RoutedEventArgs e)
        {
            this.nserv.Visibility = Visibility.Collapsed;
            tiLabel.Visibility = Visibility.Collapsed;
            errorLabel.Visibility = Visibility.Collapsed;
            dtLabel.Visibility = Visibility.Collapsed;

            this.logInGrid.Visibility = Visibility.Visible;
        }

        // Final log in button
        void finSignIn_Click(object sender, RoutedEventArgs e)
        {
            this.errorLabel.Visibility = Visibility.Hidden;
            dt = this.com.Login(this.usr_n.Text, this.user_password.Password);

            if (dt.Rows.Count == 0)
            { 
                this.errorLabel.Visibility = Visibility.Visible;
                this.usr_n.Clear();
                this.user_password.Clear();
            }
            else
            {

                string cl = dt.Rows[0][3].ToString();
                this.nm = dt.Rows[0][0].ToString();
                this.nserv.Visibility = Visibility.Collapsed;

                // Normal User
                if (cl == "1")
                {
                    this.curr_usr.Content = "You are currently signed in as " + this.nm + ".";
                    this.logInGrid.Visibility = Visibility.Collapsed;
                    this.logIn.Visibility = Visibility.Collapsed;

                    this.Employee.Visibility = Visibility.Visible;
                    this.logoff.Visibility = Visibility.Visible;

                    this.Employee.setName(nm);
                }

                // More Privilaged User not in use
                else if (cl == "2")
                {
                    this.curr_usr.Content = "You are currently signed in as " + this.nm + ".";
                    this.logInGrid.Visibility = Visibility.Collapsed;
                    this.logIn.Visibility = Visibility.Collapsed;

                    this.logoff.Visibility = Visibility.Visible;

                }

                //Administrative User
                else if (cl == "3")
                {
                    this.curr_usr.Content = "You are currently signed in as " + this.nm + ".";
                    this.logInGrid.Visibility = Visibility.Collapsed;
                    this.logIn.Visibility = Visibility.Collapsed;

                    Administrator.Visibility = Visibility.Visible;
                    this.logoff.Visibility = Visibility.Visible;
                }
                else
                {
                    this.errorLabel.Visibility = Visibility.Visible;
                }

                this.usr_n.Clear();
                this.user_password.Clear();
            }
        }

        // log off button
        private void logoff_Click(object sender, RoutedEventArgs e)
        {
            this.logoff.Visibility = Visibility.Collapsed;
            this.curr_usr.Visibility = Visibility.Collapsed;
            this.Administrator.Visibility = Visibility.Collapsed;
            this.Employee.Visibility = Visibility.Collapsed;

            tiLabel.Visibility = Visibility.Visible;
            dtLabel.Visibility = Visibility.Visible;
            logIn.Visibility = Visibility.Visible;
        }

        // Cancel Button event Handler
        private void cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            this.logInGrid.Visibility = Visibility.Collapsed;

            tiLabel.Visibility = Visibility.Visible;
            dtLabel.Visibility = Visibility.Visible;
        }

        // Exit button in file
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        // Exit button in file
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        // Settings 
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }   
}