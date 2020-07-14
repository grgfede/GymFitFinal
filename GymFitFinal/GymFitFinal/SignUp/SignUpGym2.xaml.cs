using Android.Content;
using Android.Text.Format;
using Firebase.Storage;
using GymFitFinal.Interfaces;
using Java.Net;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.SignUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpGym2 : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        string uid = App.uid;
        public SignUpGym2(string nome, string citta, string indirizzo, string email, string password)
        {
            InitializeComponent();
            App.nomeGym = nome;
            App.cittaGym = citta;
            App.indirizzoGym = indirizzo;
            App.emailGym = email;
            App.passwordGym = password;

            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            pictureModify();
        }



        public async void finishSignUp(Object sender, EventArgs e)
        {
            dataIMattina = new TimePicker();
            TimeSpan selectedTimeIM = dataIMattina.Time;

            dataFMattina = new TimePicker();
            TimeSpan selectedTimeFM = dataFMattina.Time;

            dataIPomeriggio = new TimePicker();
            TimeSpan selectedTimeIP = dataIPomeriggio.Time;

            dataFPomeriggio = new TimePicker();
            TimeSpan selectedTimeFP = dataFPomeriggio.Time;

            //CONVERTO GLI ORARI IN STRINGHE
            /*string inizioM = string.Format("{0:hh\\:mm\\:ss}", dataIMattina);
            string fineM = string.Format("{0:hh\\:mm\\:ss}", dataFMattina);
            string inizioP = string.Format("{0:hh\\:mm\\:ss}", dataIPomeriggio);
            string fineP = string.Format("{0:hh\\:mm\\:ss}", dataFPomeriggio);*/

            string inizioM = selectedTimeIM.ToString(@"hh\:mm");
            string fineM = selectedTimeFM.ToString(@"hh\:mm");
            string inizioP = selectedTimeIP.ToString(@"hh\:mm");
            string fineP = selectedTimeFP.ToString(@"hh\:mm");



            string telefono = txtRecapito.Text;

            string citta = App.cittaGym;
            string indirizzo = App.indirizzoGym;
            string nome = App.nomeGym;
            string email = App.emailGym;
            string password = App.passwordGym;
           


            //FIREBASEHELPER E' UNA CLASSE CHE CONTIENE TUTTI LE QUERY PER LA SCRITTURA/LETTURA DEL DATABASE DI FIREBASE
            await _auth.AddGym(nome, citta, indirizzo, telefono, inizioM, fineM, inizioP, fineP, uid).ContinueWith(async task =>
            {
                if (!task.IsFaulted)
                {
                    // Remove page before Edit Page
                    this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 2]);
                    // This PopAsync will now go to List Page
                    this.Navigation.PushAsync(new signUpSuccesfulGym(nome));
                }
            });
                  
            


        }



        public void pictureModify()
        {
            pictureBox.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    getImage();
                })
            });
        }

        /*
         * Metodo per recuperare un file multimediale (foto) dalla galleria del telefono
         * Prima di tutto controlla se ha i permessi per accedere nello storage interno
         * Se non dovesse avere i permessi, chiede all'utente di garantirli.
         * Una volta avuto i permessi, crea una variabile che conterrà il file multimediale scelto dall'utente
         * Per poi visualizzare questo file nell'apposito pictureBox
         */
        public async Task getImage()
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

            if (selectedImageFile == null)
            {
                pictureBox.Source = "defaultUser.png";
            }
            if (pictureBox == null)
            {
                await DisplayAlert("Error", "Could not get the image, please try again.", "Ok");
                return;
            }
            string pic = await StoreImages(selectedImageFile.GetStream());
            //App.profilePic = pic;
            Preferences.Set("profilePic", pic);
            pictureBox.Source = pic;



        }




        private async Task<string> StoreImages(Stream imageStream)
        {
            //string uid = App.uid;
            var stroageImage = await new FirebaseStorage("gymfitt-2b845.appspot.com")
                .Child(uid)
                .Child("profilePic.jpg")
                .PutAsync(imageStream);
            string imgurl = stroageImage;
            return imgurl;
        }




    }
}