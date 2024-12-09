using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class PlaneOrTrain
{
    public static void Main(string[] args)
    {
        Dictionary<string, List<string>> cityData = ReadCityData("cities.txt");
        List<string> allCities = cityData.Keys.ToList();

        Console.WriteLine("Доступные города: " + string.Join(", ", allCities));

        Console.WriteLine("\nВведите город откуда Вы хотите сбежать:");
        string cityFrom = Console.ReadLine().Trim().ToLower();

        Console.WriteLine("Введите город куда Вы хотите прибежать:");
        string cityTo = Console.ReadLine().Trim().ToLower();

        bool cityFromExists = cityData.Any(x => x.Key.ToLower() == cityFrom);
        bool cityToExists = cityData.Any(x => x.Key.ToLower() == cityTo);

        if (cityFromExists && cityToExists)
        {
            Console.WriteLine($"\nБилет из {cityFrom} в {cityTo}:");
            Random random = new Random();
            string transportType = random.Next(2) == 0 ? "Самолет" : "Поезд";
            int numTransfers = random.Next(6); 
            string flightType = numTransfers == 0 ? "Прямой" : $"С {numTransfers} пересадками";
            Console.WriteLine($"  Транспорт: {transportType}");
            Console.WriteLine($"  Тип маршрута: {flightType}");
            Console.WriteLine($"  Цена: {random.Next(1000, 10000)} руб.");

            Console.WriteLine("\nКушайте вот что:");
            string correctCityFrom = cityData.FirstOrDefault(x => x.Key.ToLower() == cityFrom).Key;
            string correctCityTo = cityData.FirstOrDefault(x => x.Key.ToLower() == cityTo).Key;
            Console.WriteLine($"  {correctCityFrom}: {cityData[correctCityFrom][0]}");
            Console.WriteLine($"  {correctCityTo}: {cityData[correctCityTo][0]}");

            if (numTransfers > 0)
            {
                Console.WriteLine("\nМаршрут:");
                string route = correctCityFrom;
                List<string> transferCities = new List<string>();

                for (int i = 0; i < numTransfers; i++)
                {
                    string transferCity = allCities[random.Next(allCities.Count)];
                    route += $" >> {transferCity}";
                    transferCities.Add(transferCity);
                }
                route += $" >> {correctCityTo}";
                Console.WriteLine(route);

                Console.WriteLine("\nПерекусите в городах пересадок этим:");
                foreach (string city in transferCities)
                {
                    if (cityData.ContainsKey(city))
                    {
                        Console.WriteLine($"  В {city} вы можете перекусить {cityData[city][0]}");
                    }
                    else
                    {
                        Console.WriteLine($"  Информация о городе {city} отсутствует.");
                    }
                }
            }
        }
        else
        {
            string message = "";
            if (!cityFromExists) message += "Город отправления не найден в базе данных. ";
            if (!cityToExists) message += "Город прибытия не найден в базе данных.";
            Console.WriteLine(message);
        }
    }

    static Dictionary<string, List<string>> ReadCityData(string filePath)
    {
        Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length >= 2)
                {
                    string city = parts[1];
                    string food = parts[2];
                    if (!data.ContainsKey(city))
                    {
                        data[city] = new List<string>();
                    }
                    data[city].Add(food);
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Файл {filePath} не найден.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
        }
        return data;
    }
}