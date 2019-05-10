using Newtonsoft.Json;
using ServicesProvider.CategorisRepo;
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
    public partial class ServicesList : ContentPage
    {
        public int ParentID;
        List<serviceResponseDto> items = new List<serviceResponseDto>();
        public ServicesList(int parentId)
        {
            ParentID = parentId;
            InitializeComponent();
            spinner.IsVisible = true;
             GetAll();
      
        }

        async public void GetAll()
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                response = await client.GetAsync("http://172.107.175.25/umbraco/api/ServicesApi/GetServiceByCategory?id=" + ParentID);
            }
            string parsedResponse = await response.Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<List<serviceResponseDto>>(parsedResponse);
            items = responseBody;
            services.ItemsSource = responseBody;
            spinner.IsVisible = false;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchtext = e.NewTextValue;
            if (string.IsNullOrEmpty(searchtext.Trim()))
            { services.ItemsSource = items; return; }


            services.ItemsSource = items.Where(x => x.FullName.ToLower().Contains(searchtext.ToLower()) || x.Name.ToLower().Contains(searchtext.ToLower()) || x.Desc.ToLower().Contains(searchtext.ToLower()) || x.phoneNumber.ToLower().Contains(searchtext.ToLower()));


        }

      async  private void Services_Refreshing(object sender, EventArgs e)
        {
            services.IsRefreshing = true;

            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                response = await client.GetAsync("http://172.107.175.25/umbraco/api/ServicesApi/GetServiceByCategory?id=" + ParentID);
            }
            string parsedResponse = await response.Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<List<serviceResponseDto>>(parsedResponse);
            items = responseBody;
            services.ItemsSource = responseBody;

            services.EndRefresh();

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var xx = (Button)sender;
            var userid = Application.Current.Properties["ID"].ToString();

            if (!Application.Current.Properties.ContainsKey("cart." + userid))
            {
                Application.Current.Properties["cart." + userid] = xx.CommandParameter.ToString() ;
            }
            else {
                if (Application.Current.Properties["cart." + userid].ToString().Contains(xx.CommandParameter.ToString()))
                {
                    DisplayAlert("Info", "Item Already In Cart!", "Ok");
                    return;
                }
                Application.Current.Properties["cart." + userid]+= "," + xx.CommandParameter.ToString();
            }
            DisplayAlert("Info", "Item Added To Cart!", "Ok");
        }

        private void wishlist_Clicked(object sender, EventArgs e)
        {
            var xx = (Button)sender;
            var userid = Application.Current.Properties["ID"].ToString();

            if (!Application.Current.Properties.ContainsKey("wishlist." + userid))
            {
                Application.Current.Properties["wishlist." + userid] = xx.CommandParameter.ToString();
            }
            else
            {
                if (Application.Current.Properties["wishlist." + userid].ToString().Contains(xx.CommandParameter.ToString()))
                {
                    DisplayAlert("Info", "Item Already In Wishlist", "Ok");
                    return;
                }
                Application.Current.Properties["wishlist." + userid] += "," + xx.CommandParameter.ToString();
            }
            DisplayAlert("Info", "Item Added To Wishlist", "Ok");

        }

        
    }
}