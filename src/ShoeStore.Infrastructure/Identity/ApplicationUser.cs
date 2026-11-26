using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ShoeStore.Infrastructure.Identity;

// IdentityUser hazır olarak Id, Email, UserName, PasswordHash gibi alanları getirir.
// Kendi eklemek istediğimiz alanları buraya yazarız.
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}