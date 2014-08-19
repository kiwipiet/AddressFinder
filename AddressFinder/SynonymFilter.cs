using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AddressFinder
{
    public class SynonymFilter : TokenFilter
    {
        private readonly ITermAttribute _termAtt;
        private readonly ISynonymEngine _termService;
        private readonly IPositionIncrementAttribute _posAtt;
        private readonly Stack<string> _currentSynonyms;
        private State currentState;

        public SynonymFilter(TokenStream input, ISynonymEngine termService)
            : base(input)
        {
            _termAtt = AddAttribute<ITermAttribute>();
            _posAtt = AddAttribute<IPositionIncrementAttribute>();
            _currentSynonyms = new Stack<string>();
            _termService = termService;
        }
        public override bool IncrementToken()
        {
            if (_currentSynonyms.Count > 0)
            {
                string synonym = _currentSynonyms.Pop();
                RestoreState(currentState);
                _termAtt.SetTermBuffer(synonym);
                _posAtt.PositionIncrement = 0;
                return true;
            }
            if (!input.IncrementToken()) return false;
            char[] currentTerm = _termAtt.TermBuffer();
            int termLength = _termAtt.TermLength();
            if (currentTerm != null)
            {
                var synonyms = _termService.GetSynonyms(currentTerm, _termAtt.TermLength());
                if (synonyms == null) return true;
                foreach (var synonym in synonyms)
                {
                    if (synonym.Length == termLength)
                    {
                        if (currentTerm.Take(termLength).SequenceEqual(synonym, CharComparer.Comparer))
                        {
                            continue;
                        }
                    }
                    _currentSynonyms.Push(synonym.ToLower());
                }
            }
            currentState = CaptureState();
            return true;
        }
        private class CharComparer : IEqualityComparer<Char>
        {
            public static readonly CharComparer Comparer = new CharComparer();
            public bool Equals(Char x, Char y)
            {
                return Char.ToLower(x) == Char.ToLower(y);
            }

            public int GetHashCode(Char obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
