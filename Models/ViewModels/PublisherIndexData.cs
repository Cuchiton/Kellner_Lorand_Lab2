using Kellner_Lorand_Lab2.Models;

namespace Kellner_Lorand_Lab2.ViewModels
{
    public class PublisherIndexData
    {
        public IEnumerable<Publisher> Publishers { get; set; }
            
        public IEnumerable<Book>? Books { get; set; }
    }
}