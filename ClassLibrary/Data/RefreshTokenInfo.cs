using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Data
{
    public class RefreshTokenInfo
    {
        public long Id { get; set; }
        public string? Token { get; set; }
        public string? UserName { get; set; }
    }
}
