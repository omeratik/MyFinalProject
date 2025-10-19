using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using WebAPI.Hubs;

namespace WebAPI.SignalR
{
    /// <summary>
    /// SignalR servislerinin implementasyonu
    /// </summary>
    public class SignalRService : ISignalRService
    {
        private readonly IHubContext<StokHub> _stokHubContext;
        private readonly IHubContext<BildirimHub> _bildirimHubContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stokHubContext">Stok hub context</param>
        /// <param name="bildirimHubContext">Bildirim hub context</param>
        public SignalRService(
            IHubContext<StokHub> stokHubContext,
            IHubContext<BildirimHub> bildirimHubContext)
        {
            _stokHubContext = stokHubContext;
            _bildirimHubContext = bildirimHubContext;
        }

        /// <summary>
        /// Stok değişimini bildirir
        /// </summary>
        /// <param name="urunKodu">Ürün kodu</param>
        /// <param name="miktar">Değişen miktar</param>
        /// <param name="islemTuru">İşlem türü (Giriş/Çıkış)</param>
        /// <param name="kullanici">İşlemi yapan kullanıcı</param>
        /// <param name="depo">İşlemin yapıldığı depo</param>
        /// <returns></returns>
        public async Task StokDegisiminiBildir(string urunKodu, int miktar, string islemTuru, string kullanici, string depo)
        {
            // Tüm bağlı istemcilere bildirim gönderme
            await _stokHubContext.Clients.All.SendAsync(
                "StokGuncellendi", 
                urunKodu, 
                miktar, 
                islemTuru, 
                kullanici, 
                depo, 
                DateTime.Now
            );
            
            // Belirli bir gruba bildirim gönderme (örn: depo bazlı)
            await _stokHubContext.Clients.Group(depo).SendAsync(
                "DepoStokGuncellendi", 
                urunKodu, 
                miktar, 
                islemTuru, 
                kullanici, 
                DateTime.Now
            );
        }

        /// <summary>
        /// Kritik stok seviyesi uyarısı gönderir
        /// </summary>
        /// <param name="urunKodu">Ürün kodu</param>
        /// <param name="mevcutMiktar">Mevcut stok miktarı</param>
        /// <param name="kritikSeviye">Kritik stok seviyesi</param>
        /// <returns></returns>
        public async Task KritikStokUyarisiGonder(string urunKodu, int mevcutMiktar, int kritikSeviye)
        {
            await _stokHubContext.Clients.Group("Yoneticiler").SendAsync(
                "KritikStokSeviyesi", 
                urunKodu, 
                mevcutMiktar, 
                kritikSeviye, 
                DateTime.Now
            );
        }

        /// <summary>
        /// Belirli bir kullanıcıya bildirim gönderir
        /// </summary>
        /// <param name="kullaniciId">Hedef kullanıcı ID</param>
        /// <param name="bildirimTipi">Bildirim tipi</param>
        /// <param name="baslik">Bildirim başlığı</param>
        /// <param name="mesaj">Bildirim mesajı</param>
        /// <param name="veri">Ek veri (opsiyonel)</param>
        /// <returns></returns>
        public async Task KullaniciyaBildirimGonder(string kullaniciId, string bildirimTipi, string baslik, string mesaj, object veri = null)
        {
            await _bildirimHubContext.Clients.Group($"User_{kullaniciId}").SendAsync(
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
        /// Belirli bir role sahip tüm kullanıcılara bildirim gönderir
        /// </summary>
        /// <param name="rol">Hedef rol</param>
        /// <param name="bildirimTipi">Bildirim tipi</param>
        /// <param name="baslik">Bildirim başlığı</param>
        /// <param name="mesaj">Bildirim mesajı</param>
        /// <param name="veri">Ek veri (opsiyonel)</param>
        /// <returns></returns>
        public async Task RoleBildirimGonder(string rol, string bildirimTipi, string baslik, string mesaj, object veri = null)
        {
            await _bildirimHubContext.Clients.Group($"Role_{rol}").SendAsync(
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
        /// Tüm bağlı kullanıcılara bildirim gönderir
        /// </summary>
        /// <param name="bildirimTipi">Bildirim tipi</param>
        /// <param name="baslik">Bildirim başlığı</param>
        /// <param name="mesaj">Bildirim mesajı</param>
        /// <param name="veri">Ek veri (opsiyonel)</param>
        /// <returns></returns>
        public async Task HerkeseBildirimGonder(string bildirimTipi, string baslik, string mesaj, object veri = null)
        {
            await _bildirimHubContext.Clients.All.SendAsync(
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