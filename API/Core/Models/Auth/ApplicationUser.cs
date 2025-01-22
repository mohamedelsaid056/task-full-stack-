using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Auth
{
    public  class ApplicationUser : IdentityUser
    {

        //i need i will add more  user properties 


        public string FirstName { get; set; }

        public string LastName { get; set; }


    }
}
