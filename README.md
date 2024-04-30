# Shunia.Primes
This repo contains a sample implementation and attempted proof of a primality test that I discovered. Several versions of the preprint paper regarding my test are also included. The proofs presented are flawed.

## Notes:
The code in this repository is unoptimized. Particularly, it uses naive polynomial multiplication ($O(\log^2 n)$ time). With an efficient polynomial multiplication algorithm (e.g. $O(\log n \log \log n)$ time) and other optimizations, the time complexity of this test is $\tilde{O}(log^3 n)$. Also, the Python implementation may not fully support arbitrary integer precision.

## New Conjecture:

Here is a new conjecture, which I first issued on April 12, 2024:

Let $n \in \mathbb{Z}_{\text{odd}}^+$. Then, $(x^4 + x)^n - x^{4n} - x^n = 0 \in (\mathbb{Z}/n\mathbb{Z})[x]/(x^8 - x^2 + 2)$ iff $n$ is prime. -Joseph M. Shunia, April 12 2024

If the above conjecture is true, then it represents a $\tilde{O}(\log^2 n)$ primality test. Using Fermat base 2 pseudoprime tables, I have experimentally verified that there are no pseudoprimes for this conjectured test up to $2^{64}$. I have also verified that no Carmichael numbers up to $10^{21} \approx 2^{70}$ pass the proposed test. However, theoretically, I have not ruled out the existence of a composite integer which passes. 

**Last Updated**: April 30, 2024
