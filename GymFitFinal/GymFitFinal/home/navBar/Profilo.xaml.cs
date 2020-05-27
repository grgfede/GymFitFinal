using Firebase.Storage;
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
        }


        protected async override void OnAppearing()
        {

            base.OnAppearing();

        }


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