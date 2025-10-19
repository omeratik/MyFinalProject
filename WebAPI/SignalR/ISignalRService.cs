using System.Threading.Tasks;

namespace WebAPI.SignalR
{
    /// <summary>
    /// SignalR servisleri için arayüz
    /// </summary>
    public interface ISignalRService
    {
        /// <summary>
        /// Stok değişimini bildirir
        /// </summary>
        /// <param name="urunKodu">Ürün kodu</param>
        /// <param name="miktar">Değişen miktar</param>
        /// <param name="islemTuru">İşlem türü (Giriş/Çıkış)</param>
        /// <param name="kullanici">İşlemi yapan kullanıcı</param>
        /// <param name="depo">İşlemin yapıldığı depo</param>
        /// <returns></returns>
        Task StokDegisiminiBildir(string urunKodu, int miktar, string islemTuru, string kullanici, string depo);

        /// <summary>
        /// Kritik stok seviyesi uyarısı gönderir
        /// </summary>
        /// <param name="urunKodu">Ürün kodu</param>
        /// <param name="mevcutMiktar">Mevcut stok miktarı</param>
        /// <param name="kritikSeviye">Kritik stok seviyesi</param>
        /// <returns></returns>
        Task KritikStokUyarisiGonder(string urunKodu, int mevcutMiktar, int kritikSeviye);

        /// <summary>
        /// Belirli bir kullanıcıya bildirim gönderir
        /// </summary>
        /// <param name="kullaniciId">Hedef kullanıcı ID</param>
        /// <param name="bildirimTipi">Bildirim tipi</param>
        /// <param name="baslik">Bildirim başlığı</param>
        /// <param name="mesaj">Bildirim mesajı</param>
        /// <param name="veri">Ek veri (opsiyonel)</param>
        /// <returns></returns>
        Task KullaniciyaBildirimGonder(string kullaniciId, string bildirimTipi, string baslik, string mesaj, object veri = null);

        /// <summary>
        /// Belirli bir role sahip tüm kullanıcılara bildirim gönderir
        /// </summary>
        /// <param name="rol">Hedef rol</param>
        /// <param name="bildirimTipi">Bildirim tipi</param>
        /// <param name="baslik">Bildirim başlığı</param>
        /// <param name="mesaj">Bildirim mesajı</param>
        /// <param name="veri">Ek veri (opsiyonel)</param>
        /// <returns></returns>
        Task RoleBildirimGonder(string rol, string bildirimTipi, string baslik, string mesaj, object veri = null);

        /// <summary>
        /// Tüm bağlı kullanıcılara bildirim gönderir
        /// </summary>
        /// <param name="bildirimTipi">Bildirim tipi</param>
        /// <param name="baslik">Bildirim başlığı</param>
        /// <param name="mesaj">Bildirim mesajı</param>
        /// <param name="veri">Ek veri (opsiyonel)</param>
        /// <returns></returns>
        Task HerkeseBildirimGonder(string bildirimTipi, string baslik, string mesaj, object veri = null);
    }
} 