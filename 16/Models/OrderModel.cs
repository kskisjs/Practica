using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRMApp.Models
{
    public enum OrderStatus
    {
        New,
        InProgress,
        Completed,
        Cancelled
    }

    public class OrderModel : INotifyPropertyChanged
    {
        private int _id;
        private int _clientId;
        private string _description = string.Empty;
        private decimal _amount;
        private DateTime _createdAt;
        private OrderStatus _status;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public int ClientId
        {
            get => _clientId;
            set { _clientId = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public decimal Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(); }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set { _createdAt = value; OnPropertyChanged(); }
        }

        public OrderStatus Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
