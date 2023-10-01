using PasswordGenerator.Models;

namespace PasswordGenerator.StringTemplateParser.Models;

public record Return(PwSequence? Concat, PwInsert? Insert, PwFill? Fill);

