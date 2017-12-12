
namespace ForeignExchange1.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ViewModel;
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            Main = new MainViewModel();
        }
    }
}
                    