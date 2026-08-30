using DomainSchema.Generated;

namespace Sample.Domain;

public static class CustomerDemo
{
    public static CustomerProjection Run()
    {
        var customer = new CustomerBuilder()
            .WithFullName("Alice")
            .WithEmail("alice@example.com")
            .WithAge(30)
            .Build();

        var mut = customer.ToMutable();
        mut.Email = "newalice@example.com";
        mut.Age = 31;

        return mut.ToImmutable().ToProjection();
    }
}
