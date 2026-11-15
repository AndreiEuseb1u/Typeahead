# Design decisions

## Problem
Return the top-K most popular completions for a typed prefix, fast enough to beat the next keystroke. Reads vastly outnumber writes.

## Decisions
- **Trie with a per-node `Dictionary<char, TrieNode>`.** Lookup costs one step per character instead of scanning all queries (O(N)). The dictionary is allocated lazily, since most nodes are leaves.
- **Top-K precomputed per node.** Reading is just returning a ready list. Building them at read time would mean scanning a whole subtree per keystroke.
- **Bottom-up (post-order) build.** A node's list depends on its children's lists, so children are processed first. Correctness: an item missing from a child's top-K is already beaten by K better items there, so it can never belong in the parent's top-K.
- **Min-heap of size K when selecting.** The top of the heap is the weakest kept candidate, so each new candidate is compared in O(1) and replaced in O(log K).
- **Two phases: insert, then build.** Frequencies are only final after all inserts, so lists are computed once, afterwards.
- **Duplicate inserts add frequencies.**
- **Encapsulation:** `TrieNode` is `internal`; the dictionary is private behind `TryGetChild` / `GetOrAddChild` so the lazy allocation and no-overwrite rules live in one place. `Suggestion` is a public immutable record; lists are exposed as `IReadOnlyList` to prevent accidental mutation of shared data.

## Open decisions
- Should `Insert` throw on `null` (programmer error) instead of silently ignoring it?
- Case normalization (`Car` vs `car`).
- Popularity over time (recent searches weighted higher, e.g. exponential decay) and rebuild frequency.
- Freezing the trie after build (static factory vs builder).