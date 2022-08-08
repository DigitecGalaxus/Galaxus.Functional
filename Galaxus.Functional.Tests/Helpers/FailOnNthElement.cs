using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Helpers;

internal class YieldElementsThenFail<T> : IEnumerable<T>
{
    private readonly T _elementToYield;
    private readonly int _numberOfElements;

    public YieldElementsThenFail(T elementToYield, int numberOfElements)
    {
        _elementToYield = elementToYield;
        _numberOfElements = numberOfElements;
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var _ in Enumerable.Range(0, _numberOfElements))
        {
            yield return _elementToYield;
        }

        Assert.Fail("Sequence was unexpectedly enumerated.");
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
