
using System.Diagnostics.CodeAnalysis;

namespace Typeahead.Core
{
    internal sealed class TrieNode
    {
        private Dictionary<char, TrieNode>? _children;
        internal IEnumerable<KeyValuePair<char, TrieNode>> Children => _children ?? Enumerable.Empty<KeyValuePair<char, TrieNode>>();
        internal bool IsCompleteWord { get; set; }
        internal ulong Frequency { get; set; }
        internal IReadOnlyList<Suggestion> Suggestions { get; set; } = Array.Empty<Suggestion>();

        internal bool TryGetChild(char key, [NotNullWhen(true)] out TrieNode? child)
        {
            if (_children is null || _children.TryGetValue(key, out child) is false)
            {
                child = null;
                return false;
            }

            return true;
        }

        internal TrieNode GetOrAddChild(char key)
        {
            if (_children is null)
            {
                _children = new();
            }

            if (TryGetChild(key, out var result) is false)
            {
                var child = new TrieNode();
                _children.Add(key, child);
                return child;
            }

            return result;
        }
    }
}
