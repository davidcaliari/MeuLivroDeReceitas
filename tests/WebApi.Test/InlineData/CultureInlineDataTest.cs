using System.Collections;

namespace WebApi.Test.InlineData;

public class CultureInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "en-CA" };
        yield return new object[] { "pt" };
        yield return new object[] { "fr" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
