# JSON Parser
---
A lightweight JSON parser written in C# that can parse JSON strings into .NET objects.

Table of Contents
- Introduction
- Features
- Usage
- Methods
- Error Handling
- Performance
- Limitations
- Future Development
- License

**Introduction**

JSON (JavaScript Object Notation) is a lightweight data interchange format that is widely used in web and mobile applications. This JSON parser is designed to parse JSON strings into .NET objects, making it easy to work with JSON data in C# applications.

**Features**

Parses JSON objects into .NET dictionaries
Parses JSON arrays into .NET lists
Supports JSON strings, numbers, booleans, and null values
Handles escape sequences in JSON strings
Throws exceptions for invalid JSON syntax

**Usage**

To use the JSON parser, simply create an instance of the JsonParser class and call the Parse method, passing in the JSON string to be parsed.

``` C#
JsonParser parser = new JsonParser();
string jsonString = "{\"name\":\"John\",\"age\":30,\"city\":\"New York\"}";
Dictionary<string, object> result = parser.Parse(jsonString);
Console.WriteLine(result["name"]); // Output: John
Console.WriteLine(result["age"]); // Output: 30
Console.WriteLine(result["city"]); // Output: New York
```

**Methods**

* Parse(string jsonString)
Parses a JSON string into a .NET object.

* ParseObject()
Parses a JSON object into a .NET dictionary.

* ParseArray()
Parses a JSON array into a .NET list.

* ParseString()
Parses a JSON string.

* ParseNumber()
Parses a JSON number.

* ParseBoolean()
Parses a JSON boolean value.

* ParseNull()
Parses a JSON null value.

* Consume(char expected)
Consumes a character from the JSON string.

* Peek()
Peeks at the next character in the JSON string.

* SkipWhitespace()
Skips whitespace characters in the JSON string.

**Error Handling**

The JSON parser throws exceptions for invalid JSON syntax. The exceptions are thrown with a descriptive error message that indicates the position of the error in the JSON string.

**Performance**

The JSON parser is designed to be lightweight and efficient. It uses a simple recursive descent parsing algorithm that minimizes the number of allocations and copies.

**Limitations**

This parser does not support JSON comments.
This parser does not perform any validation beyond what is required to parse the JSON string.

**Future Development**

Add support for JSON comments and trailing commas.
Improve performance by using a more efficient parsing algorithm.
Add support for parsing JSON schema.

**License**

This software is released under the MIT License. See the LICENSE file for details.
