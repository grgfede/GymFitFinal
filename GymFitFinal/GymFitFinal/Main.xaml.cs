﻿using GymFitFinal.Interfaces;
using GymFitFinal.SignUp;
using Java.Security.Cert;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : ContentPage
    {
        public Main()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                InitializeComponent();
                SignUpMethod();
            }
            else
            {
                DisplayAlert("Attenzione!", "Nessuna connessione ad Internet", "Ok", "Riprova");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }  
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
        }

        public async void signUpGym(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new signUpGym());

        }
        public async void LoginEmail(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginWithEmail());
        }

        void SignUpMethod()
        {
            lblSignUp.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PushAsync(new SignUp.signUp());
                })
            });
        }

       

    }
}