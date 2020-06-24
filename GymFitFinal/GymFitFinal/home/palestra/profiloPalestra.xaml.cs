using Android.Preferences;
using GymFitFinal.Interfaces;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home.palestra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class profiloPalestra : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        private List<User> lstPersons;

        string uid = App.uid;

        FirebaseStorageHelper firebaseStorageHelper = new FirebaseStorageHelper();
        MediaFile file;
        public profiloPalestra()
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();

            getInfo(uid);

            var nav = new NavigationPage(new ContentPage { Title = "" });
            nav.BarBackgroundColor = Color.FromHex("#FACO5E");

            picker.SelectedIndexChanged += async (sender, args) =>
            {
                if (picker.SelectedIndex == -1)
                {

                }
                else
                {
                    string colorName = picker.Items[picker.SelectedIndex];
                    picker.SelectedItem = -1;
                    if (colorName.Equals("Modifica Profilo"))
                    {
                        //await Navigation.PushAsync(new ModificaProfilo());
                    }
                    else
                    {
                        _auth.Logout();
                        Navigation.InsertPageBefore(new Main(), Navigation.NavigationStack[0]);
                        await Navigation.PopToRootAsync();

                    }
                }
            };



        }




        public async void getInfo(string uid)
        {
            var gym = await _auth.GetGym(uid);
            if (gym != null)
            {
                lblCitta.Text = gym.Citta;
                lblNome.Text = gym.Nome;
                App.loggedGym = gym;
                //pictureBox.Source = Preferences.Get("profilePic", "default.png");
            } else
            {
                DisplayAlert("Attenzione!", "C'è stato un errore, riprova più tardi.", "OK");
                _auth.Logout();
                Navigation.RemovePage(this);
                Navigation.PopAsync();
            }
        }




        protected async override void OnAppearing()
        {
            base.OnAppearing();
            getInfo(uid);
        }





        /*
        * Metodo che fa "visualizzare" il picker una volta premuto l'icona della gear presente nella toolbar
        */
        public void ToolbarItem_Impostazioni(Object sender, EventArgs e)
        {
            picker.Focus();

        }


    }
}