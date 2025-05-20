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

- Magic Phrase for Game Dev Standards:
  - Simply state: "Please follow game dev standards."
  - This will make AI:
    a. Review and apply industry standard game dev patterns
    b. Justify pattern choices with brief explanations
    c. Consider Unity-specific best practices
    d. Ensure solutions are production-ready

- Magic Phrase for Code State Preservation:
  - Simply state: "Please preserve code state."
  - This will make AI:
    a. Never modify commented code without explicit request
    b. Never remove unused methods without explicit request
    c. Respect existing code structure and organization
    d. Ask for clarification before making structural changes

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

  5. Code State Preservation
     a. Use: "Please preserve code state"
     b. AI will:
        i. Never modify commented code without explicit request
        ii. Never remove unused methods without explicit request
        iii. Respect existing code structure
        iv. Ask before making structural changes

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
- Game Design Patterns:
  - Solutions should follow industry standard game dev patterns
  - Each pattern choice must be justified with a brief explanation
  - Common patterns include:
    a. Observer Pattern: For event-driven systems and decoupled communication
    b. Strategy Pattern: For interchangeable behaviors and algorithms
    c. Factory Pattern: For object creation and instantiation
    d. Command Pattern: For action encapsulation and undo/redo systems
    e. Component Pattern: For flexible and reusable game object behaviors
- Code State Preservation:
  - Never modify commented code without explicit request
  - Never remove unused methods without explicit request
  - Respect existing code structure and organization
  - Ask for clarification before making structural changes
  - Preserve debugging code and commented sections
  - Maintain existing code organization
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

### AI Response Protocol
- Systematic Approach:
  1. Pre-Response Checklist
     a. Review entire PROMPT_TEMPLATE.md
     b. Check for relevant magic phrases
     c. Verify code state preservation rules
     d. Ensure proper enumeration
     e. Validate against all standards
     f. Identify any potential conflicts between template sections
     g. Ask for clarification if conflicts exist

  2. Response Structure
     a. Acknowledge the request
     b. State relevant template sections
     c. Follow enumeration rules
     d. Include proper documentation
     e. Preserve code state
     f. Highlight any template conflicts
     g. Request guidance on conflict resolution

  3. Code Changes Protocol
     a. Review existing code state
     b. Note all commented sections
     c. Identify unused methods
     d. Ask for clarification if needed
     e. Propose changes before implementing
     f. Verify changes against template
     g. Request confirmation for any template deviations

- Response Format:
  1. Initial Response
     a. Acknowledge request
     b. List relevant template sections
     c. State intended approach
     d. Ask for confirmation if needed
     e. Highlight any template conflicts
     f. Request guidance on conflict resolution

  2. Code Changes
     a. Show proposed changes
     b. Explain reasoning
     c. Wait for approval
     d. Implement only after confirmation
     e. Verify against template
     f. Request guidance for any deviations

  3. Follow-up
     a. Verify changes
     b. Check for side effects
     c. Confirm completion
     d. Ask for next steps
     e. Ensure template compliance
     f. Request feedback on template adherence

- Quality Assurance:
  1. Code Review
     a. Check against standards
     b. Verify documentation
     c. Ensure type safety
     d. Validate string formatting
     e. Verify template compliance
     f. Request guidance for any deviations

  2. State Preservation
     a. Verify commented code
     b. Check unused methods
     c. Validate structure
     d. Confirm organization
     e. Ensure template compliance
     f. Request guidance for any deviations

  3. Documentation
     a. XML comments
     b. Inline comments
     c. Method documentation
     d. Class documentation
     e. Template compliance notes
     f. Conflict resolution requests

- Template Adherence:
  1. Mandatory Compliance
     a. All responses must follow template
     b. No deviations without explicit approval
     c. All conflicts must be reported
     d. All deviations must be justified
     e. All changes must be confirmed

  2. Conflict Resolution
     a. Identify conflicting template sections
     b. Explain the conflict clearly
     c. Propose possible resolutions
     d. Wait for user guidance
     e. Implement only after approval

  3. Deviation Handling
     a. Document any necessary deviations
     b. Explain deviation reasoning
     c. Request explicit approval
     d. Implement only after confirmation
     e. Document the approved deviation

### Error Correction
// ... rest of existing content ... 