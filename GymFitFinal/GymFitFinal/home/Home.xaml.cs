using System;
using System.Collections.Generic;
using System.Globalization;
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
                checkTurnoPrenotato(uid);
                //checkSub(App.loggedUser.AbbonamentoIscrizione);
            } else
            {
                DisplayAlert("Attenzione!", "Nessuna connessione ad Internet", "Ok", "Riprova");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            NavigationPage.SetHasBackButton(this, false);
            

        }


        public async void checkTurnoPrenotato(string uidS)
        {
            var turnoPrenotato = await _auth.GetTurnoPrenotato(uidS);
            if (turnoPrenotato != null)
            {
                string dataFine = turnoPrenotato.DataPrenotazione;
                DateTime dataFineConvert = DateTime.ParseExact(dataFine, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime now = DateTime.Now;
                
                if (dataFineConvert < now)
                {
                    await _auth.DeleteTurnoPrenotato(turnoPrenotato.uid).ContinueWith(async task =>
                    {
                        if (task.IsFaulted)
                        {
                            DisplayAlert("PRova", "errore", "Ok");
                        }
                    });
                }
            }
        }



        public async void checkSub(string uidS)
        {
            var sub = await _auth.GetSub(uidS);
            if (sub != null)
            {
                string dataFine = sub.DataFine;
                DateTime dataFineConvert = DateTime.ParseExact(dataFine, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime now = DateTime.Now;
                if (dataFineConvert < now)
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
                    Children.Add(new palestra.profiloPalestra() { Title = "Palestra", IconImageSource = "bicipite.png" });             //MOSTRA LA PAGINA PROFILOPALESTRA
                } 
            } else {
                //SE L'UTENTE E' ISCRITTO AD UNA PALESTRA
                if (person.PalestraIscrizione != null)
                {
                    Children.Add(new navBar.PalestraIscrizione(person.PalestraIscrizione) {Title = "Palestra", IconImageSource = "manubrio.png" });        //MOSTRA LA PAGINA PALESTRAISCRIZIONE
                    //RECUPERO LE INFO DELLA PALESTRA A CUI E' ISCRITTO E MOSTRO LE INFO DELLA PALESTRA
                    gymIscrizione = await _auth.GetGym(person.PalestraIscrizione);
                }
                else
                {
                    //ALTIRMENTI, SE L'UTENTE NON E' ISCRITTO AD UNA PALESTRA, VISUALIZZO IL BOT
                    Children.Add(new navBar.Palestra() { Title = "Palestra", IconImageSource = "chat.png" });                      //MOSTRA LA PAGINA PALESTRA(Il bot)

                }
                //AGGIUNGO NEL TABBED PAGE, LA PAGINA DI PROFILO PER GLI UTENTI

                App.isGym = false;
                Children.Add(new home.navBar.Profilo() { Title = "Profilo", IconImageSource = "navbarProfile.png" });      //MOSTRA LA PAGINA PROFILO (utente)
            }
        }



    }




    
        
}