using Android.Net.Wifi.Aware;
using GymFitFinal.Interfaces;
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
    public partial class InserisciTurni : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        private static Random random = new Random();
        private string uidPalestra = App.uid;
        public Turno turno;
        public InserisciTurni()
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            getTurno();

        }





        public async void getTurno()
        {
            var turno = await _auth.GetTurni(uidPalestra);
            if (turno != null)
            {
                chkLunMat.IsChecked = turno.LunMat;
                chkLunPom.IsChecked = turno.LunPom;
                chkMarMat.IsChecked = turno.MarMat;
                chkMarPom.IsChecked = turno.MarPom;
                chkMerMat.IsChecked = turno.MerMat;
                chkMerPom.IsChecked = turno.MerPom;
                chkGioMat.IsChecked = turno.GioMat;
                chkGioPom.IsChecked = turno.GioPom;
                chkVenMat.IsChecked = turno.VenMat;
                chkVenPom.IsChecked = turno.VenPom;
                chkSabMat.IsChecked = turno.SabMat;
                chkSabPom.IsChecked = turno.SabPom;
                chkDomMat.IsChecked = turno.DomMat;
                chkDomPom.IsChecked = turno.DomPom;
                txtCapienza.Text = turno.Capienza;
            }
        }

        /*
            * Metodo che permette la creazione/modifica dei turni della palestra
            * Due diverse "visuali"
            * 1) La palestra è la prima volta che crea un turno
            * 2) La palestra modifica turni già esistenti
            */
        public async void modificaTurni(Object sender, EventArgs e)
        {
            /*
             * Mi ricavo l'informazione principale: La palestra è la prima volta che crea un turno?
             * Cerco nel database un turno appartenente alla palestra
             */

            var turno = await _auth.GetTurni(uidPalestra);

            //Se il turno cercato nel database esiste (!= null), proseguo con modificarlo
            if (turno != null)
            {
                bool LunMat, LunPom, MarMat, MarPom, MerMat, MerPom, GioMat, GioPom, VenMat, VenPom, SabMat, SabPom, DomMat, DomPom;
                string capienza = txtCapienza.Text;
                turno.LunMat = chkLunMat.IsChecked;
                turno.LunPom = chkLunPom.IsChecked;
                turno.MarMat = chkMarMat.IsChecked;
                turno.MarPom = chkMarPom.IsChecked;
                turno.MerMat = chkMerMat.IsChecked;
                turno.MerPom = chkMerPom.IsChecked;
                turno.GioMat = chkGioMat.IsChecked;
                turno.GioPom = chkGioPom.IsChecked;
                turno.VenMat = chkVenMat.IsChecked;
                turno.VenPom = chkVenPom.IsChecked;
                turno.SabMat = chkSabMat.IsChecked;
                turno.SabPom = chkSabPom.IsChecked;
                turno.DomMat = chkDomMat.IsChecked;
                turno.DomPom = chkDomPom.IsChecked;
                turno.Capienza = capienza;
                await _auth.UpdateTurno(turno).ContinueWith(async task =>
                {
                    if (!task.IsFaulted)
                    {
                       await DisplayAlert("Turni salvati", "Turni salvati correttamente", "ok");

                    }
                });

            } else
            {
                //Se il turno cercato nel database non esiste, lo creo
                bool LunMat, LunPom, MarMat, MarPom, MerMat, MerPom, GioMat, GioPom, VenMat, VenPom, SabMat, SabPom, DomMat, DomPom;
                string PalestraIscrizione, uidTurno, capienza;
                LunMat = chkLunMat.IsChecked;
                LunPom = chkLunPom.IsChecked;
                MarMat = chkMarMat.IsChecked;
                MarPom = chkMarPom.IsChecked;
                MerMat = chkMerMat.IsChecked;
                MerPom = chkMerPom.IsChecked;
                GioMat = chkGioMat.IsChecked;
                GioPom = chkGioPom.IsChecked;
                VenMat = chkVenMat.IsChecked;
                VenPom = chkVenPom.IsChecked;
                SabMat = chkSabMat.IsChecked;
                SabPom = chkSabPom.IsChecked;
                DomMat = chkDomMat.IsChecked;
                DomPom = chkDomPom.IsChecked;
                capienza = txtCapienza.Text;
                PalestraIscrizione = uidPalestra;
                uidTurno = RandomString(28);

                await _auth.AddTurno(uidTurno, PalestraIscrizione, capienza, LunMat, LunPom, MarMat, MarPom, MerMat, MerPom, GioMat, GioPom, VenMat, VenPom, SabMat, SabPom, DomMat, DomPom).ContinueWith(async task =>
                {
                    if (!task.IsFaulted)
                    {
                        await DisplayAlert("Turni salvati", "Turni salvati correttamente", "ok");
                    }
                });

            }
        }



        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}