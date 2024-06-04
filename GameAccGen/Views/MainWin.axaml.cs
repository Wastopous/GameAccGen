using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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
                com.Parameters.AddWithValue("@login", LoginField.Text);
                com.Parameters.AddWithValue("@pincode", PasswordField.Text);
        
                using var reader =  com.ExecuteReader();
                while ( reader.Read()) {
                    var login = reader.GetString("Login");
                    var pincode = reader.GetInt32("PinCode");
                    var ID = reader.GetInt32("UserID");
                    userident = ID;
                    if (login == LoginField.Text && pincode.ToString() == PasswordField.Text) {
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
private string GenerateNickName()
    {
        string inputText = NickMesTextBox.Text.ToLower();

        Random r = new Random();
        StringBuilder result = new StringBuilder();

        foreach (char character in inputText)
        {
            switch (character)
            {
                case ' ':
                    int choice_ = r.Next(6);
                    switch (choice_)
                    {
                        case 0:
                            result.Append("_");
                            break;
                        case 1:
                            result.Append("\ud83d\ude3b");
                            break;
                        case 2:
                            result.Append("\ud83e\udd20");
                            break;
                        case 3:
                            result.Append("༻");
                            break;
                        case 4:
                            result.Append("ღ");
                            break;
                        case 5:
                            result.Append("꧂");
                            break;
                    }

                    break;
                case 'а':
                    int choiceA = r.Next(2);
                    switch (choiceA)
                    {
                        case 0:
                            result.Append("A");
                            break;
                        case 1:
                            result.Append("@");
                            break;
                    }

                    break;
                case 'б':
                    int choiceB = r.Next(2);
                    switch (choiceB)
                    {
                        case 0:
                            result.Append("G");
                            break;
                        case 1:
                            result.Append("6");
                            break;
                    }

                    break;
                case 'в':
                    int choiceV = r.Next(2);
                    switch (choiceV)
                    {
                        case 0:
                            result.Append("B");
                            break;
                        case 1:
                            result.Append("BB");
                            break;
                    }

                    break;
                case 'г':
                    int choiceG = r.Next(2);
                    switch (choiceG)
                    {
                        case 0:
                            result.Append("r");
                            break;
                        case 1:
                            result.Append("7");
                            break;
                    }

                    break;
                case 'д':
                    int choiceD = r.Next(2);
                    switch (choiceD)
                    {
                        case 0:
                            result.Append("D");
                            break;
                        case 1:
                            result.Append("ᗪ");
                            break;
                    }

                    break;
                case 'е':
                    int choiceE = r.Next(2);
                    switch (choiceE)
                    {
                        case 0:
                            result.Append("е");
                            break;
                        case 1:
                            result.Append("Ɇ");
                            break;
                    }

                    break;
                case 'ё':
                    int choiceYO = r.Next(2);
                    switch (choiceYO)
                    {
                        case 0:
                            result.Append("e^");
                            break;
                        case 1:
                            result.Append("$");
                            break;
                    }

                    break;
                case 'ж':
                    int choiceGE = r.Next(2);
                    switch (choiceGE)
                    {
                        case 0:
                            result.Append("jk");
                            break;
                    }

                    break;
                case 'з':
                    int choiceZ = r.Next(2);
                    switch (choiceZ)
                    {
                        case 0:
                            result.Append("Z");
                            break;
                        case 1:
                            result.Append("乙\n");
                            break;
                    }

                    break;
                case 'и':
                    int choiceI = r.Next(3);
                    switch (choiceI)
                    {
                        case 0:
                            result.Append("1");
                            break;
                        case 1:
                            result.Append("I");
                            break;
                        case 2:
                            result.Append("\ud835\udd5a");
                            break;
                    }

                    break;
                case 'й':
                    int choiceYi = r.Next(2);
                    switch (choiceYi)
                    {
                        case 0:
                            result.Append("u*");
                            break;
                        case 1:
                            result.Append("Y");
                            break;
                    }

                    break;
                case 'к':
                    int choiceK = r.Next(2);
                    switch (choiceK)
                    {
                        case 0:
                            result.Append("K");
                            break;
                        case 1:
                            result.Append("k");
                            break;
                    }

                    break;
                case 'л':
                    int choiceL = r.Next(2);
                    switch (choiceL)
                    {
                        case 0:
                            result.Append("JI");
                            break;
                        case 1:
                            result.Append("L");
                            break;
                    }

                    break;
                case 'м':
                    int choiceM = r.Next(2);
                    switch (choiceM)
                    {
                        case 0:
                            result.Append("M");
                            break;
                        case 1:
                            result.Append("m");
                            break;
                    }

                    break;
                case 'н':
                    int choiceN = r.Next(2);
                    switch (choiceN)
                    {
                        case 0:
                            result.Append("H");
                            break;
                        case 1:
                            result.Append("I-I");
                            break;
                    }

                    break;
                case 'о':
                    int choiceO = r.Next(3);
                    switch (choiceO)
                    {
                        case 0:
                            result.Append("0");
                            break;
                        case 1:
                            result.Append("O");
                            break;
                        case 2:
                            result.Append("o");
                            break;
                    }

                    break;
                case 'п':
                    int choiceP = r.Next(2);
                    switch (choiceP)
                    {
                        case 0:
                            result.Append("TT");
                            break;
                        case 1:
                            result.Append("n");
                            break;
                    }

                    break;
                case 'р':
                    int choiceR = r.Next(2);
                    switch (choiceR)
                    {
                        case 0:
                            result.Append("P");
                            break;
                        case 1:
                            result.Append("R");
                            break;
                    }

                    break;
                case 'с':
                    int choiceC = r.Next(2);
                    switch (choiceC)
                    {
                        case 0:
                            result.Append("C");
                            break;
                        case 1:
                            result.Append("c");
                            break;
                    }

                    break;
                case 'т':
                    int choiceT = r.Next(2);
                    switch (choiceT)
                    {
                        case 0:
                            result.Append("t");
                            break;
                        case 1:
                            result.Append("T");
                            break;
                    }

                    break;
                case 'у':
                    int choiceU = r.Next(2);
                    switch (choiceU)
                    {
                        case 0:
                            result.Append("y");
                            break;
                        case 1:
                            result.Append("j");
                            break;
                    }

                    break;
                case 'ф':
                    int choiceF = r.Next(2);
                    switch (choiceF)
                    {
                        case 0:
                            result.Append("f");
                            break;
                        case 1:
                            result.Append("F");
                            break;
                    }

                    break;
                case 'х':
                    int choiceX = r.Next(2);
                    switch (choiceX)
                    {
                        case 0:
                            result.Append("X");
                            break;
                        case 1:
                            result.Append("x");
                            break;
                    }

                    break;
                case 'ш':
                    int choiceSHE = r.Next(2);
                    switch (choiceSHE)
                    {
                        case 0:
                            result.Append("lll");
                            break;
                        case 1:
                            result.Append("III");
                            break;
                    }

                    break;
                case 'щ':
                    int choiceSHA = r.Next(2);
                    switch (choiceSHA)
                    {
                        case 0:
                            result.Append("llj");
                            break;
                        case 1:
                            result.Append("IIL");
                            break;
                    }

                    break;
                case 'ъ':
                    int choiceTverd = r.Next(2);
                    switch (choiceTverd)
                    {
                        case 0:
                            result.Append("'b");
                            break;
                        case 1:
                            result.Append("'6");
                            break;
                    }

                    break;
                case 'ь':
                    int choiceMagk = r.Next(2);
                    switch (choiceMagk)
                    {
                        case 0:
                            result.Append("b");
                            break;
                        case 1:
                            result.Append("d");
                            break;
                    }

                    break;
                case 'ы':
                    int choiceYI = r.Next(2);
                    switch (choiceYI)
                    {
                        case 0:
                            result.Append("bl");
                            break;
                        case 1:
                            result.Append("bI");
                            break;
                    }

                    break;
                case 'э':
                    int choiceYE = r.Next(1);
                    switch (choiceYE)
                    {
                        case 0:
                            result.Append("3");
                            break;
                    }

                    break;
                case 'ю':
                    int choiceYU = r.Next(2);
                    switch (choiceYU)
                    {
                        case 0:
                            result.Append("u");
                            break;
                        case 1:
                            result.Append("U");
                            break;
                    }

                    break;
                case 'я':
                    int choiceYA = r.Next(2);
                    switch (choiceYA)
                    {
                        case 0:
                            result.Append("9I");
                            break;
                        case 1:
                            result.Append("R");
                            break;
                    }

                    break;
                default:
                    result.Append(character);
                    break;
            }
        }

        return result.ToString();
    }


    private void BackToLoginButton_OnClick(object? sender, RoutedEventArgs e)
    {
        LoginField.Text = null;
        PasswordField.Text = null;
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
        PlatformAnoTextBox.Text = null;
        LoginAcoTextBox.Text = null;
        PassAcoTextBox.Text = null;
    }

    private void NickMesTextBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
            (e.Key == Key.OemMinus && e.Key == Key.OemPlus))
        {
            e.Handled = true;
        }
    }

    private void NickMesAcc_OnClick(object? sender, RoutedEventArgs e)
    {
        string nikGen = GenerateNickName();
        NickMesTextBox.Text = nikGen;
    }

    private void PassMesAcc_OnClick(object? sender, RoutedEventArgs e)
    {
        string passGen = GeneratePassword();
        PassMesTextBox.Text = passGen;
    }
    
    private string GeneratePassword()
    {
        const string upChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowChars = "abcdefghijklmnopqrstuvwxyz";
        const string dgChars = "0123456789";
        const string specChars = "!@#$%&*_?";

        StringBuilder pass = new StringBuilder();
        Random r = new Random();


        for (int i = 0; i < 8; i++)
        {
            pass.Append(lowChars[r.Next(lowChars.Length)]);
        }

        pass.Append(upChars[r.Next(upChars.Length)]);

        for (int i = 0; i < 3; i++)
        {
            pass.Append(dgChars[r.Next(dgChars.Length)]);
        }

        pass.Append(specChars[r.Next(specChars.Length)]);

        return pass.ToString();
    }

    private void AddMesAccButton_OnClick(object? sender, RoutedEventArgs e)
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
                    com.Parameters.AddWithValue("@platform", PlatformMesTextBox.Text);
                    com.Parameters.AddWithValue("@acclogin", LoginMesTextBox.Text);
                    com.Parameters.AddWithValue("@accpassword", PassMesTextBox.Text);
                    com.ExecuteNonQuery();
                }
            }
            con.Close();
        }

        PlatformMesTextBox.Text = null;
        LoginMesTextBox.Text = null;
        PassMesTextBox.Text = null;
        NickMesTextBox.Text = null;
    }

    private void TopLevel_OnOpened(object? sender, EventArgs e)
    {
        LoginPanel.IsVisible = true;
        MainPanel.IsVisible = false;
    }
}