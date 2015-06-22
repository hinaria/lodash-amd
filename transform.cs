// @require string sourceDirectory;
// @require string prefix;

var toModuleName = new Func<string, string, string>((basePath, path) => {
    if (!basePath.EndsWith("/") && !basePath.EndsWith("\\"))
        basePath += "/";

    var folder = new Uri(sourceDirectory);
    var file = new Uri(Path.GetFullPath(Path.Combine(basePath, path)));
    
    return prefix + "/" + Uri.UnescapeDataString(
        folder.MakeRelativeUri(file).ToString().Replace(Path.DirectorySeparatorChar, '/'));
});

var define = new Regex("^define\\(\\[(?<data>.*?)\n", RegexOptions.Compiled);
var module = new Regex("['\"](?<name>.*?)['\"]", RegexOptions.Compiled);

var files = Directory.GetFiles(sourceDirectory, "*.js", SearchOption.AllDirectories);

foreach (var file in files)
{
    var contents = File.ReadAllText(file);
    var match = define.Match(contents);
    var groups = match.Groups;

    if (match.Success == false)
    {
        Console.WriteLine($"non-matching file: {file}");
        continue;
    }

    var rewritten = define.Replace(contents, _ =>
    {
        var fileDirectory = Path.GetDirectoryName(file);
        var relativeName = toModuleName(sourceDirectory, file);
        var moduleName = relativeName.Substring(0, relativeName.Length - 3); // strip `.js`

        return $"define(\"{moduleName}\", ["
            + module.Replace(groups["data"].Value, x => "\"" + toModuleName(fileDirectory, x.Groups["name"].Value) + "\"");
    });

    File.WriteAllText(file, rewritten);
}