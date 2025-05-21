# PullableXR System (Meta XR SDK v76 Compatible)

This system allows for spatial pulling of a prefab towards the user's hand (via pinch gesture), with confirm/cancel logic based on distance, and prefab-agnostic behavior toggling.

## Features

- ✋ Pull-to-confirm prefab interaction
- 🎯 Confirm via distance threshold
- 🔕 Prevents prefab interaction during pull using temporary layer
- 🎥 Billboard orientation when instantiating
- 🧠 Smooth cancel animation via DOTween
- ⚡ Lightweight, scalable, prefab-agnostic

---

## Setup

1. **Layer Setup**
   - Create a new layer named `Uninteractive`.
   - Assign this layer to objects during pull to disable interaction.

2. **Prefab Requirements**
   - No required scripts/components.
   - Optional: Meta XR `GrabInteractable` or other interactors activated after confirmation.

3. **Spawner**
   - Attach `PullableSpawner` script.
   - Assign prefab, offsets, scale values, and UnityEvents (e.g. to activate logic on confirm/cancel).

---

## Integration with Meta XR SDK

Use hand tracking or controller grab logic to:
- Call `TriggerPull(Transform handTransform)` on pinch start.
- Call `Release()` on pinch release.

Ensure `Camera.main` is properly assigned during runtime (e.g. in XR Rig prefab).

---

## Customization

- 🎨 `confirmDistance`, `minScale`, and `maxScale` control pull distance behavior.
- ⏱️ `failedReleaseDuration` and `failedReleaseEase` define cancel animation style.
- 🔄 Automatically resets original layers after success.
- 🔧 Add UnityEvents for scalable logic.
