  
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace ForeignExchange1.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.ObjectModel;
    using ForeignExchange1.Models;

    public class MainViewModel
    {
        #region Properties

        public string Amount
        {
            get;
            set;
        }

        public ObservableCollection<Rate> Rates
        {
            get;
            set;
        }

        public Rate SourceRate
        {
            get;
            set;
        }

        public Rate TargetRate
        {
            get;
            set;
        }

        public bool IsRunning
        {
            get;
            set;
        }

        public object IsEnable
        {
            get;
            set; 
        }

        public string Result
        {
            get;
            set;
        }

        #endregion


        #region Commands

        public ICommand ConvertCommand
        {
            get { return new RelayCommand(convert); }
        }

        

        #endregion

        #region Methods

        private void convert()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
