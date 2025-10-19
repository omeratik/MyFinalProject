using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebAPI.Hubs
{
    /// <summary>
    /// Stok hareketleri için gerçek zamanlı iletişimi sağlayan SignalR Hub'ı
    /// </summary>
    public class StokHub : Hub
    {
        /// <summary>
        /// Bağlantı kurulduğunda çalışan metot
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            // Bağlantı bilgilerini loglamak veya kullanıcıya özel işlemler yapmak için kullanılabilir
            string connectionId = Context.ConnectionId;
            string? userAgent = Context.GetHttpContext()?.Request.Headers["User-Agent"].ToString();
            
            // Kullanıcı bilgilerini loglama
            System.Diagnostics.Debug.WriteLine($"Yeni bağlantı: {connectionId}, User Agent: {userAgent}");
            
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Bağlantı kesildiğinde çalışan metot
        /// </summary>
        /// <param name="exception">Bağlantı kesilme nedeni</param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Kullanıcı çıkış yaptığında veya bağlantı kesildiğinde yapılacak işlemler
            string connectionId = Context.ConnectionId;
            
            System.Diagnostics.Debug.WriteLine($"Bağlantı kesildi: {connectionId}, Neden: {exception?.Message ?? "Bilinmiyor"}");
            
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Bir gruba katılmak için kullanılan metot
        /// </summary>
        /// <param name="grupAdi">Katılmak istenen grup adı</param>
        /// <returns></returns>
        public async Task GrubaKatil(string grupAdi)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, grupAdi);
            await Clients.Caller.SendAsync("GrupMesaji", $"{grupAdi} grubuna katıldınız.");
        }

        /// <summary>
        /// Bir gruptan ayrılmak için kullanılan metot
        /// </summary>
        /// <param name="grupAdi">Ayrılmak istenen grup adı</param>
        /// <returns></returns>
        public async Task GruptanAyril(string grupAdi)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, grupAdi);
            await Clients.Caller.SendAsync("GrupMesaji", $"{grupAdi} grubundan ayrıldınız.");
        }

        /// <summary>
        /// Stok değişimini bildiren metot
        /// </summary>
        /// <param name="urunKodu">Ürün kodu</param>
        /// <param name="miktar">Değişen miktar</param>
        /// <param name="islemTuru">İşlem türü (Giriş/Çıkış)</param>
        /// <param name="kullanici">İşlemi yapan kullanıcı</param>
        /// <param name="depo">İşlemin yapıldığı depo</param>
        /// <returns></returns>
        public async Task StokDegisimiBildir(string urunKodu, int miktar, string islemTuru, string kullanici, string depo)
        {
            // Tüm bağlı istemcilere bildirim gönderme
            await Clients.All.SendAsync("StokGuncellendi", urunKodu, miktar, islemTuru, kullanici, depo, DateTime.Now);
            
            // Belirli bir gruba bildirim gönderme (örn: depo bazlı)
            await Clients.Group(depo).SendAsync("DepoStokGuncellendi", urunKodu, miktar, islemTuru, kullanici, DateTime.Now);
        }

        /// <summary>
        /// Kritik stok seviyesi uyarısı gönderen metot
        /// </summary>
        /// <param name="urunKodu">Ürün kodu</param>
        /// <param name="mevcutMiktar">Mevcut stok miktarı</param>
        /// <param name="kritikSeviye">Kritik stok seviyesi</param>
        /// <returns></returns>
        public async Task KritikStokUyarisi(string urunKodu, int mevcutMiktar, int kritikSeviye)
        {
            // Sadece yöneticilere bildirim gönderme
            await Clients.Group("Yoneticiler").SendAsync("KritikStokSeviyesi", urunKodu, mevcutMiktar, kritikSeviye, DateTime.Now);
        }
    }
} 