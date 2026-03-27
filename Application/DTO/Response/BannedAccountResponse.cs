using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class BannedAccountResponse : GeneralResponse
    {
        public List<BannedAccountDTO> BannedAccounts { get; set; } = new List<BannedAccountDTO>();
    }
}
