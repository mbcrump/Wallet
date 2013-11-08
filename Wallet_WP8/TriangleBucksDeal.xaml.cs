using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Wallet_WP8
{
    public partial class TriangleBucksDeal : PhoneApplicationPage
    {
        public TriangleBucksDeal()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            IDictionary<string, string> parameters = this.NavigationContext.QueryString;
            if (parameters.ContainsKey("Added"))
            {
                txtDeal.Text = parameters["Added"] + " TrianglePoints has been added to your balance!";
            }
        }
    }
}