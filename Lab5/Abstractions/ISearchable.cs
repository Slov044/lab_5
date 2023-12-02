using Lab5.Entities;

public interface ISearchable
{
    IEnumerable<Product> Find(Func<Product, bool> predicate);
}