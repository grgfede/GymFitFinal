using GymFitFinal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.resetPassword
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPassword : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        public ResetPassword()
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
        }



        public async void DoResetPassword(Object sender, EventArgs e)
        {
            string email = lblEmailReset.Text;
            var Token = await _auth.ResetPassword(email);

            if (Token.Contains("ERROR_EMAIL_MISSING"))
            {
                DisplayAlert("Attenzione!", "Inserisci una email", "Riprova");
            }
            else if (Token.Contains("ERROR_INVALID_EMAIL"))
            {
                DisplayAlert("Attenzione!", "Email inserita non valida", "Riprova");
            }
            else if (Token.Contains("ERROR_USER_NOT_FOUND"))
            {
                DisplayAlert("Attenzione!", "Email non registrata", "Riprova");
            }
            else
            {
                Navigation.PushAsync(new resetPassword.ResetSuccessful());
            }
        }


    }
}