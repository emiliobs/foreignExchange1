using ForeignExchange1.Interfaces;
using SQLite.Net.Interop;
using Xamarin.Forms;
using Environment = System.Environment;

[assembly: Dependency(typeof(ForeignExchange1.Droid.Implementations.Config))]

namespace ForeignExchange1.Droid.Implementations
{
    public class Config : IConfig
   {
       private string directoryDB;
       private ISQLitePlatform platform;

       public string DirectoryDB
       {
           get
           {
               if (string.IsNullOrEmpty(directoryDB))
               {
                   directoryDB = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
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
                   platform = new  SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
               }

               return platform;
           }
       }
    }
}