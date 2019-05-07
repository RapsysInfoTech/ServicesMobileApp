using Newtonsoft.Json;
using ServicesProvider.Members;
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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            var tgr = new TapGestureRecognizer();
            tgr.Tapped += (s, e) => OnLabelClicked();
            lblRegister.GestureRecognizers.Add(tgr);

        }

        async void OnLabelClicked()
        {
            await Navigation.PushAsync(new NavigationPage(new Register()));
        }

       async private void Button_Clicked(object sender, EventArgs e)
        {
            usererror.Text = "";
            passworderror.Text = "";
            if (string.IsNullOrEmpty(userName.Text.Trim()))
            {
                usererror.Text = "Username Is Required";
                
                return ;
            }

            if (string.IsNullOrEmpty(password.Text.Trim()))
            {
                passworderror.Text = "Password Is Required";
             
                return ;
            }
            var auth = new UserauthenticationDto() {
                 Username = userName.Text , 
                  Password = password.Text
            };

            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                response = await client.PostAsync(
                   "http://172.107.175.25/umbraco/api/MembersApi/Authenticate",
                    new StringContent(JsonConvert.SerializeObject(auth), Encoding.UTF8, "application/json"));
            }

            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "Error From Server", "Close");

            }
            else
            {
                string parsedResponse = await response.Content.ReadAsStringAsync();
                var responseBody = JsonConvert.DeserializeObject<MemberResponse>(parsedResponse);
                if (responseBody.error_code == 0)
                {
                    
                        await Navigation.PushAsync(new NavigationPage(new HomePage()));

                }
                else
                {
                    await DisplayAlert("Error", (string)responseBody.desc, "Close");
                }

            }


        }
    }
}