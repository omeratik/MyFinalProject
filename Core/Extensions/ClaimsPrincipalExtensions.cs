using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            Console.WriteLine($"Claims metodu çağrıldı. Aranan Claim Tipi: {claimType}");

            var claims = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();

            if (claims == null || !claims.Any())
            {
                Console.WriteLine($"UYARI: {claimType} tipinde claim bulunamadı!");

                // Ek olarak tüm claim tiplerini kontrol edelim
                var allClaimTypes = claimsPrincipal?.Claims?.Select(c => c.Type).Distinct().ToList();
                Console.WriteLine($"Mevcut Claim Tipleri: {(allClaimTypes != null ? string.Join(", ", allClaimTypes) : "Hiç claim yok")}");

                // Manuel olarak Microsoft'un rol claim tipini kontrol edelim
                var msRoleClaims = claimsPrincipal?.FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Select(x => x.Value).ToList();
                Console.WriteLine($"Microsoft Role Claims: {(msRoleClaims != null && msRoleClaims.Any() ? string.Join(", ", msRoleClaims) : "Bulunamadı")}");

                return new List<string>();
            }

            Console.WriteLine($"Bulunan Claims: {string.Join(", ", claims)}");
            return claims;
        }

        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            Console.WriteLine($"ClaimRoles metodu çağrıldı. ClaimTypes.Role: {ClaimTypes.Role}");

            // Standart yöntem ile rol claim'lerini al
            var result = claimsPrincipal?.Claims(ClaimTypes.Role);
            
            // Eğer hiç rol bulunamadıysa, Microsoft'un tam claim tipini kullanarak dene
            if (result == null || !result.Any())
            {
                // Microsoft'un rol claim tipi
                const string msRoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
                result = claimsPrincipal?.FindAll(msRoleClaimType)?.Select(x => x.Value).ToList();
                
                // Hala bulunamadıysa ve debug modundaysak, mevcut claim tiplerini logla
                if ((result == null || !result.Any()) && System.Diagnostics.Debugger.IsAttached)
                {
                    Console.WriteLine("UYARI: Roller bulunamadı, mevcut claim tipleri:");
                    var claimTypes = claimsPrincipal?.Claims.Select(c => c.Type).Distinct();
                    if (claimTypes != null)
                    {
                        foreach (var type in claimTypes)
                        {
                            Console.WriteLine($"  - {type}");
                        }
                    }
                }
            }
            
            return result;
        }
    }
}
