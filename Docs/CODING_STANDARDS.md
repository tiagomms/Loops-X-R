# Unity/C# Coding Standards

## Naming Conventions

### Classes and Interfaces
- Use PascalCase for class and interface names
- Prefix interfaces with 'I' (e.g., `IInteractable`)
- Suffix MonoBehaviour classes with their primary function (e.g., `PlayerController`, `GameManager`)

### Methods and Properties
- Use PascalCase for public methods and properties
- Use camelCase for private methods and properties
- Use descriptive names that indicate their purpose
- Prefix boolean properties with 'Is', 'Has', 'Can', etc. (e.g., `IsActive`, `HasComponent`)

### Variables and Fields
- Use camelCase for private fields
- Prefix private fields with underscore (e.g., `_transform`)
- Use PascalCase for public fields
- Use [SerializeField] for private fields that need to be visible in the Inspector
- Use [Header] to organize Inspector fields
- Use [Tooltip] to provide field descriptions

### Constants
- Use UPPER_CASE for constants
- Use PascalCase for static readonly fields

## Code Organization

### File Structure
- One class per file
- File name should match class name
- Group related functionality in namespaces

### Class Structure
1. Constants and static fields
2. Serialized fields
3. Private fields
4. Properties
5. Unity lifecycle methods (Awake, Start, Update, etc.)
6. Public methods
7. Private methods

### Method Organization
- Keep methods focused and single-purpose
- Maximum method length: 30-40 lines
- Use early returns to reduce nesting
- Document public methods with XML comments

## Documentation

### Class Documentation
```csharp
/// <summary>
/// Brief description of the class's purpose
/// </summary>
/// <remarks>
/// Additional details about the class if needed
/// </remarks>
public class YourClassName : MonoBehaviour
```

### Method Documentation
```csharp
/// <summary>
/// Brief description of what the method does
/// </summary>
/// <param name="paramName">Description of the parameter</param>
/// <returns>Description of the return value</returns>
/// <exception cref="ExceptionType">Description of when this exception is thrown</exception>
public ReturnType MethodName(ParamType paramName)
```

### Field Documentation
```csharp
[Header("Configuration")]
[Tooltip("Description of what this field is used for")]
[SerializeField] private Type _fieldName;
```

## Code Comments and Special Keywords

### Special Comment Keywords
Use the following special comment keywords to highlight important code annotations. These will be color-coded in most modern IDEs:

```csharp
// TODO: Indicates code that needs to be implemented or completed
// FIXME: Marks code that needs to be fixed or improved
// BUG: Indicates a known bug that needs to be addressed
// HACK: Marks a workaround or temporary solution
// REVIEW: Indicates code that needs to be reviewed
// NOTE: Important information about the code
// OPTIMIZE: Marks code that needs performance optimization
// DEPRECATED: Indicates code that should not be used
// TEST: Marks code that needs testing
// DOC: Indicates documentation that needs to be updated
```

### Comment Guidelines
- Use special keywords consistently across the project
- Include a brief description after the keyword
- Add your initials or name for accountability (e.g., `// TODO(tiago): Implement feature X`)
- Remove or update comments when the task is completed
- Use these comments sparingly and meaningfully

Example usage:
```csharp
// TODO(tiago): Implement player movement
// FIXME(john): Handle edge case when player falls off map
// BUG: Physics calculation incorrect at high velocities
// HACK: Temporary solution until proper API is available
// REVIEW: Consider using object pooling for better performance
```

### Better Comments Configuration
To enable consistent comment highlighting across all projects, add the following configuration to your `.vscode/settings.json`:

```json
{
    "better-comments.tags": [
        {
            "tag": "!",
            "color": "#FF0000",
            "strikethrough": false,
            "underline": false,
            "backgroundColor": "transparent",
            "bold": true,
            "italic": false
        },
        {
            "tag": "?",
            "color": "#3498DB",
            "strikethrough": false,
            "underline": false,
            "backgroundColor": "transparent",
            "bold": true,
            "italic": false
        },
        {
            "tag": "//",
            "color": "#474747",
            "strikethrough": true,
            "underline": false,
            "backgroundColor": "transparent",
            "bold": false,
            "italic": false
        },
        {
            "tag": "todo",
            "color": "#FF8C00",
            "strikethrough": false,
            "underline": false,
            "backgroundColor": "transparent",
            "bold": true,
            "italic": false
        },
        {
            "tag": "fixme",
            "color": "#FF0000",
            "strikethrough": false,
            "underline": false,
            "backgroundColor": "transparent",
            "bold": true,
            "italic": false
        },
        {
            "tag": "bug",
            "color": "#FF0000",
            "strikethrough": false,
            "underline": false,
            "backgroundColor": "transparent",
            "bold": true,
            "italic": false
        },
        {
            "tag": "hack",
            "color": "#FFD700",
            "strikethrough": false,
            "underline": false,
            "backgroundColor": "transparent",
            "bold": true,
            "italic": false
        },
        {
            "tag": "review",
            "color": "#00BFFF",
            "strikethrough": false,
            "underline": false,
            "backgroundColor": "transparent",
            "bold": true,
            "italic": false
        },
        {
            "tag": "note",
            "color": "#32CD32",
            "strikethrough": false,
            "underline": false,
            "backgroundColor": "transparent",
            "bold": true,
            "italic": false
        },
        {
            "tag": "optimize",
            "color": "#9370DB",
            "strikethrough": false,
            "underline": false,
            "backgroundColor": "transparent",
            "bold": true,
            "italic": false
        },
        {
            "tag": "deprecated",
            "color": "#808080",
            "strikethrough": false,
            "underline": false,
            "backgroundColor": "transparent",
            "bold": true,
            "italic": false
        },
        {
            "tag": "test",
            "color": "#20B2AA",
            "strikethrough": false,
            "underline": false,
            "backgroundColor": "transparent",
            "bold": true,
            "italic": false
        },
        {
            "tag": "doc",
            "color": "#4169E1",
            "strikethrough": false,
            "underline": false,
            "backgroundColor": "transparent",
            "bold": true,
            "italic": false
        }
    ]
}
```

#### Setup Instructions
1. Install the "Better Comments" extension in your IDE
2. Create a `.vscode` folder in your project root if it doesn't exist
3. Create or update `settings.json` inside the `.vscode` folder with the above configuration
4. Reload your IDE to apply the changes

#### Additional Comment Types
- `// !` - Important comments (Red)
- `// ?` - Questions (Blue)
- `//` - Strikethrough comments (Gray)

## Best Practices

### Unity Specific
- Cache component references in Awake()
- Use [RequireComponent] when a component is mandatory
- Use [SerializeField] instead of public for Inspector fields
- Use [Header] and [Tooltip] for better Inspector organization
- Use [Range] for numeric fields that should be constrained

### Performance
- Avoid GetComponent() in Update()
- Use object pooling for frequently instantiated objects
- Cache Transform component if used frequently
- Use coroutines for time-based operations
- Use events instead of Update() when possible

### VR Specific
- Handle both controller and hand tracking input
- Consider comfort and safety in VR
- Use appropriate VR interaction patterns
- Consider performance implications of VR rendering

## Error Handling
- Use try-catch blocks for expected exceptions
- Log errors with appropriate context
- Use Debug.LogWarning for recoverable issues
- Use Debug.LogError for critical issues

## Version Control
- Write meaningful commit messages
- Keep commits focused and atomic
- Review code before committing
- Use .gitignore for Unity-specific files

## Code Formatting

### EditorConfig Settings
All Unity C# projects should include an `.editorconfig` file in the root directory with the following settings:

```editorconfig
# Core EditorConfig formatting
indent_style = space
indent_size = 4
end_of_line = crlf
charset = utf-8
trim_trailing_whitespace = true
insert_final_newline = true

# C# formatting conventions
csharp_new_line_before_open_brace = all
csharp_new_line_before_else = true
csharp_new_line_before_catch = true
csharp_new_line_before_finally = true
csharp_new_line_before_members_in_object_initializers = true
csharp_new_line_before_members_in_anonymous_types = true
csharp_new_line_between_query_expression_clauses = true

# Indentation preferences
csharp_indent_case_contents = true
csharp_indent_switch_labels = true
csharp_indent_labels = flush_left

# Space preferences
csharp_space_after_cast = false
csharp_space_after_keywords_in_control_flow_statements = true
csharp_space_between_method_declaration_parameter_list_parentheses = false
csharp_space_between_method_call_parameter_list_parentheses = false
csharp_space_between_parentheses = false
csharp_space_before_colon_in_inheritance_clause = true
csharp_space_after_colon_in_inheritance_clause = true
csharp_space_around_binary_operators = before_and_after
csharp_space_between_method_declaration_empty_parameter_list_parentheses = false
csharp_space_between_method_call_name_and_opening_parenthesis = false
csharp_space_between_method_call_empty_parameter_list_parentheses = false
csharp_space_after_comma_delimiter = true
csharp_space_after_dot = false
```

### Formatting Guidelines
- Use the provided `.editorconfig` settings for consistent formatting across all projects
- Format code before committing changes
- Use your IDE's format document feature (usually Shift + Alt + F)
- Enable format on save in your IDE settings
- Ensure all team members use the same `.editorconfig` settings

## Testing
- Write unit tests for critical functionality
- Test edge cases
- Document test scenarios
- Use Unity Test Framework when possible 