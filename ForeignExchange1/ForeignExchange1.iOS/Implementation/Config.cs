using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ForeignExchange1.Interfaces;
using Foundation;
using SQLite.Net.Interop;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ForeignExchange1.iOS.Implementation.Config))]

namespace ForeignExchange1.iOS.Implementation
{
    public class Config  : IConfig
    {
        private string directoryDB;
        private ISQLitePlatform platform;

        public string DirectoryDB
        {
            get
            {
                if (string.IsNullOrEmpty(directoryDB))
                {
                    var directory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    directoryDB = System.IO.Path.Combine(directory, "..", "Library");
                }

                return directoryDB;
            }
        }

       

        public ISQLitePlatform Platfrom
    {
            get
            {
                if (platform == null)
                {
                    platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
                }

                return platform;
            }
        }

    }
}