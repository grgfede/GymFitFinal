using GymFitFinal.home.palestra;
using GymFitFinal.Interfaces;
using Rg.Plugins.Popup.Services;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Runtime.CompilerServices;
using Java.Nio.Channels;

namespace GymFitFinal.home.navBar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PalestraIscrizione : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        FirebaseStorageHelper firebaseStorageHelper = new FirebaseStorageHelper();
        MediaFile file;
        public PalestraIscrizione(string uid)
        {
            Gym.uid = uid;
            InitializeComponent();
            _auth = DependencyService.Get<IFirebaseAuthenticator>();
            getInfo(uid);

            //PER POTERSI REGISTRARE ALLA PALESTRA CHE L'UTENTE STA VISUALIZZANDO
            //HO BISOGNO DEL SUO UID PER CAPIRE SE E' GIA' REGISTRATO O MENO
            string uidUser = App.uid;
            canUserRegister(uidUser);
        
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            getInfo(Gym.uid);
        }

        public async void canUserRegister(string uidUser)
        {
            var person = await _auth.GetPerson(uidUser);
            if(person.PalestraIscrizione == null)
            {
                //CASO IN CUI NON SONO ISCRITTO A NESSUNA PALESRA
                ToolbarItem item = new ToolbarItem
                {
                    IconImageSource = ImageSource.FromFile("followGym.png"),
                };
                this.ToolbarItems.Add(item);
                item.Clicked += followGym;
            } else if(string.Equals(person.PalestraIscrizione, Gym.uid))
            {
                //CASO IN CUI SONO ISCRITTO A ALLA PALESTRA CHE VEDO
                ToolbarItem item = new ToolbarItem
                {
                    IconImageSource = ImageSource.FromFile("unfollowGym.png"),
                };
                this.ToolbarItems.Add(item);
                item.Clicked += unfollowGym;
            }
            else {
                //CASO IN CUI SONO ISCRITTO AD UN'ALTRA PALESTRA
            }
        }

        public async void followGym(object sender, EventArgs e)
        {
            string uidU = App.uid;
            string uidG = Gym.uid;
            await _auth.followGym(uidU, uidG).ContinueWith(async task =>
            {
                if (task.IsFaulted)
                {
                    DisplayAlert("Attenzione", "Non è possibile iscriversi a questa palestra, riprova più tardi", "ok");
                }
            });
            Navigation.PopAsync();
            Navigation.PushAsync(new navBar.PalestraIscrizione(Gym.uid));
            
        }

        public async void unfollowGym(object sender, EventArgs e)
        {
            string uidU = App.uid;
            string uidG = Gym.uid;
            await _auth.unfollowGym(uidU).ContinueWith(async task =>
            {
                if (task.IsFaulted)
                {
                    DisplayAlert("Attenzione", "Non è possibile disinscriversi a questa palestra, riprova più tardi", "ok");
                }
            });
            Navigation.PopAsync();
            Navigation.PushAsync(new navBar.PalestraIscrizione(Gym.uid));
        }

        public async void getInfo(string uid)
        {
            var person = await _auth.GetGym(uid);
            if (person != null)
            {
                lblNome.Text = person.Nome + " ";
                lblCitta.Text = "Dove siamo: " + person.Citta;
                string pic = await _auth.getProfilePicGymIscrizione(uid);
                pictureBox.Source = pic;
                Gym.citta = person.Citta;
                Gym.indirizzo = person.Indirizzo;
            }
            else
            {
                DisplayAlert("Attenzione!", "C'è stato un errore, riprova più tardi.", "OK");
            }
        }

        public async void OpenMap(Object sender, EventArgs e)
        {
            
            await Launcher.OpenAsync("geo:0,0?q=394+" + Gym.citta + "+" + Gym.indirizzo);
        }

        private async void TurniDisponibili(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TurniDisponibili());
        }


        private async void orariPopup(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new OrariPopup(Gym.uid));
        }

    }
}