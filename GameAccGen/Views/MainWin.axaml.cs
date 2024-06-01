using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Interactivity;
using GameAccGen.Models;
using MySqlConnector;

namespace GameAccGen.Views;

public partial class MainWin : Window
{
    public MySqlConnectionStringBuilder _ConnectionSB;
    private static int userident;
    

    public MainWin()
    { 
        InitializeComponent();
        _ConnectionSB = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            Database = "gameaccgen",
            UserID = "root",
            Password = "1234",
        };
        FillBibl();
    }

    public void FillBibl()
    {
        string sql = "SELECT User, Platform, AccLogin, AccPassword FROM account;";
        using (var con = new MySqlConnection(_ConnectionSB.ConnectionString))
        {
            con.Open();
            { 
                using (var com = con.CreateCommand())
                {
                    com.CommandText = sql;
                    using var reader = com.ExecuteReader();
                    var list = new List<Accountasdkld>();
                    while (reader.Read()) {
                        var item = new Accountasdkld();
                        item.User = reader.GetInt32("User");
                        item.AccLogin = reader.GetString("AccLogin");
                        item.AccPlatform = reader.GetString("Platform");
                        item.AccPassword = reader.GetString("AccPassword");
                        list.Add(item);
                    }

                    AccGrid.ItemsSource = list;
                }
            }
            con.Close();
        }
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
                com.Parameters["@name"].Value = NameTextBox.Text;
                com.Parameters.Add("@lastname", (MySqlDbType.String));
                com.Parameters["@lastname"].Value = SurNameTextBox.Text;
                com.Parameters.Add("@birthdate", (MySqlDbType.Date));
                com.Parameters["@birthdate"].Value = BirthPicker.SelectedDate;
                com.Parameters.Add("@login", (MySqlDbType.String));
                com.Parameters["@login"].Value = LoginTextBox.Text;
                com.Parameters.Add("@pincode", (MySqlDbType.Int32));
                com.Parameters["@pincode"].Value = CodeTextBox.Text;
                com.ExecuteNonQuery();
            }

            con.Close();
            RegPanel.IsVisible = false;
            LoginPanel.IsVisible = true;
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
        string sql = "SELECT UserID, Login, PinCode FROM user WHERE  Login = @login AND PinCode = @pincode";
        
        using (var con = new MySqlConnection(_ConnectionSB.ConnectionString))
        {
            con.Open();
            using (var com = con.CreateCommand())
            {
                com.CommandText = sql;
                com.Parameters.AddWithValue("@login", Login.Text);
                com.Parameters.AddWithValue("@pincode", Password.Text);
        
                using var reader =  com.ExecuteReader();
                while ( reader.Read()) {
                    var login = reader.GetString("Login");
                    var pincode = reader.GetInt32("PinCode");
                    var ID = reader.GetInt32("UserID");
                    userident = ID;
                    if (login == Login.Text && pincode.ToString() == Password.Text) {
                        LoginPanel.IsVisible = false;
                        MainPanel.IsVisible = true;
                    }
                    else
                    {
                        Block.Text = "Неверный логин или пароль";
                    }
                }
            }
            con.Close();
        }
    }
    #endregion



    private void BackToLoginButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.IsVisible = false;
        LoginPanel.IsVisible = true;
    }

    private void AddAnoAccButton_OnClick(object? sender, RoutedEventArgs e)
    {
        string sql = "INSERT INTO account(User, Platform, AccLogin, AccPassword) VALUES (@user, @platform, @acclogin, @accpassword);";

        
        using (var con = new MySqlConnection(_ConnectionSB.ConnectionString))
        {
            con.Open();
            {
                using (var com = con.CreateCommand())
                {
                    com.CommandText = sql;
                    com.Parameters.AddWithValue("@user", userident);
                    com.Parameters.AddWithValue("@platform", PlatformAnoTextBox.Text);
                    com.Parameters.AddWithValue("@acclogin", LoginAcoTextBox.Text);
                    com.Parameters.AddWithValue("@accpassword", PassAcoTextBox.Text);
                    com.ExecuteNonQuery();
                }
            }
            con.Close();
        }
        
    }
}