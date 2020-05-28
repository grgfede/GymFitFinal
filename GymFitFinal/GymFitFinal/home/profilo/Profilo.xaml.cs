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
                        DisplayAlert("Logout", "Logout", "Ok");

                    }
                }
            };
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();
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
            }
            else
            {
                DisplayAlert("Attenzione!", "C'è stato un errore, riprova più tardi.", "OK");
                Navigation.RemovePage(this);
                Navigation.PopAsync();
            }
        }




        /*
         * Metodo per recuperare un file multimediale (foto) dalla galleria del telefono
         * Prima di tutto controlla se ha i permessi per accedere nello storage interno
         * Se non dovesse avere i permessi, chiede all'utente di garantirli.
         * Una volta avuto i permessi, crea una variabile che conterrà il file multimediale scelto dall'utente
         * Per poi visualizzare questo file nell'apposito pictureBox
         */
        public async void btnPick_Clicked(object sender, EventArgs e)
            {
                var permission = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                if (permission != Xamarin.Essentials.PermissionStatus.Granted)
                {
                    permission = await Permissions.RequestAsync<Permissions.StorageRead>();
                }
                if (permission != Xamarin.Essentials.PermissionStatus.Granted)
                {
                    return;
                }

                //! added using Plugin.Media;
                await CrossMedia.Current.Initialize();

                //// if you want to take a picture use this
                // if(!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
                /// if you want to select from the gallery use this
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Not supported", "Your device does not currently support this functionality", "Ok");
                    return;
                }

                //! added using Plugin.Media.Abstractions;
                // if you want to take a picture use StoreCameraMediaOptions instead of PickMediaOptions
                var mediaOptions = new PickMediaOptions()
                {
                    //PhotoSize = PhotoSize.Medium,
                    CustomPhotoSize = 50
                };

                // if you want to take a picture use TakePhotoAsync instead of PickPhotoAsync
                var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

                if (pictureBox == null)
                {
                    await DisplayAlert("Error", "Could not get the image, please try again.", "Ok");
                    return;
                }
                pictureBox.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
            }

      
    }

}