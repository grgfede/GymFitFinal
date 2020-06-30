using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.SignUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class signUpSuccesfulGym : ContentPage
    {
        public signUpSuccesfulGym(string nome)
        {
            InitializeComponent();
            lblWelcome.Text += nome;

        }


        public async void LoginLink(Object sender, EventArgs e)
        {
            // Remove page before Edit Page
            this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 3]);
            // This PopAsync will now go to List Page
            this.Navigation.PopAsync();
        }
    }
}