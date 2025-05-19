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
- AI will use consistent bullet point numbering:
  - Use sequential numbers (1, 2, 3...) for main points
  - Use letters (a, b, c...) for sub-points
  - Use roman numerals (i, ii, iii...) for detailed sub-points
  - Never reuse numbers within the same context
  - Maintain hierarchy in nested lists
  - Example of proper enumeration:
    1. Main point
       a. Sub-point
          i. Detailed sub-point
          ii. Another detailed sub-point
       b. Another sub-point
    2. Another main point
       a. Sub-point
          i. Detailed sub-point
  - Common mistakes to avoid:
    - DO NOT reuse numbers in different sections (e.g., don't start with 1, 2, 3 in multiple places)
    - DO NOT mix numbering styles (e.g., don't use 1, 2, 3 for sub-points)
    - DO NOT skip hierarchy levels (e.g., don't go from 1 directly to i)
    - DO NOT use the same number twice in the same context
  - Correct hierarchy flow:
    1. First main point
       a. First sub-point
          i. First detail
          ii. Second detail
       b. Second sub-point
          i. First detail
    2. Second main point
       a. First sub-point
          i. First detail

### Error Correction
- Magic Phrase for Quick Fixes:
  - Simply state: "Please follow the template."
  - This will make AI:
    a. Review the entire PROMPT_TEMPLATE.md
    b. Identify any deviations
    c. Correct all issues
    d. Continue with proper formatting

- Specific Issues and Corrections:
  1. Enumeration Mistakes
     a. Use: "Please follow the enumeration rules from PROMPT_TEMPLATE.md"
     b. AI will fix numbering hierarchy

  2. Code Style Issues
     a. Use: "Please follow the code quality standards"
     b. AI will review and apply:
        i. SOLID principles
        ii. Documentation standards
        iii. Type safety rules
        iv. String formatting rules

  3. Documentation Gaps
     a. Use: "Please follow the documentation guidelines"
     b. AI will add:
        i. XML comments
        ii. Inline comments
        iii. Class documentation
        iv. Method documentation

  4. Project Structure Issues
     a. Use: "Please follow the project structure"
     b. AI will ensure:
        i. Correct namespace usage
        ii. Proper file organization
        iii. Assembly references
        iv. Dependency management

- Best Practices for Error Correction:
  1. Start with the magic phrase for general issues
  2. Use specific phrases for targeted fixes
  3. No need for detailed explanations
  4. AI will self-correct and continue properly

### Stashed Prompts System
- Unfinished prompts are stored in the `StashedPrompts/` directory
- File naming convention: `Name-{datetime}.md`
  - Name: Required, descriptive of the prompt's purpose
  - Datetime: In Lisbon, Portugal timezone (WET/WEST)
- Prompts are not executed until explicitly requested
- AI will ask for a name if not provided
- AI will ask clarifying questions about incomplete prompts
- AI will maintain a list of TODO items from stashed prompts

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
- Enum standards:
  - Always use explicit integer values for enum members
  - Order enum members logically
  - Document enum values if they have specific meanings
  - Use [Flags] attribute for bitwise enums
  - Consider using [System.ComponentModel.Description] for display names
- UnityEvent standards:
  - UnityEvents are always public and require no instantiation
  - They are automatically instantiated when added as components
  - Always check for null before invoking UnityEvents (use the null-conditional operator `?.`)
  - Use custom UnityEvent classes for events with parameters (e.g., UnityEvent<T>)
  - Consider using [Serializable] attribute for custom UnityEvent classes

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