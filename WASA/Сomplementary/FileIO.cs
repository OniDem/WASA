using System.IO;
using System;
using System.Windows;
using Newtonsoft.Json;
using System.Text.Json;
using System.Windows.Markup;
using System.Threading.Tasks;

namespace WASA.Сomplementary
{
    internal class FileIO
    {
        private string _path = "./settings.json";

        /// <summary>
        /// Функция для добавления данных о адресе и порте подключения к БД
        /// </summary>
        /// <param name="name">Название БД</param>
        /// <param name="ip"> IP БД</param>
        /// <param name="port">Port БД</param>
        /// <param name="theme">Тема приложения</param>
        /// <param name="username">Имя пользователя  БД</param>
        /// <param name="password">Пароль пользователя  БД</param>
        public void SetData(string name, string ip, string port, string theme, string username, string password)
        {
            try
            {
                SettingsData data = new(name, ip, port, theme, username, password);

                using (StreamWriter file = File.CreateText(_path))
                {
                    Newtonsoft.Json.JsonSerializer serializer = new();
                    serializer.Serialize(file, data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        


        /// <summary>
        /// Функция для получения строковый данных адреса подключения
        /// </summary>
        /// <param name="get_data">допустимые значения name, ip, port, username, password</param>
        /// <returns></returns>
        public string GetAddressData(string? get_data)
        {
            if (File.Exists(_path) == false)
                File.CreateText(_path);
            SettingsData? data;
            using (StreamReader? file = File.OpenText(_path))
            {
                Newtonsoft.Json.JsonSerializer? serializer = new Newtonsoft.Json.JsonSerializer();
                data = (SettingsData)serializer.Deserialize(file, typeof(SettingsData));
            }
            switch (get_data)
            {
                case "name":
                    return JsonConvert.SerializeObject(data.Name);
                case "ip":
                    return JsonConvert.SerializeObject(data.Ip);
                case "port":
                    return JsonConvert.SerializeObject(data.Port);
                case "username":
                    return JsonConvert.SerializeObject(data.Username);
                case "password":
                    return JsonConvert.SerializeObject(data.Password);

                default:
                    MessageBox.Show($"Error: {get_data}");
                    break;
            }
            return "";
        }

       
        /// <summary>
        /// Функция для получения текущей темы
        /// </summary>
        /// <returns></returns>
        public bool GetThemeData()
        {
            if (File.Exists(_path) == false)
                File.CreateText(_path);

            using (StreamReader file = File.OpenText(_path))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                SettingsData data = (SettingsData)serializer.Deserialize(file, typeof(SettingsData));

                string theme = data.Theme;

                if (theme.Contains('0') == true)
                    return false;
                else
                    return true;
            }
        }

        private class SettingsData
        {            
            public string Name { get; set; }

            //по умолчанию, при потери файла адрес 0.0.0.0
            public string Ip { get; set; }

            //по умолчанию, при потери файла порт 5432
            public string Port { get; set; }

            public string Username { get; set; }

            public string Password { get; set; }

            //по умолчанию, при потере файла 0(0 - светлая, 1 - тёмная
            public string Theme { get; set; }


            public SettingsData () { }

            public SettingsData (string ip, string port)
            {
                Ip = ip;
                Port = port;
            }

            public SettingsData (string theme)
            {
                Theme = theme;
            }

            public SettingsData(string name, string ip, string port, string theme, string username, string password)
            {
                Name = name;
                Ip = ip;
                Port = port;
                Theme = theme;
                Username = username;
                Password = password;
            }
        }
    }

    

}
