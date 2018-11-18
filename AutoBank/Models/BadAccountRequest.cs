using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autobank.Models
{

    public class BadAccountRequest
    {
        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public string Amount { get; set; }

        [Required]
        public int Currency { get; set; }
    }
}