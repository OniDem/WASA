using Npgsql;
using System;
using System.Windows;

namespace WASA.Сomplementary
{
    internal class UserInfo
    {
        NpgsqlCommand? command;
        public string GetCurrenUser()
        {
            NpgsqlConnection con = new NpgsqlConnection(Connection.GetConnectionString());
            con!.Open();
            command = new NpgsqlCommand($"SELECT seller FROM settings WHERE settings_id = 1", con);
            string? user = Convert.ToString(command.ExecuteScalar());
            con.Close();
            return user!;
            
        }

        public string GetUserRole()
        {
                NpgsqlConnection con = new NpgsqlConnection(Connection.GetConnectionString());
                con!.Open();
                command = new NpgsqlCommand($"SELECT seller FROM settings WHERE settings_id = 1", con);
                string? user = Convert.ToString(command.ExecuteScalar());
                command = new NpgsqlCommand($"SELECT user_role FROM users WHERE user_name = '{user}'", con);
                string? user_role = Convert.ToString(command.ExecuteScalar());
                con.Close();
                return user_role!;
        }
    }
}
