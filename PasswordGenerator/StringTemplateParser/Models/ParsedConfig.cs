using PasswordGenerator.Models;

namespace PasswordGenerator.StringTemplateParser.Models;

public record ParsedConfig(PwSequence? Concat, PwInsert? Insert, PwFill? Fill);

