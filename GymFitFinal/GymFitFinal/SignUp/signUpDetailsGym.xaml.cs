using Android.Net.Wifi.Aware;
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

namespace GymFitFinal.SignUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class signUpDetailsGym : ContentPage
    {

        private IFirebaseAuthenticator _auth;
        string uid = App.uid;
        public signUpDetailsGym(string nome)
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            App.nome = nome;
            //INIZIALIZZO GESTURE TOCCA IMMAGINE
            pictureModify();

        }

        public async void signUpGym (Object sender, EventArgs e)
        {
            Navigation.PushAsync(new signUpSuccesfulGym(App.nome));

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