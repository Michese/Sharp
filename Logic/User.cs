using System;
using System.Collections.Generic;
using System.Text;


namespace Logic
{

    using MySql.Data.MySqlClient;
    using System.Data;

    public class User
    {
        private UInt64 user_id;
        private string name;
        private string login;
        private string password;
        private bool loggedIn;

        public UInt64 User_id { get => user_id; set => user_id = value; }
        public string Name { get => name; set => name = value; }
        public string Login { get => login; set => login = value; }
        public string Password { get => password; set => password = value; }
        public bool LoggedIn { get => loggedIn; set => loggedIn = value; }

        public User()
        {
            this.loggedIn = false;
        }

        public User(UInt64 user_id, string name, string login, string password)
        {
            this.user_id = user_id;
            this.name = name;
            this.login = login;
            this.password = password;
            this.loggedIn = true;
        }

        public static User getUserById(int user_id)
        {
            string sql = $"SELECT user_id, name, login, password FROM users" +
                $" WHERE user_id = {user_id}";

            DataRow row = DataBase.getRow(sql);

            User user = new User(
                (UInt64)row["user_id"],
                (string)row["name"],
                (string)row["login"],
                (string)row["password"]
            );

            return user;
        }

        public static User getUserById(UInt64 user_id)
        {
            string sql = $"SELECT user_id, name, login, password FROM users" +
    $" WHERE user_id = {user_id}";

            DataRow row = DataBase.getRow(sql);

            User user = new User(
                (UInt64)row["user_id"],
                (string)row["name"],
                (string)row["login"],
                (string)row["password"]
            );

            return user;
        }

        public static User getUserByLoginAndPassword(string login, string password)
        {
            string sql = $"SELECT user_id, name, login, password FROM users " +
                $"WHERE login='{login}' AND password='{password}'";
            User user = new User();

            try
            {
                DataRow row = DataBase.getRow(sql);
                user = new User(
                (UInt64)row["user_id"],
                (string)row["name"],
                (string)row["login"],
                (string)row["password"]
            );
            }
            catch (Exception)
            {
                throw new Exception("Неверный логин или пароль!");
            }
            return user;
        }

        public static User create(string name, string login, string password)
        {
            string sql = "SELECT * FROM users";

            DataRow newRow = DataBase.getNewRow(sql);
            newRow["name"] = name;
            newRow["login"] = login;
            newRow["password"] = password;
            DataBase.create(sql, newRow);

            User user = getUserByLoginAndPassword(login, password);
            return user;
        }
    }
}
