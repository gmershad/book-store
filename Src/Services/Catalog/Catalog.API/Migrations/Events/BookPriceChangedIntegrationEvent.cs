using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Events;

namespace Catalog.API.Migrations.Events
{
    public class BookPriceChangedIntegrationEvent : IntegrationEvent
    {
        public int BookId { get; set; }

        public double NewPrice { get; private set; }

        public double OldPrice { get; private set; }

        public BookPriceChangedIntegrationEvent(int bookId, double newPrice, double oldPrice)
        {
            BookId = bookId;
            NewPrice = newPrice;
            OldPrice = oldPrice;
        }
    }
}
