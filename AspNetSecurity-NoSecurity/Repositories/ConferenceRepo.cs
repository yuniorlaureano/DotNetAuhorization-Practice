using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNetSecurity_NoSecurity.Models;
using AspNetSecurityNoSecurity.AuthorizationPractice.Models;
using Microsoft.AspNetCore.DataProtection;

namespace AspNetSecurity_NoSecurity.Repositories
{
    public class ConferenceRepo
    {
        private readonly List<ConferenceModel> conferences = new List<ConferenceModel>();
        private readonly IDataProtector dataProtector;

        public ConferenceRepo(IDataProtectionProvider dataProtectionProvider, PurposeStringConstant purposeStringConstant)
        {
            dataProtector = dataProtectionProvider.CreateProtector(purposeStringConstant.ConferenceIdQueryStirng);
            conferences.Add(new ConferenceModel { Id = 1, EncriptedId = dataProtector.Protect("1"), Name = "NDC", Location = "Oslo", Start = new DateTime(2017, 6, 12)});
            conferences.Add(new ConferenceModel { Id = 2, EncriptedId = dataProtector.Protect("2"), Name = "IT/DevConnections", Location = "San Francisco", Start = new DateTime(2017, 10, 18)});
        }
        public IEnumerable<ConferenceModel> GetAll()
        {
            return conferences;
        }

        public ConferenceModel GetById(int id)
        {
            return conferences.First(c => c.Id == id);
        }

        public void Add(ConferenceModel model)
        {
            model.Id = conferences.Max(c => c.Id) + 1;
            model.EncriptedId = dataProtector.Protect(model.Id.ToString());
            conferences.Add(model);
        }
    }
}
