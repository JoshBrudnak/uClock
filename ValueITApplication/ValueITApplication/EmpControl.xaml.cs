﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace ValueITApplication
{
    /// <summary>
    /// Interaction logic for EmpControl.xaml
    /// </summary>
    public partial class EmpControl : UserControl
    {
        SqlCommand com = new SqlCommand();
        DataTable dt = new DataTable();
        private string nm;

        public EmpControl()
        {
            InitializeComponent();
        }

        public EmpControl(string name)
        {
            InitializeComponent();

            nm = name;
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

        // Clock In Button Event handler
        private void ClockIBtn_Click(object sender, RoutedEventArgs e)
        {
            this.em1.Visibility = Visibility.Collapsed;
            this.name.Visibility = Visibility.Collapsed;
            this.userName.Visibility = Visibility.Collapsed;
            this.password.Visibility = Visibility.Collapsed;
            this.level.Visibility = Visibility.Collapsed;
            
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
        private void TaskupdateBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void customizeBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
