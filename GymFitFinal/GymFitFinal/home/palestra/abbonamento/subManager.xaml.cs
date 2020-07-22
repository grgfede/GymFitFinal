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
    public partial class subManager : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        string uid = App.uid;
        public subManager()
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            //getSubs();
        }

        public async void getSubs()
        {
            List<AbbonamentoPalestra> allSubGym = await _auth.getSubGym();
            if (allSubGym != null)
            {
                int i = 0;
                foreach (AbbonamentoPalestra e in allSubGym)
                {
                    i += 1;
                    DisplayAlert("a", Convert.ToString(i), "ik");
                    Label l = new Label { Text = "prova" };
                    stack.Children.Add(l);
                }
               
            }
              
        }

        

        public void addSub(Object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddSub());
        }
    }
}