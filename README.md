# XR Loop Pedal Project 🎵

## Overview 🎯
This project is the first one-week solo prototype developed for the XR Bootcamp XR Prototyping course (May-July 2025). It implements a loop pedal system in VR, allowing users to record, play, and layer audio loops in a spatial environment. Built with Unity and Meta XR SDK v76, it provides an intuitive and immersive audio manipulation experience.

### Project Motivation 🎯
For analog performers and songwriters, technological interfaces often create barriers and frustration, detaching them from their creative flow. Traditional recording methods (phones, DAWs) require multiple steps and lack spontaneity. This project aims to bridge this gap by:
- Providing minimal UI with voice inputs
- Enabling AI-powered manipulations
- Visualizing spatial sound
- Supporting spontaneous recording and sample connection
- Creating an engaging visual experience for both performers and audience

## Features ✨

### Core Features 🎯
These are the essential features that make the prototype functional and prove the concept:
- 🎙️ Record audio on the fly and generate identifiable audio bubbles
- ▶️ Play/Stop sound up close or far away from room
- 🗑️ Dismiss bubbles
- 🎨 Minimal UI (voice/gesture activated recording)

## Tech Stack 💻

### Core Technologies
- 🎮 Meta Quest XR SDK v76
- 🎵 Audio and Voice SDK
- 👆 Hand tracking support
- 🎯 Meta Quest UI Building blocks
- 🤌 Hand tracking microgestures
- 🖐️ Hand tracking poses
- DOTween
- OpenWavParser

## Project Structure 📁

```
Project Root/
├── Assets/
│   ├── Scripts/
│   │   ├── AudioSystem/     # Audio recording and playback
│   │   ├── PullableXR/      # Spatial interaction system
│   │   ├── Utils/           # Utility classes
│   │   └── Managers/        # Manager classes
│   ├── Samples/            # Original samples (gitignored)
│   └── UsedSamples/        # Modified samples in use
├── Docs/                   # Project documentation
└── [Other Unity folders]
```

## Setup Instructions 🛠️

### Prerequisites
- Unity 6 LTS
- Meta XR SDK v76
- OpenXR
- DOTween (latest stable version)
- OpenWavParser

### Development Environment
- IDE: Rider (primary), Cursor (new)
- Version Control: Git
- Target Platform: Meta Quest 3

### Installation
1. Clone the repository
2. Open the project in Unity
3. Install required packages:
   - Meta XR SDK v76
   - OpenXR
   - DOTween
4. Configure project settings:
   - Set up XR Plugin Management
   - Configure OpenXR settings
   - Set up audio settings

## Core Systems 🎯

### Audio System
- 🎙️ MicController for centralized recording
- 🎯 AudioOrbController for individual track management
- 🎨 OrbParticleController for visual feedback
- 🔊 RecordAudioInterface for audio handling

### PullableXR System
- 🎯 Spatial interaction with prefabs
- 🎮 Pull-to-confirm mechanics
- 🎨 Visual feedback and animations
- 📡 Event-based communication

## Documentation 📚

### Project Documentation
- `Docs/PROMPT_TEMPLATE.md`: AI interaction guidelines
- `Docs/CODING_STANDARDS.md`: Coding standards and best practices
- `Docs/SAMPLES_README.md`: Sample assets management
- `Docs/PROJECT_TEMPLATE.md`: Project setup checklist

### System Documentation
- `AudioSystem/README.md`: Audio system architecture
- `PullableXR/README.md`: Spatial interaction system

## Development Guidelines 🎯

### Code Standards
- Follow SOLID principles
- Use XML comments for documentation
- Implement error checking and type validation
- Follow Unity best practices
- Use emoji-rich documentation

### Error Handling
- 🚫 Invalid state transitions are logged
- 🔍 Missing components are detected at startup
- ✅ Audio operations are validated
- 🛡️ Visual transitions are protected
- 🔒 Singleton validation
- 📝 Registration validation

### Performance Considerations
- ⚡ Uses DOTween for efficient animations
- 🎯 Volume changes are smoothed
- 🔄 State changes are validated
- 🎨 Particle system updates are optimized
- 🎙️ Centralized recording management
- 📡 Event-based communication

## Contributing 🤝

### Development Workflow
1. Create a feature branch
2. Implement changes
3. Follow coding standards
4. Update documentation
5. Submit pull request

### Code Review Process
- Review against coding standards
- Check documentation updates
- Verify error handling
- Test functionality
- Ensure performance

## Inspiration & Resources 🎵

### Music & Artists
- Holly Herndon - AI voice training in music
- Queen's "Seven Seas of Rhye" - Spatial audio experimentation
- Lucas Martinic's XR recorder project
- Boss Loop Station
- Ableton Live layout

### AI Models & Tools
- [MT3](https://huggingface.co/spaces/Hmjz100/MT3) - Audio to MIDI conversion
- [Riffusion-Melodiff](https://huggingface.co/spaces/JanBabela/Riffusion-Melodiff-v1) - Instrument transformation
- [MusicVision](https://huggingface.co/spaces/Genius-Society/MusicVision) - Audio to video
- [ACE-Step](https://huggingface.co/spaces/ACE-Step/ACE-Step) - Song generation
- [Demucs](https://huggingface.co/spaces/nakas/demucs_playground) - Audio separation

## License 📄
MIT License

## Stretch Goals 🚀
Features for future implementation:
- 🔊 Volume control for recorded play
- ✂️ Edit single samples
- 📂 Project management (new/load)
- 🎛️ Close-range UI for loop control
- 🎚️ Audio control knobs
- 🎵 Moving bubble recordings
- 🤖 AI Integration:
  - Audio to MIDI conversion
  - Lyrics extraction
  - Instrument transformation
  - Audio to video generation
  - Song iteration/generation
  - Object segmentation
- 🌟 Project auras
- 📸 Memory palace integration
- 🎧 Augmented music player features
