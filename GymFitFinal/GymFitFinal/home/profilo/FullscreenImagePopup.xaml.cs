using Rg.Plugins.Popup.Pages;
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
    public partial class FullscreenImagePopup : PopupPage
    {
        public FullscreenImagePopup(Xamarin.Forms.ImageSource pic)
        {
            InitializeComponent();
            pictureBoxFullScreen.Source = pic;

            closePicture();
        }

        void closePicture()
        {
            pictureBoxFullScreen.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    await Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();
                })
            });
        }
    }
}