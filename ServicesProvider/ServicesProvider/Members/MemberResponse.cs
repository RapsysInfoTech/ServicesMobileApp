using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesProvider.Members
{
    public class MemberResponse
    {
        public int error_code { get; set; }
        public object desc { get; set; }
    }

    public class MemberResponseobject
    {
        public int error_code { get; set; }
        public AddMemberDto desc { get; set; }
    }
}
