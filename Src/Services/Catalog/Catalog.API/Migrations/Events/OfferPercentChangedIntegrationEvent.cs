using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Events;

namespace Catalog.API.Migrations.Events
{
    public class OfferPercentChangedIntegrationEvent : IntegrationEvent
    {
        public int OfferId { get; set; }

        public int NewOfferPercent { get; private set; }

        public int OldOfferPercent { get; private set; }

        public OfferPercentChangedIntegrationEvent(int offerId, int newOfferPercent, int oldOfferPercemt)
        {
            OfferId = offerId;
            NewOfferPercent = newOfferPercent;
            OldOfferPercent = oldOfferPercemt;
        }

    }
}
