namespace DomainModel.Runtime;

public interface IChangeTracking
{
    bool HasChanges { get; }
    IReadOnlyList<string> ChangedFields { get; }
    void AcceptChanges();
}
