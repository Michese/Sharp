using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Logic
{
    public class Logic
    {
        private List<Observer> observer;

        public Logic()
        {
            observer = new List<Observer>() { 
                new ConsoleLog(), 
                new FileLog(Config.FILE_LOG) 
            };
        }

        public void observe(string text)
        {
            this.observer.ForEach(delegate (Observer item)
            {
                item.handle(text);
            });
        }

        public bool isRegister(string answer)
        {
            bool result;
            switch (answer)
            {
                case "1":
                    result = true;
                    break;
                case "2":
                    result = false;
                    break;
                default:
                    throw new Exception("Неверное действие!");
            }
            return result;
        }

        public void start()
        {
            User currentUser = new User();

            Console.WriteLine("Выберите действие: ");
            Console.WriteLine("1. Зарегистрироваться");
            Console.WriteLine("2. Войти");

            string answer = Console.ReadLine();
            try
            {
                Console.Clear();
                if (this.isRegister(answer))
                {
                    string name, login, password;

                    Console.WriteLine("Регистрация");
                    Console.Write("Введите имя нового пользователя:");
                    name = Console.ReadLine().Trim();

                    Console.Write("Логин:");
                    login = Console.ReadLine().Trim();

                    Console.Write("Пароль:");
                    password = Console.ReadLine().Trim();

                    if (name.Length == 0 || login.Length == 0 || password.Length == 0)
                    {
                        throw new Exception("Поля не должны быть пустыми!");
                    }

                    if (name.Length > 20 || login.Length > 20 || password.Length > 20)
                    {
                        throw new Exception("Превышенно максимальное значение длины!");
                    }

                    Regex regex = new Regex(@"\s+");
                    if (regex.IsMatch(name) || regex.IsMatch(login) || regex.IsMatch(password))
                    {
                        throw new Exception("Поля не должны содержать пробельные символы!");
                    }

                    regex = new Regex(@"^[a-zA-Z]+[1-9a-zA-Z]*$");
                    if (!regex.IsMatch(login))
                    {
                        throw new Exception("Логин должен содержать только цифры и латинские буквы. И он должен начинаться с буквенного символа!");
                    }

                    if (!regex.IsMatch(name))
                    {
                        throw new Exception("Имя должено содержать только цифры и латинские буквы. И оно должно начинаться с буквенного символа!");
                    }


                    currentUser = User.create(name, login, password);
                }
                else
                {
                    Console.Write("Логин:");
                    string login = Console.ReadLine();

                    Console.Write("Пароль:");
                    string password = Console.ReadLine();

                    currentUser = User.getUserByLoginAndPassword(login, password);
                }
            }
            catch (Exception exp)
            {
                this.observe(exp.Message);
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine($"Добро пожаловать, {currentUser.Name}");

            while (true)
            {

                try
                {
                    Console.WriteLine("Выберите действие:");
                    Console.WriteLine("1) Просмотреть все города;");
                    Console.WriteLine("2) Создать новый город;");
                    Console.WriteLine("3) Изменить существующий город;");
                    Console.WriteLine("4) Удалить существующий город;");
                    Console.WriteLine("0) Выйти");
                    answer = Console.ReadLine();

                    if (answer == "1")
                    {
                        this.showCities(currentUser);
                    }
                    else if (answer == "2")
                    {
                        this.createCity(currentUser);
                    }
                    else if (answer == "3")
                    {
                        this.upadateCity(currentUser);
                    }
                    else if (answer == "4")
                    {
                        this.deleteCity(currentUser);
                    }
                    else if (answer == "0")
                    {
                        Console.WriteLine($"Удачи, {currentUser.Name}!");
                        break;
                    }
                }
                catch (Exception exp)
                {
                    this.observe(exp.Message);
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine("Нажмите любую клавишу, чтобы продолжить!");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void createCity(User user)
        {
            string title;
            UInt32 area, population;

            Console.Write("1) Введите название города: ");
            title = Console.ReadLine();

            Console.Write("2) Введите площадь города: ");
            area = UInt32.Parse(Console.ReadLine());

            Console.Write("3) Введите численность населения города: ");
            population = UInt32.Parse(Console.ReadLine());

            City.create(user, title, area, population);
        }

        private void showCities(User user)
        {
            try
            {
                List<City> cities = City.getAllCityByUserId(user);
                string[] array = new string[] { "ID", "Title", "Area", "Population" };
                Console.WriteLine("{0,5}   |{1,15}   |{2,10}   |{3,10}", array);
                cities.ForEach(delegate (City city)
                {
                    array = new string[] { city.City_id.ToString(), city.Title, city.Area.ToString(), city.Population.ToString() };
                    Console.WriteLine("{0,5}   |{1,15}   |{2,10}   |{3,10}", array);
                });
            }
            catch (Exception)
            {
                throw new Exception("Не получить все города!");
            }
        }

        private void upadateCity(User user)
        {
           while(true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Введите ID города, параматры которого вы хотите изменить");
                    int city_id = Int32.Parse(Console.ReadLine());
                    City city = City.getCityById(city_id);

                    Console.WriteLine("Какой параматр вы хотите изменить?");
                    Console.WriteLine("1) Название;");
                    Console.WriteLine("2) Площадь;");
                    Console.WriteLine("3) Численность населения;");
                    Console.WriteLine("0) Отменить");
                    
                    string answer = Console.ReadLine();
                    if (answer == "1")
                    {
                        Console.Write("Введите новое название города: ");
                        string title = Console.ReadLine();

                        if (title.Length > 10)
                        {
                            throw new Exception("Название города не должно содержать более 10 символов");
                        }

                        city.Title = title;
                        city.update();
                    }
                    else if (answer == "2")
                    {
                        Console.Write("Введите новую площадь города: ");
                        UInt32 area = UInt32.Parse(Console.ReadLine());
                        city.Area = area;
                        city.update();
                    }
                    else if (answer == "3")
                    {
                        Console.Write("Введите новую численность населения города: ");
                        UInt32 population = UInt32.Parse(Console.ReadLine());
                        city.Population = population;
                        city.update();
                    }
                    else if (answer == "0")
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Не удалось изменить город!");
                }
            }

        }

        private void deleteCity(User user)
        {
           while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Введите ID города, которого вы хотите удалить");
                    int city_id = Int32.Parse(Console.ReadLine());
                    City city = City.getCityById(city_id);
                    Console.WriteLine($"Вы уверены, что хотите удалить город {city.Title}?");
                    Console.WriteLine("Введите Y/N");
                    string answer = Console.ReadLine().ToUpper();
                    if (answer == "Y")
                    {
                        city.delete();
                        break;
                    }
                    else if (answer == "N")
                    {
                        Console.WriteLine($"Удаление отменено!");
                        break;
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Не удалось удалить город!");
                }
            }
        }
    }
}