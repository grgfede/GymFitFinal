using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using GymFitFinal.Droid.Interfaces;
using Xamarin.Forms;
using Firebase.Database;
using Firebase.Firestore;
using Firebase;
using Firebase.Database.Query;
using GymFitFinal.Interfaces;
using Firebase.Storage;
using Android.Media;
using System.Drawing;
using Image = Xamarin.Forms.Image;
using GymFitFinal.home.navBar;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(AndroAuth))]

namespace GymFitFinal.Droid.Interfaces
{

   
    public class AndroAuth : IFirebaseAuthenticator
    {
        private readonly string ChildName = "utenti";
        private readonly string ChildNameGym = "palestre";
        private readonly string ChildNameSub = "abbonamenti";


        readonly FirebaseClient firebase = new FirebaseClient("https://gymfitt-2b845.firebaseio.com/");

       


        public async Task refreshUser()
        {
            await FirebaseAuth.Instance.CurrentUser.ReloadAsync();
        }

   

        

        public async Task<bool> UpdatePerson(string nome, string cognome)
        {
            bool success = false;
            var uid = App.uid;
            App.loggedUser.Nome = nome;
            App.loggedUser.Cognome = cognome;
            string ChildNameAdd = ChildName + "/" + uid ;
            await firebase.Child(ChildNameAdd).PutAsync<User>(App.loggedUser).ContinueWith(async task =>
            {
                if (!task.IsFaulted)
                    success = true; ;

            });
            return success;
        }



        public async Task<bool> UpdateGym(string nome, string citta)
        {
            bool success = false;
            var uid = App.uid;
            App.loggedGym.Nome = nome;
            App.loggedGym.Citta = citta;
            string ChildNameAdd = ChildNameGym + "/" + uid;
            await firebase.Child(ChildNameAdd).PutAsync<Gym>(App.loggedGym).ContinueWith(async task =>
            {
                if (!task.IsFaulted)
                    success = true; ;

            });
            return success;
        }


        public async Task UpdateAbboanmento(string uidAbbonamento)
        {
            var uid = App.uid;
            App.loggedUser.AbbonamentoIscrizione = uidAbbonamento;
            string ChildNameAdd = ChildName + "/" + uid;
            await firebase.Child(ChildNameAdd).PutAsync<User>(App.loggedUser);
          
        }



        //TASK PER LOGIN CON FIREBASE
        public async Task<string> DoLoginWithEP(string email, string password)
        {
           if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var _result = "";
                await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(async task =>
                {
                    if (task.IsFaulted)
                    {
                        //Noget gik galt, returner FirebaseAuth fejlkoden
                        if (task.Exception != null)
                            _result = ((FirebaseAuthException) task.Exception.GetBaseException()).ErrorCode;
                    }
                    else
                    {
                        // Login lykkedes - returner Token
                        var token = await task.Result.User.GetIdTokenAsync(false);
                        _result = token.Token;

                        var currentuser = FirebaseAuth.Instance.CurrentUser;
                        var loggedEmail = currentuser.Email;
                       
                        var uid = currentuser.Uid;
                        App.uid = uid;
                        App.loggedEmail = loggedEmail;
                        App.password = password;

                        Preferences.Set("uid", uid);
                        Preferences.Set("loggedEmail", loggedEmail);
                        Preferences.Set("password", password);

                        //RECUPERO L'IMMAGINE PROFILO
                        var storageImage = await new FirebaseStorage("gymfitt-2b845.appspot.com")
                        .Child(App.uid)
                        .Child("profilePic")
                        .GetDownloadUrlAsync();
                        //App.profilePic = storageImage;
                        Preferences.Set("profilePic", storageImage);
                    }
                });
                return _result;
            }
            return "ERROR_EMAIL_OR_PASSWORD_MISSING";
        }


        public async Task<string> getProfilePic()
        {
            var storageImage = await new FirebaseStorage("gymfitt-2b845.appspot.com")
                       .Child(App.uid)
                       .Child("profilePic")
                       .GetDownloadUrlAsync();
           // App.profilePic = storageImage;
            Preferences.Set("profilePic", storageImage);
            return storageImage;
        }


        public async Task<string> getProfilePicGymIscrizione(string uid)
        {
            var storageImage = await new FirebaseStorage("gymfitt-2b845.appspot.com")
                       .Child(uid)
                       .Child("profilePic")
                       .GetDownloadUrlAsync();
            return storageImage;
        }


        public bool IsUserLoggedIn() {
            if(FirebaseAuth.Instance.CurrentUser != null)
            {
                var currentuser = FirebaseAuth.Instance.CurrentUser;
                var uid = currentuser.Uid;
                var loggedEmail = currentuser.Email;
                App.uid = uid;
                App.loggedEmail = loggedEmail;
                return true;
            }else
            {
                return false;
            }
            
        }

        public async Task<string> UpdateEmail(string email)
        {
            string _result = null;
            var currentUser = FirebaseAuth.Instance.CurrentUser;
            var credentials = FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(App.loggedEmail, App.password).ContinueWith(async task =>{
                await FirebaseAuth.Instance.CurrentUser.UpdateEmailAsync(email).ContinueWith(async task2 =>
                {
                    if (task2.IsFaulted)
                        if (task2.Exception != null)
                            _result = ((FirebaseAuthException)task2.Exception.GetBaseException()).ErrorCode;


                });
            });

            
            return _result;
        }

        public async Task<string> UpdatePassword(string password)
        {
            
                string _result = null;
            await FirebaseAuth.Instance.CurrentUser.UpdatePasswordAsync(password).ContinueWith(async task =>
            {
                if (task.IsFaulted)
                    if (task.Exception != null)
                        _result = ((FirebaseAuthException)task.Exception.GetBaseException()).ErrorCode;
                    
                    
            });
            return _result;
        }

        public async Task<string> DoSignUp(string email, string password, string nome, string cognome)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var _result = "";
                await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(async task =>
                {
                    if (task.IsFaulted)
                    {
                        //Noget gik galt, returner FirebaseAuth fejlkoden
                        if (task.Exception != null)
                            _result = ((FirebaseAuthException)task.Exception.GetBaseException()).ErrorCode;
                    }
                    else
                    {
                        var currentuser = FirebaseAuth.Instance.CurrentUser;
                        var uid = currentuser.Uid;

                   
                        //FIREBASEHELPER E' UNA CLASSE CHE CONTIENE TUTTI LE QUERY PER LA SCRITTURA/LETTURA DEL DATABASE DI FIREBASE
                        AddPerson(nome, cognome, uid);
                    }
                });
                return _result;
            }
            return "ERROR_EMAIL_OR_PASSWORD_MISSING";
        }

        //TASK PER LOGIN CON FIREBASE
        public async Task AddPerson(string nome, string cognome, string uid)
        {
            string ChildNameAdd = ChildName + "/" + uid;
            // User user = new User(cognome, nome, uid);
            //await firebase.Child(ChildNameAdd).PostAsync(user); Il metodo postAsync genera un nodo padre random

            await firebase.Child(ChildNameAdd).PutAsync(new User() { Cognome = cognome, Nome = nome, Uid = uid, PalestraIscrizione = null, AbbonamentoIscrizione = null }); //Il metodo PutAsync non genera un nodo padre random, ma segue il percorso dato da me

        }



        public async Task<string> DoSignUpGym(string email, string password, string nome, string citta, string indirizzo)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var _result = "";
                await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(async task =>
                {
                    if (task.IsFaulted)
                    {
                        //Noget gik galt, returner FirebaseAuth fejlkoden
                        if (task.Exception != null)
                            _result = ((FirebaseAuthException)task.Exception.GetBaseException()).ErrorCode;
                    }
                    else
                    {
                        var currentuser = FirebaseAuth.Instance.CurrentUser;
                        var uid = currentuser.Uid;
                        App.uid = currentuser.Uid;

                        //FIREBASEHELPER E' UNA CLASSE CHE CONTIENE TUTTI LE QUERY PER LA SCRITTURA/LETTURA DEL DATABASE DI FIREBASE
                        AddGym(nome, citta, indirizzo, uid);
                    }
                });
                return _result;
            }
            return "ERROR_EMAIL_OR_PASSWORD_MISSING";
        }





        public async Task AddSub(string tipoAbbonamento, double costo, DateTime dataI, DateTime dataF, string uidAbbonamento, string uidUtente)
        {
            string childSub = "abbonamenti/" + uidAbbonamento;
            string childName = "utenti/" + uidUtente;
            // User user = new User(cognome, nome, uid);
            //await firebase.Child(ChildNameAdd).PostAsync(user); Il metodo postAsync genera un nodo padre random

            await firebase.Child(childSub).PutAsync(new Abbomamento() { uid = uidAbbonamento, TipoAbbonamento = tipoAbbonamento, Costo = costo, DataInizio = dataI, DataFine = dataF }).ContinueWith(async task =>
            {
                await firebase.Child(ChildName).Child("/AbbonamentoIscrizione").PutAsync(uidUtente);
            }); //Il metodo PutAsync non genera un nodo padre random, ma segue il percorso dato da me
        }






        public async Task AddGym(string nome, string citta, string indirizzo, string uid)
        {
            string childGym = "palestre/" + uid;
            // User user = new User(cognome, nome, uid);
            //await firebase.Child(ChildNameAdd).PostAsync(user); Il metodo postAsync genera un nodo padre random

            await firebase.Child(childGym).PutAsync(new Gym() { Citta = citta, Indirizzo = indirizzo, Nome = nome, Uid = uid}); //Il metodo PutAsync non genera un nodo padre random, ma segue il percorso dato da me
        }




        //TASK PER REIMPOSTA PASSWORD
        public async Task<string> ResetPassword(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var err = "";
                await FirebaseAuth.Instance.SendPasswordResetEmailAsync(email).ContinueWith(async task =>
                {
                    if (task.IsFaulted)
                        if (task.Exception != null)
                            err = ((FirebaseAuthException)task.Exception.GetBaseException()).ErrorCode;
                });
                return err;
            }
            return "ERROR_EMAIL_MISSING";
        }

       


        public async Task<List<User>> GetAllPerson()
        {
            return (await firebase
            .Child(ChildName)
             .OnceAsync<User>()).Select(item => new User
             {
                 Nome = item.Object.Nome,
                 Cognome = item.Object.Cognome,
                 Uid = item.Object.Uid,
                 PalestraIscrizione = item.Object.PalestraIscrizione, 
                 AbbonamentoIscrizione = item.Object.AbbonamentoIscrizione
             }).ToList();


        }


        public async Task<User> GetPerson(string uid)
        {
            var allPersons = await GetAllPerson();
            await firebase
                .Child(ChildName)
                .OnceAsync<User>();
            return allPersons.FirstOrDefault(a => a.Uid == uid);
        }


        public async Task<List<Gym>> GetAllGym()
        {
            return (await firebase
            .Child(ChildNameGym)
             .OnceAsync<Gym>()).Select(item => new Gym
             {
                 Nome = item.Object.Nome,
                 Citta = item.Object.Citta,
                 Indirizzo = item.Object.Indirizzo,
                 Uid = item.Object.Uid
             }).ToList();


        }


        public async Task<Gym> GetGym(string uid)
        {
            var allGym = await GetAllGym();
            await firebase
                .Child(ChildNameGym)
                .OnceAsync<Gym>();
            return allGym.FirstOrDefault(a => a.Uid == uid);
        }


        public async Task DeletePerson()
        {
            string uid = App.uid;
            await firebase.Child(ChildName).Child(uid).DeleteAsync();
        }

        public void Logout()
        {
            Preferences.Set("uid", null);
            Preferences.Set("loggedEmail", null);
            Preferences.Set("password", null);
            Preferences.Set("profilePic", null);
            FirebaseAuth.Instance.SignOut();
        }
    }
}