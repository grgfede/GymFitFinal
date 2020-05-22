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

[assembly: Xamarin.Forms.Dependency(typeof(AndroAuth))]

namespace GymFitFinal.Droid.Interfaces
{

   
    public class AndroAuth : GymFitFinal.Interfaces.IFirebaseAuthenticator
    {
        private readonly string ChildName = "utenti";

        readonly FirebaseClient firebase = new FirebaseClient("https://gymfitt-2b845.firebaseio.com/");

       



   

        //TASK PER LOGIN CON FIREBASE
        public async Task AddPerson(string nome, string cognome, string uid)
        {
            string ChildNameAdd = ChildName + "/" + uid;
            // User user = new User(cognome, nome, uid);
            //await firebase.Child(ChildNameAdd).PostAsync(user); Il metodo postAsync genera un nodo padre random

            await firebase.Child(ChildNameAdd).PutAsync(new User() { Cognome = cognome, Nome = nome, Uid = uid }); //Il metodo PutAsync non genera un nodo padre random, ma segue il percorso dato da me

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
                        //var token2 = await task.Result.User.Uid();
                        _result = token.Token;

                        var currentuser = FirebaseAuth.Instance.CurrentUser;
                        var uid = currentuser.Uid;
                        App.uid = uid;

                        App.IsUserLoggedIn = true;
                    }
                });
                return _result;
            }
            return "ERROR_EMAIL_OR_PASSWORD_MISSING";
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
                 Uid = item.Object.Uid
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


        public async Task DeletePerson()
        {
            string uid = App.uid;
            await firebase.Child(ChildName).Child(uid).DeleteAsync();
        }


    }
}