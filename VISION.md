# Vision

Build a declarative domain modelling architecture where domain types are defined externally from the C# codebase and source generators synthesize the complete runtime infrastructure automatically.

The goal is to dramatically reduce handwritten domain boilerplate while generating rich, highly optimized, strongly typed application infrastructure.

## Desired Workflow

Developers should define domain semantics using external schema definitions such as:

- YAML
- JSONnet
- CueLang
- Other structured schema systems

Those definitions should contain:

- Type information
- Relationships
- Constraints
- Metadata
- Projection semantics
- Mutability intent
- Supporting behavioral semantics

The source generators should then synthesize the entire runtime model automatically.

## Generated Runtime Infrastructure

A single domain declaration should generate extensive supporting infrastructure, including:

- Mutable models
- Immutable models
- Read-only wrappers
- Builders
- Projection systems
- Equality systems
- Cloning systems
- Change tracking
- Indexing systems
- Reference lookup systems
- Supporting interfaces
- Metadata bindings

The resulting generated codebase may be dramatically larger than the original schema definitions.

## Semantic Code Generation

The external definitions are not intended to merely restate C# syntax.

Instead, they represent semantic intent that expands into large-scale runtime infrastructure.

The architecture should embrace disproportionate code generation where:

- Small schema definitions
- Produce massive generated support systems

Automatically and consistently.

## Projection-Oriented Modelling

The system should support flexible projections and specialized representations.

Examples include:

- Lightweight projections
- Excluding heavy child hierarchies
- Specialized read models
- Reduced transport structures
- Partial representations

Developers should be able to express these concepts declaratively without duplicating large portions of schema definitions.

## Optimized Change Tracking

The generated runtime infrastructure should support highly efficient mutation tracking.

Potential behaviors include:

- Instant “has anything changed” checks
- Efficient changed-field discovery
- Generated field metadata mappings
- Source-generated mutation infrastructure

The generated implementations should be aggressively optimized because all structure is known at generation time.

## Representation Flexibility

The same conceptual domain model should be automatically available in multiple forms, including:

- Mutable
- Immutable
- Read-only
- Builder-oriented
- Projection-oriented

The generated infrastructure should provide ergonomic transitions between these representations.

## Cascading Generation Ecosystem

The broader architecture may involve multiple cooperating source generators that synthesize additional capabilities from the canonical domain definitions.

Examples include:

- Equality systems
- Clone generation
- Projection generation
- Indexing systems
- Reference lookup systems
- Mutation tracking systems

The external schema definitions become the foundation for an ecosystem of generated infrastructure.

## Long-Term Direction

The long-term goal is a semantic domain modelling architecture where:

- Domain models are externally declared.
- Runtime infrastructure is synthesized automatically.
- Boilerplate becomes generated rather than handwritten.
- Rich supporting systems emerge mechanically from metadata and constraints.
- Developers focus primarily on semantic modelling rather than repetitive infrastructure implementation.

The resulting system should combine:

- Strong typing
- Modern C# design
- High-performance generated infrastructure
- Projection flexibility
- Mutation tracking
- Ergonomic APIs

Into a cohesive declarative code-generation ecosystem.