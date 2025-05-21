# XR Loop Pedal - Orb System Architecture

> 💡 **Documentation Style**: This project uses concise, emoji-rich documentation for better readability and visual organization. Each feature and component is marked with relevant emojis to make the documentation more scannable and engaging.

This system enables audio recording and playback in a loop pedal style interface, with each orb representing a single audio track that can be recorded, played, and paused independently. The system uses a centralized `MicController` to manage recording sessions across multiple orbs.

## Features

- 🎙️ Centralized recording management via MicController
- 🎯 State-based orb behavior (Ready, Recording, Pausing, Playing)
- 🎨 Visual feedback with particle effects and color transitions
- 🔊 Volume-based particle alpha adjustments
- 🎵 Interface naming system (A, B, C, etc.)
- ⚡ Smooth transitions using DOTween
- 🧠 Event-based communication between components

---

## Core Components

### 1. MicController
- Singleton pattern for global access
- Manages recording state across all orbs
- Handles tap gestures for recording control
- Assigns interface names to orbs
- Increments interface letter after each recording session

### 2. AudioOrbController
- Coordinates between audio and visual components
- Manages state transitions
- Handles playback control
- Integrates with ControlsManager for gestures

### 3. OrbParticleController
- Handles visual feedback
- Color transitions based on state
- Alpha level based on audio volume
- Smooth transitions using DOTween

### 4. RecordAudioInterface
- Audio recording and playback
- Volume management
- File saving/loading
- Interface naming

---

## State Management

### Orb States
- `ReadyToRecord`: Initial state, ready to start recording
- `Recording`: Currently recording audio
- `Pausing`: Transitional state between recording/playing
- `Playing`: Playing back recorded audio
- `Disabled`: Orb is inactive

### State Colors
| State | Default Color | Description |
|-------|--------------|-------------|
| ReadyToRecord | Green (0.2, 0.8, 0.2) | Ready to start recording |
| Recording | Red (0.8, 0.2, 0.2) | Currently recording |
| Pausing | Yellow (0.8, 0.8, 0.2) | Transitional state |
| Playing | Blue (0.2, 0.2, 0.8) | Playing back audio |
| Disabled | Gray (0.5, 0.5, 0.5) | Inactive state |

---

## Setup Instructions

1. **MicController Setup**
   - Add MicController to scene
   - Configure ControlsManager reference
   - Set up tap gesture handling

2. **Orb Setup**
   - Create orb GameObject
   - Add required components:
     - AudioOrbController
     - OrbParticleController
     - ParticleSystem (child)
     - RecordAudioInterface

3. **Configuration**
   - Adjust particle settings (alpha, transitions)
   - Configure audio settings
   - Set up state colors

---

## Integration

### MicController Registration
```csharp
// Register orb with MicController
MicController.Instance.RegisterOrb(audioOrbController);

// Unregister when done
MicController.Instance.UnregisterOrb(audioOrbController);
```

### State Management
```csharp
// State changes are handled through MicController events
// Recording starts/stops are controlled by tap gestures
// Playback is controlled by play/stop gestures
```

---

## Error Handling
- 🚫 Invalid state transitions are logged
- 🔍 Missing components are detected at startup
- ✅ Audio operations are validated
- 🛡️ Visual transitions are protected against null references
- 🔒 MicController singleton validation
- 📝 Orb registration validation

## Performance Considerations
- ⚡ Uses DOTween for efficient animations
- 🎯 Volume changes are smoothed to prevent jitter
- 🔄 State changes are validated before execution
- 🎨 Particle system updates are optimized
- 🎙️ Centralized recording management prevents conflicts
- 📡 Event-based communication reduces coupling 