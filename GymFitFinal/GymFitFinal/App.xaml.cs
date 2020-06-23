﻿using GymFitFinal.Interfaces;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal
{
    public partial class App : Application
    {
        internal static string Token;
        private readonly IFirebaseAuthenticator _auth;

        public static bool IsUserLoggedIn { get; set; }
        public static string uid { get; set; }
        public NavigationPage ContentPage { get; }

        public static User loggedUser { get; set; }     //loggedUser è un oggetto di tipo USER che contiene tutte le informazioni dell'utente loggato
        
        public static string loggedEmail { get; set; }
       
        public static string password { get; set; }
        public App()
        {
            
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();

            //MainPage = new NavigationPage(new SignUp.signUpGym());
            MainPage = new NavigationPage(new home.palestra.profiloPalestra());
            /* if (DependencyService.Get<IFirebaseAuthenticator>().IsUserLoggedIn())
                 MainPage = new NavigationPage(new home.Home());
             else
                 MainPage = new NavigationPage(new Main());*/



        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        internal static void ShowError(string token)
        {
            throw new NotImplementedException();
        }
    }
}
