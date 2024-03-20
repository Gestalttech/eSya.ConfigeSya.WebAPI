using eSya.ConfigeSya.DL.Entities;
using eSya.ConfigeSya.DO;
using eSya.ConfigeSya.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigeSya.DL.Repository
{
    public class LicenseRepository: ILicenseRepository
    {
        private readonly IStringLocalizer<LicenseRepository> _localizer;
        public LicenseRepository(IStringLocalizer<LicenseRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Business Subscription
        public async Task<List<DO_BusinessSubscription>> GetBusinessSubscription(int BusinessKey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcbssus.Where(bs => bs.BusinessKey == BusinessKey)
                                  .Select(be => new DO_BusinessSubscription
                                  {
                                      SubscribedFrom = be.SubscribedFrom,
                                      SubscribedTill = be.SubscribedTill,
                                      ActiveStatus = be.ActiveStatus
                                  }).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateBusinessSubscription(DO_BusinessSubscription businessSubscription)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (businessSubscription.isEdit == 0)
                        {
                            GtEcbssu is_SubsFrmDateExist = db.GtEcbssus.FirstOrDefault(be => be.BusinessKey == businessSubscription.BusinessKey && be.SubscribedFrom >= businessSubscription.SubscribedFrom);
                            if (is_SubsFrmDateExist != null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0058", Message = string.Format(_localizer[name: "W0058"]) };
                            }

                            GtEcbssu is_SubstoDateExist = db.GtEcbssus.FirstOrDefault(be => be.BusinessKey == businessSubscription.BusinessKey && be.SubscribedTill >= businessSubscription.SubscribedTill);
                            if (is_SubstoDateExist != null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0059", Message = string.Format(_localizer[name: "W0059"]) };

                            }

                            var is_SubsCheck = db.GtEcbssus.FirstOrDefault(be => be.BusinessKey == businessSubscription.BusinessKey && (be.SubscribedTill >= businessSubscription.SubscribedFrom || businessSubscription.SubscribedTill >= businessSubscription.SubscribedFrom));
                            if (is_SubsCheck != null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0060", Message = string.Format(_localizer[name: "W0060"]) };

                            }
                        }
                        GtEcbssu b_Susbs = db.GtEcbssus.Where(be => be.BusinessKey == businessSubscription.BusinessKey && be.SubscribedFrom == businessSubscription.SubscribedFrom).FirstOrDefault();
                        if (b_Susbs != null)
                        {
                            b_Susbs.SubscribedTill = businessSubscription.SubscribedTill;
                            b_Susbs.ModifiedBy = businessSubscription.UserID;
                            b_Susbs.ModifiedOn = System.DateTime.Now;
                            b_Susbs.ModifiedTerminal = businessSubscription.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }
                        else
                        {
                            var b_Subs = new GtEcbssu
                            {
                                BusinessKey = businessSubscription.BusinessKey,
                                SubscribedFrom = businessSubscription.SubscribedFrom,
                                SubscribedTill = businessSubscription.SubscribedTill,
                                ActiveStatus = businessSubscription.ActiveStatus,
                                FormId = businessSubscription.FormID,
                                CreatedBy = businessSubscription.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = businessSubscription.TerminalID
                            };

                            db.GtEcbssus.Add(b_Subs);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        #endregion  Business Subscription

        #region Location License Info
        public async Task<List<DO_BusinessLocation>> GetActiveLocationsforLicenses()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var bk = db.GtEcbslns
                        .Where(w => w.ActiveStatus && w.Lstatus)
                        .Select(r => new DO_BusinessLocation
                        {
                            BusinessKey = r.BusinessKey,
                            LocationDescription = r.BusinessName + "-" + r.LocationDescription
                        }).ToListAsync();

                    return await bk;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdateLocationLicenseInfo(DO_LocationLicenseInfo obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {


                        GtEcbsli licenseinfo = db.GtEcbslis.FirstOrDefault(l => l.BusinessKey == obj.BusinessKey);

                        byte[] eBKey = Encoding.UTF8.GetBytes(obj.BusinessKey.ToString());

                        byte[] tbUserLicenses = Encoding.UTF8.GetBytes(obj.EUserLicenses.ToString());
                        byte[] tbActiveUser = Encoding.UTF8.GetBytes(0.ToString());
                        byte[] tbNoOfBeds = Encoding.UTF8.GetBytes(obj.ENoOfBeds.ToString());

                        if (licenseinfo != null)
                        {
                            licenseinfo.BusinessKey = obj.BusinessKey;
                            licenseinfo.EBusinessKey = eBKey;
                            licenseinfo.ESyaLicenseType = obj.ESyaLicenseType;
                            licenseinfo.EUserLicenses = tbUserLicenses;
                            licenseinfo.EActiveUsers = tbActiveUser;
                            licenseinfo.ENoOfBeds = tbNoOfBeds;
                            licenseinfo.ActiveStatus = obj.ActiveStatus;
                            licenseinfo.ModifiedBy = obj.UserID;
                            licenseinfo.ModifiedOn = System.DateTime.Now;
                            licenseinfo.ModifiedTerminal = obj.TerminalId;
                            GtEcbsln sln = db.GtEcbslns.Where(x => x.BusinessKey == obj.BusinessKey).FirstOrDefault();
                            if(sln != null)
                            {
                                sln.Lstatus = true;
                            }
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };

                        }
                        else
                        {
                            var licinfo = new GtEcbsli
                            {

                                BusinessKey = obj.BusinessKey,
                                EBusinessKey = eBKey,
                                ESyaLicenseType = obj.ESyaLicenseType,
                                EUserLicenses = tbUserLicenses,
                                EActiveUsers = tbActiveUser,
                                ENoOfBeds = tbNoOfBeds,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalId,
                            };
                            db.GtEcbslis.Add(licinfo);
                            GtEcbsln sln = db.GtEcbslns.Where(x => x.BusinessKey == obj.BusinessKey).FirstOrDefault();
                            if (sln != null)
                            {
                                sln.Lstatus = true;
                            }
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<List<DO_LocationLicenseInfo>> GetLocationLicenseInfo(int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    if (BusinessKey == 0)
                    {
                        var bk = db.GtEcbslns
                       .Where(w => w.ActiveStatus && !w.Lstatus)
                       .Select(r => new DO_LocationLicenseInfo
                       {
                           BusinessKey = r.BusinessKey,
                           LocationDescription = r.BusinessName + "-" + r.LocationDescription,
                           ActiveStatus = r.ActiveStatus

                       }).ToListAsync();

                        return await bk;
                    }
                    else
                    {


                        var ds = db.GtEcbslis.Where(x=>x.BusinessKey==BusinessKey).
                            Join(db.GtEcbslns.Where(x=>x.BusinessKey==BusinessKey),
                            l=>l.BusinessKey,li=>li.BusinessKey,
                            (l,li)=> new DO_LocationLicenseInfo
                          {
                              BusinessKey = l.BusinessKey,
                              EBusinessKey = l.EBusinessKey,
                              ESyaLicenseType = l.ESyaLicenseType,
                              EUserLicenses = Convert.ToInt32(Encoding.UTF8.GetString(l.EUserLicenses)),
                              ENoOfBeds = Convert.ToInt32(l.ENoOfBeds != null ? Encoding.UTF8.GetString(l.ENoOfBeds) : "0"),
                              EActiveUsers = l.EActiveUsers,
                              ActiveStatus = l.ActiveStatus,
                              Lstatus=li.Lstatus,
                              LocationDescription = li.BusinessName + "-" + li.LocationDescription,

                            }).ToListAsync();

                        return await ds;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
