<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Configuration.Ini</NuGetReference>
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <NuGetReference>NetEscapades.Configuration.Yaml</NuGetReference>
  <Namespace>Microsoft.Extensions.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
</Query>

#nullable enable

private static void Main() => TimesheetScript.BuildServiceProvider()
	.GetService<ScriptConfig>()
	.Dump(1);
		
public partial class TimesheetScript
{
	static partial void InitializeAdditionalServices(IServiceCollection builder);
	internal static IServiceProvider BuildServiceProvider()
	{
		var builder = new ServiceCollection()
			.AddSingleton<IConfiguration>(LoadConfiguration)
			.AddSingleton<ScriptConfig>()
			.AddSingleton<HarvestCredential>(provider => provider.GetRequiredService<ScriptConfig>().Harvest);
		InitializeAdditionalServices(builder);
		
		return builder.BuildServiceProvider();
	}

	private static IConfiguration LoadConfiguration(IServiceProvider provider) => new ConfigurationBuilder()
		.SetBasePath(Util.CurrentQuery.Location)
		.AddIniFile("duration calculator.ini", optional: false, true)
		
		.Build();
}

// Define other methods, classes and namespaces here
public class ScriptConfig
{
	public HarvestCredential Harvest { get; }
	public IReadOnlyDictionary<string, string> GithubMappings { get; }

	public ScriptConfig(IConfiguration config)
	{
		//config.GetSection("test").GetChildren().Dump();
		
		Harvest = new(config.GetSection(nameof(Harvest)));
		
		GithubMappings = config.GetSection(nameof(GithubMappings)).GetChildren()
			.SelectMany(x => x.Value!.Split(',', StringSplitOptions.TrimEntries).Select(y => new { Repo = x.Key, Alias = y }))
			.GroupBy(x => x.Alias, (k, g) =>
			{
				var collisions = g.ToArray();
				if (collisions.Length > 1)
				{
					Util.WithStyle($"warn: Alias '{k}' is used for multiple repositories: {string.Join(", ", collisions.Select(x => x.Repo))} (last one win)", "color: orange").Dump();
				}
				
				return collisions[^1];
			})
			.ToDictionary(x => x.Alias is (null or "*") ? string.Empty : x.Alias, x => x.Repo.ToLowerInvariant());
	}
}
public class HarvestCredential
{
	public string? AccountID { get; init; }
	public MaskedPassword? PAT { get; init; }
	
	internal HarvestCredential(IConfigurationSection section)
	{
		AccountID = section[nameof(AccountID)];
		PAT = section[nameof(PAT)];
	}
}

public class MaskedPassword // to prevent accidental dumping
{
	private string password;

	public MaskedPassword(string value) => password = value;
	
	[return: NotNullIfNotNull(nameof(value))]
	public static implicit operator MaskedPassword?(string? value) => value is { } ? new MaskedPassword(value) : null;
	
	public string Get() => password;
	
	private object ToDump() => Util.OnDemand(new string('*', password.Length), Get);
}