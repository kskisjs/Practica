using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMApp.Models;

namespace CRMApp.Services
{
    public interface IClientService
    {
        Task<List<ClientModel>> GetAllClientsAsync();
        Task<ClientModel?> GetClientByIdAsync(int id);
        Task AddClientAsync(ClientModel client);
        Task UpdateClientAsync(ClientModel client);
        Task DeleteClientAsync(int id);

        Task<List<OrderModel>> GetOrdersByClientAsync(int clientId);
        Task AddOrderAsync(OrderModel order);
        Task UpdateOrderAsync(OrderModel order);
        Task DeleteOrderAsync(int id);
        Task<List<OrderModel>> GetAllOrdersAsync();
    }

    public class ClientService : IClientService
    {
        private CrmDataStore _store;

        // Событие для уведомления FileSystemWatcher-а — вызывается после сохранения
        public event Action? DataChanged;

        public ClientService()
        {
            _store = JsonStorage.Load<CrmDataStore>(JsonStorage.CrmDataPath);

            if (_store.Clients.Count == 0)
                SeedDemo();
        }

        private void SeedDemo()
        {
            _store.Clients.AddRange(new[]
            {
                new ClientModel { Id = _store.NextClientId++, FullName = "Иванов Иван Иванович",   Phone = "+7 900 111-22-33", Email = "ivanov@mail.ru",  Company = "ООО Ромашка",  ClientType = ClientType.VIP     },
                new ClientModel { Id = _store.NextClientId++, FullName = "Петрова Мария Сергеевна", Phone = "+7 900 444-55-66", Email = "petrova@mail.ru", Company = "ИП Петрова",   ClientType = ClientType.Regular },
                new ClientModel { Id = _store.NextClientId++, FullName = "Сидоров Алексей Юрьевич", Phone = "+7 900 777-88-99", Email = "sidorov@mail.ru", Company = "ЗАО Сидор",   ClientType = ClientType.Regular },
                new ClientModel { Id = _store.NextClientId++, FullName = "Козлова Анна Дмитриевна", Phone = "+7 900 000-11-22", Email = "kozlova@mail.ru", Company = "ООО Лютик",   ClientType = ClientType.VIP     },
            });

            var now = DateTime.Now;
            _store.Orders.AddRange(new[]
            {
                new OrderModel { Id = _store.NextOrderId++, ClientId = 1, Description = "Поставка оборудования",  Amount = 150000, CreatedAt = now.AddDays(-10), Status = OrderStatus.InProgress },
                new OrderModel { Id = _store.NextOrderId++, ClientId = 2, Description = "Консультационные услуги", Amount = 30000,  CreatedAt = now.AddDays(-5),  Status = OrderStatus.New       },
                new OrderModel { Id = _store.NextOrderId++, ClientId = 1, Description = "Техническое обслуживание",Amount = 25000,  CreatedAt = now.AddDays(-2),  Status = OrderStatus.Completed },
            });

            SaveData();
        }

        private void SaveData()
        {
            JsonStorage.Save(JsonStorage.CrmDataPath, _store);
            DataChanged?.Invoke();
        }

        // ── Clients ────────────────────────────────────────────────────────────

        public Task<List<ClientModel>> GetAllClientsAsync()
        {
            _store = JsonStorage.Load<CrmDataStore>(JsonStorage.CrmDataPath);
            return Task.FromResult(_store.Clients.ToList());
        }

        public Task<ClientModel?> GetClientByIdAsync(int id)
        {
            return Task.FromResult(_store.Clients.FirstOrDefault(c => c.Id == id));
        }

        public Task AddClientAsync(ClientModel client)
        {
            client.Id = _store.NextClientId++;
            _store.Clients.Add(client);
            SaveData();
            return Task.CompletedTask;
        }

        public Task UpdateClientAsync(ClientModel client)
        {
            var existing = _store.Clients.FirstOrDefault(c => c.Id == client.Id)
                ?? throw new InvalidOperationException($"Клиент с Id={client.Id} не найден.");

            existing.FullName   = client.FullName;
            existing.Phone      = client.Phone;
            existing.Email      = client.Email;
            existing.Company    = client.Company;
            existing.Notes      = client.Notes;
            existing.ClientType = client.ClientType;
            SaveData();
            return Task.CompletedTask;
        }

        public Task DeleteClientAsync(int id)
        {
            var client = _store.Clients.FirstOrDefault(c => c.Id == id)
                ?? throw new InvalidOperationException($"Клиент с Id={id} не найден.");
            _store.Clients.Remove(client);
            // Удаляем связанные заказы
            _store.Orders.RemoveAll(o => o.ClientId == id);
            SaveData();
            return Task.CompletedTask;
        }

        // ── Orders ─────────────────────────────────────────────────────────────

        public Task<List<OrderModel>> GetAllOrdersAsync()
        {
            return Task.FromResult(_store.Orders.ToList());
        }

        public Task<List<OrderModel>> GetOrdersByClientAsync(int clientId)
        {
            return Task.FromResult(_store.Orders.Where(o => o.ClientId == clientId).ToList());
        }

        public Task AddOrderAsync(OrderModel order)
        {
            order.Id = _store.NextOrderId++;
            order.CreatedAt = DateTime.Now;
            _store.Orders.Add(order);
            SaveData();
            return Task.CompletedTask;
        }

        public Task UpdateOrderAsync(OrderModel order)
        {
            var existing = _store.Orders.FirstOrDefault(o => o.Id == order.Id)
                ?? throw new InvalidOperationException($"Заказ с Id={order.Id} не найден.");
            existing.Description = order.Description;
            existing.Amount      = order.Amount;
            existing.Status      = order.Status;
            SaveData();
            return Task.CompletedTask;
        }

        public Task DeleteOrderAsync(int id)
        {
            var order = _store.Orders.FirstOrDefault(o => o.Id == id)
                ?? throw new InvalidOperationException($"Заказ с Id={id} не найден.");
            _store.Orders.Remove(order);
            SaveData();
            return Task.CompletedTask;
        }
    }
}
