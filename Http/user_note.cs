using System;
using System.Collections.Generic;
using System.Text;

namespace CloudPadApi.Http
{
    public class User_note
    {
        public int user_id { get; set; }
        public string user_email { get; set; }

        public int note_id { get; set; }

        public string note { get; set; }


    }
}
