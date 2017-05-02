using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiberaryWeb
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public string AccHolderName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AccountType { get; set; }
        public int AccountBalance { get; set; }
    }
}