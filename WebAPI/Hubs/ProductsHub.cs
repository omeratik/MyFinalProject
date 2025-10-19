using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Entities.Concrete;
using System.Collections.Generic;
using System;

namespace WebAPI.Hubs
{
    /// <summary>
    /// Ürün işlemleri için gerçek zamanlı iletişimi sağlayan SignalR Hub'ı
    /// </summary>
    public class ProductsHub : Hub
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
            System.Diagnostics.Debug.WriteLine($"ProductsHub - Yeni bağlantı: {connectionId}, User Agent: {userAgent}");
            
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
            
            System.Diagnostics.Debug.WriteLine($"ProductsHub - Bağlantı kesildi: {connectionId}, Neden: {exception?.Message ?? "Bilinmiyor"}");
            
            await base.OnDisconnectedAsync(exception);
        }
        
        /// <summary>
        /// Yeni ürün eklendiğinde tüm istemcilere bildirim gönderir
        /// </summary>
        /// <param name="product">Eklenen ürün</param>
        /// <returns></returns>
        public async Task NotifyProductAdded(Product product)
        {
            System.Diagnostics.Debug.WriteLine($"ProductsHub - Product added notification: {product.ProductId} - {product.ProductName}");
            await Clients.Others.SendAsync("ProductAdded", product);
        }
        
        /// <summary>
        /// Ürün güncellendiğinde tüm istemcilere bildirim gönderir
        /// </summary>
        /// <param name="product">Güncellenen ürün</param>
        /// <returns></returns>
        public async Task NotifyProductUpdated(Product product)
        {
            System.Diagnostics.Debug.WriteLine($"ProductsHub - Product updated notification: {product.ProductId} - {product.ProductName}");
            await Clients.Others.SendAsync("ProductUpdated", product);
        }
        
        /// <summary>
        /// Ürün silindiğinde tüm istemcilere bildirim gönderir
        /// </summary>
        /// <param name="productId">Silinen ürün ID</param>
        /// <returns></returns>
        public async Task NotifyProductDeleted(int productId)
        {
            System.Diagnostics.Debug.WriteLine($"ProductsHub - Product deleted notification: {productId}");
            await Clients.Others.SendAsync("ProductDeleted", productId);
        }
        
        /// <summary>
        /// Ürün listesinin yenilenmesi gerektiğini bildirir
        /// </summary>
        /// <returns></returns>
        public async Task RequestRefreshProducts()
        {
            System.Diagnostics.Debug.WriteLine("ProductsHub - Refresh products request");
            await Clients.Others.SendAsync("RefreshProducts");
        }
    }
} 