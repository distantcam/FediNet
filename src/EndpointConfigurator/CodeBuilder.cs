﻿using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace EndpointConfigurator;

public class CodeBuilder
{
    private readonly StringBuilder _stringBuilder = new();
    private int _indent = 0;

    public void StartBlock()
    {
        AppendLine("{");
        IncreaseIndent();
    }
    public void EndBlock()
    {
        DecreaseIndent();
        AppendLine("}");
    }
    public void IncreaseIndent() => _indent++;
    public void DecreaseIndent() => _indent--;
    public void AppendLine(string line) => _stringBuilder.AppendLine(new string('\t', _indent) + line);
    public void AppendLines(IEnumerable<string> lines)
    {
        foreach (var line in lines)
            AppendLine(line.TrimEnd('\r'));
    }
    public override string ToString() => _stringBuilder.ToString();
    public SourceText ToSourceText(Encoding? encoding = null) => SourceText.From(_stringBuilder.ToString(), encoding);
}
