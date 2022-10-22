﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourWeather.IService
{
    public interface IThemeService
    {
        bool IsDark();
        void ThemeChanged();
        event Action Onchange;
    }
}
