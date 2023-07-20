# Shunia.Primes
This repo contains a sample implementation and attempted proof of a primality test that I found a few years ago.

## Conjecture:
- Let `n` be an odd integer `> 3` and `D` be the least integer `> 2` which does not divide `n-1`.
- If `2^Floor((n-1)/D) + 1 = (2^Floor((n-1)/D) + 1)^(n) = sum(k=0, n, binomial(n,k) * 2^Floor((k-1)/D)) (mod n)` then `n` is prime.

## Proof:
Proof is included in this repo.

## Notes:
The code in this repository is not optimized. Particularly, it uses naive polynomial multiplication (`O(n^2)` time). With an efficient polynomial multiplication algorithm (`O(n log(n))` time) and other optimizations, the time complexity of this test should be roughly `O(n^3)`. Also, the Python implementation was translated by AI and may not fully support arbitrary integer precision.

-Joseph M. Shunia, July 20 2023