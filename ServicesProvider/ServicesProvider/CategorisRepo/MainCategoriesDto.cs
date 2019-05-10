using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesProvider.CategorisRepo
{
    public class MainCategoriesDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Categorydesc { get; set; }
        
        public string ImageUrl { get; set; }
    }
}
