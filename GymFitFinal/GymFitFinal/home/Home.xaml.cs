﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymFitFinal.Interfaces;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : TabbedPage
    {
        private IFirebaseAuthenticator _auth;
        private List<User> lstPersons;
        public bool flagGym { get; set; }
        public Home()
        {
            string uid = App.uid;
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            //VERIFICO SE CHI SI E' LOGGATO E' UTENTE O PALESTRA
            getInfo(uid);

            NavigationPage.SetHasBackButton(this, false);

        }

        public async void getInfo(string uid)
        {
            var person = await _auth.GetPerson(uid);
            var gym = await _auth.GetGym(uid);

            //CICLO GLI UTENTI NEL DATABASE, SE NON TROVO UN UTENTE CON QUEL UID
            if (person == null)
            {
             //PASSO A CERCARE LE PALESTRE CON QUEL UID  
                if (gym != null)
                {
                    //SE TROVO UNA PALESTRA, SALVO LA VARIABILE APP COME TRUE
                    App.isGym = true;
                    //AGGIUNGO NEL TABBED PAGE, LA PAGINA DI PROFILO PER LE PALESTRE
                    this.Title = "Profilo";
                    this.IconImageSource = "navbarProfile.png";
                    this.Children.Add(new palestra.profiloPalestra());

                } 
            } else {
                App.isGym = false;
                //AGGIUNGO NEL TABBED PAGE, LA PAGINA DI PROFILO PER GLI UTENTI
                this.Title = "Profilo";
                this.IconImageSource = "navbarProfile.png";
                this.Children.Add(new home.navBar.Profilo());
            }
        }
       


        }




    
        
}