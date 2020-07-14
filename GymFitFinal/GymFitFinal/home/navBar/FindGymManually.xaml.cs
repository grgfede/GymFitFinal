using Android.App;
using Android.Content;
using GymFitFinal.Interfaces;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home.navBar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindGymManually : PopupPage
    {
        private IFirebaseAuthenticator _auth;

        public FindGymManually()
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            List<string> filtri = new List<string>();
            filtri.Add("Nome");
            filtri.Add("Citta");
            pickerFilterFind.ItemsSource = filtri;


            pickerFilterFind.SelectedIndexChanged += async (sender, args) =>
            {
                if (pickerFilterFind.SelectedIndex == -1)
                {

                }
                else
                {
                    string colorName = pickerFilterFind .Items[pickerFilterFind.SelectedIndex];
                    pickerFilterFind.SelectedItem = -1;
                    if (colorName.Equals("Nome"))
                    {

                        result.IsVisible = false;
                        parameter.IsVisible = true;
                        parameter.Placeholder = "Inserisci nome";
                        btnFindGymName.IsVisible = true;
                    }
                    else
                    {
                        result.IsVisible = false;
                        parameter.IsVisible = true;
                        parameter.Placeholder = "Inserisci città";
                        btnFindGymCity.IsVisible = true;
                    }
                }
            };

        }

        public async void findGymName(Object Sender, EventArgs e)
        {
            string nome = parameter.Text;
            if (String.IsNullOrEmpty(nome))
            {
                DisplayAlert("Attenzione!", "Inserisci un nome valido!", "ok");

            }
            else
            {
                nome = nome.First().ToString().ToUpper() + nome.Substring(1);
                List<Gym> palestreRisultato = await _auth.GetGymByName(nome);

                if (palestreRisultato != null)
                {
                    result.ItemsSource = palestreRisultato;
                    result.IsVisible = true;
                }
                else
                {
                    DisplayAlert("Nessun risultato trovato", "Nessun risultato trovato!", "ok");
                }
            }
            
        }

        public async void findGymCity(Object Sender, EventArgs e)
        {
            string nome = parameter.Text;
            if (String.IsNullOrEmpty(nome))
            {
                DisplayAlert("Attenzione!", "Inserisci una città valida!", "ok");
            } else
            {
                nome = nome.First().ToString().ToUpper() + nome.Substring(1);
                List<Gym> palestreRisultato = await _auth.GetGymByCity(nome);

                if (palestreRisultato != null)
                {
                   result.ItemsSource = palestreRisultato;
                   result.IsVisible = true;
                }
                else
                {
                    DisplayAlert("Nessun risultato trovato", "Nessun risultato trovato!", "ok");
                }
            }
           
        }


        public async void lvItemTapped(object sender, ItemTappedEventArgs e)
        {
            var myListView = (ListView)sender;
            var myItem = myListView.SelectedItem;
            Gym g1 = (Gym) myItem;
            await Navigation.PushAsync(new navBar.PalestraIscrizione(g1.Uid));
            await PopupNavigation.Instance.PopAsync();
        }
    }
}