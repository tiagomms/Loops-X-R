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

## Testing
- Write unit tests for critical functionality
- Test edge cases
- Document test scenarios
- Use Unity Test Framework when possible 