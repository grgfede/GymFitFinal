using Android.App;
using GymFitFinal.Interfaces;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private static double costo;
        private static string descrizione;

        public AbbonamentoPopup(string uidG)
        {
            InitializeComponent();

            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            this.BindingContext = new model.MyViewModel();


            List<string> tipoabbonamento = new List<string>();
           
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = new model.MyViewModel();
        }





        public async void createSub(Object sender, EventArgs e)
        {
            DateTime dataI = DateTime.Today;
            string dataInizio = String.Format("{0:MM/dd/yyyy}", dataI);
            DateTime dataF = dataI.AddMonths(1);
            string dataFine = String.Format("{0:MM/dd/yyyy}", dataF);
            string uidSub = RandomString(28);
            string TipoAbb = IwTipoAbbonamento.Items[IwTipoAbbonamento.SelectedIndex];
            double Costo = costo;

            await _auth.AddSub(TipoAbb, Costo, dataInizio, dataFine, uidSub, uid).ContinueWith(async task =>
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
                            await PopupNavigation.Instance.PopAsync();
                        }
                    });
            });
        }

        private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var selectedItem = (AbbonamentoPalestra)picker.SelectedItem; // This is the model selected in the picker
            lblCosto.Text = "Costo: " + Convert.ToString(selectedItem.Costo) + " €";
            lblNote.Text = selectedItem.Descrizione;
            lblCosto.IsVisible = true;
            lblNote.IsVisible = true;
            costo = selectedItem.Costo;
            descrizione = selectedItem.Descrizione;
        }

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}