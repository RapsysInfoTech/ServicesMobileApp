using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ServicesProvider
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyServices : ContentPage
    {
        List<serviceResponseDto> items = new List<serviceResponseDto>();

        public MyServices()
        {
            InitializeComponent();
            spinner.IsVisible = true;
            GetAll();
        }


        async public void GetAll()
        {
         
             items  = await new CategoriesRepo().GetAllservices();
            items = items.Where(x => x.UserId == Application.Current.Properties["ID"].ToString()).ToList();
            services.ItemsSource = items;
            spinner.IsVisible = false;

        }


        async private void Services_Refreshing(object sender, EventArgs e)
        {
            services.IsRefreshing = true;

            items = await new CategoriesRepo().GetAllservices();
            items = items.Where(x => x.UserId == Application.Current.Properties["ID"].ToString()).ToList();
            services.ItemsSource = items;
           
            services.EndRefresh();

        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchtext = e.NewTextValue;
            if (string.IsNullOrEmpty(searchtext.Trim()))
            { services.ItemsSource = items; return; }


            services.ItemsSource = items.Where(x => x.FullName.ToLower().Contains(searchtext.ToLower()) || x.Name.ToLower().Contains(searchtext.ToLower()) || x.Desc.ToLower().Contains(searchtext.ToLower()) || x.phoneNumber.ToLower().Contains(searchtext.ToLower()));


        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            var xx = (Button)sender;
            var answer = await DisplayAlert("Confirm", "Are You Sure You Want Remove This Item?", "Yes", "No");
            if (answer)
            {
                spinner.IsVisible = true;
                HttpResponseMessage response;
                using (var client = new HttpClient())
                {
                    response = await client.DeleteAsync("http://172.107.175.25/umbraco/api/ServicesApi/DeleteService?id="+ xx.CommandParameter.ToString());
                }
                string parsedResponse = await response.Content.ReadAsStringAsync();
             //   var responseBody = JsonConvert.DeserializeObject<serviceResponseDto>(parsedResponse);
               await DisplayAlert("Info", "Service Deleted", "OK");
                GetAll();
            }
        }
    }
}