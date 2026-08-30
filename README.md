# Domain Types Source Generator

Reads YAML domain schemas and generates immutable records, builders, mutable change-tracking types, and projections.

```yaml
type: Customer
fields:
  - name: Id
    type: guid
  - name: FullName
    type: string
    constraint:
      non_empty: true
  - name: Email
    type: string
  - name: Age
    type: int
    constraint:
      range: { min: 0, max: 150 }
  - name: Notes
    type: string
    exclude_from_projection: true
relationships:
  - name: Address
    target: Address
    cardinality: one
```

Generated `CustomerBuilder` (see `evidence/`):

```csharp
public CustomerImmutable Build()
{
    if (string.IsNullOrEmpty(_fullName)) throw new ArgumentException("FullName must not be empty.", "FullName");
    if (_age < 0 || _age > 150) throw new ArgumentOutOfRangeException("Age", "Age must be between 0 and 150.");
    return new CustomerImmutable(_id, _fullName, _email, _age, _notes, _address);
}
```

```sh
make build
make test
```

## Notes

- Schema files enter the incremental generator through `AdditionalFiles`.
- A small line-oriented parser produces a semantic model before independent source emission.
- Builder `Build()` methods enforce constraints while immutable records remain data containers.
- Stable schema field order assigns relationship and ordinary fields to a compact change-tracking bit vector.
