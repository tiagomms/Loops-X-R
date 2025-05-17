# Project Prompt Template

## Environment Details
- **O/S**: 64-bit Windows 11
- **Terminal**: Cursor's Integrated Powershell with Administrator rights
- **Browser**: Chrome

## Project Guidelines

### Communication Style
- AI will ask clarifying questions when needed
- AI will provide prompt suggestions for better communication
- AI will store "future considerations" for later discussion
- AI will prioritize tasks based on user's "let's prioritize/do this first" statements

### Code Quality Standards
- Follow SOLID principles unless explicitly stated otherwise
- Production-ready code with clear documentation
- Comprehensive inline comments
- Error checking and type validation
- Strict TypeScript notation (when applicable)
- String standards:
  - Use double quotes (`"`)
  - Use string templates or `.join()`
  - No operational concatenation

### AI Model Usage
- AI will suggest appropriate agent algorithms based on task complexity
- Focus on cost-effective solutions
- Break down complex tasks into manageable pieces
- Provide clear, concise prompts

### Documentation & Knowledge
- AI will be transparent about:
  - Areas needing more information
  - Required documentation or resources
  - Assumptions made
  - Uncertainties

## Project Setup

### Tech Stack
- Unity 6 LTS
- Meta XR SDK (latest stable version)
- OpenXR
- DOTween (latest stable version)
- Python (for local AI model backend if needed)

### Development Environment
- IDE: Rider (primary), Cursor (new)
- Version Control: Git
- Target Platform: Meta Quest 3

### Project Structure
```
Project Root/
├── Assets/
│   ├── Scripts/
│   │   ├── Core/           # Core game systems
│   │   ├── UI/            # UI-related scripts
│   │   ├── VR/            # VR-specific functionality
│   │   ├── Utils/         # Utility classes
│   │   └── Managers/      # Manager classes
│   ├── Samples/           # Original samples (gitignored)
│   ├── UsedSamples/       # Modified samples in use
│   └── [Other Unity folders]
├── Docs/                  # Project documentation
│   ├── CODING_STANDARDS.md
│   ├── PROMPT_TEMPLATE.md
│   └── SAMPLES_README.md
└── [Other project files]
```

### Sample Assets Management
- Keep original samples in `Assets/Samples/` (gitignored)
- Store modified samples in `Assets/UsedSamples/`
- Document used samples in `Docs/SAMPLES_README.md`
- Track modifications to used samples

## Development Workflow

### Code Organization
- Follow Unity best practices
- Use PascalCase for public methods and properties
- Use camelCase for private fields and local variables
- Follow Unity's component naming conventions

### Documentation
- Use XML comments for public methods
- Include clear inline comments
- Document class purposes and functionality
- Maintain clear method documentation

### Version Control
- Write meaningful commit messages
- Keep commits focused and atomic
- Review code before committing
- Use appropriate .gitignore for Unity

## Future Considerations
- Testing implementation
- CI/CD setup
- Performance optimization
- AI model integration
- Additional documentation needs

## Notes
- This is a living document
- Update as project evolves
- Add new guidelines as needed
- Document special considerations 