using Npgsql;
using System;

namespace WASA.Сomplementary
{
    internal class UserInfo
    {
        NpgsqlCommand? command;
        public string GetCurrentUser()
        {
            NpgsqlConnection con = new NpgsqlConnection(Connection.GetConnectionString());
            con!.Open();
            command = new NpgsqlCommand($"SELECT seller FROM settings WHERE settings_id = 1", con);
            string? user = Convert.ToString(command.ExecuteScalar());
            con.Close();
            return user!;

        }

        public string GetUserRole(string user)
        {
            NpgsqlConnection con = new NpgsqlConnection(Connection.GetConnectionString());
            con!.Open();
            command = new NpgsqlCommand($"SELECT user_role FROM users WHERE user_name = '{user}'", con);
            string? user_role = Convert.ToString(command.ExecuteScalar());
            con.Close();
            return user_role!;
        }
    }
}
