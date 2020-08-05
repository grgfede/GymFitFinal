using GymFitFinal.Interfaces;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home.palestra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CercaTurno : ContentPage
    {
        private static Random random = new Random();
        string uidUtente = App.uid;
        string uidPalestra = Gym.uid;
        private IFirebaseAuthenticator _auth;

        public CercaTurno()
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
        }




        public async void TurnoPop(Object sender, EventArgs e)
        {
            DateTime dataI = (DateTime)calendar.SelectedDate;
            string dataInizio = String.Format("{0:MM/dd/yyyy}", dataI);
            DateTime dataO = DateTime.Today;
            string DataOdierna = String.Format("{0:MM/dd/yyyy}", dataO);

            if (dataI >= dataO)
            {
                await PopupNavigation.PushAsync(new PrenotaTurno(uidPalestra));
                /*TurnoPrenotato prenotazione = new TurnoPrenotato();
                prenotazione.uid = RandomString(28);
                prenotazione.DataPrenotazione = dataInizio;
                prenotazione.uidUtente = uidUtente;
                prenotazione.uidPalestra = uidPalestra;
                await _auth.AddPrenotazione(prenotazione).ContinueWith(async task =>
                {
                    DisplayAlert("Complimenti", "Prenotazione avvenuta con successo", "ok");
                });*/
            }
            else
            {
                DisplayAlert("Attenzione!", "Ciao viaggiatore del tempo! Quest'applicazione non è ancora pensata per i viaggitori del tempo! Scegli un giorno valido.", "ok");
            }
        }

        public async void prenotaTurno(Object sender, EventArgs e)
        {
            
           
        }




        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void calendar_DateClicked(object sender, XamForms.Controls.DateTimeEventArgs e)
        {

        }
    }
}