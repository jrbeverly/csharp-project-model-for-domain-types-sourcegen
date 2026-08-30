# Problem

Modern application codebases frequently contain large amounts of repetitive domain model infrastructure surrounding relatively small core type definitions.

A single conceptual domain type often requires many related implementations and supporting systems, including:

- Mutable models
- Immutable models
- Read-only wrappers
- Builders
- Projection models
- Change-tracking systems
- Equality infrastructure
- Cloning infrastructure
- Indexing systems
- Reference lookup systems
- Supporting interfaces
- Serialization bindings
- Validation helpers

In practice, developers repeatedly hand-author or partially generate these structures even though most of the resulting code is mechanically derivable from higher-level domain semantics.

## Core Problem

The actual conceptual information required to define a domain type is often small relative to the volume of infrastructure required to support that type inside a mature application architecture.

For example:

- A concise domain definition may only require a small amount of semantic information.
- The resulting generated infrastructure may expand into thousands of lines of supporting code.

The problem is therefore:

How can rich domain models be defined declaratively outside the C# codebase and then expanded into comprehensive generated runtime infrastructure?

## External Domain Definitions

The system should support defining domain models externally from the runtime codebase.

Potential definition formats include:

- YAML
- JSONnet
- CueLang
- Other structured schema or configuration systems

The exact format is less important than the ability to:

- Represent rich metadata
- Compose definitions
- Minimize duplication
- Support validation
- Support projections
- Support large-scale code generation

## Requirements

The generated system should support:

- Mutable models
- Immutable models
- Read-only wrappers
- Builders
- Projection systems
- Change tracking
- Equality infrastructure
- Cloning infrastructure
- Reference lookup systems
- Indexing systems
- Interface generation
- Supporting metadata bindings

The generated infrastructure should be strongly typed and highly optimized.

## Change Tracking Concerns

One major area of interest is efficient generated change tracking.

The system should support concepts such as:

- Detecting whether any fields changed
- Detecting which fields changed
- Mapping field identifiers to metadata
- Efficient mutation tracking

The architecture should support highly optimized implementations generated automatically from the external schema definitions.

## Projection Concerns

The system should support projections of domain types.

Examples include:

- Lightweight projections
- Partial views
- Excluding nested hierarchies
- Specialized read models
- Reduced data representations

The goal is to maximize flexibility while minimizing duplication within the external schema definitions.

## Inverse Code Generation

The external schema format is not intended to merely restate C# type information in another syntax.

Instead, it exists specifically to support disproportionate code generation.

A relatively small declarative definition should expand into a very large amount of generated infrastructure.

## Success Criteria

The system succeeds if it can:

- Define domain models externally from the codebase
- Generate comprehensive strongly typed runtime infrastructure
- Minimize repetitive handwritten domain boilerplate
- Support optimized change tracking
- Support mutable and immutable representations
- Support projections and specialized views
- Generate modern C# implementations automatically
- Preserve ergonomic developer workflows despite large-scale generated infrastructure