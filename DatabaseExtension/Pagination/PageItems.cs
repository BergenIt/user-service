using System.Collections.Generic;

namespace DatabaseExtension
{
    public interface IPageItems<T>
    {
        long CountItems { get; }
        IEnumerable<T> Items { get; }
    }

    public record PageItems<T>(IEnumerable<T> Items, long CountItems) : IPageItems<T>;
}
