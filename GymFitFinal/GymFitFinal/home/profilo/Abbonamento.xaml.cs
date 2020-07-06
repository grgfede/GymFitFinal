using GymFitFinal.Interfaces;
using Rg.Plugins.Popup.Services;
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
    public partial class Abbonamento : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        public Abbonamento()
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();

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
                    await PopupNavigation.PushAsync(new AbbonamentoPopup());
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