<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>TextCopy</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#define TRACE

void Main() => """
	   6.7.0-dev.52
""".Trim().Apply(
	async x => OverrideUnoFromSdkWithNugetApi(x)
	//OverrideUnoFromSdk
	//OverrideUnoAnyFromSdk
	//OverrideUnoCore
);

async Task OverrideUnoFromSdkWithNugetApi(string sdkVersion)
{
	var sdkReadme = await NugetApi.GetReadme("uno.sdk", sdkVersion);
	var sdkJson = Regex.Match(sdkReadme, @"```json(?<json>.+)```", RegexOptions.Singleline).Groups["json"].Value;
	var sdkPackages = JsonConvert.DeserializeObject<UnoPackageInfo[]>(sdkJson);
	var unoVersion = sdkPackages?.FirstOrDefault(x => x.Group == "Core")?.Version ?? throw default!;
	
	var unoNuspec = await NugetApi.GetNuspec("uno.winui", unoVersion);
	using var unoXmlReader = new XmlTextReader(unoNuspec, XmlNodeType.Document, context: null) { Namespaces = false };
	var unoPackage = XElement.Load(unoXmlReader);
	var unoHash = unoPackage.XPathSelectElement(".//repository")?.Attribute("commit")?.Value!;

	Console.WriteLine();

	Util.Metatext("click to copy").Dump();
	Clickable.CopyText(unoVersion).Dump();
	Clickable.CopyText(unoHash).Dump();
	Clickable.CopyText($"git checkout {unoHash}").Dump();
	Clickable.CopyText(new XElement("UnoNugetOverrideVersion", unoVersion).ToString()).Dump();

	Console.WriteLine();
	// you need to fetch up to that hash at least, not sure if need to checkout
	Util.Cmd($@"git -C D:\code\uno\framework\Uno show {unoHash} --oneline");
	Clickable.Cmd($@"git -C D:\code\uno\framework\Uno log {unoHash}~20..{unoHash} --oneline").Dump();
	Clickable.Cmd($@"git -C D:\code\uno\framework\Uno log {unoHash}~20..{unoHash} --oneline --merges").Dump();
}

void OverrideUnoFromSdk(string sdkVersion)
{
	if (LookupUnoVersion(sdkVersion) is { } version && 
		LookupUnoCommitHash(version) is { } hash)
	{
		Console.WriteLine();
		
		Util.Metatext("click to copy").Dump();
		Clickable.CopyText(version).Dump();
		Clickable.CopyText(hash).Dump();
		Clickable.CopyText($"git checkout {hash}").Dump();
		Clickable.CopyText(new XElement("UnoNugetOverrideVersion", version).ToString()).Dump();
	}
}
void OverrideUnoAnyFromSdk(string sdkVersion)
{
	var packages = ParseUnoSdkPackages(sdkVersion);
	if (packages is null) throw null!;
	
	Util.AutoScrollResults = false;
	"Core,Themes,Toolkit".Split(',')
		.Aggregate(packages.OrderBy(_ => 0), (acc, x) => acc.ThenByDescending(y => y.Group == x), acc => acc)
		
		.ThenBy(x => x.Group)
		.Dump("groups", 1);
}
void OverrideUnoCore(string version)
{
	if (LookupUnoCommitHash(version) is not { } hash)
		throw new Exception();
	
	Console.WriteLine();
	Util.Metatext("click to copy").Dump();
	Clickable.CopyText(version).Dump();
	Clickable.CopyText(hash).Dump();
	Clickable.CopyText($"git checkout {hash}").Dump();
	Clickable.CopyText(new XElement("UnoNugetOverrideVersion", version).ToString()).Dump();
}

public static class Config
{
	public static string NugetCacheDirectory = GetNugetCacheDirectory();

	private static string GetNugetCacheDirectory()
	{
		return Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
			".nuget", "packages"
		);
	}
}

public record UnoPackageInfo(string Group, string Version, string[] Packages);

public static class NugetApi
{
	public static Task<string> GetNuspec(string package, string version)
	{
		return new HttpClient().GetStringAsync($"https://api.nuget.org/v3-flatcontainer/{package}/{version}/{package}.nuspec".DumpTrace());
	}
	public static Task<string> GetReadme(string package, string version)
	{
		return new HttpClient().GetStringAsync($"https://api.nuget.org/v3-flatcontainer/{package}/{version}/readme".DumpTrace());
	}
}

UnoPackageInfo[]? ParseUnoSdkPackages(string version)
{
	var directory = Path.Combine(Config.NugetCacheDirectory, "uno.sdk", version);
	var path = Path.Combine(directory, "targets", "netstandard2.0", "packages.json");
	
	Util.Metatext($"parsing: {path}").DumpTrace();
	return JArray.Parse(File.ReadAllText(path))
		.ToObject<UnoPackageInfo[]>();
}

string? LookupUnoVersion(string sdkVersion)
{
	var directory = Path.Combine(Config.NugetCacheDirectory, "uno.sdk", sdkVersion);
	var path = Path.Combine(directory, "Sdk", "Sdk.props");

	Util.Metatext($"parsing: {path}").DumpTrace();
	var sdkProps = LoadXmlWithoutNamespaces(path);
	
	return sdkProps.XPathSelectElement(".//UnoVersion").DumpTrace()?.Value.DumpTrace();
}
string? LookupUnoCommitHash(string unoVersion)
{
	var directory = Path.Combine(Config.NugetCacheDirectory, "uno.winui", unoVersion);
	var path = Path.Combine(directory, "uno.winui.nuspec");
	
	Util.Metatext($"parsing: {path}").DumpTrace();
	var nuspec = LoadXmlWithoutNamespaces(path);
	
	return nuspec.XPathSelectElement(".//repository").DumpTrace()?.Attribute("commit")?.Value.DumpTrace();
}

XElement LoadXmlWithoutNamespaces(string path)
{
	using var reader = new XmlTextReader(path);
	reader.Namespaces = false;

	return XElement.Load(reader);
}

public static class Clickable
{
	public static Hyperlinq Create(string header, Action action) => new Hyperlinq(action, header);
	public static Hyperlinq Create<T>(string header, Func<T> func) => new Hyperlinq(() => func(), header);
	
	public static Hyperlinq Cmd(string command) => Cmd(command, command);
	public static Hyperlinq Cmd(string header, string command)
	{
		return Create(header, () => Util.Cmd(command));
	}

	public static Hyperlinq CopyText(string text) => CopyText(text, text);
	public static Hyperlinq CopyText(string header, string text)
	{
		return Create(header, () => TextCopy.ClipboardService.SetText(text));
	}
}
public static class FluentExtensions
{
	public static void Apply<T>(this T left, Action<T> apply)
	{
		apply(left);
	}
}