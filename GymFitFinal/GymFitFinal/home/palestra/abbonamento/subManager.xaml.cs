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
            List<AbbonamentoPalestra> sub = new List<AbbonamentoPalestra>();
            if (allSubGym != null)
            {
                foreach (AbbonamentoPalestra e in allSubGym)
                {
                    if (String.Equals(e.uidPalestra, uid))
                    {
                        sub.Add(e);
                    }
                   
                }
                lstAbbonamenti.ItemsSource = sub;
            }
              
        }




        public async void lvItemTapped(object sender, ItemTappedEventArgs e)
        {
            var myListView = (ListView)sender;
            var myItem = myListView.SelectedItem;
            AbbonamentoPalestra a1 = (AbbonamentoPalestra)myItem;
            await Navigation.PushAsync(new home.palestra.abbonamento.CreatedSub(a1.uid));
        }


        public void addSub(Object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddSub());
        }
    }
}