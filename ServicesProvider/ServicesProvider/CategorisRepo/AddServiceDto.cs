using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesProvider.CategorisRepo
{
   public class AddServiceDto
    {
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public byte[] image { get; set; }
        public string FullName { get; set; }
        public string phoneNumber {get; set;}
        public string UserId { get; set; }
    }
}
