﻿using Npgsql;
using System;
using System.Windows;
using WASA.Сomplementary;

namespace WASA
{
    /// <summary>
    /// Interaction logic for HelloWindow.xaml
    /// </summary>
    public partial class HelloWindow : Window
    {

        NpgsqlConnection? con;
        NpgsqlCommand command;
        string ver = "1.3";
        UserInfo userInfo;
        public HelloWindow()
        {
            InitializeComponent();
            version.Content = ver;
            try
            {
                con = new NpgsqlConnection(Connection.GetConnectionString());
                con.Open();
                NpgsqlCommand command = new NpgsqlCommand($"SELECT version FROM settings WHERE settings_id=1;", con);
                if (command.ExecuteScalar()!.ToString() != ver)
                {
                    MessageBox.Show("У вас не актуальная версия");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                con!.Open();
                if (login.Text.Length > 1 && password.Password.Length > 1)
                {
                    command = new NpgsqlCommand($"SELECT user_password FROM users WHERE user_name = '{login.Text}'", con);
                    if (command.ExecuteScalar()!.ToString() == password.Password)
                    {
                        command = new NpgsqlCommand($"SELECT verifided FROM users WHERE user_name = '{login.Text}'", con);
                        bool verifided = Convert.ToBoolean(command.ExecuteScalar()!);
                        if (verifided == true)
                        {
                            userInfo.SetCurrenUser(login.Text);
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            Close();
                        }
                        else
                        {
                            Close();
                            MessageBox.Show("Ваша учётная запись не верифицирована, обратитесь к администратору!");
                        }
                    }
                    else
                        MessageBox.Show("Неккоректные данные!");
                }
                else
                    MessageBox.Show("Одно или оба поля пустые!");
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Create_User_Click(object sender, RoutedEventArgs e)
        {
            Reg_Window reg_Window = new Reg_Window();
            reg_Window.Show();
            Close();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
