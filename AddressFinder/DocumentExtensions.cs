using Lucene.Net.Documents;

namespace AddressFinder
{
    internal static class DocumentExtensions
    {
        internal static void AddField(this Document doc, string propertyName, string propertyValue, Field.Store fieldStore, Field.Index fieldIndex)
        {
            if (string.IsNullOrWhiteSpace(propertyValue))
                return;

            doc.Add(new Field(propertyName, propertyValue, fieldStore, fieldIndex));
        }
    }
}
