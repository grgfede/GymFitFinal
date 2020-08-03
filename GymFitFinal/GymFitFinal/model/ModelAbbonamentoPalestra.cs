using GymFitFinal.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace GymFitFinal.model
{
    class ModelAbbonamentoPalestra
    {
        private IFirebaseAuthenticator _auth;

        public ObservableCollection<AbbonamentoPalestra> AbbonamentoPalestras { get; set; }


        public string TipoAbbonamentoUtente { get; set; }
        public string DataInizioUtente { get; set; }
        public string DataFineUtente { get; set; }
        public string CostoAbbonamentoUtente { get; set; }




        public ModelAbbonamentoPalestra()
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
                if (String.Equals(item.uidPalestra, App.uidAbbonamentoPalestra))
                {
                    AbbonamentoPalestras.Add(item);

                }
            }
        }



    }
}
