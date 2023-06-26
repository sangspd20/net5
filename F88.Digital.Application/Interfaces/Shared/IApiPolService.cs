using F88.Digital.Application.DTOs.POL.Request;
using F88.Digital.Application.DTOs.POL.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.Shared
{
    public interface IApiPolService
    {
        ReponseSendPolCancelModel ReSendPolCancel();
    }
}
