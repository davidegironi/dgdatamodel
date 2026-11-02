[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$ContextFile,

    [Parameter(Mandatory = $true)]
    [string]$ConnectionStringName,

    [string]$AdditionalOnConfiguringCode = ''
)

$ErrorActionPreference = 'Stop'

try {
    $contextPath = [System.IO.Path]::GetFullPath($ContextFile)
    if (-not [System.IO.File]::Exists($contextPath)) {
        throw "Scaffolded context not found: $contextPath"
    }

    $content = [System.IO.File]::ReadAllText($contextPath)
    $newline = if ($content.Contains("`r`n")) { "`r`n" } else { "`n" }
    $escapedConnectionStringName = $ConnectionStringName.Replace('\', '\\').Replace('"', '\"')

    $onModelCreatingPattern = [regex]'(?m)^(?<indent>[ \t]*)protected override void OnModelCreating\(ModelBuilder modelBuilder\)[ \t]*\r?$'
    $onModelCreatingMatches = $onModelCreatingPattern.Matches($content)
    if ($onModelCreatingMatches.Count -ne 1) {
        throw "Expected one OnModelCreating method in '$contextPath'; found $($onModelCreatingMatches.Count)."
    }

    $onConfiguringPattern = [regex]'(?m)^[ \t]*protected override void OnConfiguring\(DbContextOptionsBuilder optionsBuilder\)'
    $onConfiguringMatches = $onConfiguringPattern.Matches($content)
    if ($onConfiguringMatches.Count -gt 1) {
        throw "Found multiple OnConfiguring methods in '$contextPath'."
    }

    $indent = $onModelCreatingMatches[0].Groups['indent'].Value
    $connectionStringExpression = 'System.Configuration.ConfigurationManager.AppSettings["' + $escapedConnectionStringName + '"]'
    $onConfiguringMethod = @(
        ($indent + 'protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)'),
        ($indent + '{'),
        ($indent + '    if (!optionsBuilder.IsConfigured)'),
        ($indent + '    {'),
        ($indent + '        optionsBuilder.UseSqlServer(' + $connectionStringExpression + ');'),
        ($indent + '    }'),
        ($indent + '}')
    ) -join $newline

    if ($onConfiguringMatches.Count -eq 1) {
        if (-not $content.Contains($connectionStringExpression)) {
            throw "OnConfiguring already exists in '$contextPath'; scaffold with --no-onconfiguring before finalizing."
        }
    }
    else {
        $match = $onModelCreatingMatches[0]
        $additionalCode = $AdditionalOnConfiguringCode.Replace('\r\n', $newline).Replace('\n', $newline).Replace('\r', $newline)
        $insertion = $onConfiguringMethod + $newline
        if (-not [string]::IsNullOrWhiteSpace($additionalCode)) {
            $insertion += $additionalCode.Trim("`r", "`n") + $newline
        }
        $insertion += $newline
        $content = $content.Insert($match.Index, $insertion)
    }

    $contextName = [System.IO.Path]::GetFileNameWithoutExtension($contextPath)
    $escapedContextName = [regex]::Escape($contextName)
    $parameterlessConstructorPattern = [regex]("(?m)^[ \t]*public\s+" + $escapedContextName + "\s*\(\s*\)")
    if (-not $parameterlessConstructorPattern.IsMatch($content)) {
        $onConfiguringMatch = $onConfiguringPattern.Match($content)
        $constructorIndent = $onModelCreatingPattern.Match($content).Groups['indent'].Value
        $constructor = @(
            ($constructorIndent + 'public ' + $contextName + '()'),
            ($constructorIndent + '{'),
            ($constructorIndent + '}'),
            ''
        ) -join $newline
        $content = $content.Insert($onConfiguringMatch.Index, $constructor)
    }

    $namedIndexPattern = [regex]'(?m)^(?<indent>[ \t]*)entity\.HasIndex\((?<expression>.*=>.*),\s*"(?<name>[^"]+)"\)'
    $namedIndexMatches = $namedIndexPattern.Matches($content)
    for ($i = $namedIndexMatches.Count - 1; $i -ge 0; $i--) {
        $match = $namedIndexMatches[$i]
        $indexIndent = $match.Groups['indent'].Value
        $expression = $match.Groups['expression'].Value
        $indexName = $match.Groups['name'].Value.Replace('\', '\\').Replace('"', '\"')
        $replacement = @(
            ($indexIndent + 'entity.HasIndex(' + $expression + ')'),
            ($indexIndent + '#if NETFRAMEWORK'),
            ($indexIndent + '    .HasName("' + $indexName + '")'),
            ($indexIndent + '#else'),
            ($indexIndent + '    .HasDatabaseName("' + $indexName + '")'),
            ($indexIndent + '#endif'),
            ($indexIndent + '    ')
        ) -join $newline
        $content = $content.Remove($match.Index, $match.Length).Insert($match.Index, $replacement)
    }

    $collationPattern = [regex]'(?m)^(?<indent>[ \t]*)modelBuilder\.UseCollation\([^;\r\n]*\);[ \t]*$'
    $collationMatches = $collationPattern.Matches($content)
    for ($i = $collationMatches.Count - 1; $i -ge 0; $i--) {
        $match = $collationMatches[$i]
        $prefix = $content.Substring(0, $match.Index)
        $suffix = $content.Substring($match.Index + $match.Length)
        $alreadyWrapped = [regex]::IsMatch($prefix, '(?s)(?:\A|\r?\n)[ \t]*#if !NETFRAMEWORK[ \t]*\r?\n[ \t]*\z') -and
            [regex]::IsMatch($suffix, '\A\r?\n[ \t]*#endif(?:\r?\n|\z)')
        if (-not $alreadyWrapped) {
            $indent = $match.Groups['indent'].Value
            $replacement = @(
                ($indent + '#if !NETFRAMEWORK'),
                $match.Value.TrimEnd(),
                ($indent + '#endif')
            ) -join $newline
            $content = $content.Remove($match.Index, $match.Length).Insert($match.Index, $replacement)
        }
    }

    if ([regex]::IsMatch($content, '(?m)entity\.HasIndex\(.*=>.*,\s*"[^"]+"\)')) {
        throw "A named HasIndex call was not converted in '$contextPath'."
    }

    [System.IO.File]::WriteAllText($contextPath, $content, [System.Text.UTF8Encoding]::new($false))
    Write-Host "Finalized context: $contextPath"
}
catch {
    [Console]::Error.WriteLine($_.Exception.Message)
    exit 1
}
