using System.Text;

namespace JSONParser
{
    /// <summary>
    /// A JSON parser that can parse JSON strings into .NET objects.
    /// </summary>
    public class JsonParser
    {
        private int currentIndex;
        private string? json;

        /// <summary>
        /// Parses a JSON string into a .NET object.
        /// </summary>
        /// <param name="jsonString">The JSON string to parse.</param>
        /// <returns>The parsed .NET object.</returns>
        /// <example>
        /// <code>
        /// JsonParser parser = new JsonParser();
        /// string jsonString = "{\"name\":\"John\",\"age\":30,\"city\":\"New York\"}";
        /// Dictionary&lt;string, object&gt; result = parser.Parse(jsonString);
        /// Console.WriteLine(result["name"]); // Output: John
        /// Console.WriteLine(result["age"]); // Output: 30
        /// Console.WriteLine(result["city"]); // Output: New York
        /// </code>
        /// </example>
        public Dictionary<string, object> Parse(string jsonString)
        {
            json = jsonString;
            currentIndex = 0;
            return ParseObject();
        }

        /// <summary>
        /// Parses a JSON object into a .NET dictionary.
        /// </summary>
        /// <returns>The parsed .NET dictionary.</returns>
        private Dictionary<string, object> ParseObject()
        {
            var result = new Dictionary<string, object>();
            Consume('{');

            while (Peek() != '}')
            {
                string key = ParseString();
                Consume(':');
                object value = ParseValue();
                result[key] = value;

                if (Peek() == ',')
                {
                    Consume(',');
                    if (Peek() == '}')
                    {
                        throw new Exception("Unexpected trailing comma.");
                    }
                }
                else if (Peek() == '}')
                {
                    break;
                }
                else
                {
                    throw new Exception(string.Format("Expected ',' or '}', but found '{0}'.", Peek()));
                }
            }

            Consume('}');
            return result;
        }

        /// <summary>
        /// Skips whitespace characters in the JSON string.
        /// </summary>
        private void SkipWhitespace()
        {
            while (currentIndex < json?.Length && char.IsWhiteSpace(json[currentIndex]))
            {
                currentIndex++;
            }
        }

        /// <summary>
        /// Consumes a character from the JSON string.
        /// </summary>
        /// <param name="expected">The expected character.</param>
        private void Consume(char expected)
        {
            try
            {
                SkipWhitespace();
                if (currentIndex >= json?.Length)
                    throw new Exception("Unexpected end of input.");

                if (json?[currentIndex] != expected)
                    throw new Exception($"Expected '{expected}', but found '{json?[currentIndex]}'.");

                currentIndex++;
                SkipWhitespace();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Error in Consume: {ex.Message}"));
                throw;
            }
        }

        /// <summary>
        /// Peeks at the next character in the JSON string.
        /// </summary>
        /// <returns>The next character.</returns>
        private char Peek()
        {
            try
            {
                SkipWhitespace();
                if (currentIndex >= json?.Length)
                    throw new Exception("Unexpected end of input.");

                return json[currentIndex];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Error in Peek: {ex.Message}"));
                throw;
            }
        }

        /// <summary>
        /// Parses an escape sequence in a JSON string.
        /// </summary>
        /// <returns>The parsed character.</returns>
        private char ParseEscapeSequence()
        {
            try
            {
                currentIndex++; // Consume the backslash
                if (currentIndex >= json?.Length)
                    throw new Exception("Unexpected end of input while parsing escape sequence.");

                char escapeChar = json[currentIndex];
                currentIndex++; // Consume the escape character

                switch (escapeChar)
                {
                    case '"': return '"';
                    case '\\': return '\\';
                    case '/': return '/';
                    case 'n': return '\n';
                    case 'r': return '\r';
                    case 't': return '\t';
                    case 'b': return '\b';
                    case 'f': return '\f';
                    default:
                        throw new Exception($"Invalid escape sequence: '\\{escapeChar}'");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Error in ParseEscapeSequence: {ex.Message}"));
                throw; // Re-throw the exception after logging
            }
        }

        /// <summary>
        /// Parses a JSON string.
        /// </summary>
        /// <returns>The parsed string.</returns>
        private string ParseString()
        {
            try
            {
                Consume('"'); // Expect an opening double quote

                StringBuilder sb = new StringBuilder();
                while (true)
                {
                    if (currentIndex >= json?.Length)
                        throw new Exception("Unexpected end of input while parsing string.");

                    char c = json[currentIndex];
                    if (c == '"')
                    {
                        currentIndex++;
                        SkipWhitespace();
                        return sb.ToString();
                    }
                    else if (c == '\\')
                    {
                        sb.Append(ParseEscapeSequence());
                    }
                    else
                    {
                        sb.Append(c);
                        currentIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Error in ParseString: {ex.Message}"));
                throw;
            }
        }

        private object ParseNumber()
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                // Read digits (including optional minus sign)
                if (Peek() == '-')
                {
                    sb.Append(Peek());
                    currentIndex++;
                }

                while (currentIndex < json?.Length && char.IsDigit(json[currentIndex]))
                {
                    sb.Append(json[currentIndex]);
                    currentIndex++;
                }

                // Read decimal part (if present)
                if (currentIndex < json?.Length && json[currentIndex] == '.')
                {
                    sb.Append(json[currentIndex]);
                    currentIndex++;

                    while (currentIndex < json.Length && char.IsDigit(json[currentIndex]))
                    {
                        sb.Append(json[currentIndex]);
                        currentIndex++;
                    }
                }

                // Parse the numeric value
                string numericString = sb.ToString();
                if (int.TryParse(numericString, out int intValue))
                    return intValue;
                else if (double.TryParse(numericString, out double doubleValue))
                    return doubleValue;
                else
                    throw new Exception($"Invalid numeric value: {numericString}");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Error in ParseNumber: {ex.Message}"));
                throw;
            }
        }

        /// <summary>
        /// Parses a JSON value, calls appropriate method based on first non whitespace character detected.
        /// </summary>
        /// <returns>The parsed value.</returns>
        private object ParseValue()
        {
            try
            {
                SkipWhitespace();
                char firstChar = Peek();

                if (char.IsDigit(firstChar) || firstChar == '-')
                    return ParseNumber();
                else if (firstChar == '"')
                    return ParseString();
                else if (firstChar == 't' || firstChar == 'f')
                    return ParseBoolean();
                else if (firstChar == 'n')
                    return ParseNull();
                else if (firstChar == '{')
                    return ParseObject();
                else if (firstChar == '[')
                    return ParseArray();
                else
                    throw new Exception($"Unexpected token: '{firstChar}'");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Error in ParseValue: {ex.Message}"));
                throw;
            }
        }

        /// <summary>
        /// Parses a JSON null value.
        /// </summary>
        /// <returns>The parsed null value.</returns>
        private object ParseNull()
        {
            try
            {
                Consume('n');
                Consume('u');
                Consume('l');
                Consume('l');
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Error in ParseNull: {ex.Message}"));
                throw;
            }
        }

        /// <summary>
        /// Parses a JSON array.
        /// </summary>
        /// <returns>The parsed array.</returns>
        private List<object> ParseArray()
        {
            try
            {
                var result = new List<object>();
                Consume('[');

                while (Peek() != ']')
                {
                    object value = ParseValue();
                    result.Add(value);

                    if (Peek() == ',')
                        Consume(',');
                    else if (Peek() != ']')
                        throw new Exception($"Expected ',' or ']', but found '{Peek()}'.");
                }

                Consume(']');
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Error in ParseArray: {ex.Message}"));
                throw;
            }
        }


        /// <summary>
        /// Parses a JSON boolean value.
        /// </summary>
        /// <returns>The parsed boolean value.</returns>
        private bool ParseBoolean()
        {
            try
            {
                if (Peek() == 't')
                {
                    Consume('t');
                    Consume('r');
                    Consume('u');
                    Consume('e');
                    return true;
                }
                else if (Peek() == 'f')
                {
                    Consume('f');
                    Consume('a');
                    Consume('l');
                    Consume('s');
                    Consume('e');
                    return false;
                }
                else
                    throw new Exception($"Expected 'true' or 'false', but found '{Peek()}'.");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Error in ParseBoolean: {ex.Message}"));
                throw;
            }
        }
    }
}
