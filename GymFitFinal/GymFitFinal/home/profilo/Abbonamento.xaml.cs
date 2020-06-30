using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home.profilo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Abbonamento : ContentPage
    {
        public Abbonamento()
        {
            InitializeComponent();
        }

        void BackMethod()
        {
            lblBack.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PushAsync(new home.Home());
                })
            });
        }
    }
}