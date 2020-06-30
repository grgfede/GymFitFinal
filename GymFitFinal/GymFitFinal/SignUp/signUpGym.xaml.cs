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
                    //DisplayAlert("prova", uid, "ok");
                    Navigation.PushAsync(new signUpDetailsGym(nome));

                  
                }

            }
        }











            

        




       








    }
}