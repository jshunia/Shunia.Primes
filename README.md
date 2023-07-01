# Shunia.Primes
This repo contains a sample implementation of a primality test that I found a few years ago. I'll add a paper to explain when I have time.

The general idea is that we are testing whether the congruence `1 + a(n) = (1 + a(n))^n mod n` holds under specific circumstances, where `a(n)` is a constant-recursive sequence (e.g. Fibonacci). However, instead of the Fibonacci sequence, we use the integer roots of `2^n`. The trick is to calculate the binomial transform quickly. I found a way to calculate binomial (and multinomial) transforms for any constant-recursive sequence in polynomial time, which led me to this.

## Conjecture:
- Let `n` be an odd integer > 3 such that `2^(n-1) = 1 mod n`.
- Let `D` be the least integer which does not divide `n - 1`.
- If `2^Floor(n/D) + 1 = sum(k=0, n, binomial(n,k) * 2^Floor(k/D)) (mod n)` then either `n` is prime or `GCD((2^Floor(n/D) mod n) - 1, n)` is a non-trivial factor of `n`.

## Notes:
The code in this repository is not optimized. Particularly, it uses naive polynomial multiplication (`O(n^2)` time). With an efficient polynomial multiplication algorithm (`O(n log(n)` time) and other optimizations, the time complexity of this test should be roughly `O(n^3)`. Also, the Python implementation was translated by AI and may not fully support arbitrary integer precision.

I do not have a proof to provide for now, but I have verified my algorithm is accurate for all integers up to 2^64. The dataset I used is too large to include here, so I've included a much smaller dataset which contains all Miller-Rabin base 2 pseudoprimes which are also Perrin pseudoprimes up to 2^64.

-Joseph M. Shunia, June 30 2023
