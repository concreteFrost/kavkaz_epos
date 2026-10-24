# Главный герой: референс и промпт Meshy

Источник: https://drive.google.com/file/d/1MUYxHUqEt7wITtma9q7u_r8EectojUUz/view — исходный полный скетч главного героя. При унификации папок исходник перемещён в «Главный герой / Исходники». Новая версия создана встроенным imagegen и загружена после разрешения пользователя организовать референсы на Диске: https://drive.google.com/file/d/1d5R3Wp7Vy_w5v-jCV7_J1PXPpvIDaVd5/view . Общий референс и три отдельных вида находятся в «Главный герой / Актуальные».

Общий референс: `Assets/Art/References/Hero Modeling.png` (1536×1024). Раздельные виды: `F:\Hero Front.png`, `F:\Hero Side.png`, `F:\Hero Back.png`. Обрезки просмотрены, персонажи целиком. Это художественная основа: точная согласованность проекций и подгонка под Rigify в Blender не проверены; кисти спереди видны преимущественно с ребра.

Размер взят из `player_model.fbx`: максимум по Y 1,96099 м, минимум 0,00015 м; масштаб импортера 1, единица FBX соответствует метру. Рост геометрии около 1,96 м. Корневой коллайдер PlayerCharacter имеет высоту 2,1 м и не использован как рост модели. Для генерации новой модели 1,96 м задано как общий габарит вместе со шлемом; размер после экспорта следует проверить и выставить в Blender.

## Промпт для Meshy

```text
Recreate ONE main hero matching the supplied front, side and back references. Correct, clean geometry is the highest priority; surface detail is secondary. Preserve his face, short dark beard, proportions, silhouette, armor design and muted earthy colors.

Target overall height: 1.96 meters from boot soles to the highest point of the helmet, matching the current game model's overall height. Adult human male with believable anatomy, not a giant.

Preserve the pointed aged iron helmet with nasal guard and chainmail aventail, dark scale-pattern torso armor and split armored skirt, brown leather shoulder pieces, reddish-brown tunic, waist belt, steel forearm bracers, fingerless leather gloves, charcoal trousers and tall worn leather boots.

Strict bilateral geometric symmetry across the character's vertical center plane is essential. Match left and right limb lengths, shoulder and hip heights, hand sizes, boots, bracers and corresponding armor shapes. Keep the helmet, nasal guard and face centered. Preserve intentional asymmetric accessories shown in the reference, such as the diagonal belt strap; do not mirror them into extra accessories. Resolve accidental differences between reference views into one coherent three-dimensional design.

Neutral symmetrical A-pose for Blender Rigify: nearly straight arms approximately 40 degrees away from the torso, neutral wrists, five distinct fingers on each hand with clear gaps, thumbs correctly positioned, straight legs and feet forward. Maintain correct anatomy and clearly defined shoulders, elbows, hips, knees and ankles.

The chainmail aventail MUST be ONE continuous connected mesh surface, following its garment shape around the head, neck and shoulders. Do NOT create individual rings, interlocking links, disconnected ring geometry, perforations or thousands of separate pieces. Represent the ring pattern ONLY through textures and normal/bump maps. The scale-pattern armor must likewise use continuous garment panels, with the small scale pattern represented as surface detail rather than separate floating scales. Preserve the large garment openings and panel splits visible in the reference.

Keep clothing and armor as separate mesh objects from the body where possible, with separate material slots for skin, cloth, leather and metal. Preserve clear garment boundaries. No weapons, shield, cape, backpack or additional accessories.

Use clean connected surfaces with consistent normals, no accidental holes, duplicate faces, internal debris, floating fragments or self-intersections. Keep fingers separate from each other and hands separate from the sleeves. Maintain clear separation and sensible clearance between the body, clothing and armor. Preserve the reference silhouette without lumpy, melted or inflated shapes. For each solid garment shell, use consistent thickness and properly joined inner and outer surfaces at its edges.

Stylized hand-crafted North Caucasus inspired medieval dark fantasy, matte natural materials and restrained wear. Generate a detailed source model for subsequent manual retopology and texture baking in Blender. Do not sacrifice correct geometry, symmetry or anatomical structure to add decorative detail.
```

## Промпт для изображения

Edit the supplied APPROVED MAIN HERO full sketch into a clean 3D modeling turnaround. Preserve EXACTLY the same human male character, face, short dark beard, anatomy, colors, armor construction, clothing and equipment. This is a North Caucasus inspired medieval dark fantasy hero, not a generic European knight. Do not redesign.
Preserve: aged iron pointed conical helmet with central nasal guard and small top finial; chainmail aventail around sides/back of neck; dark overlapping scale-pattern body armor with matching short split armored skirt panels, restrained brown leather shoulder pieces and fastening straps; weathered reddish-brown long-sleeve knee-length tunic underneath; simple brown leather waist belt with plain metal buckle; fitted aged steel forearm bracers over dark fingerless leather gloves with bare fingertips; charcoal trousers; tall worn dark brown leather boots. No held or sheathed weapons, no shield, no cape, no backpack, no new accessories.
OUTPUT EXACTLY THREE full-body depictions: FRONT on left, exact LEFT SIDE PROFILE facing left at center, exact BACK on right. Absolutely nothing else: no head or armor closeups, no captions, text, measurements, frames, guide lines, symbols, logos, extra poses or scenery. Uniform plain neutral warm light gray background without paper texture. Wide landscape composition, generous gaps and margins, every figure completely isolated for clean cropping.
POSE: clear neutral upright anatomical A-pose for Blender Rigify. Front and back both arms nearly straight and spread diagonally downward approximately 40 degrees from the vertical torso, with large open gaps between wrists and hips. Relaxed hands, all five fingers separated and visible, wrists straight, no fist, no hand overlapping clothing. Legs straight hip-width apart, feet forward, head level. Side view is a true orthographic projection of the SAME A-pose, with the projected arm length and wrist/fingertip height matching the front/back; do not lower the side-view hands. All three figures same scale, crowns and boot soles aligned; no perspective distortion or 3/4 views. Full helmet finial, fingertips and boots visible with margin.
Preserve original restrained hand-painted stylized dark fantasy rendering, readable simplified forms, believable aged iron/leather/cloth, matte materials, controlled detail, muted earthy palette and soft even lighting. Same armor panels, belt placement, bracers and helmet in all three views. Show only the existing character with no invented ornamentation. Max image dimension 2048 pixels.

После первой генерации уточнены разведение рук и выпрямление локтей с сохранением дизайна.
