using Newtonsoft.Json;
using Plugin.InputKit.Shared.Controls;
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
    public partial class Register : ContentPage
    {
        private string geneder = "Male";
        public Register()
        {
            InitializeComponent();
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            if (spinner.IsVisible)
                return;

            if (!await validate())
            { return; }
            var newmember = new AddMemberDto {
                Address = address.Text,
                Email = email.Text,
                FullName = fullname.Text,
                Id = null,
                MemberTypeAlias = "serviceConsumerUser",
                Password = password.Text,
                PhoneNumber = phonenumer.Text,
                Sex = geneder             
            };
            spinner.IsVisible = true;
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                response = await client.PostAsync(
                   "http://172.107.175.25/umbraco/api/MembersApi/add",
                    new StringContent(JsonConvert.SerializeObject(newmember), Encoding.UTF8, "application/json"));
            }
            spinner.IsVisible = false;
            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "Error From Server", "Close");

            }
            else {
                string parsedResponse = await response.Content.ReadAsStringAsync();
                var responseBody = JsonConvert.DeserializeObject<MemberResponse>(parsedResponse);
                if (responseBody.error_code == 0)
                {
                    await DisplayAlert("success", "User Added!", "Close");
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

        async Task<bool> validate()
        {
            reset();
            if (string.IsNullOrEmpty(fullname.Text.Trim()))
            {
                fullNameError.Text = "FullName Is Required";
                await scroll.ScrollToAsync(0, 0, false);
                return false;
            }
            if (string.IsNullOrEmpty(address.Text.Trim()))
            {
                addressError.Text = "Address Is Required";
                await scroll.ScrollToAsync(0, 0, false);
                return false;
            }
            if (string.IsNullOrEmpty(phonenumer.Text.Trim()))
            {
                phoneNumberError.Text = "Phone Number Is Required";
                await scroll.ScrollToAsync(0, 0, false);
                return false;
            }

            if (string.IsNullOrEmpty(email.Text.Trim()))
            {
                emailError.Text = "Email Is Required";
                await scroll.ScrollToAsync(0, 0, false);
                return false;
            }

            if (!IsValidEmail(email.Text.Trim()))
            {
                emailError.Text = "Please enter a valid email address";
                await scroll.ScrollToAsync(0, 0, false);
                return false;
            }

            if (string.IsNullOrEmpty(password.Text.Trim()))
            {
                passwordError.Text = "Password Is Required";
                await scroll.ScrollToAsync(0, 0, false);
                return false;
            }

            if ((password.Text.Trim().Length<10))
            {
                passwordError.Text = "Password must be 10 characters or more";
                await scroll.ScrollToAsync(0, 0, false);
                return false;
            }

            if (string.IsNullOrEmpty(confirm.Text.Trim()))
            {
                confirmError.Text = "Please renter your password";

                return false;
            }


            if (password.Text.Trim() != confirm.Text.Trim())
            {
                confirmError.Text = "the passwords you entered are not the same";

                return false;
            }

            return true;
        }
        private void Female_Clicked(object sender, EventArgs e)
        {
            RadioButton xx = (RadioButton)sender;
            geneder = xx.Value.ToString();
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
        void reset()
        {
            fullNameError.Text = "";
            addressError.Text = "";
            phoneNumberError.Text = "";
            emailError.Text = "";
            passwordError.Text = "";
            confirmError.Text = "";
        }
    }
}