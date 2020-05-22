using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.resetPassword
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetSuccessful : ContentPage
    {
        public ResetSuccessful()
        {
            InitializeComponent();
        }

        public void LoginLink(Object sender, EventArgs e)
        {
            Navigation.RemovePage(this);
            Navigation.PopAsync();
        }
    }
}