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
    public partial class TurniDisponibili : ContentPage
    {
        private IFirebaseAuthenticator _auth;

        public TurniDisponibili(string uidPalestra)
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            getTurni(uidPalestra);
        }

        public async void getTurni(string uidPalestra)
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
            }
        }
    }
}