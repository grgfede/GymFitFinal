using Android.App;
using GymFitFinal.Interfaces;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home.profilo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AbbonamentoPopup : PopupPage
    {
        private IFirebaseAuthenticator _auth;
        private static Random random = new Random();
        string uid = App.uid;

        public AbbonamentoPopup()
        {
            InitializeComponent();

            _auth = DependencyService.Get<IFirebaseAuthenticator>();

            List<string> tipoabbonamento = new List<string>();
            tipoabbonamento.Add("Mensile");
            tipoabbonamento.Add("Trimestrale");
            tipoabbonamento.Add("Semestrale");
            tipoabbonamento.Add("Annuale");
            IwTipoAbbonamento.ItemsSource = tipoabbonamento;


        }
        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            ViewCell lastCell = new ViewCell();

            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
           if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.BlueViolet;
                lastCell = viewCell;
            }
        }

        public async void createSub(Object sender, EventArgs e)
        {
            /*
             string tipoAbbonamento = lblTipoAbbonamento.Text;
            double costo = lblCosto.text;
            DateTime dataI = lblDataI.text;
            DateTime dataF = lblDataF.text;
             */
            DateTime dataI = DateTime.Today;
            string dataInizio = String.Format("{0:MM/dd/yyyy}", dataI);
            DateTime dataF = dataI.AddMonths(1);
            string dataFine = String.Format("{0:MM/dd/yyyy}", dataF);
            string uidSub = RandomString(28);
            DisplayAlert("Prova", uidSub, "ok");
            await _auth.AddSub("mensile", 80, dataInizio, dataFine, uidSub, uid).ContinueWith(async task =>
            {
                if (!task.IsFaulted)
                    await _auth.UpdateAbboanmento(uidSub).ContinueWith(async task2 =>
                    {
                        if (task2.IsFaulted)
                        {
                            DisplayAlert("Attenzione!", "Abbonamento non riuscito, provare più tardi", "ok");
                        } else
                        {
                            App.loggedUser.AbbonamentoIscrizione = uidSub;
                        }
                    });
            });
        }

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}