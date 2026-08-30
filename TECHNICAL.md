# Technical

## Language and Platform

The implementation should be built in C#.

The architecture should heavily leverage C# source generators and modern C# language features.

The generated output should prioritize modern patterns such as:

- Sealed records
- Strong typing
- Immutable-friendly APIs
- Source-generated infrastructure

## External Schema Definitions

Domain types should be defined externally from the runtime codebase.

Potential schema formats include:

- YAML
- JSONnet
- CueLang
- Other structured schema systems

The architecture should support composed schema definitions rather than monolithic single-file specifications.

## Schema System Goals

The external schema definitions should support:

- Rich metadata
- Validation
- Constraints
- Composition
- Projection semantics
- Supporting infrastructure metadata
- Mutability semantics
- Relationship definitions

The schema system should minimize duplication and avoid repeatedly restating identical definitions.

## Source Generator Responsibilities

The source generators should synthesize:

- Domain types
- Interfaces
- Read-only wrappers
- Immutable variants
- Mutable variants
- Builders
- Projection models
- Change-tracking infrastructure
- Equality infrastructure
- Cloning infrastructure
- Reference lookup systems
- Indexing structures
- Supporting metadata bindings

The generated output may be substantially larger than the original schema definition.

## Inverse / Disproportionate Code Generation

The external definitions are specifically intended for disproportionate code generation.

Example expectation:

- Small declarative schema definitions expand into thousands of lines of generated C# infrastructure.

The external schema should represent semantic intent rather than directly mirroring handwritten C# syntax.

## Change Tracking Architecture

The generated models should support highly optimized change tracking.

Potential implementation concepts include:

- Bit-vector-based field tracking
- Source-generated field identifiers
- Generated field metadata maps
- Efficient “has anything changed” checks
- Efficient “which fields changed” queries

Example behaviors:

- “Has anything changed?” → Check whether a generated bit vector is zero.
- “Which fields changed?” → Use generated mappings from field identifiers to metadata.

## Controlled Mutation

The generated mutable models should support controlled mutation semantics.

Potential mechanisms include:

- Property wrappers
- Generated mutation accessors
- Change-aware setters
- Mutation interception

The system should support automatic mutation tracking during field updates.

## Representation Variants

The generated system should support multiple representations of the same conceptual domain type, including:

- Mutable models
- Immutable models
- Read-only models
- Builder systems
- Projection models
- Lightweight variants

The generator should synthesize conversion paths between representations where appropriate.

## Projection System

The architecture should support generated projections.

Examples:

- Excluding heavy child hierarchies
- Partial representations
- Specialized read models
- Lightweight transport projections

The projection system should avoid excessive duplication in the external schema definitions.

## Cascading Source Generation

The architecture may require multiple cooperating or cascading source generators.

Potential generated capabilities include:

- Clonability
- Equality
- Reference lookup systems
- Indexing systems
- Projection infrastructure
- Builders
- Immutable wrappers
- Read-only wrappers

The generation pipeline may need explicit integration points between generators.

## Architectural Direction

The architecture should treat the external schema definitions as canonical semantic models from which extensive runtime infrastructure is synthesized automatically.

The system should prioritize:

- Strong typing
- High-performance generated infrastructure
- Minimal handwritten boilerplate
- Semantic-driven generation
- Modern C# design
- Projection flexibility
- Efficient mutation tracking
- Large-scale generated support systems

Rather than manually authored repetitive domain infrastructure.