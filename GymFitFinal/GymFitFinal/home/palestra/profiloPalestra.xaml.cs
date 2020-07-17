//using Android.Preferences;
using GymFitFinal.home.profilo;
using GymFitFinal.Interfaces;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
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

            pictureFullScreen();

            getInfo(uid);

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
                        await Navigation.PushAsync(new home.palestra.Page1());
                    }
                    else
                    {
                        _auth.Logout();
                        Preferences.Clear("profilePic");
                        Navigation.InsertPageBefore(new Main(), Navigation.NavigationStack[0]);
                        await Navigation.PopToRootAsync();

                    }
                }
            };

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            getInfo(uid);
        }

        public async void openMap(Object sender, EventArgs e)
        {
            await Launcher.OpenAsync("geo:0,0?q=394+" + App.loggedGym.Citta + "+" + App.loggedGym.Indirizzo);

        }

        private async void orariPopup(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new OrariPopup(uid));
        }

        /*
        * Metodo che fa "visualizzare" il picker una volta premuto l'icona della gear presente nella toolbar
        */
        public void ToolbarItem_Impostazioni(Object sender, EventArgs e)
        {
            picker.Focus();

        }
        /*
        * Metodo per recuperare le informazioni dell'utente.
        * Nella variabile _auth abbiamo tutti i metodi di connessione al database firebase.
        * In questo caso utilizziamo il metodo GetPerson che restituisce un oggetto "User" specifico grazie all'uid.
        * In questo modo possiamo recuperare il nome e cognome grazie ai metodi get della classe User.
        */
        public async void getInfo(string uid)
        {
            var person = await _auth.GetGym(uid);
            if (person != null)
            {
                lblNome.Text = person.Nome + " ";
                lblCitta.Text = "Dove siamo: " + person.Citta;
                App.loggedGym = person;
                string pic = Preferences.Get("profilePic", "defaultUser.png");
                pictureBox.Source = pic;
                App.profilePic = pic;

            }
            else
            {
                DisplayAlert("Attenzione!", "C'è stato un errore, riprova più tardi.", "OK");
                _auth.Logout();
                Navigation.RemovePage(this);
                Navigation.PopAsync();
            }
        }

        void pictureFullScreen()
        {
            pictureBox.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    await PopupNavigation.PushAsync(new FullscreenImagePopup(pictureBox.Source));
                })
            });
        }


    }
   
}