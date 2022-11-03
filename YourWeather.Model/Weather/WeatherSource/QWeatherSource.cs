﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using YourWeather.Model.Weather.WeatherResult;
using static System.Net.WebRequestMethods;

namespace YourWeather.Model.Weather.WeatherSource
{
    public class QWeatherSource : IWeatherSource
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Key { get; set; }

        public async Task<List<WeatherForecastDay>?> ForecastDay(double lat, double lon)
        {
            using HttpClient Http = new HttpClient();
            var forecastUrl = $"https://devapi.qweather.com/v7/weather/7d?location={lon},{lat}&key={Key}";
            QWeatherResultForeastDay? forecast = null;
            try
            {
                forecast = await Http.GetFromJsonAsync<QWeatherResultForeastDay>(forecastUrl);
            }
            catch (Exception)
            {

                throw;
            }

            if (forecast is null)
                return null;

            bool state = Convert.ToInt16(forecast.code) != 200;
            if (state)
                return null;

            List<WeatherForecastDay> forecastDays = new();
            foreach (var item in forecast.daily)
            {
                WeatherForecastDay forecastDay = new()
                {
                    Weather = item.textDay,
                    Date = DateTime.ParseExact(item.fxDate, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture),
                    MaxTemp = item.tempMax,
                    MinTemp = item.tempMin
                };
                forecastDays.Add(forecastDay);
            }

            return forecastDays;
        }

        public async Task<List<WeatherForecastHours>?> ForecastHours(double lat, double lon)
        {
            using HttpClient Http = new HttpClient();
            var forecastUrl = $"https://devapi.qweather.com/v7/weather/24h?location={lon},{lat}&key={Key}";
            QWeatherResultForeastHours? forecast = null;
            try
            {
                forecast = await Http.GetFromJsonAsync<QWeatherResultForeastHours>(forecastUrl);
            }
            catch (Exception)
            {

                throw;
            }

            if (forecast is null)
                return null;

            bool state = Convert.ToInt16(forecast.code) != 200;
            if (state)
                return null;

            List<WeatherForecastHours> forecastHours = new();
            foreach (var item in forecast.hourly)
            {
                WeatherForecastHours forecastHour = new()
                {
                    Weather = item.text,
                    DateTime = item.fxTime,
                    Temp = item.temp
                };
                forecastHours.Add(forecastHour);
            }

            return forecastHours;
        }

        public async Task<WeatherLives?> Lives(double lat, double lon)
        {
            using HttpClient Http = new HttpClient();

            QWeatherResultCity? city = await GetCityAsync(lat, lon);

            if (city == null)
                return null;
            
            //获取天气实况
            var livesUrl = $"https://devapi.qweather.com/v7/weather/now?location={lon},{lat}&key={Key}";
            QWeatherResultLives? lives = null;
            try
            {
                lives = await Http.GetFromJsonAsync<QWeatherResultLives>(livesUrl);
            }
            catch (Exception)
            {

                throw;
            }

            if (lives is null)
                return null;

            bool state = Convert.ToInt16(lives.code) != 200;
            if (state)
                return null;

            WeatherLives weatherLives = new WeatherLives()
            {
                City = city.location[0].name,
                Weather = lives.now.text,
                Temp = lives.now.temp,
                WindDeg = lives.now.windDir,
                WindSpeed = lives.now.windSpeed,
                Humidity = lives.now.humidity,
                FeelsLike = lives.now.feelsLike,
                Pressure = lives.now.pressure,
                Visibility = lives.now.vis,
                LastUpdate = lives.updateTime
            };
            return weatherLives;
        }
        public async Task<QWeatherResultCity> GetCityAsync(double lat, double lon)
        {
            using HttpClient Http = new HttpClient();
            var cityUrl = $"https://geoapi.qweather.com/v2/city/lookup?location={lon},{lat}&key={Key}";
            QWeatherResultCity? city = null;
            try
            {
                city = await Http.GetFromJsonAsync<QWeatherResultCity>(cityUrl);
            }
            catch (Exception)
            {

                throw;
            }
            return city;
        }
    }
}
