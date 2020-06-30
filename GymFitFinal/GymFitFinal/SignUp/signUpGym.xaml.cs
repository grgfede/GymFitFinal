//using Android.App;
//using Android.Graphics;
using Firebase.Storage;
using GymFitFinal.Interfaces;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
//using Bitmap = Android.Graphics.Bitmap;

namespace GymFitFinal.SignUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class signUpGym : ContentPage
    {
        public Stream stream { get; set; }
        private IFirebaseAuthenticator _auth;
       
        public signUpGym()
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
           
           
            //INIZIALIZZO GESTURE TOCCA IMMAGINE
            pictureModify();
        }

        public async void DoSignUpMethod(Object sender, EventArgs e)
        {
            bool error = false;
            string nome = txtNome.Text;
            string citta = txtCitta.Text;
            string email = txtEmail.Text;
            string password = txtPass.Text;
           
            //RENDO IL NOME E COGNOME CON LA PRIMA LETTERA MAIUSCOLA
            nome = nome.First().ToString().ToUpper() + nome.Substring(1);
            citta = citta.First().ToString().ToUpper() + citta.Substring(1);


           
            //CONTROLLO SE I CAMPI SONO VUOTI
            if (String.IsNullOrEmpty(nome))
            {
                DisplayAlert("Attenzione!", "Uno o più campi non sono completi", "OK");
                error = true;
            }
           

            if (!chkTerm.IsChecked)
            {
                error = true;
                DisplayAlert("Attenzione!", "Per proseguire accetta i Termini e Condizioni d'uso", "Ok");
            }

         

            if (!error)
            {
                var Token = await _auth.DoSignUpGym(email, password, nome, citta);
                if (Token.Contains("ERROR_INVALID_EMAIL"))
                {
                    DisplayAlert("Attenzione!", "Email inserita non valida", "Riprova");
                }
                else if (Token.Contains("ERROR_EMAIL_ALREADY_IN_USE"))
                {
                    DisplayAlert("Attenzione!", "Email già registrata", "Riprova");
                }
                else if (Token.Contains("ERROR_WEAK_PASSWORD"))
                {
                    DisplayAlert("Attenzione!", "La password inserita è debole, scegline una più complessa", "Riprova");
                }
                else
                {
                 
                    string uid = App.uid;
                    //string pic = await StoreImages(stream, uid);
                    //DisplayAlert("prova", uid, "ok");
                    Navigation.PushAsync(new signUpSuccesful(nome));
                }

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
                return;
               
            }
            if (pictureBox == null)
            {
                await DisplayAlert("Error", "Could not get the image, please try again.", "Ok");
               
                return;
            }
            pictureBox.Source = ImageSource.FromStream(() =>
            {
                stream = selectedImageFile.GetStream();
                //selectedImageFile.Dispose();
                return stream;
            });
            
            

        }




        private async Task<string> StoreImages(Stream imageStream, string uid)
        {
            DisplayAlert("Error", uid, "ok");
            var stroageImage = await new FirebaseStorage("gymfitt-2b845.appspot.com")
                .Child(uid)
                .Child("profilePic.jpg")
                .PutAsync(imageStream);
            string imgurl = stroageImage;
            return imgurl;
        }








    }
}