import time
import math
import sys
import os
import gmpy2
from gmpy2 import mpz

ENABLE_LOG_ALL_RESULTS = True

def is_prime_shunia(n):
    if n % 2 == 0: return n == 2
    if n == 3: return True
    
    n1 = n - 1
    d = 2
    ilimit = max(int(gmpy2.log2(n)), 3)
    for i in range(3, ilimit+1):
        d = i
        if is_prime_expected(d) == False: continue
        m1 = n1 % d
        if m1 != 0: break

    ndm = n % d
    fermat = pow(2, n1, n)
    if fermat != 1:
        return False
        
    v0 = pow(2, n1 // d, n)
    q = [2]
    a = [1, 1]
    pd = d - 1
    p1 = poly_pow(a, n, n, pd, q)
    if p1[0] != 1:
        return False
        
    for i in range(1, len(p1), 1):
        if (i == ndm):
            if (p1[i] != v0):
                return False
        else:
            if (p1[i] != 0):
                return False

    return True
import math

def is_prime_expected(n):
    if n < 2: return False
    if n % 2 == 0: return n == 2
    if n % 3 == 0: return n == 3
    if n % 5 == 0: return n == 5
    if n % 7 == 0: return n == 7
    if n % 11 == 0: return n == 11
    if n % 13 == 0: return n == 13
    if n % 17 == 0: return n == 17
    if n % 19 == 0: return n == 19

    # We perform 2 log(2,n) Miller-Rabin tests to check primality.
    # This is NOT deterministic, but it is highly unlikely for composite to pass.
    log2 = n.bit_length() + 1
    limit = log2 * 2
    if limit > (n // 2):
        limit = log2

    for b in range(2, int(limit) + 1):
        if not is_prime_mr(n, b):
            return False

    return True

def is_prime_mr(n, b):
    if n < 2:
        return False

    n1 = n - 1
    d, s = n1, 1
    while d > 0 and d % 2 == 0:
        d //= 2
        s += 1

    if d == 0:
        return True

    mp = pow(b, abs(d), n)
    if mp == 1 or mp == n1:
        return True

    for j in range(s):
        mp = (mp * mp) % n
        if mp == 1:
            return False
        if mp == n1:
            break
    return mp == n1
    
def poly_pow(a, k, m, degree, q):
    b = [1]
    while k > 0:
        if k % 2 == 1:
            b = poly_mul(a, b, m)
            b = poly_reduce(b, degree, q, m)
            if k == 1: break
        a = poly_mul(a, a, m)
        a = poly_reduce(a, degree, q, m)
        k //= 2
    return b

def poly_mul(a, b, m=0):
    c_length = max(1, len(a) + len(b) - 1)
    c = [0]*c_length
    for i in range(len(a)):
        if a[i] == 0: continue
        for j in range(len(b)):
            k = i + j
            c[k] += a[i] * b[j]
            if m != 0: c[k] %= m
    last_term_index = 0
    for i in range(len(c)):
        if c[i] == 0: continue
        last_term_index = i
    c = c[:last_term_index + 1]
    return c

def poly_reduce(a, degree, q, m=0):
    if len(a) <= degree: return a
    b = a.copy()
    q_degree = poly_degree(q)
    q_offset = degree - q_degree
    for i in range(len(b) - 1, degree, -1):
        if b[i] == 0: continue
        for j, k in zip(range(i - 1 - q_offset, -1, -1), range(q_degree, -1, -1)):
            b[j] += b[i] * q[k]
        b[i] = 0
    c = [0]*(degree + 1)
    for i in range(len(c)):
        if i < len(b):
            c[i] = b[i]
            if m != 0: c[i] %= m
    return c

def poly_degree(p):
    degree = 0
    for i in range(len(p)):
        if p[i] == 0: continue
        degree = i
    return degree

def poly_eval(p, x, m=0):
    if len(p) == 0: return 0
    s = p[0]
    for i in range(1, len(p)):
        if p[i] == 0: continue
        v = x * p[i] if i == 0 else pow(x, i, m) * p[i]
        s += v
    if m != 0: s %= m
    return s

def mod_wrap(a, n):
    if n == 0: raise ZeroDivisionError('n')
    a = a % n if abs(a) > abs(n) else a
    if a < 0: a = (n - abs(a)) % n
    if a == n: return 0
    return a

def read_integers_from_file(file_path, count=0):
    with open(file_path) as f:
        for i, line in enumerate(f):
            if count > 0 and i >= count: break
            if line.strip():
                yield int(line.strip())

def run_primality_test(n, is_prime):
    isprimeshunia = is_prime_shunia(n)
    if ENABLE_LOG_ALL_RESULTS:
        print(f"N={n}, IsPrimeShunia: {isprimeshunia}, Expected: {is_prime}")
    if isprimeshunia != is_prime:
        raise Exception(f"Expected IsPrimeShunia to be '{is_prime}' but found '{isprimeshunia}' for N={n}")

print("PRIMALITY TEST")
print("By: Joseph M. Shunia, 2023")
print()
start_time = time.time()
try:
    print("Testing pseudoprimes...")
    for n in read_integers_from_file("../perrin_mr2_pseudoprimes.txt"):
        run_primality_test(n, False)
        
    print("Testing primes...")
    for n in read_integers_from_file("../primes.txt"):
        run_primality_test(n, True)
except Exception as ex:
    print(ex)

end_time = time.time()
print("Done.")
print(f"Total time: {end_time - start_time} seconds")