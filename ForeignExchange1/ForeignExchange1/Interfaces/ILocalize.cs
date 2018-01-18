using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ForeignExchange1.Interfaces
{
    public interface ILocalize
    {

        //dos métodos
        CultureInfo GetCurrentCultureInfo();
        void SetLocale(CultureInfo ci);
    }
}
