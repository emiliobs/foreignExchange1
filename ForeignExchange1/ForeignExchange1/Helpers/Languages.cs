using System;
using System.Collections.Generic;
using System.Text;
using ForeignExchange1.Interfaces;
using ForeignExchange1.Resources;
using Xamarin.Forms;

namespace ForeignExchange1.Helpars
{
    public class Languages
    {

        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }

        }

        public static string AmountValidation
        {
            get { return Resource.AmountValidation; }
        }
        public static string Error
        {
            get { return Resource.Error; }
        }
        public static string Title
        {
            get { return Resource.Title; }
        }

    }
}
