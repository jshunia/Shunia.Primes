# Shunia.Primes
This repo contains a sample implementation and attempted proof of a primality test that I discovered. Several versions of the preprint paper regarding my test are also included.

## Theorem:
Let $n$ be an odd integer $>3$ that satisfies $2^{n-1} \equiv 1 \pmod{n}$. Denote $D$ as the smallest integer strictly greater than $2$ which does not divide $n-1$. If $n$ does not have a prime factor $\leq D+1$ and if it holds that $2^{\left\lfloor \frac{n-1}{D} \right\rfloor} + 1 \equiv (2^{\left\lfloor \frac{n-1}{D} \right\rfloor} + 1)^{n} \equiv \sum_{k=0}^{n} \binom{n}{k}2^{\left\lfloor \frac{k}{D} \right\rfloor} \pmod{n}$, then $n$ is prime.

## Notes:
The code in this repository is unoptimized. Particularly, it uses naive polynomial multiplication ($O(\log^2(n))$ time). With an efficient polynomial multiplication algorithm ($O(\log(n) \log \log(n))$ time) and other optimizations, the time complexity of this test is $\tilde{O}(log^3(n))$. Also, the Python implementation may not fully support arbitrary integer precision.

-Joseph M. Shunia, July 28 2023

Last Updated: December 10 2023
