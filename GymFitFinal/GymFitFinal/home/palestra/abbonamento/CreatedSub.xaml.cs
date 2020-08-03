using GymFitFinal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home.palestra.abbonamento
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatedSub : ContentPage
    {

        private IFirebaseAuthenticator _auth;
        string uidPalestra = App.uid;
        public CreatedSub(string uidAbbonamentoPalestra)
        {
            InitializeComponent();
            App.uidAbbonamentoPalestra = uidAbbonamentoPalestra;
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            this.BindingContext = new model.ModelAbbonamentoPalestra();
            getInfoSub(uidAbbonamentoPalestra);
        }



        public async void getInfoSub(string uidS)
        {
            var sub = await _auth.GetSubGymSpecific(uidS);
            if (sub != null)
            {
                lblTipoAbbonamento.Text = "Tipo abbonamento: " + sub.TipoAbbonamento;
                lblCosto.Text = "Costo: " + sub.Costo + "€";
                lblDescrizione.Text = "Descrizione: " + sub.Descrizione;
            }
            else
            {
                lblTipoAbbonamento.IsVisible = false;
                lblDescrizione.IsVisible = false;
                lblCosto.IsVisible = false;
                lblNoSub.IsVisible = true;
            }

        }
    }
}