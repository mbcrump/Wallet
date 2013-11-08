using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Wallet;
using System;

namespace Wallet_BG_Agent
{
    public class MyWalletAgent : WalletAgent
    {
        private static volatile bool _classInitialized;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static MyWalletAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += UnhandledException;
                });
            }
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// The refresh agent is run when a user taps refresh on an item in the wallet, or when it has been viewed enough for the agent to deem it refreshable. 
        /// The sorts of things you might want to do here include:
        /// 1) Update the transaction history for the item
        /// 2) Update the logo, contact information, and other metadata for the item
        /// 3) Update the status message to inform the user of required actions or present them with useful information.
        /// </summary>
        /// <param name="args">The args contain the list of wallet items that are currently being refreshed. This way, if there are multiple services/items in the 
        /// wallet, you'll know which ones the user is looking at, and hence which ones need to be updated.</param>
        /// </remarks>

        protected override async void OnRefreshData(RefreshDataEventArgs args)
        {
            Random rand = new Random();

            foreach (WalletItem item in args.Items)
            {
                WalletTransactionItem card = item as WalletTransactionItem;
                if (card != null)
                {
                    int i = rand.Next(5, 50);
                    card.Message = i.ToString() + " TrianglePoints has been added!";
                    card.MessageNavigationUri = new Uri("/TriangleBucksDeal.xaml?Added=" + i.ToString(), UriKind.Relative);

                    await card.SaveAsync();
                }
            }

            foreach (WalletItem item in args.Items)
            {
                PaymentInstrument card = item as PaymentInstrument;
                if (card != null)
                {
                    if (card.Id == "Credit")
                    {
                        card.Message = "New statement available";
                        card.MessageNavigationUri = new Uri("/AccountPage.xaml", UriKind.Relative);

                        await card.SaveAsync();
                    }
                }
            }

            NotifyComplete();

        }
    }
}