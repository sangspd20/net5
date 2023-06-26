using F88.Digital.Application.Features.AppPartner.Payment.Queries;
using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F88.Digital.Application.Features.AppPartner.GoogleSheet.Queries;
using F88.Digital.Domain.Entities.Share;

namespace F88.Digital.Application.Interfaces.Repositories.AppPartner
{
    public interface ILocationShopRepository
    {
        void AddLocationShop(LocationShop locationShop, string spreadSheetId, string sheetName, int position);
        bool ReadGoogleSheetLocationShop();
    }
}
