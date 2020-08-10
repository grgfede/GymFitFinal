using GymFitFinal.Interfaces;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home.palestra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrenotaTurno : PopupPage
    {
        private static Random random = new Random();
        private IFirebaseAuthenticator _auth;
        string uidUtente = App.uid;
        string uidPalestra = Gym.uid;

        public PrenotaTurno(string uidPalestra, DateTime giornoScelto)
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            string DataOdierna = String.Format("{0:MM/dd/yyyy}", giornoScelto);
            App.giornoscelto = DataOdierna;

            getTurni(uidPalestra, giornoScelto);
        }


        public async void prenotaTurno(object sender, EventArgs e)
        {
            if ((chkMat.IsChecked) && (chkPom.IsChecked))
            {
                DisplayAlert("Attenzione!", "Si prega di selezionare solo Mattina o Pomeriggio.", "ok");
            }
            else
            {
                TurnoPrenotato prenotazione = new TurnoPrenotato();
                prenotazione.uid = RandomString(28);
                prenotazione.DataPrenotazione = App.giornoscelto;
                prenotazione.uidUtente = uidUtente;
                prenotazione.uidPalestra = uidPalestra;
                await _auth.AddPrenotazione(prenotazione).ContinueWith(async task =>
                {
                    DisplayAlert("Complimenti", "Prenotazione avvenuta con successo", "ok");
                });
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async void getTurni(string uidPalestra, DateTime giornoScelto)
        {
            var turniPalestra = await _auth.GetTurni(uidPalestra);
            DayOfWeek giorno = giornoScelto.DayOfWeek;
            if (turniPalestra != null)
            {
              if (giorno == DayOfWeek.Monday)
                {
                    if (!turniPalestra.LunMat)
                    {
                        chkMat.IsVisible = false;
                        lblMat.IsVisible = false;
                    } 
                    if (!turniPalestra.LunPom)
                    {
                        chkPom.IsVisible = false;
                        lblPom.IsVisible = false;
                    }
                } else if (giorno == DayOfWeek.Tuesday)
                {
                    if (!turniPalestra.MarMat)
                    {
                        chkMat.IsVisible = false;
                        lblMat.IsVisible = false;
                    } 
                    if (!turniPalestra.MarPom)
                    {
                        chkPom.IsVisible = false;
                        lblPom.IsVisible = false;
                    }
                } else if (giorno == DayOfWeek.Wednesday)
                {
                    if (!turniPalestra.MerMat)
                    {
                        chkMat.IsVisible = false;
                        lblMat.IsVisible = false;
                    } 
                    if (!turniPalestra.MerPom)
                    {
                        chkPom.IsVisible = false;
                        lblPom.IsVisible = false;
                    }
                } else if (giorno == DayOfWeek.Thursday)
                {
                    if (!turniPalestra.GioMat)
                    {
                        chkMat.IsVisible = false;
                        lblMat.IsVisible = false;
                    } 
                    if (!turniPalestra.GioPom)
                    {
                        chkPom.IsVisible = false;
                        lblPom.IsVisible = false;
                    }
                } else if(giorno == DayOfWeek.Friday)
                {
                    if (!turniPalestra.VenMat)
                    {
                        chkMat.IsVisible = false;
                        lblMat.IsVisible = false;
                    } 
                    if (!turniPalestra.VenPom)
                    {
                        chkPom.IsVisible = false;
                        lblPom.IsVisible = false;
                    }
                } else if (giorno == DayOfWeek.Saturday)
                {
                    if (!turniPalestra.SabMat)
                    {
                        chkMat.IsVisible = false;
                        lblMat.IsVisible = false;
                    } 
                    if (!turniPalestra.SabPom)
                    {
                        chkPom.IsVisible = false;
                        lblPom.IsVisible = false;
                    }
                } else if (giorno == DayOfWeek.Sunday)
                {
                    if (!turniPalestra.DomMat)
                    {
                        chkMat.IsVisible = false;
                        lblMat.IsVisible = false;
                    } 
                    if (!turniPalestra.DomPom)
                    {
                        chkPom.IsVisible = false;
                        lblPom.IsVisible = false;
                    }
                }
            }
        }
    }
}