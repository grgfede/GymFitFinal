using GymFitFinal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.SignUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class signUp : ContentPage
    {
        private IFirebaseAuthenticator _auth;

        public signUp()
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
        }

        public async void DoSignUpMethod(Object sender, EventArgs e)
        {
            bool error = false;
            string nome =  txtNome.Text;
            string cognome = txtCognome.Text;
            string email = txtEmail.Text;
            string password = txtPass.Text;

            //RENDO IL NOME E COGNOME CON LA PRIMA LETTERA MAIUSCOLA
            nome = nome.First().ToString().ToUpper() + nome.Substring(1);
            cognome = cognome.First().ToString().ToUpper() + cognome.Substring(1);


            //CONTROLLO SE I CAMPI SONO VUOTI
            if ((string.IsNullOrEmpty(nome)) || (string.IsNullOrEmpty(cognome)) || (string.IsNullOrEmpty(email)) || (string.IsNullOrEmpty(password)))
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
                var Token = await _auth.DoSignUp(email, password, nome, cognome);
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
                    Navigation.PushAsync(new signUpSuccesful(nome));
                }

            }
        }

       
        
    }
}