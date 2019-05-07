using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ServicesProvider
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePageMaster : ContentPage
    {
        public ListView ListView;

        public HomePageMaster()
        {
            InitializeComponent();

            BindingContext = new HomePageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class HomePageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<HomePageMenuItem> MenuItems { get; set; }
            
            public HomePageMasterViewModel()
            {
                MenuItems = new ObservableCollection<HomePageMenuItem>(new[]
                {
                    new HomePageMenuItem { Id = 0, Title = "Home" },
                    new HomePageMenuItem { Id = 1, Title = "Account" },
                    new HomePageMenuItem { Id = 2, Title = "Log Out" },
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

        private void MenuItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItemIndex == 2)
            {
                Application.Current.Properties.Remove("ID");
                Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new NavigationPage(new LoginPage()));
            }

            if (e.SelectedItemIndex == 1)
            {
                Navigation.PushAsync(new NavigationPage(new AccountDetails()));
            }
        }
    }
}