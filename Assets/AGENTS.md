# Project Rules

## Communication

- Communicate with me in Russian.
- Write all code summaries, headers, tooltips, and Inspector descriptions in Russian.
- After completing a task, briefly explain what changed, why, and how it was verified.
- Never claim that something was tested unless it was actually tested.

## General Development

- Study the relevant existing implementation before making changes.
- Follow the project's established architecture, coding style, and conventions.
- Prefer existing solutions over introducing new abstractions.
- Keep changes minimal and focused on the requested task.
- Do not perform unrelated refactoring.
- Do not add dependencies without approval.
- Ask for clarification when ambiguity could significantly affect the implementation or existing behavior.
- Preserve existing functionality unless changes are explicitly requested.
- Do not introduce unnecessary complexity or overengineering.

## Code Quality

- Write clean, readable, and maintainable code.
- Prefer simple and explicit solutions over clever or overly abstract ones.
- Avoid deeply nested conditionals; prefer guard clauses when appropriate.
- Avoid unnecessary type casting and redundant checks.
- Keep methods focused on a single clear responsibility.
- Avoid premature optimization, but consider performance in frequently executed code.
- Avoid unnecessary memory allocations in performance-critical paths.
- Add comments only when they explain non-obvious behavior or important design decisions.
- Do not create abstractions, interfaces, or base classes without a clear practical need.

## Unity Guidelines

- Do not modify scenes, prefabs, or .meta files unless necessary for the task.
- Preserve serialized data, Inspector references, and existing asset relationships.
- Do not rename or remove serialized fields without considering data migration.
- Preserve asset GUIDs and references when moving or renaming assets.
- Respect Unity component lifecycle and properly manage event subscriptions.
- Avoid expensive object searches and unnecessary allocations in Update, FixedUpdate, and other frequently executed methods.
- Follow the project's existing patterns for MonoBehaviours, ScriptableObjects, and component communication.
- When renaming prefabs, check related prefab variants, instances, and references. Preserve their connections.
- Do not assume that Unity APIs or packages are available without checking project compatibility.

## Verification

- Review changed code for obvious errors and unintended side effects.
- Run relevant tests or compilation checks when available.
- Clearly state what was verified and what could not be verified.
- Do not modify unrelated files to resolve unrelated warnings or issues.

# Naming Conventions

- Folders: PascalCase words separated by spaces, e.g. `Combat System`.
- ScriptableObject assets: lowercase snake_case, e.g. `enemy_skeleton`, `stats_enemy`.
- Materials: descriptive names with spaces, e.g. `Rock Dark`, `Rock (Biome Name)`.
- Prefabs: PascalCase with `_prefab` suffix, e.g. `EnemySkeleton_prefab`.
- Animator Controllers: `Controller_CharacterName`, e.g. `Controller_Humanoid`.
- Follow existing naming conventions for asset types not listed above.

# Third Party Assets

- do not modify or change or rename anything in \_ThirdParties folder
