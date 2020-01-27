using System;
using System.Collections.Generic;
using System.Text;

namespace Aidbox.Generator
{
    public class AttributeDescription
    {
        public List<string> Path { get; set; }
        public string Type { get; set; }
        public List<string> Union { get; set; }
        public List<string> Refers { get; set; }
        public bool IsCollection { get; set; }
        public bool IsRequired { get; set; }
        public bool IsSummary { get; set; }
        public int? Order { get; set; }
        public string ValueSet { get; set; }

        public List<AttributeDescription> BackboneDescription { get; set; }

        public TypeDescription AttributeTypeDescription { get; set; }

        public AttributeDescription()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is AttributeDescription other &&
                   EqualityComparer<List<string>>.Default.Equals(Path, other.Path) &&
                   Type == other.Type &&
                   EqualityComparer<List<string>>.Default.Equals(Union, other.Union);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Path, Type, Union);
        }
        public override string ToString()
        {
            return $"{{[{string.Join(", ", Path)}], {Type}, {ValueSet}, [{string.Join(", ", Union ?? new List<string>())}], [{string.Join(", ", Refers ?? new List<string>())}]}}";
        }

        public string Name()
        {
            var p = Path[0].Replace("-", "_");
            return cSharpKeywords.Contains(p) ? $"_{p}" : p;
        }

        public bool IsDate()
        {
            return Type == "date";
        }

        public string GetDotNetType()
        {
            var t = GetPrimitiveType(Type ?? "BaseClass");

            if (t != "string" && t != Type && IsRequired && !IsCollection)
            {
                t = $"{t}?";
            }

            return t;
        }

        public string GetDotNetCollectionType()
        {
            if (IsCollection)
                return $"List<{GetDotNetType()}>";
            return GetDotNetType();
        }

        public string GetPrimitiveType(string type)
        {
            switch (type)
            {
                case "unsignedInt":
                    return "uint";
                case "markdown":
                    return "string";
                case "email":
                    return "string";
                case "date":
                    return "DateTime";
                case "instant":
                    return "DateTimeOffset";
                case "number":
                    return "int";
                case "password":
                    return "string";
                case "time":
                    return "DateTime";
                case "base64Binary":
                    return "byte[]";
                case "string":
                    return "string";
                case "secret":
                    return "string";
                case "dateTime":
                    return "DateTime";
                case "integer":
                    return "int";
                case "oid":
                    return "string";
                case "keyword":
                    return "string";
                case "decimal":
                    return "decimal";
                case "id":
                    return "string";
                case "canonical":
                    return "string";
                case "url":
                    return "string";
                case "code":
                    return "string";
                case "positiveInt":
                    return "uint";
                case "uri":
                    return "string";
                case "uuid":
                    return "Guid";
                case "xhtml":
                    return "string";
                case "boolean":
                    return "bool";

                default:
                    return type;
                    //throw new NotImplementedException($"Unknow type [{type}]");
            }
        }

        private readonly List<string> cSharpKeywords = new List<string>
        {
            "event",
            "operator",
            "base",
            "public",
            "abstract",
            "enum",
            "class",
            "params",
            "as",
            "for",
            "implicit",
            "ref",
            "fixed"
        };
    }
}
