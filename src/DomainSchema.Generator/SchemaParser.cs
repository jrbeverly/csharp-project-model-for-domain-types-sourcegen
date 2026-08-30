using System;
using System.Collections.Generic;

namespace DomainSchema.Generator;

internal sealed class DomainType
{
    public string Name = "";
    public List<DomainField> Fields = new List<DomainField>();
    public List<DomainRelationship> Relationships = new List<DomainRelationship>();
}

internal sealed class DomainField
{
    public string Name = "";
    public string Kind = "";
    public DomainConstraint? Constraint;
    public bool ExcludeFromProjection;
}

internal sealed class DomainConstraint
{
    public string Kind = "";
    public int RangeMin;
    public int RangeMax;
}

internal sealed class DomainRelationship
{
    public string Name = "";
    public string Target = "";
    public string Cardinality = "";
}

internal static class SchemaParser
{
    public static DomainType? Parse(string text)
    {
        var result = new DomainType();
        var section = Section.None;
        DomainField? currentField = null;
        DomainRelationship? currentRel = null;
        bool inConstraint = false;

        foreach (var rawLine in text.Split('\n'))
        {
            var line = rawLine.TrimEnd('\r');
            if (string.IsNullOrWhiteSpace(line)) continue;
            var trimmed = line.TrimStart();
            if (trimmed.StartsWith("#")) continue;

            int indent = line.Length - trimmed.Length;

            if (indent <= 4) inConstraint = false;

            if (indent == 0)
            {
                if (trimmed.StartsWith("type: "))
                {
                    result.Name = trimmed.Substring("type: ".Length).Trim();
                }
                else if (trimmed == "fields:")
                {
                    Flush(result, ref currentField, ref currentRel);
                    section = Section.Fields;
                }
                else if (trimmed == "relationships:")
                {
                    Flush(result, ref currentField, ref currentRel);
                    section = Section.Relationships;
                }
            }
            else if (indent == 2 && trimmed.StartsWith("- name: "))
            {
                Flush(result, ref currentField, ref currentRel);
                var name = trimmed.Substring("- name: ".Length).Trim();
                if (section == Section.Fields)
                    currentField = new DomainField { Name = name };
                else if (section == Section.Relationships)
                    currentRel = new DomainRelationship { Name = name };
            }
            else if (indent == 4)
            {
                if (section == Section.Fields && currentField != null)
                {
                    if (trimmed.StartsWith("type: "))
                        currentField.Kind = trimmed.Substring("type: ".Length).Trim();
                    else if (trimmed == "constraint:")
                        inConstraint = true;
                    else if (trimmed == "exclude_from_projection: true")
                        currentField.ExcludeFromProjection = true;
                }
                else if (section == Section.Relationships && currentRel != null)
                {
                    if (trimmed.StartsWith("target: "))
                        currentRel.Target = trimmed.Substring("target: ".Length).Trim();
                    else if (trimmed.StartsWith("cardinality: "))
                        currentRel.Cardinality = trimmed.Substring("cardinality: ".Length).Trim();
                }
            }
            else if (indent == 6 && inConstraint && currentField != null)
            {
                if (trimmed == "non_empty: true")
                {
                    currentField.Constraint = new DomainConstraint { Kind = "non_empty" };
                }
                else if (trimmed.StartsWith("range:"))
                {
                    currentField.Constraint = ParseRange(trimmed);
                }
            }
        }

        Flush(result, ref currentField, ref currentRel);
        return string.IsNullOrEmpty(result.Name) ? null : result;
    }

    static void Flush(DomainType result, ref DomainField? field, ref DomainRelationship? rel)
    {
        if (field != null) { result.Fields.Add(field); field = null; }
        if (rel != null) { result.Relationships.Add(rel); rel = null; }
    }

    static DomainConstraint? ParseRange(string content)
    {
        // range: { min: 0, max: 150 }
        int open = content.IndexOf('{');
        int close = content.IndexOf('}');
        if (open < 0 || close <= open) return null;

        var inner = content.Substring(open + 1, close - open - 1);
        int min = 0, max = 0;
        foreach (var part in inner.Split(','))
        {
            var kv = part.Trim();
            if (kv.StartsWith("min: ") && int.TryParse(kv.Substring("min: ".Length).Trim(), out int v))
                min = v;
            else if (kv.StartsWith("max: ") && int.TryParse(kv.Substring("max: ".Length).Trim(), out v))
                max = v;
        }
        return new DomainConstraint { Kind = "range", RangeMin = min, RangeMax = max };
    }

    enum Section { None, Fields, Relationships }
}
