using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tumin.Models.AccountViewModels
{
    public class RegisterWithUserInfo
    {
        public RegisterViewModel IdentityRegister { get; set; }
        public UserInformation UserInformation { get; set; }
    }
}
