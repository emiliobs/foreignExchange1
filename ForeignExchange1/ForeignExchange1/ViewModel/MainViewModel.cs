  
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ForeignExchange1.Annotations;
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


        #endregion

        #region Properties

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

            try
            {
                  //Aqui consumo los datos
                //aqui cargo la clase:
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://apiexchangerates.azurewebsites.net");
                var controller = "/api/rates";

                var response = await  client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    IsRunning = false;
                    Result = result;
                }

                //aqui serializo el string que vienes desde el rest:
                var rates = JsonConvert.DeserializeObject<List<Rate>>(result);


                //aqui  convierto la list en un observablecollection:
                Rates = new ObservableCollection<Rate>(rates);

                IsRunning = false;
                //Result = result;   
                IsEnable = true;
                Result = "Ready to Convert";
                                            
            }
            catch (Exception ex)
            {

                //sihay error:
                IsRunning = false;
                Result = ex.Message;
            }

        }

        private async void Convert()
        {
            if (string.IsNullOrEmpty(Amount))
            {
                await Application.Current.MainPage.DisplayAlert("Error","You must enter a value in amount.","Acept");
                return;
            }

            decimal amount = 0;
            if (!decimal.TryParse(Amount, out  amount))
            {
                await Application.Current.MainPage.DisplayAlert("Error","You must enter a numeric value in amount.","Acept");
                return;
            }

            if (SourceRate == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error","You must select a source rate.","Acept");
                return;
            }

            if (TargetRate == null)
            {
                await  Application.Current.MainPage.DisplayAlert("Error","You must select a target rate.","Acept");
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
