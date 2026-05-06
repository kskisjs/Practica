using System;
using System.Collections.ObjectModel;

namespace CRMApp
{
    public class Client
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public ObservableCollection<Interaction> History { get; set; }

        public Client()
        {
            History = new ObservableCollection<Interaction>();
        }
    }

    public class Interaction
    {
        public DateTime Date { get; set; }
        public string Note { get; set; }
    }
}