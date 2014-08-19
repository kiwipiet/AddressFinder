using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AddressFinder
{
    public class PostalAddressConverterLuceneDocument : PostalAddressConverter<Document>
    {
        public PostalAddressConverterLuceneDocument(PostalAddress postalAddress)
            : base(postalAddress)
        {
        }

        public override Document Convert()
        {
            var doc = new Document();

            doc.AddField(PostalAddress.PropertyName(x => x.AddressType), PostalAddress.AddressType, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.BoxBagLobbyName), PostalAddress.BoxBagLobbyName, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.BoxBagNumber), PostalAddress.BoxBagNumber, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.BuildingName), PostalAddress.BuildingName, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.DeliveryServiceType), PostalAddress.DeliveryServiceType, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.Floor), PostalAddress.Floor, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.Id), PostalAddress.Id, Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.PostCode), PostalAddress.PostCode, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.RDNumber), PostalAddress.RDNumber, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.StreetAlpha), PostalAddress.StreetAlpha, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.StreetDirection), PostalAddress.StreetDirection, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.StreetName), PostalAddress.StreetName, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.StreetNumber), PostalAddress.StreetNumber, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.StreetType), PostalAddress.StreetType, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.SuburbName), PostalAddress.SuburbName, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.TownCityMailTown), PostalAddress.TownCityMailTown, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.UnitId), PostalAddress.UnitId, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.UnitType), PostalAddress.UnitType, Field.Store.YES, Field.Index.ANALYZED);
            doc.AddField(PostalAddress.PropertyName(x => x.AddressLine), PostalAddress.AddressLine, Field.Store.YES, Field.Index.ANALYZED);

            return doc;
        }
    }
}
