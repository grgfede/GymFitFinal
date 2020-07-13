using GymFitFinal.home.palestra;
using GymFitFinal.Interfaces;
using Rg.Plugins.Popup.Services;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home.navBar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PalestraIscrizione : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        FirebaseStorageHelper firebaseStorageHelper = new FirebaseStorageHelper();
        MediaFile file;
        public PalestraIscrizione(string uid)
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            getInfo(uid);
        }


        public async void getInfo(string uid)
        {
            var person = await _auth.GetGym(uid);
            if (person != null)
            {
                lblNome.Text = person.Nome + " ";
                lblCitta.Text = "Dove siamo: " + person.Citta;
                //string pic = await _auth.getProfilePicGymIscrizione(uid);
                pictureBox.Source = "defaultUser.png";
                Gym.citta = person.Citta;
                Gym.indirizzo = person.Indirizzo;

            }
            else
            {
                DisplayAlert("Attenzione!", "C'è stato un errore, riprova più tardi.", "OK");
            }
        }

        public async void OpenMap(Object sender, EventArgs e)
        {
            
            await Launcher.OpenAsync("geo:0,0?q=394+" + Gym.citta + "+" + Gym.indirizzo);
        }

        private async void TurniDisponibili(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TurniDisponibili());
        }

        private async void giorniPopup (object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new GiorniPopup());
        }

        private async void orariPopup(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new OrariPopup());
        }

    }
}