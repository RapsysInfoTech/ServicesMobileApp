using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesProvider
{
    public class serviceResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Desc { get; set; }
        public string image { get; set; }
        public string FullName { get; set; }
        public string UserId { get; set; }
        public string phoneNumber { get; set; }
    }
}
