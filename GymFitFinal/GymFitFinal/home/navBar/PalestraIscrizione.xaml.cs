using GymFitFinal.Interfaces;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            }
            else
            {
                DisplayAlert("Attenzione!", "C'è stato un errore, riprova più tardi.", "OK");
            }
        }


    }
}