using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Data
{
    public class UserExtend : IdentityUser
    {
        public bool IsDisabled { get; set; } //扩展属性
        public DateTime CreateDatetime { get; set; } //扩展属性

    }
}
