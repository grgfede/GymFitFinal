//using Android.Preferences;
using Android.App;
using Firebase.Storage;
using GymFitFinal.Interfaces;
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

namespace GymFitFinal.home.palestra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        string uid = App.uid;
        public Page1()
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            showInfo();
            pictureModify();
        }
        protected override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
        }

        private void showInfo()
        {
            lblName.Placeholder = App.loggedGym.Nome;
            lblCitta.Placeholder = App.loggedGym.Citta;
            lblIndirizzo.Placeholder = App.loggedGym.Indirizzo;
            lblEmail.Placeholder = App.loggedEmail;
            if (string.IsNullOrEmpty(App.loggedGym.Telefono))
            {
                lblTelefono.Placeholder = "Recapito Telefonico";
            } else
            {
                lblTelefono.Placeholder = App.loggedGym.Telefono;
            }
            string pic = Preferences.Get("profilePic", "defaultUser.png");
            pictureBox.Source = pic;
        }



        public async void modificaCheck(Object sender, EventArgs e)
        {
            string email = lblEmail.Text;
            string citta = lblCitta.Text;
            string indirizzo = lblCitta.Text;
            string nome = lblName.Text;
            string telefono = lblTelefono.Text;
            TimeSpan IM = dataIMattina.Time;
            TimeSpan FM = dataFMattina.Time;
            TimeSpan IP = dataIPomeriggio.Time;
            TimeSpan FP = dataFPomeriggio.Time;
            //CONVERTO GLI ORARI IN STRINGHE
            string inizioM = IM.ToString(@"hh\:mm");
            string fineM = FM.ToString(@"hh\:mm");
            string inizioP = IP.ToString(@"hh\:mm");
            string fineP = FP.ToString(@"hh\:mm");

            DisplayAlert("ok", inizioM, "ok");
            if (string.IsNullOrEmpty(nome))
            {
                nome = App.loggedGym.Nome;

            }
            if (string.IsNullOrEmpty(citta))
            {
                citta = App.loggedGym.Citta;
            }
            if (string.IsNullOrEmpty(indirizzo))
            {
                indirizzo = App.loggedGym.Indirizzo;
            }
            if (string.IsNullOrEmpty(telefono))
            {
                telefono = App.loggedGym.Telefono;
            }
            if (string.Equals(inizioM, "00:00"))
            {
                inizioM = App.loggedGym.DataIMattina;
            }
            if (string.Equals(fineM, "00:00"))
            {
                fineM = App.loggedGym.DataFMattina;
            }
            if (string.Equals(inizioP, "00:00"))
            {
                inizioP = App.loggedGym.DataIPomeriggio;
            }
            if (string.Equals(fineP, "00:00"))
            {
                fineP = App.loggedGym.DataFPomeriggio;
            }

            //RENDO IL NOME E COGNOME CON LA PRIMA LETTERA MAIUSCOLA
            nome = nome.First().ToString().ToUpper() + nome.Substring(1);
            citta = citta.First().ToString().ToUpper() + citta.Substring(1);
            indirizzo = indirizzo.First().ToString().ToUpper() + indirizzo.Substring(1);
            updateProfile(nome, citta, indirizzo, telefono, inizioM, fineM, inizioP, fineP);
        }

        private async void updateProfile(string nome, string citta, string indirizzo, string telefono, string IM, string FM, string IP, string FP)
        {
            bool update = false;
            update = await _auth.UpdateGym(nome, citta, indirizzo, telefono, IM, FM, IP, FP);
            if (!update)
            {
                DisplayAlert("Attenzione!", "Qualcosa è andato storto, riprova più tardi", "ok");

            }
            else
            {
                _auth.refreshUser();
                // Remove page before Edit Page
                this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);
                // This PopAsync will now go to List Page
                this.Navigation.PopAsync();

            }
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