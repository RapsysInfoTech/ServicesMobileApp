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
            if(Application.Current.Properties.ContainsKey("ID"))
            if (Application.Current.Properties["ID"] != null &&!string.IsNullOrEmpty(Application.Current.Properties["ID"].ToString()))
            {
                    Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new NavigationPage(new HomePage()));



                }
            var tgr = new TapGestureRecognizer();
            tgr.Tapped += (s, e) => OnLabelClicked();
            lblRegister.GestureRecognizers.Add(tgr);

        }

        async void OnLabelClicked()
        {
            await Navigation.PopAsync();
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
            spinner.IsVisible = true;
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                response = await client.PostAsync(
                   "http://172.107.175.25/umbraco/api/MembersApi/Authenticate",
                    new StringContent(JsonConvert.SerializeObject(auth), Encoding.UTF8, "application/json"));
            }
            spinner.IsVisible = false;
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
                    var user = JsonConvert.DeserializeObject<MemberResponseobject>(parsedResponse).desc;
                    Application.Current.Properties["ID"] = user.Id;
                    Application.Current.Properties["FullName"] = user.FullName;
                    Application.Current.Properties["PhoneNumber"] = user.PhoneNumber;
                    Application.Current.Properties["Email"] = user.Email;
                    Application.Current.Properties["Sex"] = user.Sex;
                    Application.Current.Properties["MemberTypeAlias"] = user.MemberTypeAlias;
                    Application.Current.Properties["Password"] = user.Password;
                    Application.Current.Properties["Address"] = user.Address;
                    Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new NavigationPage(new HomePage()));

                }
                else
                {
                    await DisplayAlert("Error", (string)responseBody.desc, "Close");
                }

            }


        }
    }
}