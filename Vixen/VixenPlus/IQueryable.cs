namespace VixenPlus
{
    internal interface IQueryable
    {
        int Count { get; }
        string QueryInstance(int index);
    }
}