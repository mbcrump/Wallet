using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Wallet;

namespace Wallet_WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Add wallet item task
        static AddWalletItemTask addWalletItemTask = new AddWalletItemTask();

        public MainPage()
        {
            InitializeComponent();
            // Set completion event after wallet item task
            addWalletItemTask.Completed += addWalletItemTask_Completed;
        }

        private void addWalletItemTask_Completed(object sender, AddWalletItemResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                MessageBox.Show(e.Item.DisplayName + " created");
            }
            else
            {
                MessageBox.Show(e.Item.DisplayName + " creation failed " + e.Error.Message);
            }
        }

        private void btnAddMembership_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                WalletTransactionItem membershipItem;
                membershipItem = new WalletTransactionItem("Membership");
                membershipItem.IssuerName = "TriangleBucks";
                membershipItem.DisplayName = " TriangleBucks Membership Card";
                membershipItem.IssuerPhone.Business = "987-654-321";
                membershipItem.CustomerName = "Michael Crump";
                membershipItem.AccountNumber = "103";
                membershipItem.BillingPhone = "205-999-9999";
                membershipItem.IssuerWebsite = new Uri("http://www.trianglebucks.com", UriKind.RelativeOrAbsolute);
                membershipItem.DisplayAvailableBalance = "1000 TrianglePoints";

                ////Specify Logo Image
                BitmapImage bmp = new BitmapImage();
                using (System.IO.Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Wallet_WP8.Assets.trianglebucks.bmp"))
                    bmp.SetSource(stream);

                membershipItem.Logo99x99 = bmp;
                membershipItem.Logo159x159 = bmp;
                membershipItem.Logo336x336 = bmp;

                addWalletItemTask.Item = membershipItem;
                addWalletItemTask.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There were the following errors when saving your membership to the wallet: " + ex.Message);
            }
        }

        private async void btnFreeCoffee_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Deal coffeeDeal = new Deal("1FREECOFFEE");

                coffeeDeal.MerchantName = "TriangleBucks";
                coffeeDeal.DisplayName = "1 free coffee!";
                coffeeDeal.Description = "Good for 1 free Coffee at TriangleBucks";
                coffeeDeal.CustomerName = "Michael Crump";
                coffeeDeal.ExpirationDate = DateTime.Now.AddDays(7);
                coffeeDeal.IssuerName = "TriangleBucks";
                coffeeDeal.IssuerWebsite = new Uri("http://www.trianglebucks.com", UriKind.RelativeOrAbsolute);
                coffeeDeal.TermsAndConditions = "Only valid for one use.";

                ////Specify Logo Image
                BitmapImage bmp = new BitmapImage();
                using (System.IO.Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Wallet_WP8.Assets.trianglebucks.bmp"))
                    bmp.SetSource(stream);

                coffeeDeal.Logo99x99 = bmp;
                coffeeDeal.Logo159x159 = bmp;
                coffeeDeal.Logo336x336 = bmp;

                //Specify Barcode
                BitmapImage bc = new BitmapImage();
                using (System.IO.Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Wallet_WP8.Assets.barcode.bmp"))
                    bc.SetSource(stream);

                coffeeDeal.BarcodeImage = bc;

                await coffeeDeal.SaveAsync();
                MessageBox.Show("Saved Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("There were the following errors when saving your deal to the wallet: " + ex.Message);
            }
        }

        void btnAddBank_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentInstrument accountItem;
                accountItem = new PaymentInstrument("Credit");
                accountItem.PaymentInstrumentKinds = PaymentInstrumentKinds.Credit;
                accountItem.IssuerName = "TriangleBucks";
                accountItem.DisplayName = "TriangleBucks Bank Card";
                accountItem.IssuerPhone.Business = "987-654-321";
                accountItem.CustomerName = "Michael Crump";
                accountItem.AccountNumber = "103";
                accountItem.BillingPhone = "205-999-9999";
                accountItem.IssuerWebsite = new Uri("http://www.trianglebucks.com", UriKind.RelativeOrAbsolute);
                accountItem.ExpirationDate = DateTime.Now.AddDays(90);

                accountItem.DisplayCreditLimit = "150";
                accountItem.DisplayAvailableCredit = "15";

                ////Specify Logo Image
                BitmapImage bmp = new BitmapImage();
                using (System.IO.Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Wallet_WP8.Assets.trianglebucks.bmp"))
                    bmp.SetSource(stream);

                accountItem.Logo99x99 = bmp;
                accountItem.Logo159x159 = bmp;
                accountItem.Logo336x336 = bmp;

                addWalletItemTask.Item = accountItem;
                addWalletItemTask.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There were the following errors when saving your account to the wallet: " + ex.Message);
            }
        }

        private async void btnSpendCash_Click_1(object sender, RoutedEventArgs e)
        {
            // Find the payment instrument to use 
            PaymentInstrument walletPay;

            walletPay = Microsoft.Phone.Wallet.Wallet.FindItem("Credit") as PaymentInstrument;

            if (walletPay == null)
            {
                MessageBox.Show("Wallet not found");
                return;
            }

            // Create the transaction
            WalletTransaction transaction = new WalletTransaction();

            Random rand = new Random();
            transaction.DisplayAmount = rand.Next(5, 50).ToString();

            transaction.Description = "Coffee Purchase";
            transaction.TransactionDate = DateTime.Now;

            // Add the transaction to the wallet
            walletPay.TransactionHistory.Add("Coffee Purchase " + DateTime.Now, transaction);

            await walletPay.SaveAsync();

            MessageBox.Show("Transaction stored");
        }
    }
}