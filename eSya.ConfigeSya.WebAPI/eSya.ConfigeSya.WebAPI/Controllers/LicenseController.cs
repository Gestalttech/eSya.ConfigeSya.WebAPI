using eSya.ConfigeSya.DO;
using eSya.ConfigeSya.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigeSya.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseRepository _licenseRepository;
        public LicenseController(ILicenseRepository licenseRepository)
        {
            _licenseRepository = licenseRepository;
        }
        #region Business Subscription
        /// <summary>
        /// Getting  Business Subscription List.
        /// UI Reffered - Business Subscription Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessSubscription(int BusinessKey)
        {
            var b_entities = await _licenseRepository.GetBusinessSubscription(BusinessKey);
            return Ok(b_entities);

        }

        /// <summary>
        /// Update Business Subscription .
        /// UI Reffered -Business Subscription
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateBusinessSubscription(DO_BusinessSubscription businessubs)
        {
            var msg = await _licenseRepository.InsertOrUpdateBusinessSubscription(businessubs);
            return Ok(msg);
        }
        #endregion  Business Subscription

        #region Location License Info
        /// <summary>
        /// Insert Location License Info .
        /// UI Reffered -Business Location
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateLocationLicenseInfo(DO_LocationLicenseInfo obj)
        {
            var msg = await _licenseRepository.InsertOrUpdateLocationLicenseInfo(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Get Location License Info by BusinessKey.
        /// UI Reffered -Business Location 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLocationLicenseInfo(int BusinessKey)
        {
            var locinfo = await _licenseRepository.GetLocationLicenseInfo(BusinessKey);
            return Ok(locinfo);
        }
        #endregion
    }
}
