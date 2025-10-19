using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebAPI.Hubs
{
    /// <summary>
    /// Genel bildirimler için gerçek zamanlı iletişimi sağlayan SignalR Hub'ı
    /// </summary>
    public class BildirimHub : Hub
    {
        /// <summary>
        /// Bağlantı kurulduğunda çalışan metot
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            // Kullanıcı kimliğini alabiliriz (JWT'den)
            var kullaniciId = Context.User?.Identity?.Name;
            
            if (!string.IsNullOrEmpty(kullaniciId))
            {
                // Kullanıcıyı kendi grubuna ekle (kişisel bildirimler için)
                await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{kullaniciId}");
                
                // Kullanıcının rollerini kontrol edebiliriz (JWT claim'lerinden)
                var roller = Context.User?.Claims.Where(c => c.Type == "role").Select(c => c.Value);
                
                if (roller != null)
                {
                    foreach (var rol in roller)
                    {
                        // Kullanıcıyı rol bazlı gruplara ekleme
                        await Groups.AddToGroupAsync(Context.ConnectionId, $"Role_{rol}");
                    }
                }
            }
            
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Belirli bir kullanıcıya bildirim gönderen metot
        /// </summary>
        /// <param name="kullaniciId">Bildirimin gönderileceği kullanıcı ID</param>
        /// <param name="bildirimTipi">Bildirim tipi</param>
        /// <param name="baslik">Bildirim başlığı</param>
        /// <param name="mesaj">Bildirim içeriği</param>
        /// <param name="veri">Ek veri (JSON olarak)</param>
        /// <returns></returns>
        public async Task KullaniciyaBildirimGonder(string kullaniciId, string bildirimTipi, string baslik, string mesaj, object veri = null)
        {
            await Clients.Group($"User_{kullaniciId}").SendAsync(
                "BildirimAlindi", 
                new { 
                    tip = bildirimTipi, 
                    baslik = baslik, 
                    mesaj = mesaj, 
                    tarih = DateTime.Now,
                    veri = veri
                }
            );
        }

        /// <summary>
        /// Belirli bir role sahip kullanıcılara bildirim gönderen metot
        /// </summary>
        /// <param name="rol">Bildirimin gönderileceği rol</param>
        /// <param name="bildirimTipi">Bildirim tipi</param>
        /// <param name="baslik">Bildirim başlığı</param>
        /// <param name="mesaj">Bildirim içeriği</param>
        /// <param name="veri">Ek veri (JSON olarak)</param>
        /// <returns></returns>
        public async Task RoleBildirimGonder(string rol, string bildirimTipi, string baslik, string mesaj, object veri = null)
        {
            await Clients.Group($"Role_{rol}").SendAsync(
                "BildirimAlindi", 
                new { 
                    tip = bildirimTipi, 
                    baslik = baslik, 
                    mesaj = mesaj, 
                    tarih = DateTime.Now,
                    veri = veri
                }
            );
        }

        /// <summary>
        /// Tüm kullanıcılara bildirim gönderen metot
        /// </summary>
        /// <param name="bildirimTipi">Bildirim tipi</param>
        /// <param name="baslik">Bildirim başlığı</param>
        /// <param name="mesaj">Bildirim içeriği</param>
        /// <param name="veri">Ek veri (JSON olarak)</param>
        /// <returns></returns>
        public async Task HerkeseBildirimGonder(string bildirimTipi, string baslik, string mesaj, object veri = null)
        {
            await Clients.All.SendAsync(
                "BildirimAlindi", 
                new { 
                    tip = bildirimTipi, 
                    baslik = baslik, 
                    mesaj = mesaj, 
                    tarih = DateTime.Now, 
                    veri = veri
                }
            );
        }
    }
} 