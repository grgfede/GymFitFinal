using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymFitFinal.home.navBar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Palestra : ContentPage
    {
        public Palestra()
        {
            InitializeComponent();
            /*var htmlSource = new HtmlWebViewSource();


            htmlSource.Html = @"<html>
<haed>
    <script type='text / javascript'
            id = 'botcopy-embedder-d7lcfheammjct'
            class='botcopy-embedder-d7lcfheammjct'
            data-botId='5f638e4c06c5030008d4612d'
    >
        var s = document.createElement('script');
        s.type = 'text/javascript'; s.async = true;
        s.src = 'https://widget.botcopy.com/js/injection.js';
        document.getElementById('botcopy-embedder-d7lcfheammjct').appendChild(s);
    </script>
</haed>
<body>
<h1>PROVA</h1>
</body>
</html>
";
            view.EvaluateJavaScriptAsync(htmlSource.Html);
            view.Source = htmlSource;*/




        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new FindGymManually());
        }
    }
}