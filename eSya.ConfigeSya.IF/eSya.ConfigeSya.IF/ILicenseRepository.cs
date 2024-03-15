using eSya.ConfigeSya.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigeSya.IF
{
    public interface ILicenseRepository
    {
        #region Business Subscription
        Task<List<DO_BusinessSubscription>> GetBusinessSubscription(int BusinessKey);
        Task<DO_ReturnParameter> InsertOrUpdateBusinessSubscription(DO_BusinessSubscription businessSubscription);
        #endregion

        #region Location License Info
        Task<DO_ReturnParameter> InsertOrUpdateLocationLicenseInfo(DO_LocationLicenseInfo obj);
        Task<DO_LocationLicenseInfo> GetLocationLicenseInfo(int BusinessKey);
        #endregion
    }
}
