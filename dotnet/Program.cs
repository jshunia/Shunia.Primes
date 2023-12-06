using System.Diagnostics;
using System.Numerics;

const bool ENABLE_LOG_ALL_RESULTS = false;
const bool ENABLE_FILE_TESTS = true;
const bool ENABLE_RUNTIME_TESTS = true;

Console.WriteLine("PRIMALITY TEST");
Console.WriteLine("By: Joseph M. Shunia, 2023");
Console.WriteLine();
var sw = Stopwatch.StartNew();
try
{
    if (ENABLE_FILE_TESTS)
    {
        // Test all base 2 Miller-Rabin pseudoprimes which are also Perrin pseudoprimes up to 2^64.
        Console.WriteLine($"[{sw.Elapsed}] Testing pseudoprimes ...");
        foreach (BigInteger n in ReadIntegersFromFile("../../../../perrin_mr2_pseudoprimes.txt"))
            RunPrimalityTest(n, false);

        // Test all primes up to 10^7.
        Console.WriteLine($"[{sw.Elapsed}] Testing primes...");
        foreach (BigInteger n in ReadIntegersFromFile("../../../../primes.txt"))
            RunPrimalityTest(n, true);
    }

    if (ENABLE_RUNTIME_TESTS)
    {
        // Test all odd n > 3 up to 2^32.
        Console.WriteLine($"[{sw.Elapsed}] Testing odd composites...");
        BigInteger limit = BigInteger.Pow(2, 32);
        for (BigInteger n = 5; n < limit; n += 2)
        {
            bool expected = IsPrimeExpected(n);
            RunPrimalityTest(n, expected);
        }
    }
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.ToString());
}
sw.Stop();
Console.WriteLine($"[{sw.Elapsed}] Done.");
Console.WriteLine($"Press any key to exit...");
Console.ReadKey();

void RunPrimalityTest(BigInteger n, bool isPrime)
{
    bool isPrimeShunia = IsPrimeShunia(n);
    if (ENABLE_LOG_ALL_RESULTS)
        Console.WriteLine($"[{sw.Elapsed}] N={n}, IsPrimeShunia: {isPrimeShunia}, Expected: {isPrime}");
    if (isPrimeShunia != isPrime)
        throw new InvalidOperationException($"Expected IsPrimeShunia to be '{isPrime}' but found '{isPrimeShunia}' for N={n}");
}

static bool IsPrimeShunia(BigInteger n)
{
    if (n % 2 == 0) return n == 2;
    if (n == 3) return true;

    BigInteger n1 = n - 1;
    BigInteger fermat = Pow(2, n1, n);
    if (fermat != 1)
        return false;

    BigInteger d = 2;
    BigInteger ilimit = BigInteger.Max(Log2(n), 3);
    for (BigInteger i = 3; i <= ilimit; i++)
    {
        d = i;
        BigInteger m1 = n1 % d;
        if (m1 != 0) break;
    }

    BigInteger v0 = Pow(2, n1 / d, n);
    BigInteger v1Expected = ModWrap(v0 + 1, n);
    BigInteger v0a = Pow(v0 + 1, n, n);
    if (v0a != v1Expected)
        return false;

    var q = new BigInteger[] { 2 };
    var a = new BigInteger[] { 1, 1 };
    BigInteger pd = d - 1;
    BigInteger[] p1 = PolyPow(a, n, n, pd, q);
    BigInteger v1 = ModWrap(PolyEval(p1, 1, n), n);
    if (v1 != v1Expected)
        return false;

    return true;
}

static BigInteger[] PolyPow(BigInteger[] a, BigInteger k, BigInteger m, BigInteger degree, BigInteger[] q)
{
    var b = new[] { BigInteger.One }; 
    while (k > 0)
    {
        if (k % 2 == 1)
        {
            b = PolyMul(a, b, m);
            b = PolyReduce(b, degree, q, m);
            if (k == 1) break;
        }
        a = PolyMul(a, a, m);
        a = PolyReduce(a, degree, q, m);
        k /= 2;
    }
    return b;
}

static BigInteger[] PolyMul(BigInteger[] a, BigInteger[] b, BigInteger m = default)
{
    // Naive polynomial multiplication (slow).
    long cLength = Math.Max(1, a.LongLength + b.LongLength - 1L);
    var c = new BigInteger[cLength];
    for (long i = 0; i < a.LongLength; i++)
    {
        if (a[i] == 0) continue;
        for (long j = 0; j < b.LongLength; j++)
        {
            long k = i + j;
            c[k] += a[i] * b[j];
            if (m != 0) c[k] %= m;
        }
    }
    int lastTermIndex = 0;
    for (int i = 0; i < c.Length; i++)
    {
        if (c[i] == 0) continue;
        lastTermIndex = i;
    }
    var ct = c;
    c = c.AsSpan().Slice(0, lastTermIndex + 1).ToArray();
    return c;
}

static BigInteger[] PolyReduce(BigInteger[] a, BigInteger degree, BigInteger[] q, BigInteger m = default)
{
    if (a.LongLength <= degree) return a;
    BigInteger[] b = PolyClone(a);
    long qDegree = (long)PolyDegree(q);
    var longDegree = (long)degree;
    long qOffset = longDegree - qDegree;
    for (long i = b.LongLength - 1; i > degree; i--)
    {
        if (b[i] == 0) continue;
        for (long j = i - 1 - qOffset, k = qDegree; j >= 0 && k >= 0; j--, k--)
        {
            b[j] += b[i] * q[k];
        }
        b[i] = 0;
    }
    var c = new BigInteger[(long)degree + 1L];
    for (long i = 0; i < c.LongLength && i < b.LongLength; i++)
    {
        c[i] = b[i];
        if (m != 0) c[i] %= m;
    }
    return c;
}

static BigInteger[] PolyClone(BigInteger[] p, BigInteger length = default)
{
    if (length == default) length = p.LongLength;
    var longLength = (long)length;
    var clone = new BigInteger[longLength];
    Array.Copy(p, clone, Math.Min(longLength, p.LongLength));
    return clone;
}

static BigInteger PolyDegree(BigInteger[] p)
{
    BigInteger degree = 0;
    for (long i = 0; i < p.LongLength; i++)
    {
        if (p[i] == 0) continue;
        degree = i;
    }
    return degree;
}

static BigInteger PolyEval(BigInteger[] p, BigInteger x, BigInteger m = default)
{
    if (p.LongLength == 0) return 0;
    BigInteger s = p[0];
    for (long i = 1; i < p.LongLength; i++)
    {
        if (p[i] == 0) continue;
        BigInteger v = i == 0 ? x * p[i] : Pow(x, i, m) * p[i];
        s += v;
    }
    if (m != 0) s %= m;
    return s;
}

static BigInteger Pow(BigInteger n, BigInteger exponent, BigInteger modulus = default)
    => modulus == 0 ? BigInteger.Pow(n, checked((int)exponent)) : BigInteger.ModPow(n, exponent, modulus);

static BigInteger Log2(BigInteger n) => BigInteger.Max(0, n.GetBitLength() - 1);

static BigInteger ModWrap(BigInteger a, BigInteger n)
{
    if (n == 0) throw new DivideByZeroException(nameof(n));
    if (BigInteger.Abs(a) > BigInteger.Abs(n)) a %= n;
    if (a < 0) a = (n - BigInteger.Abs(a)) % n;
    if (a == n) return 0;
    return a;
}

static IEnumerable<BigInteger> ReadIntegersFromFile(string filePath, BigInteger count = default)
{
    using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096);
    using var reader = new StreamReader(fs);
    BigInteger i = 0;
    while (!reader.EndOfStream)
    {
        if (count > 0 && i >= count) break;
        string? line = reader.ReadLine();
        if (string.IsNullOrWhiteSpace(line)) continue;
        yield return BigInteger.Parse(line);
        i++;
    }
}

static bool IsPrimeExpected(BigInteger n)
{
    if (n < 2) return false;
    if (n % 2 == 0) return n == 2;
    if (n % 3 == 0) return n == 3;
    if (n % 5 == 0) return n == 5;
    if (n % 7 == 0) return n == 7;
    if (n % 11 == 0) return n == 11;
    if (n % 13 == 0) return n == 13;
    if (n % 17 == 0) return n == 17;
    if (n % 19 == 0) return n == 19;

    // We perform 4 log(2,n) Miller-Rabin tests to check primality.
    // This is NOT deterministic, but it is highly unlikely for composite to pass.
    BigInteger log2 = n.GetBitLength() + 1;
    BigInteger limit = log2 * 4;
    for (BigInteger b = 2; b <= limit; b++)
    {
        if (!IsPrimeMr(n, b))
            return false;
    }

    return true;
}

static bool IsPrimeMr(BigInteger n, BigInteger b)
{
    if (n < 2) return false;
    BigInteger n1 = n - 1;

    BigInteger d = n1, s = 1;
    while (d > 0 && d % 2 == 0)
    {
        d /= 2;
        s++;
    }
    if (d == 0) return true;

    BigInteger mp = BigInteger.ModPow(b, BigInteger.Abs(d), n);
    if (mp == 1 || mp == n1) return true;
    for (BigInteger j = 0; j < s; j++)
    {
        mp = (mp * mp) % n;
        if (mp == 1) return false;
        if (mp == n1) break;
    }

    return mp == n1;
}
