# Shunia.Primes
This repo contains a sample implementation and attempted proof of a primality test that I discovered. Several versions of the preprint paper regarding my test are also included.

## Abstract
A deterministic primality test with a polynomial time complexity of $\tilde{O}(\log^3(n))$ is presented. Central to our test is a novel polynomial reduction ring, applied to efficiently validate the polynomial congruence $(1 + x)^n \equiv 1 + x^n \pmod{n}$, a condition that holds exclusively for prime $n$. The test is grounded in our main theorem, supported by a series of lemmas, which collectively show how the congruence is verified through polynomial expansion within our reduction ring.

## Theorem:
Let $n$ be an odd integer $> 3$ satisfying $2^{n-1} \equiv 1 \pmod{n}$. Denote $D$ as the least integer greater than $2$ which does not divide $n-1$. If the following condition holds for all $0 \leq j < D$, then $n$ is prime:

\begin{align}
    \sum_{k=0}^{n} \binom{n}{Dk + j} 2^k
    \equiv
    \begin{cases} 
        1 & \text{if } j=0, \\
        0 & \text{if } 0 < j < D \text{ and } j \neq n \bmod{D}, \\
        2^{\floor{\frac{n}{D}}} & \text{if } j = n \bmod{D}
    \end{cases}
    \pmod{n}
\end{align}

## Notes:
The code in this repository is unoptimized. Particularly, it uses naive polynomial multiplication ($O(\log^2(n))$ time). With an efficient polynomial multiplication algorithm ($O(\log(n) \log \log(n))$ time) and other optimizations, the time complexity of this test is $\tilde{O}(log^3(n))$. Also, the Python implementation may not fully support arbitrary integer precision.

-Joseph M. Shunia, July 28 2023

Last Updated: December 19 2023
