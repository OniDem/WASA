using Npgsql;
using System;

namespace WASA.Сomplementary
{
    internal class Current_User
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
    }
}
