# Mock Design

## Executive Summary

The proposed system is conceptually viable.

The core idea - defining domain semantics outside the C# codebase and expanding them into a much larger generated runtime surface - is realistic, and it matches what source generation is good at when the generation target is strongly typed, repetitive, and semantically derivable.

The biggest challenges are not "can C# generate this?" but:

- choosing the right schema authoring model
- defining a stable canonical semantic model between schema and code generation
- deciding how much work belongs in source generators versus pre-build tooling
- keeping projection and representation growth under control
- defining change-tracking semantics clearly enough to generate them consistently

The design is most likely to succeed if it is treated as a small compiler pipeline rather than a single monolithic source generator.

## Design Intent

This repository appears to be aiming for a semantic domain modeling system with these characteristics:

- external domain definitions are the primary authored artifact
- C# types are generated outputs, not the canonical source of truth
- the schema captures intent, not a one-to-one restatement of C# syntax
- one conceptual domain type may expand into many runtime representations
- the generated output should be ergonomic, strongly typed, and performance-aware

That overall direction is coherent.

## Recommended Architectural Shape

### 1. Schema Authoring Layer

Human-authored domain definitions should live outside the runtime C# types, but still inside the repository and normal developer workflow.

This layer should express:

- conceptual types and members
- relationships and identities
- mutability intent
- metadata and constraints
- projection intent
- infrastructure hints such as indexing or lookup requirements

This layer should not try to encode every implementation detail of the generated C# output. If the schema becomes a second handwritten programming language for C#, the design loses much of its value.

### 2. Schema Front-End / Semantic Compiler

The system should have a dedicated front-end that parses, composes, validates, and normalizes the external definitions into a canonical semantic model.

This is an important architectural boundary because it:

- isolates schema syntax from code generation
- allows the schema format to change without rewriting the generation strategy
- gives one place for diagnostics, validation, and composition rules
- prevents multiple generators from reparsing raw schema independently
- creates a stable contract for future tooling

This front-end is the difference between "a generator that reads files" and "a domain modeling system."

### 3. Canonical Semantic Model

The canonical semantic model should be the central abstraction of the system.

It should represent the meaning of the domain, independent of:

- the chosen input syntax
- the exact generated class shapes
- the number of runtime representations emitted

At a high level, this model would likely carry concepts such as:

- type identity
- field and relationship semantics
- representation flags or capabilities
- projection declarations
- validation and metadata annotations
- change-tracking intent
- indexing and lookup intent

The canonical model should be rich enough to drive many generators, but stable enough that individual generators do not need to understand raw YAML, JSONnet, or CUE semantics directly.

### 4. Generation Planning Layer

Between the semantic model and emitted C# there should be a planning step that decides which artifacts are produced for each conceptual type.

Examples of generated artifacts include:

- mutable representations
- immutable representations
- read-only wrappers or views
- builders
- projection models
- interfaces
- metadata bindings
- equality and cloning helpers
- indexing and reference lookup helpers
- change-tracking infrastructure

This planning layer matters because not every domain type should necessarily get every possible artifact. Without a planning boundary, the system risks turning every type into an explosion of generated surface area whether it is needed or not.

### 5. Code Generation Back-End

The C# generation back-end should focus on translating the planned semantic artifacts into source code that fits naturally into the build and IDE workflow.

A hybrid architecture is the most practical direction:

1. external schema definitions are resolved and normalized
2. a canonical semantic model or equivalent intermediate representation is produced
3. one or more C# generators consume that model and emit runtime code

This is preferable to pushing all responsibilities directly into a single source generator, especially if the schema language needs rich composition, reusable imports, or non-trivial validation.

### 6. Runtime Support Layer

Not every capability should be fully re-generated from scratch for every type.

The system will likely need a small runtime support library for reusable concepts such as:

- base interfaces and marker contracts
- metadata containers
- shared change-tracking primitives
- conversion helper abstractions
- projection support contracts
- lookup or indexing support primitives

Generated code should specialize type-specific logic and hot paths, while reusable mechanics live in a shared runtime. That balance helps control code size, build time, and maintenance cost.

### 7. Extension Layer

The generated system needs a deliberate way for developers to add custom behavior without forking generated code.

Possible extension points conceptually include:

- partial types
- partial methods
- companion handwritten services
- extension methods
- validation or lifecycle hooks

Without this boundary, the system may reduce boilerplate while still making the resulting models hard to adapt in real applications.

## Build and Tooling Direction

The architecture should not assume that "source generator" and "entire pipeline" are the same thing.

There are two viable tool shapes:

- Pure source-generator-centric pipeline. This is viable if the external schema format remains simple, .NET-friendly, and easy to parse incrementally.
- Hybrid pipeline. This is likely better if the schema needs composition-heavy authoring, richer validation, or a language such as JSONnet or CUE.

The hybrid model is the stronger default recommendation because it separates concerns more cleanly and reduces pressure on the Roslyn integration layer.

One particularly important consequence: if multiple generators participate, they should fan out from the same canonical semantic model or normalized input. They should not depend on each other's generated C# output inside the same compilation.

That means the "cascading ecosystem" described in the vision is viable, but it should be implemented as coordinated generation from a shared semantic source, not as generator A feeding generator B through generated code.

## High-Level Workflow

The intended workflow can be made coherent with the following shape:

1. Developers author or update external domain definitions.
2. The schema front-end composes and validates those definitions.
3. The system produces a canonical semantic model.
4. Generation planning decides which runtime artifacts exist for each conceptual type.
5. C# generators emit strongly typed runtime infrastructure.
6. Application code consumes the generated models plus any shared runtime library.
7. Diagnostics map back to the external definitions rather than only to generated C#.

This workflow fits the repository's goals well and preserves the idea that developers focus on semantic modeling instead of repetitive implementation.

## Conceptual Viability Assessment

### What looks viable

- External domain definitions as the canonical input are viable.
- Strongly typed C# generation for multiple representations is viable.
- Generated change-tracking infrastructure is viable if semantics are explicit.
- Projection-oriented generation is viable if projections are declared and controlled.
- Equality, cloning, builders, wrappers, and interfaces are natural generation targets.

### What needs architectural discipline

- Rich schema composition across many files and modules.
- Keeping the generated API surface understandable.
- Preventing projections and representation variants from multiplying uncontrollably.
- Maintaining build and IDE performance as generated output grows.
- Preserving readable diagnostics and traceability back to authored schema.

### What is not yet sufficiently defined

- the exact identity model behind indexing and reference lookups
- the boundary between static schema validation and runtime/business validation
- the mutation semantics for nested object graphs and collections
- the degree of runtime-library reuse versus per-type bespoke generation
- the preferred authoring format and what tradeoff it is meant to optimize

## Major Risks and Ambiguities

### 1. Schema Language Choice Is Still Open

YAML, JSONnet, and CUE are not interchangeable choices.

- YAML is approachable but weak for composition and semantic validation unless heavily constrained.
- JSONnet is strong for templating and composition but adds a more programming-like authoring model.
- CUE is strong for constraints and schema logic but introduces a less common toolchain for many C# teams.

This is currently one of the biggest unresolved decisions because it strongly affects build tooling, editor experience, validation strategy, and long-term maintainability.

### 2. Source Generator Boundaries Need To Be Respected

Source generators are a good output mechanism, but they are not automatically the best home for every part of the pipeline.

If the system tries to do parsing, composition, validation, planning, and all code emission inside one generator assembly, complexity will accumulate in the least flexible part of the toolchain.

### 3. Change Tracking Semantics Are Under-Specified

The documents clearly want optimized change tracking, but the required semantics are still open.

Important unanswered questions include:

- Is tracking per-object only, or graph-aware?
- Do nested mutations bubble upward automatically?
- How are collection changes represented?
- Is "changed field" a structural concept, a semantic concept, or both?

This area is feasible, but it needs an explicit scope boundary early.

### 4. Projection Scope Can Become Combinatorial

Projection support is a compelling feature, but it can easily become a combinatorial expansion problem.

If every type can project into many partial and nested shapes without a strong planning model, code size and conceptual complexity may grow faster than the value returned.

### 5. Representation Consistency Will Be Hard To Maintain Implicitly

Mutable, immutable, read-only, builder, and projection forms are all reasonable, but the rules for converting between them need to be coherent.

If those rules are only implicit, the generated system may become surprising even when technically correct.

### 6. Indexing And Reference Lookup Need A Clear Domain Story

These features suggest the system wants more than just DTO generation. They imply stable identity, navigable relationships, and possibly domain-graph semantics.

That is viable, but the design needs a clear answer to whether these are:

- purely in-memory navigation helpers
- application-layer query helpers
- persistence-adjacent constructs

The correct architecture differs depending on that answer.

### 7. Developer Ergonomics Could Fail Even If Generation Works

The technical generation may succeed while the developer experience fails due to:

- opaque diagnostics
- hard-to-navigate generated code
- slow design-time builds
- confusing public APIs
- unclear customization points

Ergonomics should be treated as a first-class design constraint, not a polishing phase.

## Recommended Scope Boundaries For An Initial Version

A realistic first version should prove the architecture before attempting the full ecosystem.

Recommended initial boundaries:

- support one schema format first, even if the front-end remains replaceable
- support a small but representative domain graph
- generate mutable and immutable forms first
- include metadata and direct field-level change tracking
- include one explicit projection mechanism
- defer graph-wide change propagation, advanced indexing, and broad lookup systems until identity semantics are clearer

This still validates the core premise without forcing every hard problem to be solved at once.

## Proposed Repository Shape

One reasonable high-level repository layout would be:

- `schema/` for authored domain definitions
- `src/DomainSchema/` for parsing, validation, and canonical semantic modeling
- `src/DomainGeneration/` for planning and C# emission
- `src/DomainRuntime/` for shared runtime abstractions
- `src/DomainBuild/` for MSBuild or CLI integration if needed
- `tests/` split across schema, generator, and integration coverage
- `samples/` for one or two reference modeled domains
- `docs/` for design notes, decisions, and authoring guidance

The important point is not the exact folder names. It is the separation of front-end, semantic model, generation logic, runtime support, and tooling.

## Clarifying Questions

These questions are important enough to affect architecture, but not so blocking that this mock design cannot proceed without answers:

1. Is the external schema expected to live only inside the same repository and build, or must it support cross-package or cross-repository composition?
2. Is live IDE-time generation a hard requirement, or would a pre-build compile step be acceptable if it enables a richer schema system?
3. Which matters most in the schema format choice: familiarity, composition power, or validation strength?
4. Should change tracking initially stop at direct field mutation, or is nested graph awareness part of the minimum viable design?
5. Are projections always explicitly declared, or should the system infer and generate them by convention?
6. Are indexing and reference lookup intended mainly for in-memory object graph support, or do they need to align with storage/query concerns?
7. Will generated types be the public application-facing model, or mostly infrastructure underneath hand-authored domain logic?

## Overall Recommendation

The proposal should move forward, but with one key reframing:

Treat the project as a semantic modeling pipeline with C# generation back-ends, not as "just a source generator."

That framing makes the current goals much more coherent:

- it supports disproportionate code generation
- it fits multiple runtime representations
- it leaves room for a richer schema language
- it makes a multi-generator ecosystem possible without fragile generator chaining
- it creates a practical path to optimized change tracking and projection support

In short: the system is implementable, the components can fit together coherently, and the biggest risks are architectural clarity and scope discipline rather than impossibility.
