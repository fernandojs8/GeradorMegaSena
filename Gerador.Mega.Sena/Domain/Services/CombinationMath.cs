namespace Gerador.Mega.Sena.Domain.Services;

/// <summary>
/// Provides bounded combinatorics operations used to validate request feasibility.
/// </summary>
internal static class CombinationMath
{
    public static long BoundedCombination(int n, int k, int limit)
    {
        if (k < 0 || n < 0 || k > n)
        {
            return 0;
        }

        if (k == 0 || k == n)
        {
            return 1;
        }

        if (k > n - k)
        {
            k = n - k;
        }

        decimal result = 1m;
        decimal decimalLimit = limit;

        for (int i = 1; i <= k; i++)
        {
            result *= (n - (k - i));
            result /= i;

            if (result >= decimalLimit)
            {
                return limit;
            }
        }

        return (long)result;
    }
}
