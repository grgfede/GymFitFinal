using Firebase.Storage;
using GymFitFinal.home.profilo;
using GymFitFinal.Interfaces;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home.navBar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profilo : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        private List<User> lstPersons;

        string uid = App.uid;

        FirebaseStorageHelper firebaseStorageHelper = new FirebaseStorageHelper();
        MediaFile file;

        public Profilo()
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();

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
                        await Navigation.PushAsync(new ModificaProfilo());
                    } else
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
            var person = await _auth.GetPerson(uid);
            if (person != null)
            {
                lblNomeCognome.Text = person.Nome + " ";
                lblNomeCognome.Text += person.Cognome;
                App.loggedUser = person;
                string pic = Preferences.Get("profilePic", "defaultUser.png");
                pictureBox.Source = pic;
                
            }
            else
            {
                DisplayAlert("Attenzione!", "C'è stato un errore, riprova più tardi.", "OK");
                _auth.Logout();
                Navigation.RemovePage(this);
                Navigation.PopAsync();
            }
        }

    }

}