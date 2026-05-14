using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMApp.Models;

namespace CRMApp.Services
{
    // В реальном проекте здесь был бы репозиторий с базой данных.
    // Пока используем in-memory хранилище с имитацией задержки сети.
    public class ClientService : IClientService
    {
        private readonly List<ClientModel> _clients = new()
        {
            new ClientModel { Id = 1, FullName = "Иванов Иван Иванович",   Phone = "+7 900 111-22-33", Email = "ivanov@mail.ru",  ClientType = ClientType.VIP     },
            new ClientModel { Id = 2, FullName = "Петрова Мария Сергеевна", Phone = "+7 900 444-55-66", Email = "petrova@mail.ru", ClientType = ClientType.Regular },
            new ClientModel { Id = 3, FullName = "Сидоров Алексей Юрьевич", Phone = "+7 900 777-88-99", Email = "sidorov@mail.ru", ClientType = ClientType.Regular },
            new ClientModel { Id = 4, FullName = "Козлова Анна Дмитриевна",  Phone = "+7 900 000-11-22", Email = "kozlova@mail.ru", ClientType = ClientType.VIP     },
        };

        private int _nextId = 5;

        // Имитация загрузки из БД/API
        public async Task<List<ClientModel>> GetAllClientsAsync()
        {
            await Task.Delay(1500); // имитация задержки сети
            return _clients.ToList();
        }

        public async Task<ClientModel?> GetClientByIdAsync(int id)
        {
            await Task.Delay(200);
            return _clients.FirstOrDefault(c => c.Id == id);
        }

        public async Task AddClientAsync(ClientModel client)
        {
            await Task.Delay(300);
            client.Id = _nextId++;
            _clients.Add(client);
        }

        public async Task UpdateClientAsync(ClientModel client)
        {
            await Task.Delay(300);
            var existing = _clients.FirstOrDefault(c => c.Id == client.Id)
                ?? throw new InvalidOperationException($"Клиент с Id={client.Id} не найден.");

            existing.FullName   = client.FullName;
            existing.Phone      = client.Phone;
            existing.Email      = client.Email;
            existing.ClientType = client.ClientType;
        }

        public async Task DeleteClientAsync(int id)
        {
            await Task.Delay(300);
            var client = _clients.FirstOrDefault(c => c.Id == id)
                ?? throw new InvalidOperationException($"Клиент с Id={id} не найден.");
            _clients.Remove(client);
        }
    }
}
