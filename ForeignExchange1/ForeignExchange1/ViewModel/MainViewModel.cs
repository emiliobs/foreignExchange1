  
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ForeignExchange1.Annotations;
using ForeignExchange1.Helpars;
using ForeignExchange1.Service;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ForeignExchange1.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.ObjectModel;
    using ForeignExchange1.Models;

    public class MainViewModel : INotifyPropertyChanged
    {
        #region Attributos

         bool _isRunning;
         bool _isEnable;   
         string _result;
         ObservableCollection<Rate> _rates;
        private Rate _sourceRate;
        private Rate _targetRate;
        private string _status;
        private List<Rate> rates;

        #endregion

        #region Properties

        public String Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                   OnPropertyChanged();
                }
            }
        }

        public string Amount
        {
            get;
            set;
        }

        public ObservableCollection<Rate> Rates
        {
            get => _rates;
            set
            {
                if (_rates != value)
                {
                    //aqui notifico a la view si la propiedad cambio:

                    _rates = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rates)));
                }
            }
        }

        public Rate SourceRate
        {
            get => _sourceRate;
            set
            {
                if (_sourceRate != value)
                {
                    _sourceRate = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SourceRate)));
                }
            }
        }

        public Rate TargetRate
        {
            get => _targetRate;
            set
            {
                if (_targetRate != value)
                {
                    _targetRate = value;
                   // PropertyChanged?.Invoke(this, new  PropertyChangedEventArgs(nameof(TargetRate)));
                    OnPropertyChanged();
                }
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                if (_isRunning != value)
                {
                    //aqui notifico a la view si la propiedad cambio:

                    _isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                if (_isEnable != value)
                {
                    //aqui notifico a la view si la propiedad cambio:

                    _isEnable = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnable)));
                }
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                if (_result != value)
                {
                    _result = value;

                   PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result))); 
                }
            }
        }
        #endregion

        #region Contructor

        public MainViewModel()
        {
            //intance of service:
            apiService  = new ApiService.ApiService();
            dialogService = new DialogService();
            dataService = new DataService();

             //Methods
            LoadRates();
        }

        

        #endregion

        #region Commands

        public ICommand ConvertCommand
        {
            get { return  new RelayCommand(Convert);}
        }

        public ICommand SwitcheCommand
        {
            get
            {
                return  new RelayCommand(Switche);
            }
        }



        #endregion

        #region Services

        private ApiService.ApiService apiService;
        private DialogService dialogService;
        private DataService dataService;
        #endregion

        #region Methods

        private void Switche()
        {
           //aqui intercambio los valores de los rates, origen y destino:
            var auxRates = SourceRate;
            SourceRate = TargetRate;
            TargetRate = auxRates;

            //Aqui llamo al metodo de conver que ya existe y funciona.!!!
            Convert();
        }

        private async void LoadRates()
        {
            IsRunning = true;      
            Result = "Loading Rates.!!";

            var connection = await apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                LoadLocalDates();
            }
            else
            {
               await LoadDataFromApi();
            }

            //if (!connection.IsSuccess)
            //{
            //    IsRunning = false;
            //    Result = connection.Message;
            //    return;
                
            //}

            if (rates.Count == 0)
            {
                IsRunning = false;
                IsEnable = false;
                Result = "There are not internet connection and not load previously rate, Please try again with internet connection.";
                return;
            }
           
            // if I arrive here, I already have the list of fees.
            Rates = new ObservableCollection<Rate>(rates);
            IsRunning = false;
            IsEnable = true;
            Result = "Ready to Convert.!";
            



        }

        private async Task LoadDataFromApi()
        {
            //aqui consumo el string desde resorce dictionary:
            var url = Application.Current.Resources["URLAPI"].ToString();
            var response = await apiService.GetList<Rate>(url, "/api/rates");

            //var response = await apiService.GetList<Rate>("http://apiexchangerates.azurewebsites.net", "/api/rates");

            if (!response.IsSuccess)
            {
                //IsRunning = false;
                //Result = response.Message;

                LoadLocalDates();

                return;

            }
           
            //storage data local.
            rates = (List<Rate>)response.Result;
            //here to the collection of fees, rase everyting, if they exist
            dataService.DeleteAll<Rate>();
            //here records all dates from api in the data base:
            dataService.Save(rates);

            Status = "Rates loaded from internet.!";

        }

        private void LoadLocalDates()
        {
            rates = dataService.Get<Rate>(false);

            Status = "Rates loaded from local Data.!";

        }

        private async void Convert()
        {
            if (string.IsNullOrEmpty(Amount))
            {
                await dialogService.ShowMessage(Languages.Error, Languages.AmountValidation);
                //await Application.Current.MainPage.DisplayAlert(Languages.Error,Languages.AmountValidation,Languages.Accept);
                return;
            }

            decimal amount = 0;

            if (!decimal.TryParse(Amount, out  amount))
            {

                await dialogService.ShowMessage("Error", "You must enter a numeric value in amount");
                //await Application.Current.MainPage.DisplayAlert("Error","You must enter a numeric value in amount.","Acept");
                return;
            }

            if (SourceRate == null)
            {
                await dialogService.ShowMessage("Error", "You must select a source rate");

                //await Application.Current.MainPage.DisplayAlert("Error","You must select a source rate.","Acept");
                return;
            }

            if (TargetRate == null)
            {
                await dialogService.ShowMessage("Error", "You must select a target rate.");

                //await  Application.Current.MainPage.DisplayAlert("Error","You must select a target rate.","Acept");
                return;
            }

            var amountConvert = amount / (decimal)SourceRate.TaxRate * (decimal)TargetRate.TaxRate;

            Result = $"{SourceRate.Code} {amount:C2} = {TargetRate.Code} {amountConvert:C2}";
        }

        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


    }
}
