using Newtonsoft.Json;
using ServicesProvider.CategorisRepo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.ImagePicker;
using Xamarin.Forms.Xaml;

namespace ServicesProvider
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddService : ContentPage
    {
        IImagePickerService _imagePickerService;
        int CategoryID;
        System.IO.Stream Image;

        public AddService()
        {
            InitializeComponent();
            populatelist();
            _imagePickerService = DependencyService.Get<IImagePickerService>();


        }
        public List<MainCategoriesDto> _mainCategoriesDto;
        public string SelectedItem;

        async void populatelist()
        {
            _mainCategoriesDto = await new CategoriesRepo().GetAll();
            //SelectedItem = _mainCategoriesDto.First();
            dropdown.ItemsSource = _mainCategoriesDto.Select(x => x.CategoryName).ToList();


            //  _mainCategoriesDto = await new CategoriesRepo().GetAll();
        }

        private void Dropdown_SelectedItemChanged(object sender, Plugin.InputKit.Shared.Utils.SelectedItemChangedArgs e)
        {
            //
            CategoryID = _mainCategoriesDto.ElementAt(e.NewItemIndex).Id;

        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            var imageSource = await _imagePickerService.PickImageAsync();

            if (imageSource != null) // it will be null when user cancel
            {
                image.Source = imageSource;
                Image = await _imagePickerService.ImageSourceUtility.ToJpegStreamAsync(imageSource);
            }
            else
            {
                Image = null;
            }

        }

        async private void saveService(object sender, EventArgs e)
        {
            if (spinner.IsVisible)
                return;

            ServicenameError.Text = "";
            Imageerror.Text = "";
            descriptionError.Text = "";
            
            if (string.IsNullOrEmpty(Servicename.Text.Trim()))
            {
                ServicenameError.Text = "Name is required";
                return;
            }

            if (string.IsNullOrEmpty(description.Text.Trim()))
            {
                descriptionError.Text = "Description is required";
                return;

            }

            if (Image == null)
            {
                Imageerror.Text = "Image is required";
                return;
            }

            if (!dropdown.IsValidated)
            {
                return;
            }

            var serverdto = new AddServiceDto()
            {
                ParentID = CategoryID,
                image = ReadFully(Image),
                Desc = description.Text,
                Name = Servicename.Text,
                FullName = Application.Current.Properties["FullName"].ToString(),
                phoneNumber = Application.Current.Properties["PhoneNumber"].ToString(),
            };
            spinner.IsVisible = true;
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                response = await client.PostAsync(
                   "http://172.107.175.25/umbraco/api/ServicesApi/AddService",
                    new StringContent(JsonConvert.SerializeObject(serverdto), Encoding.UTF8, "application/json"));
            }
            spinner.IsVisible = false;

            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "Error From Server", "Close");
                image.Source = null;

            }
            else
            {
                await DisplayAlert("Done!", "Service Added", "Close");
                image.Source = null;
            }

        }


        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }


    }

}