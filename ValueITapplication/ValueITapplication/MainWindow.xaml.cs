namespace ValueITapplication
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    /// <summary>
    /// Used to create a list of users
    /// </summary>
    public class Person
    {
        private string _name;

        public string Name
        {
            get { return _name; }

            set { _name = value; }
        }
    }

    public partial class MainWindow : Window
    {
        
        private SqlCommand com = new SqlCommand();
        private DataTable dt = new DataTable();
        private Util tool = new Util();
        private ObservableCollection<Person> Names;

        
        

        private string col;
        private string nm;
        private string sort;
        private string view;
        private string selVal;
        private bool timecardb;

        public MainWindow()
        {       

            Names = new ObservableCollection<Person>();

            bool inConn = tool.CheckConnection();
            bool dbConn = com.useDb();

            this.InitializeComponent();

            com.useDb();

            this.Visibility = Visibility.Visible;
            this.ShowInTaskbar = true;
            this.WindowState = WindowState.Normal;

            if (dbConn == false)
            {
                nserv.Visibility = Visibility.Visible;
            }

            if (inConn == false)
            {
                inconlab.Visibility = Visibility.Visible;
            }
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

        private void setupAdmin()
        {
            dt.Clear();
            dt = com.selectEmployee();
            tabempgrid.ItemsSource = dt.DefaultView;

            dt = com.SelectAllTime();
            tabtimeData.ItemsSource = dt.DefaultView;

            dt = com.SelectAllProject();
            tabproddata.ItemsSource = dt.DefaultView;

        }

        private void setupUser()
        {
            dt = com.SelectAllTask();
            emptaskdata.ItemsSource = dt.DefaultView;

            dt = com.SelectAllProject();
            emproddata.ItemsSource = dt.DefaultView;

            dt = com.SelectTime(nm);
            data.ItemsSource = dt.DefaultView;
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

            if (dt.Rows.Count == 0){
            
                this.errorLabel.Visibility = Visibility.Visible;
                this.usr_n.Clear();
                this.user_password.Clear();
            }
            else {
            
                string cl = dt.Rows[0][3].ToString();
                this.nm = dt.Rows[0][0].ToString();
                this.nserv.Visibility = Visibility.Collapsed;

                // Normal User
                if (cl == "1")
                {
                    this.curr_usr.Content = "You are currently signed in as " + this.nm + ".";
                    this.logInGrid.Visibility = Visibility.Collapsed;
                    this.logIn.Visibility = Visibility.Collapsed;

                    this.logoff.Visibility = Visibility.Visible;
                    this.ClockIBtn.Visibility = Visibility.Visible;
                    this.ClockOutBtn.Visibility = Visibility.Visible;
                    this.emptool.Visibility = Visibility.Visible;
                    this.timgrid.Visibility = Visibility.Visible;

                    setupUser();
                }

                // More Privilaged User not in use
                else if (cl == "2")
                {
                    this.curr_usr.Content = "You are currently signed in as " + this.nm + ".";
                    this.logInGrid.Visibility = Visibility.Collapsed;
                    this.logIn.Visibility = Visibility.Collapsed;
                    this.logInGrid.Visibility = Visibility.Collapsed;
                    this.logIn.Visibility = Visibility.Collapsed;

                    this.logoff.Visibility = Visibility.Visible;
                    this.AdminPanel.Visibility = Visibility.Visible;
                }

                //Administrative User
                else if (cl == "3")
                {
                    this.curr_usr.Content = "You are currently signed in as " + this.nm + ".";
                    this.logInGrid.Visibility = Visibility.Collapsed;
                    this.logIn.Visibility = Visibility.Collapsed;
                    this.logInGrid.Visibility = Visibility.Collapsed;
                    this.logIn.Visibility = Visibility.Collapsed;

                    this.logoff.Visibility = Visibility.Visible;
                    this.AdminPanel.Visibility = Visibility.Visible;
                    this.AdminPanel.Visibility = Visibility.Visible;
                    this.tabCon.Visibility = Visibility.Visible;

                    setupAdmin();

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
            this.emptool.Visibility = Visibility.Collapsed;
            this.curr_usr.Visibility = Visibility.Collapsed;
            this.ClockIBtn.Visibility = Visibility.Collapsed;
            this.ClockOutBtn.Visibility = Visibility.Collapsed;
            this.data.Visibility = Visibility.Collapsed;
            this.admingrid.Visibility = Visibility.Collapsed;
            this.AdminPanel.Visibility = Visibility.Collapsed;
            this.data2.Visibility = Visibility.Collapsed;
            this.slab1.Visibility = Visibility.Collapsed;
            this.emproddata.Visibility = Visibility.Collapsed;

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

        // Clock In Button Event handler
        private void ClockIBtn_Click(object sender, RoutedEventArgs e)
        {
            this.em1.Visibility = Visibility.Collapsed;
            this.name.Visibility = Visibility.Collapsed;
            this.userName.Visibility = Visibility.Collapsed;
            this.password.Visibility = Visibility.Collapsed;
            this.level.Visibility = Visibility.Collapsed;
            this.em.Visibility = Visibility.Collapsed;
            this.tiLabel.Visibility = Visibility.Collapsed;

            this.data.Visibility = Visibility.Visible;

            this.com.ClockIn(nm);
            dt = this.com.SelectTime(nm);
            this.data.ItemsSource = dt.DefaultView;

        }

        // CLock out button event handler
        private void ClockOutBtn_Click(object sender, RoutedEventArgs e)
        {
            this.em1.Visibility = Visibility.Collapsed;
            this.name.Visibility = Visibility.Collapsed;
            this.userName.Visibility = Visibility.Collapsed;
            this.password.Visibility = Visibility.Collapsed;
            this.level.Visibility = Visibility.Collapsed;
            this.data.Visibility = Visibility.Visible;

            this.com.ClockOut();
            dt = this.com.SelectTime(nm);
            this.data.ItemsSource = dt.DefaultView;
        }

        // 1st Administrative tools button
        private void AdminTools_Click(object sender, RoutedEventArgs e)
        {
            
            this.usrpan.Visibility = Visibility.Collapsed;
            this.data3.Visibility = Visibility.Collapsed;
            this.vwTime.Visibility = Visibility.Collapsed;
            this.data2.Visibility = Visibility.Collapsed;
            this.back2.Visibility = Visibility.Collapsed;
            this.bugEnter.Visibility = Visibility.Collapsed;
            this.empGrid.Visibility = Visibility.Collapsed;
            this.emppan.Visibility = Visibility.Collapsed;
            this.sqlpan.Visibility = Visibility.Collapsed;
            this.bugPanel.Visibility = Visibility.Collapsed;
            this.empAddGrid.Visibility = Visibility.Collapsed;
            this.txtpan.Visibility = Visibility.Collapsed;
            this.labpan.Visibility = Visibility.Collapsed;
            this.sqlpan.Visibility = Visibility.Collapsed;
            this.projectScheduleGrid.Visibility = Visibility.Collapsed;
            this.taskScheduleGrid.Visibility = Visibility.Collapsed;
            this.tabCon.Visibility = Visibility.Collapsed;

            this.bugR.Visibility = Visibility.Visible;
        }
        
        // 2nd Administrative Projects Button
        private void projSched_Click(object sender, RoutedEventArgs e)
        {
            this.slab1.Visibility = Visibility.Collapsed;
            this.bugR.Visibility = Visibility.Collapsed;
            this.empGrid.Visibility = Visibility.Collapsed;
            this.usrpan.Visibility = Visibility.Collapsed;
            this.sqls.Visibility = Visibility.Collapsed;
            this.data3.Visibility = Visibility.Collapsed;
            this.vwTime.Visibility = Visibility.Collapsed;
            this.data2.Visibility = Visibility.Collapsed;
            this.back2.Visibility = Visibility.Collapsed;
            this.bugPanel.Visibility = Visibility.Collapsed;
            this.bugEnter.Visibility = Visibility.Collapsed;
            this.projectScheduleGrid.Visibility = Visibility.Collapsed;
            this.taskScheduleGrid.Visibility = Visibility.Collapsed;
            this.empAddGrid.Visibility = Visibility.Collapsed;
            this.txtpan.Visibility = Visibility.Collapsed;
            this.labpan.Visibility = Visibility.Collapsed;
            this.sqlpan.Visibility = Visibility.Collapsed;
            this.tabCon.Visibility = Visibility.Collapsed;

            this.emppan.Visibility = Visibility.Visible;
        }
        
        // 3rd Administrative Button user Control
        private void AdminT_Click(object sender, RoutedEventArgs e)
        {
            this.sqlpan.Visibility = Visibility.Collapsed;
            this.sqls.Visibility = Visibility.Collapsed;
            this.data2.Visibility = Visibility.Collapsed;
            this.bugR.Visibility = Visibility.Collapsed;
            this.bugPanel.Visibility = Visibility.Collapsed;
            this.data3.Visibility = Visibility.Collapsed;
            this.vwTime.Visibility = Visibility.Collapsed;
            this.back2.Visibility = Visibility.Collapsed;
            this.bugEnter.Visibility = Visibility.Collapsed;
            this.sqlbtn.Visibility = Visibility.Collapsed;
            this.slab1.Visibility = Visibility.Collapsed;
            this.empGrid.Visibility = Visibility.Collapsed;
            this.emppan.Visibility = Visibility.Collapsed;
            this.sqls.Visibility = Visibility.Collapsed;
            this.projectScheduleGrid.Visibility = Visibility.Collapsed;
            this.taskScheduleGrid.Visibility = Visibility.Collapsed;
            this.empAddGrid.Visibility = Visibility.Collapsed;
            this.txtpan.Visibility = Visibility.Collapsed;
            this.labpan.Visibility = Visibility.Collapsed;
            this.tabCon.Visibility = Visibility.Collapsed;

            this.admingrid.Visibility = Visibility.Visible;
            this.usrpan.Visibility = Visibility.Visible;
        }

        // 4th Administrative Button Customize
        private void Cust_Click(object sender, RoutedEventArgs e)
        {

            this.bugR.Visibility = Visibility.Collapsed;
            this.bugPanel.Visibility = Visibility.Collapsed;
            this.back2.Visibility = Visibility.Collapsed;
            this.data3.Visibility = Visibility.Collapsed;
            this.vwTime.Visibility = Visibility.Collapsed;
            this.txtpan.Visibility = Visibility.Collapsed;
            this.sqlpan.Visibility = Visibility.Collapsed;
            this.bugEnter.Visibility = Visibility.Collapsed;
            this.data2.Visibility = Visibility.Collapsed;
            this.slab1.Visibility = Visibility.Collapsed;
            this.usrpan.Visibility = Visibility.Collapsed;
            this.empGrid.Visibility = Visibility.Collapsed;
            this.emppan.Visibility = Visibility.Collapsed;
            this.projectScheduleGrid.Visibility = Visibility.Collapsed;
            this.taskScheduleGrid.Visibility = Visibility.Collapsed;
            this.empAddGrid.Visibility = Visibility.Collapsed;
            this.txtpan.Visibility = Visibility.Collapsed;
            this.labpan.Visibility = Visibility.Collapsed;
            this.sqlpan.Visibility = Visibility.Collapsed;
            this.tabCon.Visibility = Visibility.Collapsed;

            this.admingrid.Visibility = Visibility.Visible;
            this.sqls.Visibility = Visibility.Visible;
            this.sqlbtn.Visibility = Visibility.Visible;
        }

        // 5th Administrative Button time Card
        private void timc_Click(object sender, RoutedEventArgs e)
        {
           
            this.sqlpan.Visibility = Visibility.Collapsed;
            this.bugEnter.Visibility = Visibility.Collapsed;
            this.bugR.Visibility = Visibility.Collapsed;
            this.bugPanel.Visibility = Visibility.Collapsed;
            this.back2.Visibility = Visibility.Collapsed;
            this.sqls.Visibility = Visibility.Collapsed;
            this.usrpan.Visibility = Visibility.Collapsed;
            this.sqlpan.Visibility = Visibility.Collapsed;
            this.bugEnter.Visibility = Visibility.Collapsed;
            this.data2.Visibility = Visibility.Collapsed;
            this.slab1.Visibility = Visibility.Collapsed;
            this.empGrid.Visibility = Visibility.Collapsed;
            this.emppan.Visibility = Visibility.Collapsed;
            this.projectScheduleGrid.Visibility = Visibility.Collapsed;
            this.taskScheduleGrid.Visibility = Visibility.Collapsed;
            this.empAddGrid.Visibility = Visibility.Collapsed;
            this.txtpan.Visibility = Visibility.Collapsed;
            this.labpan.Visibility = Visibility.Collapsed;
            this.tabCon.Visibility = Visibility.Collapsed;

            this.data3.Visibility = Visibility.Visible;
            this.vwTime.Visibility = Visibility.Visible;
        }

        // 6th Administrative Button Employee Status
        private void empst_Click(object sender, RoutedEventArgs e)
        {
            this.data3.Visibility = Visibility.Collapsed;
            this.vwTime.Visibility = Visibility.Collapsed;
            this.sqls.Visibility = Visibility.Collapsed;
            this.bugR.Visibility = Visibility.Collapsed;
            this.usrpan.Visibility = Visibility.Collapsed;
            this.sqls.Visibility = Visibility.Collapsed;
            this.sqlbtn.Visibility = Visibility.Collapsed;
            this.bugPanel.Visibility = Visibility.Collapsed;
            this.bugEnter.Visibility = Visibility.Collapsed;
            this.emppan.Visibility = Visibility.Collapsed;
            this.projectScheduleGrid.Visibility = Visibility.Collapsed;
            this.taskScheduleGrid.Visibility = Visibility.Collapsed;
            this.data2.Visibility = Visibility.Collapsed;
            this.empAddGrid.Visibility = Visibility.Collapsed;
            this.txtpan.Visibility = Visibility.Collapsed;
            this.labpan.Visibility = Visibility.Collapsed;
            this.sqlpan.Visibility = Visibility.Collapsed;
            this.tabCon.Visibility = Visibility.Collapsed;

            empGrid.Visibility = Visibility.Visible;
           
            dt = com.selectEmployee();
            this.empDataGrid.ItemsSource = dt.DefaultView;
            dt.Clear();
        }

        // Employee status Task Schedule
        private void schedt_Click(object sender, RoutedEventArgs e)
        {

            titleField1.Clear();
            descField1.Clear();
            nameField1.Clear();

            this.emppan.Visibility = Visibility.Collapsed;


            this.taskScheduleGrid.Visibility = Visibility.Visible;
        }

        // Employee status Project Schedule
        private void schedp_Click(object sender, RoutedEventArgs e)
        {
            titleField2.Clear();
            descField2.Clear();

            this.emppan.Visibility = Visibility.Collapsed;

            this.projectScheduleGrid.Visibility = Visibility.Visible;
        }

        // view users back button
        private void back2_Click(object sender, RoutedEventArgs e)
        {
            this.back2.Visibility = Visibility.Collapsed;
            this.data2.Visibility = Visibility.Collapsed;
            this.usrpan.Visibility = Visibility.Visible;
        }

        //button to send a bug report
        private void bugEnter_Click(object sender, RoutedEventArgs e)
        {
            string title = this.bgTxt.Text;
            string body = this.bg1Txt.Text;

            this.tool.BugReport(title, body);
            this.bugPanel.Visibility = Visibility.Collapsed;
        }

        // Bug report button in administrative tools
        private void bugR_Click(object sender, RoutedEventArgs e)
        {
            this.bugR.Visibility = Visibility.Collapsed;
            this.bugPanel.Visibility = Visibility.Visible;
            this.bugEnter.Visibility = Visibility.Visible;
        }

        // personal Information icon
        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        // time card icon
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        // Create User Button
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string usrnm = this.usrnameField.Text;
            string pass = this.passwordField.Text;
            string lev = this.levelField.Text;

            try
            {
                this.com.NewUsr(col, usrnm, pass, lev);

                this.slab.Visibility = Visibility.Visible;
                this.usrnameField.Clear();
                this.passwordField.Clear();
                this.levelField.Clear();
            }
            catch (Exception ex)
            {
                this.elab.Visibility = Visibility.Visible;
                this.slab.Visibility = Visibility.Collapsed;
            }
        }

        // create user back button
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.slab.Visibility = Visibility.Collapsed;
            this.elab.Visibility = Visibility.Collapsed;
            this.txtpan.Visibility = Visibility.Collapsed;
            this.labpan.Visibility = Visibility.Collapsed;

            this.usrpan.Visibility = Visibility.Visible;

            this.usrnameField.Clear();
            this.passwordField.Clear();
            this.levelField.Clear();
        }

        // Create Task Final Button
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string title = this.titleField1.Text.ToString();
            string desc = this.descField1.Text.ToString();
            string emp = this.nameField1.Text.ToString();
            string i = this.date1.SelectedDate.ToString();
            DateTime d = this.date1.SelectedDate.Value;
            string dt = d.Year.ToString() + "-" + d.Month.ToString() + "-" + d.Day.ToString();

            this.com.CreateTask(title, desc, emp, dt);
            this.slab1.Visibility = Visibility.Visible;
        }
        
        //Create Project final button
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {

            string title = this.titleField2.Text.ToString();
            string desc = this.descField2.Text.ToString();
            DateTime d = this.projDate.SelectedDate.Value;
            string dt = d.Year.ToString() + "-" + d.Month.ToString() + "-" + d.Day.ToString();

            this.com.CreateProject(title, desc, dt);
            this.slab1.Visibility = Visibility.Visible;
        }

        // Create task back button
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            this.projectScheduleGrid.Visibility = Visibility.Collapsed;
            this.taskScheduleGrid.Visibility = Visibility.Collapsed;
            this.slab1.Visibility = Visibility.Collapsed;

            this.emppan.Visibility = Visibility.Visible;
        }

        

        // View combo box
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.data3.Visibility = Visibility.Visible;
            this.view = this.viewSelect.SelectedValue.ToString();
            if (this.viewSelect.SelectedValue.ToString() == "System.Windows.Controls.ComboBoxItem: Week") { 
                dt = this.com.SelectAllTime();
                this.data3.ItemsSource = dt.DefaultView;
            }
            else if (this.viewSelect.SelectedValue.ToString() == "System.Windows.Controls.ComboBoxItem: Month") { 
                dt = this.com.SelectTimeM();
                this.data3.ItemsSource = dt.DefaultView;
            }
            else if (this.viewSelect.SelectedValue.ToString() == "System.Windows.Controls.ComboBoxItem: Year") { 
                dt = this.com.SelectTimeY();
                this.data3.ItemsSource = dt.DefaultView;
            }
        }

        // sort by combo box
        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            this.sort = this.sortSelect.SelectedValue.ToString();
            if (this.sort == "Date")
            {
                if (this.view.Equals("Week"))
                {
                }

                if (this.view == "Month")
                {
                }

                if (this.view == "Year")
                {
                }
            }
            else if (this.sort == "Employee")
            {
                this.sort = "employee";
            }
        }

        

        // 2nd Administrative Tools Button Edit Users
        private void editusr_Click(object sender, RoutedEventArgs e)
        {
            dt = this.com.SelectLogin();
            this.data2.ItemsSource = dt.DefaultView;

            this.date2.Visibility = Visibility.Collapsed;
            this.usrpan.Visibility = Visibility.Collapsed;
            this.empName2.Visibility = Visibility.Collapsed;
            this.ClockinT2.Visibility = Visibility.Collapsed;
            this.ClockOut2T.Visibility = Visibility.Collapsed;
            this.totH2.Visibility = Visibility.Collapsed;
            this.empGrid.Visibility = Visibility.Collapsed;

            this.data2.Visibility = Visibility.Visible;
            this.back2.Visibility = Visibility.Visible;
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

        // New User
        private void newusr_Click(object sender, RoutedEventArgs e)
        {
            this.txtpan.Visibility = Visibility.Visible;
            this.labpan.Visibility = Visibility.Visible;
            this.usrpan.Visibility = Visibility.Collapsed;
        }

        

        private void schedpv_Click(object sender, RoutedEventArgs e)
        {
            this.emppan.Visibility = Visibility.Collapsed;
        }

        

        // Employee stat Task View
        private void schedv_Click(object sender, RoutedEventArgs e)
        {
            this.emppan.Visibility = Visibility.Collapsed;
        }

        private void sqlbtn_Click(object sender, RoutedEventArgs e)
        {
            this.data2.Visibility = Visibility.Visible;
            this.usrpan.Visibility = Visibility.Collapsed;

            try
            {
                this.sqllab.Visibility = Visibility.Collapsed;

                string sqlst = this.sqltxt.Text.ToString();
                dt = this.com.SqlShell(sqlst);
                this.data2.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                this.sqllab.Visibility = Visibility.Visible;
            }
        }

        // SQL shell button
        private void sqls_Click(object sender, RoutedEventArgs e)
        {
            this.sqls.Visibility = Visibility.Collapsed;
            this.sqlpan.Visibility = Visibility.Visible;
        }

        

        // view Users
        private void viewusr_Click(object sender, RoutedEventArgs e)
        {
            dt = this.com.SelectLogin();
            this.data2.ItemsSource = dt.DefaultView;

            this.data2.Visibility = Visibility.Visible;
            this.back2.Visibility = Visibility.Visible;

            this.usrpan.Visibility = Visibility.Collapsed;
            this.bugR.Visibility = Visibility.Collapsed;
            this.bugPanel.Visibility = Visibility.Collapsed;
            this.date2.Visibility = Visibility.Collapsed;
            this.empName2.Visibility = Visibility.Collapsed;
            this.ClockinT2.Visibility = Visibility.Collapsed;
            this.ClockOut2T.Visibility = Visibility.Collapsed;
            this.totH2.Visibility = Visibility.Collapsed;
        }

        

        // Add an employee button
        private void empAdd_Click(object sender, RoutedEventArgs e)
        {
            empGrid.Visibility = Visibility.Collapsed;

            empAddGrid.Visibility = Visibility.Visible;
        }

        // Edit an employee button
        private void empEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        // Remove an employee button
        private void empRemove_Click(object sender, RoutedEventArgs e)
        {
            Window1 win = new Window1();
            win.ShowDialog();

            if (win.remUsr())
            {
                com.DeleteEmployee(selVal);      
                win.Close();
                
            }
        
            else if(win.remUsr() == false)
            {
                win.Close();
            }
         
        }

        // Final add employee button
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            string[] emDt = new string[8];
            emDt[0] = empNamefd.Text.ToString();
            emDt[1] = empTitlefd.Text.ToString();
            emDt[2] = empAddrfd.Text.ToString();
            emDt[3] = empPhoneNumfd.Text.ToString();
            emDt[4] = empEmailfd.Text.ToString();
            
            try
            {
                com.AddEmployee(emDt[0], emDt[1], emDt[2], emDt[3], emDt[4]);

                if (timecardb)
                {
                    emDt[5] = timeCAddnameField.Text.ToString();
                    emDt[6] = timeCAddpwdField.Text.ToString();
                    emDt[7] = timeCAddlevelField.Text.ToString();

                    com.NewUsr(emDt[0], emDt[5], emDt[6], emDt[7]);
                }
            }
            catch (Exception ex)
            {
                empError.Visibility = Visibility.Visible;
                Console.WriteLine(ex.Message);
            }
            

            empSucess.Visibility = Visibility.Visible;
            empError.Visibility = Visibility.Collapsed;
        }

        // Add employee back button
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            empNamefd.Clear();
            empTitlefd.Clear();
            empAddrfd.Clear();
            empPhoneNumfd.Clear();
            empEmailfd.Clear();
            timeCAddnameField.Clear();
            timeCAddpwdField.Clear();
            timeCAddlevelField.Clear();

            empAddGrid.Visibility = Visibility.Collapsed;

            empGrid.Visibility = Visibility.Visible;
        }

        private void empDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (empDataGrid.SelectedCells.Count > 0)
            {
                var info = empDataGrid.SelectedCells[0];
                var slVl = (TextBlock)info.Column.GetCellContent(info.Item);
                selVal = slVl.Text;
                empDataGrid.UnselectAll();
            }

            
        }

        private void usrAddcheck_Checked(object sender, RoutedEventArgs e)
        {
            timeCAddnameField.Visibility = Visibility.Visible;
            timeCAddpwdField.Visibility = Visibility.Visible;
            timeCAddlevelField.Visibility = Visibility.Visible;
            userLab.Visibility = Visibility.Visible;
            passwordLab.Visibility = Visibility.Visible;
            levelLab.Visibility = Visibility.Visible;

            timecardb = true;
        }

        private void usrAddcheck_UNChecked(object sender, RoutedEventArgs e)
        {

            timeCAddnameField.Visibility = Visibility.Collapsed;
            timeCAddpwdField.Visibility = Visibility.Collapsed;
            timeCAddlevelField.Visibility = Visibility.Collapsed;
            userLab.Visibility = Visibility.Collapsed;
            passwordLab.Visibility = Visibility.Collapsed;
            levelLab.Visibility = Visibility.Collapsed;

            timecardb = false;

        }

        //Normal user update tasks Button
        private void TaskupdateBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        //Normal user Customize Button
        private void customizeBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}