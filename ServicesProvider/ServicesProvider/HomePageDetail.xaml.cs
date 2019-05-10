using ServicesProvider.CategorisRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ServicesProvider
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
   public partial class HomePageDetail : ContentPage
    {
        private List<MainCategoriesDto> _mainCategoriesDto ;
        public HomePageDetail()
        {
            populatelist();
            _mainCategoriesDto = new List<MainCategoriesDto>();

            InitializeComponent();
        }


        async void populatelist()
        {
            

            _mainCategoriesDto = await new CategoriesRepo().GetAll();

            serviceCaegories.ItemsSource = _mainCategoriesDto;
        }

        private void ServiceCaegories_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //  Navigation.PushAsync(new NavigationPage(new ServicesList(_mainCategoriesDto.ElementAt(e.SelectedItemIndex).Id)));

            serviceCaegories.SelectedItem = null;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchtext = e.NewTextValue;
            if (string.IsNullOrEmpty(searchtext.Trim()))
            {  serviceCaegories.ItemsSource = _mainCategoriesDto;  return; }
            serviceCaegories.ItemsSource = _mainCategoriesDto.Where(x => x.CategoryName.ToLower().Contains(searchtext.ToLower()) || x.Categorydesc.ToLower().Contains(searchtext.ToLower()));
        }

      async  private void ServiceCaegories_Refreshing(object sender, EventArgs e)
        {
            serviceCaegories.IsRefreshing = true;

            _mainCategoriesDto = await new CategoriesRepo().GetAll();

            serviceCaegories.ItemsSource = _mainCategoriesDto;

            serviceCaegories.EndRefresh();
        }

        private void ServiceCaegories_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new NavigationPage(new ServicesList(_mainCategoriesDto.ElementAt(e.ItemIndex).Id)));

         
        }
    }
}