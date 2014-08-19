using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LuceneDirectory = Lucene.Net.Store.Directory;

namespace AddressFinder
{
    public class AddressRepository : IAddressRepository
    {
        private const int MaxResults = 1000;
        readonly static PostalAddress address = null;
        public bool GetExplanations { get; set; }

        private sealed class TermAddDetail
        {
            private readonly string _name;
            public TermAddDetail(string name)
            {
                _name = name;
            }
            public string Name { get { return _name; } }
        }

        private static readonly DirectoryInfo INDEX_DIR = new DirectoryInfo(ConfigurationManager.AppSettings["IndexDir"]);
        private static readonly Analyzer Analyzer;
        private static readonly LuceneDirectory Directory;
        private static readonly IDictionary<string, HashSet<string>> FieldTerms = new Dictionary<string, HashSet<string>>();
        private static readonly TermAddDetail[] FieldTermDetails;

        static AddressRepository()
        {
            if (!INDEX_DIR.Exists)
            {
                INDEX_DIR.Create();
            }
            Directory = FSDirectory.Open(INDEX_DIR);
            Analyzer = new SynonymAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            FieldTermDetails = new[]
            {
                new TermAddDetail(address.PropertyName(x => x.DeliveryServiceType)),
                new TermAddDetail(address.PropertyName(x => x.StreetNumber)),
                new TermAddDetail(address.PropertyName(x => x.Floor)),
                new TermAddDetail(address.PropertyName(x => x.StreetName)),
                new TermAddDetail(address.PropertyName(x => x.SuburbName)),
                new TermAddDetail(address.PropertyName(x => x.TownCityMailTown)),
                new TermAddDetail(address.PropertyName(x => x.PostCode)),
                new TermAddDetail(address.PropertyName(x => x.BoxBagNumber)),
                new TermAddDetail(address.PropertyName(x => x.BoxBagLobbyName)),
                new TermAddDetail(address.PropertyName(x => x.UnitType)),
                new TermAddDetail(address.PropertyName(x => x.BuildingName)),
                new TermAddDetail(address.PropertyName(x => x.RDNumber)),
                new TermAddDetail(address.PropertyName(x => x.UnitId))
            };

            if (INDEX_DIR.GetFiles().Count() > 0)
            {
                using (var indexreader = IndexReader.Open(Directory, true))
                {
                    foreach (var property in FieldTermDetails)
                    {
                        FieldTerms.Add(property.Name,
                            new HashSet<string>(indexreader.UniqueTermsFromField(property.Name).ToList()));
                    }
                }
            }
        }
        public AddressRepository()
        {
        }

        private static bool ContainsTerm(SearchTerm term, string propertyName)
        {
            var fieldTerms = FieldTerms[propertyName];
            return fieldTerms.Contains(term.Term);
        }
        private static void AddAllTerms(BooleanQuery query, SearchTerms terms)
        {
            foreach (SearchTerm term in terms)
            {
                foreach (var property in FieldTermDetails)
                {
                    if (ContainsTerm(term, property.Name))
                    {
                        TermQuery termQry = new TermQuery(new Term(address.PropertyName(x => x.AddressLine), term.Term));
                        query.Add(termQry, Occur.SHOULD);
                        break;
                    }
                }
                if (term.Length > 1)
                {
                    PrefixQuery prefixQry = new PrefixQuery(new Term(address.PropertyName(x => x.AddressLine), term.Term));
                    //prefixQry.Boost = containsTerm ? property.Boost / 2 : property.Boost;
                    query.Add(prefixQry, Occur.SHOULD);
                }
                if (!term.IsNumeric)
                {
                    if (FieldTerms[address.PropertyName(x => x.DeliveryServiceType)].Contains(term.Term))
                    {
                        TermQuery termQry = new TermQuery(new Term(address.PropertyName(x => x.DeliveryServiceType), term.Term));
                        termQry.Boost = terms.OriginalSearchTerms.StartsWith("po b", StringComparison.InvariantCultureIgnoreCase) ? 800 : 100;
                        query.Add(termQry, Occur.SHOULD);
                    }
                    if (!terms.OriginalSearchTerms.StartsWith("po b", StringComparison.InvariantCultureIgnoreCase))
                    {
                        bool containstStreetName = FieldTerms[address.PropertyName(x => x.StreetName)].Contains(term.Term);
                        if (containstStreetName)
                        {
                            TermQuery termQry = new TermQuery(new Term(address.PropertyName(x => x.StreetName), term.Term));
                            termQry.Boost = 100;
                            query.Add(termQry, Occur.SHOULD);
                        }
                        if (term.Length > 1 && FieldTerms[address.PropertyName(x => x.StreetName)].Where(s => s.StartsWith(term.Term)).Any())
                        {
                            PrefixQuery prefixQry = new PrefixQuery(new Term(address.PropertyName(x => x.StreetName), term.Term));
                            prefixQry.Boost = containstStreetName ? 50 : 100;
                            query.Add(prefixQry, Occur.SHOULD);
                        }
                    }
                }
                else if (term.IsNumeric)
                {
                    bool containstStreetNumber = FieldTerms[address.PropertyName(x => x.StreetNumber)].Contains(term.Term);
                    if (containstStreetNumber)
                    {
                        TermQuery termQry = new TermQuery(new Term(address.PropertyName(x => x.StreetNumber), term.Term));
                        termQry.Boost = 100;
                        query.Add(termQry, Occur.SHOULD);
                    }
                    bool containstUnitId = FieldTerms[address.PropertyName(x => x.UnitId)].Contains(term.Term);
                    if (containstUnitId)
                    {
                        TermQuery termQry = new TermQuery(new Term(address.PropertyName(x => x.UnitId), term.Term));
                        termQry.Boost = 10;
                        query.Add(termQry, Occur.SHOULD);
                    }
                }
            }
        }

        public IEnumerable<PostalAddressSearchResult> Read(ISearchQuery searchQuery, out int hits)
        {
            return Read(searchQuery, 25, out hits);
        }
        public IEnumerable<PostalAddressSearchResult> Read(ISearchQuery searchQuery, int top, out int hits)
        {
            var result = new List<PostalAddressSearchResult>();
            hits = 0;
            var postalAddressFormatter = new PostalAddressFormatter();

            using (var indexreader = IndexReader.Open(Directory, true))
            using (var indexsearch = new IndexSearcher(indexreader))
            {
                var query = new BooleanQuery();
                var terms = searchQuery.SearchTerms;

                AddAllTerms(query, terms);

                Trace.TraceInformation("Query: {0}", query);
                var topDocs = indexsearch.Search(query, top);
                hits = Math.Min(MaxResults, topDocs.TotalHits);
                for (var i = 0; i < topDocs.ScoreDocs.Length; i++)
                {
                    int docId = topDocs.ScoreDocs[i].Doc;
                    var doc = indexsearch.Doc(docId);

                    var docScore = topDocs.ScoreDocs[i];
                    var score = docScore.Score;

                    var item = new PostalAddressSearchResult(score)
                    {
                        Id = doc.Get(address.PropertyName(x => x.Id)),
                        AddressType = doc.Get(address.PropertyName(x => x.AddressType)),
                        BoxBagLobbyName = doc.Get(address.PropertyName(x => x.BoxBagLobbyName)),
                        BoxBagNumber = doc.Get(address.PropertyName(x => x.BoxBagNumber)),
                        BuildingName = doc.Get(address.PropertyName(x => x.BuildingName)),
                        DeliveryServiceType = doc.Get(address.PropertyName(x => x.DeliveryServiceType)),
                        Floor = doc.Get(address.PropertyName(x => x.Floor)),
                        PostCode = doc.Get(address.PropertyName(x => x.PostCode)),
                        RDNumber = doc.Get(address.PropertyName(x => x.RDNumber)),
                        StreetAlpha = doc.Get(address.PropertyName(x => x.StreetAlpha)),
                        StreetDirection = doc.Get(address.PropertyName(x => x.StreetDirection)),
                        StreetName = doc.Get(address.PropertyName(x => x.StreetName)),
                        StreetNumber = doc.Get(address.PropertyName(x => x.StreetNumber)),
                        StreetType = doc.Get(address.PropertyName(x => x.StreetType)),
                        SuburbName = doc.Get(address.PropertyName(x => x.SuburbName)),
                        TownCityMailTown = doc.Get(address.PropertyName(x => x.TownCityMailTown)),
                        UnitId = doc.Get(address.PropertyName(x => x.UnitId)),
                        UnitType = doc.Get(address.PropertyName(x => x.UnitType))
                    };

                    item.Format = postalAddressFormatter.Format(item);

                    if (GetExplanations)
                    {
                        //item.Explanations = indexsearch.Explain(query, docId).GetDetails();
                    }
                    result.Add(item);
                }
            }
            return result.Where(searchResult => !string.IsNullOrWhiteSpace(searchResult.Format.AddressOneLine));
        }

        public void Write(IEnumerable<PostalAddress> postalAddresses)
        {
            using (var writer = new IndexWriter(Directory, Analyzer, new IndexWriter.MaxFieldLength(1000000)))
            {
                foreach (PostalAddress postalAddresse in postalAddresses)
                {
                    writer.AddDocument(new PostalAddressConverterLuceneDocument(postalAddresse).Convert());
                }

                Trace.TraceInformation("Optimizing index...");
                writer.Optimize();
                writer.Commit();
            }
        }
    }
}
