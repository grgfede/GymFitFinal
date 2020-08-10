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
                var turnoPrenotato = await _auth.GetTurnoPrenotato(uidUtente);
                if (turnoPrenotato == null)
                    await PopupNavigation.PushAsync(new PrenotaTurno(uidPalestra, dataI));
                else
                    DisplayAlert("Attenzione!", "Hai già una prenotazione per un turno.", "ok");
            }
            else
            {
                DisplayAlert("Attenzione!", "Ciao viaggiatore del tempo! Quest'applicazione non è ancora pensata per i viaggitori del tempo! Scegli un giorno valido.", "ok");
            }
        }

        public async void prenotaTurno(Object sender, EventArgs e)
        {
            
           
        }



    }
}