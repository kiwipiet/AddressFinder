using System.Diagnostics;

namespace AddressFinder
{
    [DebuggerDisplay("Term = {Term}, IsNumeric = {IsNumeric}")]
    public class SearchTerm
    {
        public string Term { get; set; }
        public bool IsNumeric { get; set; }
        public int Length { get; set; }
    }
}
