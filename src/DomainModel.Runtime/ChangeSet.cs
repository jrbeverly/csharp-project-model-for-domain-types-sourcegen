namespace DomainModel.Runtime;

public struct ChangeSet
{
    private ulong _bits;

    public bool Any => _bits != 0;

    public bool IsSet(int index) => (_bits & (1UL << index)) != 0;

    public void Set(int index) => _bits |= 1UL << index;

    public void Clear() => _bits = 0;
}
