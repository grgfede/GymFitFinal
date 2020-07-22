using Android.OS.Health;
using GymFitFinal.Interfaces;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home.profilo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Abbonamento : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        string uidUser = App.uid;
        string uidSub = App.loggedUser.AbbonamentoIscrizione;
        public Abbonamento()
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            getInfoSub(uidSub);

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            DisplayAlert("Ciao", "Ciao", "Ok");
            getInfoSub(uidSub);
        }

   

        public async void getInfoSub(string uidS)
        {
            var sub = await _auth.GetSub(uidS);
            if(sub != null)
            {
                lblTipoAbbonamento.Text += " " + sub.TipoAbbonamento;
                lblDataI.Text += " " + sub.DataInizio;
                lblDataF.Text += " " + sub.DataFine;
                lblCosto.Text += " " + sub.Costo + "€";
            } else
            {
                lblTipoAbbonamento.IsVisible = false;
                lblDataI.IsVisible = false;
                lblDataF.IsVisible = false;
                lblCosto.IsVisible = false;
                lblNoSub.IsVisible = true;
            }
           
        }

        public void ToolbarItem_Follow (Object sender, EventArgs e)
        {
            checkSub();
        }

        public async void checkSub()
        {
            string uid = App.uid;
            var person = await _auth.GetPerson(uid);

            if (person.PalestraIscrizione != null)
            {
                if (person.AbbonamentoIscrizione == null)
                {
                    await PopupNavigation.PushAsync(new AbbonamentoPopup(person.PalestraIscrizione));
                }
                else
                {
                    DisplayAlert("Attenzione!", "Abbonamento già presente", "ok");
                }
            } else
            {
                DisplayAlert("Attenzione!", "Non sei inscritto a nessuna palestra!", "ok");
            }
            
        }

    }
}