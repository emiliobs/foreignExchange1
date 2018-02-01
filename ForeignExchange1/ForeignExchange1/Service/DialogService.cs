using System.Net.Mime;
using System.Threading.Tasks;
using ForeignExchange1.Helpars;
using Xamarin.Forms;

namespace ForeignExchange1.Service
{
    public class DialogService
    {
        public async Task ShowMessage(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, Languages.Accept);
        }
    }
}
