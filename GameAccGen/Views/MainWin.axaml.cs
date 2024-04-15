using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Interactivity;
using MySqlConnector;

namespace GameAccGen.Views;

public partial class MainWin : Window
{
    public MySqlConnectionStringBuilder _ConnectionSB;
   
    public MainWin()
    {
        InitializeComponent();
        _ConnectionSB = new MySqlConnectionStringBuilder
        {
            Server = "localhost", 
            Database = "gaminggeneration",
            UserID = "root", 
            Password = "1234",
        };
        // if (Login.Text == null || Password.Text == null)
        // {
        //     LoginButton.IsVisible = false;
        // }
        // else
        // {
        //     LoginButton.IsEnabled = true;
        // }
    }
    #region RegPanel
    private void SignInButton_OnClick(object? sender, RoutedEventArgs e)
    {
        string sql = """
                     insert into user (Name, LastName, BirthDate, Login, PinCode)
                     VALUES  (@name, @lastname, @birthdate, @login, @pincode)
                     """;
        using (var con = new MySqlConnection(_ConnectionSB.ConnectionString))
        {
            con.Open();
            using (var com = con.CreateCommand())
            {
                com.CommandText = sql;
                com.Parameters.Add("@name", (MySqlDbType.String)); 
                com.Parameters["@name"].Value = NameTextBox.Text ; 
                com.Parameters.Add("@lastname", (MySqlDbType.String)); 
                com.Parameters["@lastname"].Value = SurNameTextBox.Text ; 
                com.Parameters.Add("@birthdate", (MySqlDbType.Date)); 
                com.Parameters["@birthdate"].Value = BirthPicker.SelectedDate ; 
                com.Parameters.Add("@login", (MySqlDbType.String)); 
                com.Parameters["@login"].Value = LoginTextBox.Text ; 
                com.Parameters.Add("@pincode", (MySqlDbType.Int32)); 
                com.Parameters["@pincode"].Value = CodeTextBox.Text ; 
                com.ExecuteNonQuery();
            }
            con.Close();
            this.Close();
        }
    }

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        RegPanel.IsVisible = false;
        LoginPanel.IsVisible = true;
    }
    #endregion
    #region LoginPanel
    private void RegButton_OnClick(object? sender, RoutedEventArgs e)
    {
        LoginPanel.IsVisible = false;
        RegPanel.IsVisible = true;
    }
    
    private void Login_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
            (e.Key >= Key.OemMinus && e.Key >= Key.OemPlus))
        {
            e.Handled = true;
        }
    }
    private void LoginButton_OnClick(object? sender, RoutedEventArgs e)
    {


        LoginPanel.IsVisible = false;
        MainPanel.IsVisible = true;
    }
    #endregion
}