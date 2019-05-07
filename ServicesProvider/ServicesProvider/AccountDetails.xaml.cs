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
    public partial class AccountDetails : TabbedPage
    {
        private AddMemberDto newmember;
        public AccountDetails ()
        {
            InitializeComponent();

            newmember = new AddMemberDto
            {
                Id = Application.Current.Properties["ID"].ToString(),
                Address = Application.Current.Properties["Address"].ToString(),
                Email = Application.Current.Properties["Email"].ToString(),
                FullName = Application.Current.Properties["FullName"].ToString(),
                MemberTypeAlias = Application.Current.Properties["MemberTypeAlias"].ToString(),
                Password = null,
                PhoneNumber = Application.Current.Properties["PhoneNumber"].ToString(),
                Sex = Application.Current.Properties["Sex"].ToString()
            };

        }

      async  private void email_Clicked(object sender, EventArgs e)
        {
            if (!IsValidEmail(Email.Text))
            {
                emailerror.Text = "Please Enter a valid Email";
                return;
            }
            emailspinner.IsVisible = true;
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                newmember.Email = Email.Text;

                response = await client.PostAsync(
                   "http://172.107.175.25/umbraco/api/MembersApi/UpdateEmail",
                    new StringContent(JsonConvert.SerializeObject(newmember), Encoding.UTF8, "application/json"));
            }
            emailspinner.IsVisible = false;
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
                    Email.Text = string.Empty;
                    await DisplayAlert("success", "Email Changed", "Close");
                    var user = JsonConvert.DeserializeObject<MemberResponseobject>(parsedResponse).desc;
                    Application.Current.Properties["ID"] = user.Id;
                    Application.Current.Properties["FullName"] = user.FullName;
                    Application.Current.Properties["PhoneNumber"] = user.PhoneNumber;
                    Application.Current.Properties["Email"] = user.Email;
                    Application.Current.Properties["Sex"] = user.Sex;
                    Application.Current.Properties["MemberTypeAlias"] = user.MemberTypeAlias;
                    Application.Current.Properties["Password"] = user.Password;

                    //  Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new NavigationPage(new HomePage()));
                }
                else
                {
                    await DisplayAlert("Error", (string)responseBody.desc, "Close");
                }

            }
        }

       async private void password_Clicked(object sender, EventArgs e)
        {

            if ((Password.Text.Trim().Length < 10) || string.IsNullOrEmpty(Password.Text.Trim()))
            {
                Passworderror.Text = "Password must be 10 characters or more";
                return ;
            }

            passwordspinner.IsVisible = true;
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                newmember.Password = Password.Text;
                response = await client.PostAsync(
                   "http://172.107.175.25/umbraco/api/MembersApi/UpdatePassword",
                    new StringContent(JsonConvert.SerializeObject(newmember), Encoding.UTF8, "application/json"));
            }
            passwordspinner.IsVisible = false;
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
                    Password.Text = string.Empty;
                    await DisplayAlert("success", "Password Changed", "Close");
                    var user = JsonConvert.DeserializeObject<MemberResponseobject>(parsedResponse).desc;
                    Application.Current.Properties["ID"] = user.Id;
                    Application.Current.Properties["FullName"] = user.FullName;
                    Application.Current.Properties["PhoneNumber"] = user.PhoneNumber;
                    Application.Current.Properties["Email"] = user.Email;
                    Application.Current.Properties["Sex"] = user.Sex;
                    Application.Current.Properties["MemberTypeAlias"] = user.MemberTypeAlias;
                    Application.Current.Properties["Password"] = user.Password;

                  //  Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new NavigationPage(new HomePage()));
                }
                else
                {
                    await DisplayAlert("Error", (string)responseBody.desc, "Close");
                }

            }


        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}