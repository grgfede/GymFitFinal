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
            getSubs();
        }

        public async void getSubs()
        {
            List<AbbonamentoPalestra> allSubGym = await _auth.getSubGym();
            if (allSubGym != null)
            {
               foreach (AbbonamentoPalestra e in allSubGym)
               {
                    StackLayout stackLayout = new StackLayout
                    {
                        Spacing = 0,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Children =
                        {
                            new Label
                            {
                                Text = "StackLayout",
                                HorizontalOptions = LayoutOptions.Start
                            },
                        }
                    };
               }

            }
        }

        public void addSub(Object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddSub());
        }
    }
}