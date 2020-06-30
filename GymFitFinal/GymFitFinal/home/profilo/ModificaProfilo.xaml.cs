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

namespace GymFitFinal.home.profilo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModificaProfilo : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        string uid = App.uid;
        public ModificaProfilo()
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
            lblCognome.Placeholder = App.loggedUser.Cognome;
            lblName.Placeholder = App.loggedUser.Nome;
            lblEmail.Placeholder = App.loggedEmail;
            string pic = Preferences.Get("profilePic", "defaultUser.png");
            pictureBox.Source = pic;
        }


        public async void modificaCheck(Object sender, EventArgs e)
        {
            string email = lblEmail.Text;
            string cognome = lblCognome.Text;
            string nome = lblName.Text;
            

            if (string.IsNullOrEmpty(nome))
            {
                nome = App.loggedUser.Nome;

            } 
            if (string.IsNullOrEmpty(cognome))
            {
                cognome = App.loggedUser.Cognome;
            }

            //RENDO IL NOME E COGNOME CON LA PRIMA LETTERA MAIUSCOLA
            nome = nome.First().ToString().ToUpper() + nome.Substring(1);
            cognome = cognome.First().ToString().ToUpper() + cognome.Substring(1);

            updateProfile(nome, cognome);
        }

        private async void updateProfile(string nome, string cognome)
        {
            bool update = false;
            update = await _auth.UpdatePerson(nome, cognome);
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
                App.profilePic = pic;
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