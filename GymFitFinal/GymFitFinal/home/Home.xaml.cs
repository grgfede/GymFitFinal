using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymFitFinal.home.navBar;
using GymFitFinal.Interfaces;
using Plugin.Connectivity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : TabbedPage
    {
        private IFirebaseAuthenticator _auth;
        private List<User> lstPersons;
       
        public Home()
        {
            App.uid = Preferences.Get("uid", null);
            App.loggedEmail = Preferences.Get("loggedEmail", null);
            App.password = Preferences.Get("password", null);

            string uid = App.uid;
            if (CrossConnectivity.Current.IsConnected)
            {
                InitializeComponent();
                _auth = DependencyService.Get<IFirebaseAuthenticator>();
                //VERIFICO SE CHI SI E' LOGGATO E' UTENTE O PALESTRA
                getInfo(uid);
                //checkSub(App.loggedUser.AbbonamentoIscrizione);
            } else
            {
                DisplayAlert("Attenzione!", "Nessuna connessione ad Internet", "Ok", "Riprova");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            NavigationPage.SetHasBackButton(this, false);
            

        }

        public async void checkSub(string uidS)
        {
            var sub = await _auth.GetSub(uidS);
            if (sub != null)
            {
                string dataF = sub.DataFine;
                DateTime dt1 = DateTime.Parse(dataF);
                DateTime dt2 = DateTime.Now;
                if (dt1.Date < dt2.Date)
                {
                    DisplayAlert("PROVA", "peova", "Ok");
                    await _auth.DeleteSub(uidS).ContinueWith(async task =>
                    {
                        if (task.IsFaulted)
                        {
                            await _auth.DeleteSub(uidS);
                        }
                    });
                }
            }
        }

        public async void getInfo(string uid)
        {
            var person = await _auth.GetPerson(uid);
            var gym = await _auth.GetGym(uid);
            Gym gymIscrizione = null;

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
                //SE L'UTENTE E' ISCRITTO AD UNA PALESTRA
                if (person.PalestraIscrizione != null)
                {
                    this.Title = "Profilo";
                    this.IconImageSource = "navbarProfile.png";
                    this.Children.Add(new navBar.PalestraIscrizione(person.PalestraIscrizione));

                    //RECUPERO LE INFO DELLA PALESTRA A CUI E' ISCRITTO E MOSTRO LE INFO DELLA PALESTRA
                    gymIscrizione = await _auth.GetGym(person.PalestraIscrizione);                }
                else
                {
                    this.Title = "Profilo";
                    this.IconImageSource = "navbarProfile.png";
                    this.Children.Add(new navBar.Palestra());
                    //ALTIRMENTI, SE L'UTENTE NON E' ISCRITTO AD UNA PALESTRA, VISUALIZZO IL BOT

                }

                App.isGym = false;
                
                //AGGIUNGO NEL TABBED PAGE, LA PAGINA DI PROFILO PER GLI UTENTI
                this.Title = "Profilo";
                this.IconImageSource = "navbarProfile.png";
                this.Children.Add(new home.navBar.Profilo());
                

                
            }
        }
       


        }




    
        
}