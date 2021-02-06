using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Model
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int GenresId { get; set; }

        public Genres Genres { get; set; }

        public int AuthorId { get; set; }

        public Author Author { get; set; }

        public int PublisherId { get; set; }

        public Publisher Publisher { get; set; }

        public int LanguageId { get; set; }

        public Language Langauge { get; set; }

        public int NoOfPages { get; set; }

        public string ISBN10No { get; set; }

        public string ISBN13No { get; set; }

        public string CountryOfOrigin { get; set; }

        public DateTime PublicationDate { get; set; }

        public int OfferId { get; set; }

        public Offer Offer { get; set; }

        public string Dimension { get; set; }

        public int BookFormatId { get; set; }

        public BookFormat BookFormat { get; set; }

        public int SellerId { get; set; }

        public Seller Seller { get; set; }

        public float AvgCustomerRating { get; set; }
    }
}
