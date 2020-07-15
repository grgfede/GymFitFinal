using Firebase.Storage;
using GymFitFinal.home;
using GymFitFinal.home.navBar;
using GymFitFinal.home.profilo;
using GymFitFinal.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GymFitFinal
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class LoginWithEmail : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        public LoginWithEmail()
        {
            InitializeComponent();

            lblPsswdClick();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
        }

      

        public async void DoLogin(object sender, EventArgs e)
        {
            

            string email = lblEmail.Text;
            string password = lblPassword.Text;


            var Token = await _auth.DoLoginWithEP(email, password);
            //var Token = info[0];
            if (!Token.Contains("ERROR"))
            {

                Preferences.Set("logged", true);
                //Der er logget ind og returneret et token
                App.IsUserLoggedIn = true;
                App.Token = Token;

                //CAMBIO PAGINA
                Application.Current.MainPage = new NavigationPage(new Home());

                
                this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);
                await Navigation.PushAsync(new home.Home());
            }
            else if (Token.Contains("ERROR_USER_NOT_FOUND"))
            {
                Token = "Utente non trovato!";
                DisplayAlert("Attenzione", Token, "Riprova");
            }
            else if (Token.Contains("ERROR_INVALID"))
            {
                Token = "Email non corretta!";
                DisplayAlert("Attenzione", Token, "Riprova");
            }
            else if (Token.Contains("ERROR_WRONG_P"))
            {
                Token = "Password errata!";
                DisplayAlert("Attenzione", Token, "Riprova");
            }
            else if (Token.Contains("ERROR_EMAIL_OR_PASSWORD_MISSING"))
            {
                Token = "Email o Password mancanti!";
                DisplayAlert("Attenzione", Token, "Riprova"); 
            } else if (Token.Contains("ERR_PROFILE_PIC"))
            {
                Token = "Errore immagine profilo";
                DisplayAlert("Attenzione", "Token", "Rirpova");
            }


        }



        void lblPsswdClick()
        {
            string email = lblEmail.Text;
            lblForgotPass.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PushAsync(new resetPassword.ResetPassword());
                })
            });
        }





    }
    
}
