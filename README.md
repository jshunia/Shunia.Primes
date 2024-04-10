# Shunia.Primes
This repo contains a sample implementation and attempted proof of a primality test that I discovered. Several versions of the preprint paper regarding my test are also included. The proofs presented are flawed.

## Notes:
The code in this repository is unoptimized. Particularly, it uses naive polynomial multiplication ($O(\log^2(n))$ time). With an efficient polynomial multiplication algorithm ($O(\log(n) \log \log(n))$ time) and other optimizations, the time complexity of this test is $\tilde{O}(log^3(n))$. Also, the Python implementation may not fully support arbitrary integer precision.

-Joseph M. Shunia, July 28 2023

Last Updated: April 10 2024
