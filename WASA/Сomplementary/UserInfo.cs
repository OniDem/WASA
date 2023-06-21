using Npgsql;
using System;
using System.Windows;

namespace WASA.Сomplementary
{
    internal class UserInfo
    {
        NpgsqlCommand? command;
        private string _current_user = "";
        public void SetCurrenUser(string user)
        {
            _current_user = user;
            
        }

        public string GetCurrentUser()
        {
            return _current_user;
        }

        public string GetUserRole()
        {
                NpgsqlConnection con = new NpgsqlConnection(Connection.GetConnectionString());
                con!.Open();
                command = new NpgsqlCommand($"SELECT user_role FROM users WHERE user_name = '{_current_user}'", con);
                string? user_role = Convert.ToString(command.ExecuteScalar());
                con.Close();
                return user_role!;
        }
    }
}
