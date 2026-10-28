# Project Rules

## Communication

- After the user approves Kickstarter images, move them to Google Drive under `Кавказский эпос/kickstarter campaign`. Verify that the upload succeeded before deleting temporary local copies of these images. Do not delete original Unity game assets.

- Maintain useful spreadsheets, estimates, calculations, collections, and other project organizational materials on Google Drive in the appropriate project folder. Do not create or maintain local copies unless the user explicitly requests them. Store funding and Kickstarter materials under `Кавказский эпос/kickstarter campaign`.

- Write all instructions in this AGENTS.md in English. Preserve exact folder names, paths, and quoted user phrases in their original language.
- Communicate with me in Russian.
- Write all commit messages in English.
- Write all code summaries, headers, tooltips, and Inspector descriptions in Russian.
- After completing a task, briefly explain what changed, why, and how it was verified.
- Never claim that something was tested unless it was actually tested.
- When the user asks to adopt a rule (for example, "возьми за правило" or "возьми за правильно"), record the requested rule in this AGENTS.md as part of the request.

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
- No hardcoded values.

## Unity Guidelines

- Each character must have a total ragdoll mass of 100, calculated as the sum of all its Rigidbody masses, not the mass of each bone.

- Shared construction assets must be material-independent. Keep reusable geometry, colliders, tags, layers, LOD configuration, and other common physical/gameplay settings in base prefabs under `Assets/Art/Shared`.
- Create biome-specific Prefab Variants from these Shared base prefabs. Biome variants should only override material assignments (and their identifying names), inheriting geometry, colliders, tags, layers, and other common settings from the base.
- Do not use an imported FBX directly as the base of a biome construction variant; create or reuse a Shared base prefab first. Shared base renderers must retain the required material slots with null material references, and Shared model importers must not remap materials to biome assets.
- When correcting existing prefab inheritance, preserve asset GUIDs, component/object references, scene placement, existing collider shapes, and LOD behavior. Verify all dependent variants and scene instances before removing duplicate overrides.

- Shader property display names and reference names must be in English. This is an exception to the Russian Inspector descriptions rule.

- For new spells, enable a visible trace in the magic prefab using the existing trace controls in Projectile.

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

- do not modify or change or rename anything in `Assets/_ThirdParties` folder

# Art Direction

## General Visual Style

- The project is a stylized dark fantasy action RPG inspired by the mythology, folklore, architecture, and material culture of the North Caucasus.
- The world is inspired by the 12th–13th centuries, but it is fictional and does not require strict historical accuracy.
- Maintain a consistent visual identity across characters, enemies, environments, equipment, and UI assets.
- Prefer stylized, hand-crafted visuals over photorealism.
- Use readable silhouettes, simplified shapes, and carefully controlled details.
- Materials should feel natural, aged, and believable within the fictional world.
- The atmosphere should communicate ancient legends, forgotten civilizations, mystery, and a world whose balance has been disturbed.
- Avoid generic high-fantasy aesthetics when a more distinctive Caucasian-inspired solution is possible.
- Avoid modern, futuristic, sci-fi, anime, and excessively cartoonish visual elements.
- Do not introduce visual elements that contradict the established art direction without approval.

## Cultural and Historical Inspiration

- Draw inspiration from the traditional clothing, weapons, armor, ornaments, architecture, and craftsmanship of the North Caucasus.
- Use historical and ethnographic references as a foundation, adapting them to the fictional setting when appropriate.
- Prefer restrained and meaningful ornamentation over excessive decorative complexity.
- Fantasy elements should feel integrated into the local cultural and mythological context.
- Do not automatically apply stereotypical medieval European fantasy designs.
- Distinguish historically documented elements from fictional interpretations when explaining design decisions.

## UI Art Style

- UI assets should follow a stylized medieval dark fantasy aesthetic influenced by North Caucasian craftsmanship.
- Favor materials such as aged iron, darkened steel, weathered wood, carved stone, worn leather, and bronze.
- Use restrained ornamentation inspired by traditional Caucasian decorative patterns.
- UI elements should look intentionally crafted rather than mass-produced or technologically advanced.
- Prefer muted, earthy colors: dark brown, charcoal, warm gray, desaturated bronze, and aged metal tones.
- Use brighter colors sparingly for important states, magical effects, and gameplay feedback.
- Preserve clear readability and strong contrast between UI elements and their backgrounds.
- Decorative elements must not interfere with usability.
- Avoid excessive visual noise, heavy ornamentation, and unnecessary embellishments.
- Avoid glossy mobile-game interfaces, neon effects, futuristic panels, and generic fantasy UI kits.
- Naming should start with component type. for example Image_ItemIcon

## UI Asset Generation

- Generate individual UI assets rather than complete interface mockups unless explicitly requested.
- Prefer transparent backgrounds for standalone UI assets.
- Avoid adding text, labels, numbers, or icons unless explicitly requested.
- Do not add unrelated decorations or surrounding objects.
- Keep assets visually consistent in materials, lighting, proportions, and ornamentation.
- Prefer clean silhouettes and clearly defined borders.
- Design assets to remain readable at their intended in-game size.
- Avoid excessive baked-in shadows, lighting, and highlights that reduce flexibility in Unity.
- When generating frames, panels, buttons, or slots, consider whether the asset may need to support Unity 9-slicing.
- Do not assume that a generated image is ready for 9-slicing without checking its borders and stretchable regions.
- When the intended use is unclear, ask whether the asset is decorative, interactive, or part of a reusable UI component.

## Character and Enemy Design

- Characters should follow a stylized dark fantasy aesthetic with recognizable North Caucasian influences.
- Prioritize distinctive silhouettes, believable anatomy, and readable proportions.
- Clothing, armor, and equipment should reflect the character's role, origin, and place in the game world.
- Favor layered fabrics, leather, metal, fur, and historically inspired protective equipment where appropriate.
- Avoid excessive armor complexity, oversized fantasy weapons, and unnecessary decorative elements.
- Human characters should retain believable anatomy, even when stylized.
- Mythological creatures may have exaggerated proportions and unusual anatomy when supported by their concept.
- Enemy designs should communicate their nature, combat role, and level of danger through their silhouette and appearance.
- Undead characters should appear aged, desiccated, damaged, or corrupted rather than resembling generic fantasy skeletons unless explicitly requested.
- Maintain a consistent level of stylization between ordinary humans, undead enemies, and mythological creatures.

## Character Reference Generation

- Generate references suitable for high-quality AI model reconstruction (for example, with Tripo): use clear, solid forms, consistent views, readable boundaries, and simple, large folds. Avoid excessive tears, small holes, fringe, thin loose strips, intersections, and effects that obscure the structure. Convey wear primarily through materials and textures rather than a complex, torn silhouette.

- When the user asks to split an image into three parts, save the resulting images directly to the root of drive `F:\` unless the current request explicitly specifies another location.

- Before generating a mythological creature, research available internet sources and distinguish information from those sources from artistic interpretation.
- Before generation, study the existing approved project character references and preserve their visual style.

- Character references are primarily intended for 3D modeling in Blender.
- Unless otherwise specified, a character reference must contain exactly three full-body views: front, side, and back, with no additional images, text, labels, frames, or decorative elements.
- All three views must show the same character with identical clothing, equipment, proportions, and details.
- Use a neutral A-pose suitable for modeling and fitting to Rigify in Blender: arms held away from the torso, with limbs and joints clearly readable.
- Keep the character's anatomy and proportions consistent between views.
- Use orthographic-style views without perspective distortion.
- Front, side, and back views must use the same scale and be aligned vertically.
- Show the entire character from head to feet without cropping.
- Keep limbs and important anatomical features clearly visible.
- Avoid dramatic poses, cinematic camera angles, and exaggerated perspective.
- Use neutral, even lighting to reveal forms and materials clearly.
- Prefer a plain, neutral background without scenery or environmental objects.
- Avoid unnecessary text, labels, measurements, and decorative presentation elements.
- Do not depict weapons or shields in character and creature references, including weapons in scabbards, on the back, on the belt, or beside a leg, unless the user explicitly requests them. When revising an old reference, remove such equipment even if it appeared in the original.
- Do not add extra weapons, accessories, or equipment unless requested.
- Prioritize modeling clarity and structural consistency over cinematic presentation.

## Creature Reference Generation

- Apply the same requirements to humanoid creatures: three views (front, side, and back) in a consistent A-pose suitable for Rigify.
- Preserve consistent anatomy, limb proportions, and distinctive features between views.
- Clearly communicate unusual anatomical structures required for modeling.
- Do not conceal important body parts behind dramatic poses or effects.
- For non-humanoid creatures, choose views that best communicate their three-dimensional structure.
- Avoid environmental backgrounds and cinematic compositions unless explicitly requested.

## Blender Texturing

- Work directly in the .blend project specified by the user. Do not create separate, intermediate, or alternative .blend projects unless the user explicitly requests them.
- Use Ucupaint when texturing models in Blender.
- All construction models may be exported using the `Plain Object` preset.
- Texture dimensions must not exceed 2048 pixels on either side.
- Store generated images for Blender within the directory of the currently open `.blend` project. First find and use an appropriate existing subfolder, such as `refs`, `resource images`, or a similar folder, following the project's existing structure.

## Texture Reference Generation

- Generate textures and material references suitable for high-quality AI reconstruction and texturing of the 3D model: preserve clear material separation, consistent detail scale, and soft, even lighting. Avoid visual noise, overly high-contrast microdetails, painted holes, and shadows that AI could mistake for geometry; match the complexity to the target model.

- When asked to generate a material or texture reference, focus on the requested surface material only.
- Do not automatically include complete objects, props, characters, or environmental elements.
- For skin textures, show the skin surface rather than a complete character.
- For wood textures, show the wood surface without metal fittings, nails, handles, or other props unless requested.
- For metal textures, show the metal surface without attached decorative objects unless requested.
- Prefer consistent lighting and material appearance.
- Avoid strong directional shadows and excessive baked-in highlights.
- When a seamless texture is requested, ensure that the design is suitable for tiling.
- Do not generate UV layouts, texture atlases, normal maps, or additional texture channels unless explicitly requested.
- Distinguish between a visual material reference and a production-ready texture map.
- Do not assume that a generated texture is technically seamless or production-ready without verification.

## Consistency and Reference Priority

- Use existing approved project assets and references as the primary source of visual consistency.
- When a reference image is provided, preserve its important shapes, proportions, materials, and stylistic characteristics.
- Do not introduce significant design changes unless requested.
- Match the level of stylization already established by the project.
- Do not automatically add visual complexity to make an asset appear more detailed.
- When generating variations, preserve the recognizable identity of the original design.
- If an existing project reference conflicts with general art direction rules, prioritize the explicitly approved reference.
- If the desired style cannot be determined from existing assets, ask for clarification rather than inventing a new visual direction.

## Asset Review

- Check generated assets against the established project art direction.
- Verify that character references are suitable for 3D modeling.
- Verify consistency between front and side views.
- Check that UI assets remain readable at their intended size.
- Check that standalone assets do not contain unnecessary backgrounds, props, or text.
- Clearly identify technical limitations of generated assets.
- Do not describe generated images as game-ready without verifying their technical requirements.
