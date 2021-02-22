using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Logic
{
    public class City
    {
        private UInt64 city_id;
        private User user_id;
        private string title;
        private UInt32 area;
        private UInt32 population;

        public UInt64 City_id { get => city_id; set => city_id = value; }
        public string Title { get => title; set => title = value; }
        public UInt32 Area { get => area; set => area = value; }
        public UInt32 Population { get => population; set => population = value; }
        public User User_id { get => user_id; set => user_id = value; }

        public City(){}

        public City(UInt64 city_id, User user_id, string title, UInt32 area, UInt32 population)
        {
            this.city_id = city_id;
            this.user_id = user_id;
            this.title = title;
            this.area = area;
            this.population = population;
        }

        public static List<City> getAllCityByUserId(User user)
        {
            string sql = "SELECT city_id, title, area, population FROM cities " +
                $"WHERE user_id={user.User_id}";
            List<City> cities = new List<City>();
            DataRowCollection rows = DataBase.getRows(sql);
            foreach (DataRow row in rows)
            {
                cities.Add(new City(
                    (UInt64)row["city_id"],
                    user,
                    (string)row["title"],
                    (UInt32)row["area"],
                    (UInt32)row["population"]
                    ));
            }
            return cities;
        }

        public static void create(User user, string title, UInt32 area, UInt32 population)
        {
            string sql = "SELECT * FROM cities";
            DataRow newRow = DataBase.getNewRow(sql);
            newRow["title"] = title;
            newRow["area"] = area;
            newRow["population"] = population;
            newRow["user_id"] = user.User_id;
            DataBase.create(sql, newRow);
        }

        public static City getCityById(int city_id)
        {
            string sql = $"SELECT city_id, user_id, title, area, population FROM cities " +
                $"WHERE city_id = {city_id}";

            DataRow row = DataBase.getRow(sql);

            City city = new City(
                (UInt64)row["city_id"],
                User.getUserById((UInt64)row["user_id"]),
                (string)row["title"],
                (UInt32)row["area"],
                (UInt32)row["population"]
            );

            return city;
        }

        public void update()
        {
            string sql = $"UPDATE cities SET " +
                $"title='{this.title}', " +
                $"area={this.area}, " +
                $"population={this.population} " +
                $"WHERE city_id = {this.city_id}";
            DataBase.query(sql);
        }

        public void delete()
        {
            string sql = $"DELETE FROM cities " +
                $"WHERE city_id = {this.city_id}";
            DataBase.query(sql);
        }
    }
}
