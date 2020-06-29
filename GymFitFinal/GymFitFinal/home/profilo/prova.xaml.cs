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
    public partial class prova : ContentPage
    {
        public prova()
        {
            InitializeComponent();

            SetValue(NavigationPage.HasNavigationBarProperty, false);
        }


    }


}