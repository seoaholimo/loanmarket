using BeyondIT.MicroLoan.Infrastructure.Database;
using BeyondIT.MicroLoan.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeyondIT.MicroLoan.Infrastructure.Database
{
    public class SeedDataService
    {
         private readonly MicroLoanContext _microLoanContext;
         public SeedDataService(MicroLoanContext microLoanContext)
         {
             _microLoanContext = microLoanContext;

         }
         public void SeedData()
         {
             Console.WriteLine("Migrating Database to :" + _microLoanContext.Database.GetDbConnection().ConnectionString);
             _microLoanContext.Database.Migrate();
             SeedUserData();

         }

         private void SeedUserData()
         {
             if (_microLoanContext.User.Any()) return;

             _microLoanContext.User.Add(new User
             {
                 UserId = User.GetSystemUser().UserId,
                 IdentityDocumentNumber = "0000000000000",
                 Name = "System",
                 Surname = "Admin",
                 Password = "wZ9swJpwk56XyJPkjuKLRlw5YRSxokHSUifPEbxNYqk=", //Password123
                 PasswordSalt = "jI9HjpI8x2hP5eOrkIgRa0lO",
                 Email = "seoaholimo@etl.co.ls",
                 CellphoneNumber = "0000000000",
                 DateCreated = DateTime.Now,
                 DateUpdated = DateTime.Now,
                 IsSuperUser = true,
                 IsFirstLogin = false
             });

             using (var transaction = _microLoanContext.Database.BeginTransaction())
             {
                 _microLoanContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [User] ON;");
                 _microLoanContext.Save();
                 _microLoanContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [User] OFF");
                 transaction.Commit();
             }
         }
    }
}
