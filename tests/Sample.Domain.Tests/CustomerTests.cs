using System;
using DomainSchema.Generated;

namespace Sample.Domain.Tests;

public sealed class CustomerTests
{
    [Fact]
    public void FreshMutable_HasNoChanges()
    {
        var mut = new CustomerMutable();

        Assert.False(mut.HasChanges);
        Assert.Empty(mut.ChangedFields);
    }

    [Fact]
    public void MutatingTwoFields_ReportsExactlyThoseFields()
    {
        var mut = new CustomerMutable();
        mut.FullName = "Alice";
        mut.Email = "alice@example.com";

        Assert.True(mut.HasChanges);
        Assert.Equal(new[] { "FullName", "Email" }, mut.ChangedFields);
    }

    [Fact]
    public void Projection_OmitsAddressAndRetainsKeptFields()
    {
        var customer = new CustomerBuilder().WithFullName("Alice").Build();
        var projection = customer.ToProjection();

        Assert.Equal("Alice", projection.FullName);
        Assert.Null(typeof(CustomerProjection).GetProperty("Address"));
        Assert.Null(typeof(CustomerProjection).GetProperty("Notes"));
    }

    [Fact]
    public void Builder_ThrowsOnConstraintViolation()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new CustomerBuilder().WithFullName("Alice").WithAge(-1).Build());
    }

    [Fact]
    public void RoundTrip_ImmutableToMutableToImmutable_PreservesData()
    {
        var original = new CustomerBuilder()
            .WithFullName("Alice")
            .WithEmail("alice@example.com")
            .WithAge(30)
            .Build();

        var restored = original.ToMutable().ToImmutable();

        Assert.Equal(original, restored);
    }
}
