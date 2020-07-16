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
    public partial class OrariPopup : PopupPage
    {
        private IFirebaseAuthenticator _auth;
        public OrariPopup(string uid)
        {
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            InitializeComponent();
            showTime(uid);
        }
       
        public async void showTime(string uid)
        {
            var g1 = await _auth.GetGym(uid);
            lblMat.Text += " " + g1.DataIMattina + "-" + g1.DataFMattina;
            lblPom.Text += " " + g1.DataIPomeriggio + "-" + g1.DataFPomeriggio;

        }

    }
}