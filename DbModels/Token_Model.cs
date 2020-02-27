using System;
using System.Collections.Generic;
using System.Text;

namespace CloudPadApi.DbModels
{
    class Token_Model
    {
        public string Token { get; set; }

        public int User_id { get; set; }

        public Int32 expiration_date { get; set; }
    }
}
