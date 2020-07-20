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
    public partial class AddSub : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        string uid = App.uid;
        private static Random random = new Random();

        public AddSub()
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            string tipoAbb = null;

        }

        public async void creaSub(Object sender, EventArgs e)
        {
            double costo = Convert.ToDouble(lblCosto.Text);
            string descrizione = lblNote.Text;
            string tipoAbb = null;
            int indice = pckTipoAbbonamento.SelectedIndex;
            switch (indice)
            {
                case 0:
                    tipoAbb = "Mensile";
                    break;
                case 1:
                    tipoAbb = "Trimestrale";
                    break;
                case 2:
                    tipoAbb = "Semestrale";
                    break;
                case 3:
                    tipoAbb = "Annuale";
                    break;
            }

            if ((String.IsNullOrEmpty(tipoAbb)) || (Double.Equals(costo, 00.00)))
            {
                DisplayAlert("Attenzione!", "Uno o più campi sono vuoti", "Riprova");
            } else
            {
                string uidAbb = RandomString(28);
                await _auth.addSubGym(uidAbb, tipoAbb, descrizione, costo).ContinueWith(async task =>
                {
                    if (task.IsFaulted)
                    {
                        DisplayAlert("Attenzione!", "C'è stato un errore nel creare l'abbonamento, riprova più tardi", "Riprova");
                    }
                    else
                    {
                        DisplayAlert("Complimenti!", "Abbonamento creato con successo!", "ok");
                        Navigation.PopAsync();
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