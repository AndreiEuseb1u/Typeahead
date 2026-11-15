using System;
using System.Collections.Generic;
using System.Text;

namespace Typeahead.Core
{
    public sealed class Trie
    {
        private TrieNode _root;

        private Trie()
        {
            _root = new TrieNode();
        }

        public void Insert(string query, ulong frequency)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return;
            }

            TrieNode currentNode = _root;

            foreach (var character in query)
            {
                currentNode = currentNode.GetOrAddChild(character);
            }

            currentNode.IsCompleteWord = true;
            currentNode.Frequency += frequency;
        }

        public static Trie BuildTrie() => new Trie();
    }
}
