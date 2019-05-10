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
	public partial class cart : ContentPage
	{
        private List<serviceResponseDto> serviceResponselist = new List<serviceResponseDto>();

        public cart ()
		{
			InitializeComponent ();
            var userid = Application.Current.Properties["ID"].ToString();

            if (Application.Current.Properties.ContainsKey("cart." + userid))
            {
                getlist();
            }

        }

        async Task getlist()
        {
            serviceResponselist = new List<serviceResponseDto>();
            var userid = Application.Current.Properties["ID"].ToString();
            var ids = Application.Current.Properties["cart." + userid].ToString().Split(',').ToList();
            var serviceslist = await new CategoriesRepo().GetAllservices();
            foreach (var id in ids)
            {
                var item = serviceslist.SingleOrDefault(x=>x.Id.ToString() ==id);
                if (item != null)
                    serviceResponselist.Add(item);
            }

            services.ItemsSource = serviceResponselist;
        }

       async private void Button_Clicked(object sender, EventArgs e)
        {
            var userid = Application.Current.Properties["ID"].ToString();
            if (string.IsNullOrEmpty(Application.Current.Properties["cart." + userid].ToString()))
            {
                return;
            }      
            var answer = await DisplayAlert("Confirm", "Are You Sure You Want To Check Out", "Yes", "No");
            if (answer)
            {
                Application.Current.Properties["cart." + userid] = "";
                await getlist();
                await DisplayAlert("Thank You", "Thank You For Dealing With Us", "Ok");
            }
        }

       async private void Button_Clicked_1(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirm", "Are You Sure You Want Remove This Item from Cart", "Yes", "No");
            if (answer)
            {
                var xx = (Button)sender;
                var userid = Application.Current.Properties["ID"].ToString();
                var ids = Application.Current.Properties["cart." + userid].ToString().Split(',').ToList();
                ids.Remove(xx.CommandParameter.ToString());
                Application.Current.Properties["cart." + userid] = string.Join(",", ids.ToList()); // ids.Join(",")
                await getlist();
            }
        }
    }
}