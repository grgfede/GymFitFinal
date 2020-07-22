using GymFitFinal.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace GymFitFinal.model
{
    class MyViewModel
    {
        private IFirebaseAuthenticator _auth;
        public ObservableCollection<AbbonamentoPalestra> AbbonamentoPalestras { get; set; }

        public MyViewModel()
        {
            _auth = DependencyService.Get<IFirebaseAuthenticator>();

            getTipoAbbonamento();
           
        }
        public async void getTipoAbbonamento()
        {
            AbbonamentoPalestras = new ObservableCollection<AbbonamentoPalestra>();
            List<AbbonamentoPalestra> allSubGym = await _auth.getSubGym();
            foreach (var item in allSubGym)
            {
                if (String.Equals(item.uidPalestra, App.loggedUser.PalestraIscrizione))
                {
                    AbbonamentoPalestras.Add(item);

                }
            }
        }
    }
}
