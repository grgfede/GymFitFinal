using GymFitFinal.Interfaces;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home.palestra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class profiloPalestra : ContentPage
    {
        private IFirebaseAuthenticator _auth;
        private List<User> lstPersons;

        string uid = App.uid;

        FirebaseStorageHelper firebaseStorageHelper = new FirebaseStorageHelper();
        MediaFile file;
        public profiloPalestra()
        {
            InitializeComponent();

            var nav = new NavigationPage(new ContentPage { Title = "" });
            nav.BarBackgroundColor = Color.FromHex("#FACO5E");

            picker.SelectedIndexChanged += async (sender, args) =>
            {
                if (picker.SelectedIndex == -1)
                {

                }
                else
                {
                    string colorName = picker.Items[picker.SelectedIndex];
                    picker.SelectedItem = -1;
                    if (colorName.Equals("Modifica Profilo"))
                    {
                        //await Navigation.PushAsync(new ModificaProfilo());
                    }
                    else
                    {
                        _auth.Logout();
                        Navigation.InsertPageBefore(new Main(), Navigation.NavigationStack[0]);
                        await Navigation.PopToRootAsync();

                    }
                }
            };



        }


        /*
        * Metodo che fa "visualizzare" il picker una volta premuto l'icona della gear presente nella toolbar
        */
        public void ToolbarItem_Impostazioni(Object sender, EventArgs e)
        {
            picker.Focus();

        }


    }
}