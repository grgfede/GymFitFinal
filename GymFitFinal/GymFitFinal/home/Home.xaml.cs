using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymFitFinal.Interfaces;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        private List<User> lstPersons;
        string uid = App.uid;

        public Home()
        {
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            getInfo(uid);   
        }

        public async void esegui (Object args, EventArgs e)
        {
            await _auth.DeletePerson();
        }









        public async void getInfo(string uid)
        {

            var person = await _auth.GetPerson(uid);

            if (person != null)
            {
                lblWelcome.Text = person.Nome;
                lblWelcome.Text += person.Cognome;
            }
            else
            {
                DisplayAlert("Attenzione!", "C'è stato un errore, riprova più tardi.", "OK");
                Navigation.RemovePage(this);
                Navigation.PopAsync();
            }
               
                
            

        }




       


        private async Task FetchAllPersons()
        {
            var allPersons = await _auth.GetAllPerson();

        }




    }
        
}