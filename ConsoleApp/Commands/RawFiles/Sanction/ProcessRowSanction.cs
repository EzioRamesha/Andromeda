using BusinessObject.Sanctions;
using DataAccess.Entities.Identity;
using Services.Sanctions;
using Shared;
using Shared.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.Sanction
{
    public class ProcessRowSanction : Command
    {
        public ProcessSanctionBatch ProcessSanctionBatch { get; set; }
        public MappingSanction MappingSanction { get; set; }
        public SanctionBo SanctionBo { get; set; }

        public ProcessRowSanction(ProcessSanctionBatch batch, MappingSanction mappingSanction)
        {
            ProcessSanctionBatch = batch;
            MappingSanction = mappingSanction;
            SanctionBo = mappingSanction.SanctionBo;
        }

        public void Save()
        {
            if (SanctionBo == null)
                return;

            var sanctionBo = SanctionBo;
            sanctionBo.SanctionBatchId = ProcessSanctionBatch.SanctionBatchBo.Id;
            sanctionBo.CreatedById = User.DefaultSuperUserId;
            sanctionBo.UpdatedById = User.DefaultSuperUserId;
            Result result = SanctionService.Create(ref sanctionBo);

            if (result.Valid)
            {
                SaveChildren(sanctionBo.Id);
            }
        }

        public void SaveChildren(int sanctionBoId)
        {
            if (!MappingSanction.Addresses.IsNullOrEmpty())
            {
                foreach (string address in MappingSanction.Addresses)
                {
                    if (string.IsNullOrEmpty(address))
                        continue;

                    var sanctionAddress = new SanctionAddressBo()
                    {
                        SanctionId = sanctionBoId,
                        Address = address,
                        CreatedById = User.DefaultSuperUserId,
                        UpdatedById = User.DefaultSuperUserId,
                    };
                    SanctionAddressService.Create(ref sanctionAddress);
                }
            }

            if (!MappingSanction.BirthDates.IsNullOrEmpty())
            {
                foreach (string birthDateStr in MappingSanction.BirthDates)
                {
                    if (string.IsNullOrEmpty(birthDateStr))
                        continue;

                    int? birthYear = null;
                    DateTime? birthDate = null;
                    if (birthDateStr.Length == 4)
                    {
                        birthYear = Util.GetParseInt(birthDateStr);
                    }
                    else
                    {
                        birthDate = Util.GetParseDateTime(birthDateStr);
                        if (birthDate.HasValue)
                        {
                            birthYear = birthDate.Value.Year;
                        }
                    }

                    var sanctionBirthDate = new SanctionBirthDateBo()
                    {
                        SanctionId = sanctionBoId,
                        DateOfBirth = birthDate,
                        YearOfBirth = birthYear,
                        CreatedById = User.DefaultSuperUserId,
                        UpdatedById = User.DefaultSuperUserId,
                    };

                    SanctionBirthDateService.Create(ref sanctionBirthDate);
                }
            }

            if (!MappingSanction.Comments.IsNullOrEmpty())
            {
                foreach (string comment in MappingSanction.Comments)
                {
                    if (string.IsNullOrEmpty(comment))
                        continue;

                    var sanctionComment = new SanctionCommentBo()
                    {
                        SanctionId = sanctionBoId,
                        Comment = comment,
                        CreatedById = User.DefaultSuperUserId,
                        UpdatedById = User.DefaultSuperUserId,
                    };
                    SanctionCommentService.Create(ref sanctionComment);
                }
            }

            if (!MappingSanction.Countries.IsNullOrEmpty())
            {
                foreach (string country in MappingSanction.Countries)
                {
                    if (string.IsNullOrEmpty(country))
                        continue;

                    var sanctionCountry = new SanctionCountryBo()
                    {
                        SanctionId = sanctionBoId,
                        Country = country,
                        CreatedById = User.DefaultSuperUserId,
                        UpdatedById = User.DefaultSuperUserId,
                    };
                    SanctionCountryService.Create(ref sanctionCountry);
                }
            }

            if (!MappingSanction.IdTypes.IsNullOrEmpty() && !MappingSanction.IdNumbers.IsNullOrEmpty())
            {
                int identityCount = Math.Max(MappingSanction.IdTypes.Count(), MappingSanction.IdNumbers.Count());
                for (int i = 0; i < identityCount; i++)
                {
                    string idType = MappingSanction.IdTypes.Count() > i ? MappingSanction.IdTypes[i] : "";
                    string idNumber = MappingSanction.IdNumbers.Count() > i ? MappingSanction.IdNumbers[i] : "";

                    if (string.IsNullOrEmpty(idType) && string.IsNullOrEmpty(idNumber))
                        continue;

                    var sanctionIdentity = new SanctionIdentityBo()
                    {
                        SanctionId = sanctionBoId,
                        IdType = idType,
                        IdNumber = idNumber,
                        CreatedById = User.DefaultSuperUserId,
                        UpdatedById = User.DefaultSuperUserId,
                    };
                    SanctionIdentityService.Create(ref sanctionIdentity);
                }
            }

            if (!MappingSanction.Aliases.IsNullOrEmpty())
            {
                foreach (string alias in MappingSanction.Aliases)
                {
                    if (string.IsNullOrEmpty(alias))
                        continue;

                    var sanctionName = new SanctionNameBo()
                    {
                        SanctionId = sanctionBoId,
                        Name = alias,
                        CreatedById = User.DefaultSuperUserId,
                        UpdatedById = User.DefaultSuperUserId,
                    };
                    SanctionNameService.Create(ref sanctionName);
                }
            }
        }
    }
}
