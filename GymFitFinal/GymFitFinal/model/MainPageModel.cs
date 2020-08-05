using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace GymFitFinal.model
{
        public class MainPageModel : BindableObject
        {
            private DateTime? _date;
            public DateTime? Date
            {
                get
                {
                    return _date;
                }
                set
                {
                    _date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }

            private ObservableCollection<XamForms.Controls.SpecialDate> attendances;
            public ObservableCollection<XamForms.Controls.SpecialDate> Attendances
            {
                get
                {
                    return attendances;
                }
                set
                {
                    attendances = value;
                    OnPropertyChanged(nameof(Attendances));
                }
            }

            public Command DateChosen
            {
                get
                {
                    return new Command((obj) =>
                    {
                        System.Diagnostics.Debug.WriteLine(obj as DateTime?);
                    });
                }
            }
        }
}
