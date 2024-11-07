<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Configuration.Ini</NuGetReference>
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Microsoft.Extensions.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Security</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
</Query>

#nullable enable

private static void Main() =>
	//DumpConfigAndSecrets();
	SetSecrets();
	//Test();

private static void DumpConfigAndSecrets()
{
	var provider = TimesheetScript.BuildServiceProvider();

	provider.GetRequiredService<ScriptConfig>()
		.Dump(1);

	provider.GetRequiredService<IPersistenceService>()
		.Read<ScriptSecrets>()
		?.Declassify()
		.Dump(1);
}
private static void SetSecrets()
{
	var provider = TimesheetScript.BuildServiceProvider();
	var persistence = provider.GetRequiredService<IPersistenceService>();
	var persistableSecrets = persistence.Get<ScriptSecrets>();
	
	var dumpster = new DumpContainer().Dump(persistableSecrets.GetSourceDescription());
	Refresh();

	void Refresh()
	{
		var state = persistableSecrets.Read();
		if (state is not { })
		{
			dumpster.Content = Util.VerticalRun(
				Util.Metatext($"No saved ScriptSecrets found: {persistableSecrets.GetSourceDescription()}"),
				new Hyperlinq(CreateNewSecrets, "Create new secrets")
			);
			return;
		}

		var editables = new List<object>();
		void AddProperty<T>(string Name, Func<T> Get, Action<T> Set, Func<T, T> Edit, Func<T, object>? CustomViewer = null)
		{
			editables.Add(new
			{
				Property = Name,
				Value = CustomViewer is { } ? CustomViewer(Get()) : Get(),
				Edit = new Hyperlinq(() => { Set(Edit(Get())); Refresh(); }, "Edit"),
			});
		}
		AddProperty<string?>("HarvestAccountID", () => state.HarvestAccountID, x => persistence.Write(state with { HarvestAccountID = x }), x => Util.ReadLine("HarvestAccountID", x));
		AddProperty<SecureString?>("HarvestPAT", () => state.HarvestPAT, x => persistence.Write(state with { HarvestPAT = x }), x => Util.ReadLine("HarvestAccountID", x.MarshalToString()).ToSecureString(), x => new MaskedNotSoSecureString(x));

		dumpster.Content = editables;
	}
	void CreateNewSecrets()
	{
		if (persistableSecrets.Read() is not { })
		{
			persistableSecrets.Write(ScriptSecrets.Empty);
		}
		
		Refresh();
	}
}
private static void Test()
{
	//TimesheetScript.BuildServiceProvider()
	//	.GetService<ScriptConfig>()
	//	.Dump(1);

	//var json = JsonConvert.SerializeObject(new ScriptSecrets("qwe", "asd".ToSecureString())).Dump();
	//var secrets = JsonConvert.DeserializeObject<ScriptSecrets>(json).Dump()!;
	//secrets.PAT.MarshalToString().Dump();

	//var provider = TimesheetScript.BuildServiceProvider();
	//var persistence = provider.GetRequiredService<IPersistenceService>();
	//File.Delete(Path.Combine(Util.CurrentQuery.Location, "duration calculator.secrets"));
	//var secrets = persistence.Read<ScriptSecrets>().Dump();
	//persistence.Write<ScriptSecrets>(new("qwe", "asd".ToSecureString()));
	//persistence.Read<ScriptSecrets>().Dump();
	//File.ReadAllText(Path.Combine(Util.CurrentQuery.Location, "duration calculator.secrets")).Dump();

	//new ScriptSecrets(null, null).Declassify().Dump();
	//new ScriptSecrets("qwe", "asd".ToSecureString()).Declassify().Dump();
}

public partial class TimesheetScript
{
	static partial void InitializeAdditionalServices(IServiceCollection builder);
	internal static IServiceProvider BuildServiceProvider()
	{
		var builder = new ServiceCollection()
			.AddSingleton<IConfiguration>(LoadConfiguration)
			.AddSingleton<IPersistenceService>(CreatePersistenceService)
			.AddSingleton<ScriptConfig>();
		InitializeAdditionalServices(builder);

		return builder.BuildServiceProvider();
	}

	private static IConfiguration LoadConfiguration(IServiceProvider provider) => new ConfigurationBuilder()
		.SetBasePath(Util.CurrentQuery.Location)
		.AddIniFile("duration calculator.ini", optional: false, true)
		.Build();

	private static IPersistenceService CreatePersistenceService(IServiceProvider provider) => new PersistenceServiceBuilder()
		.AddJsonFile<ScriptSecrets>(Path.Combine(Util.CurrentQuery.Location, "duration calculator.secrets"))
		.Build();
}

// Define other methods, classes and namespaces here
public class ScriptConfig
{
	private IPersistenceService _persistence;

	public ScriptSecrets? Secrets
	{
		get => _persistence.Read<ScriptSecrets>();
		set => _persistence.Write<ScriptSecrets>(value);
	}
	public IReadOnlyDictionary<string, string> GithubMappings { get; }

	public ScriptConfig(IConfiguration config, IPersistenceService persistence)
	{
		_persistence = persistence;

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
public record ScriptSecrets(string? HarvestAccountID, [property: JsonConverter(typeof(SecureStringConverter))] SecureString? HarvestPAT)
{
	public static ScriptSecrets Empty => new(null, null);

	public record DeclassifiedScriptSecrets(string? HarvestAccountID, MaskedNotSoSecureString? HarvestPAT);
	public DeclassifiedScriptSecrets Declassify() => new(HarvestAccountID, new(HarvestPAT));
}

public sealed class MaskedNotSoSecureString
{
	private readonly SecureString? _value;

	public MaskedNotSoSecureString(SecureString? value) => this._value = value;

	[return: NotNullIfNotNull(nameof(value))]
	public static implicit operator MaskedNotSoSecureString?(SecureString? value) => value is { } ? new MaskedNotSoSecureString(value) : null;

	private object? ToDump() => _value is { } ? Util.OnDemand(new string('*', _value.Length), () => _value.MarshalToString()) : default(object);
}

public class SecureStringConverter : JsonConverter
{
	public override bool CanConvert(Type objectType) => objectType == typeof(SecureString);

	public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
	{
		return reader.TokenType switch
		{
			JsonToken.Null => null,
			JsonToken.String => DPAPI.Decrypt((string?)reader.Value).ToSecureString(),

			_ => throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing SecureString"),
		};
	}
	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
	{
		if (value is null)
		{
			writer.WriteNull();
		}
		if (value is SecureString s)
		{
			writer.WriteValue(DPAPI.Encrypt(s.MarshalToString()));
		}
		else
		{
			throw new JsonSerializationException("Expected SecureString object value");
		}
	}
}

#region Persistence
public interface IPersistableSource
{
	virtual string? GetSourceDescription() => ToString();
}
public interface IPersistableSource<T> : IPersistableSource where T : class
{
	T? Read();
	void Write(T? value);
}
public class PersistableJsonFile<T>(string Path, JsonSerializerSettings? Settings = null) : IPersistableSource<T> where T : class
{
	public PersistableJsonFile(string Path) : this(Path, new()) { }

	public T? Read()
	{
		if (!File.Exists(Path))
		{
			return default;
		}

		var json = File.ReadAllText(Path);
		var value = JsonConvert.DeserializeObject<T>(json, new SecureStringConverter());

		return value;
	}

	public void Write(T? value)
	{
		if (value != null)
		{
			var json = JsonConvert.SerializeObject(value, Settings);

			File.WriteAllText(Path, json);
		}
		else
		{
			File.Delete(Path);
		}
	}

	public override string? ToString() => $"PersistableJsonFile<{typeof(T).Name}>: {Path}";
}
public static class JsonFilePersistenceServiceBuilder
{
	public static IPersistenceServiceBuilder AddJsonFile<T>(this IPersistenceServiceBuilder builder, string path, string? key = null, JsonSerializerSettings? settings = null) where T : class
	{
		return builder.Add(new PersistableJsonFile<T>(path, settings), key);
	}
}

public interface IPersistenceServiceBuilder
{
	public IPersistenceServiceBuilder Add<T>(IPersistableSource<T> source, string? key = null) where T : class;
	public IPersistenceService Build();
}
public class PersistenceServiceBuilder : IPersistenceServiceBuilder
{
	private readonly Dictionary<(Type Type, string? Key), IPersistableSource> _sourceMap = new();

	public IPersistenceServiceBuilder Add<T>(IPersistableSource<T> source, string? key = null) where T : class
	{
		_sourceMap.Add((typeof(T), key), source);

		return this;
	}

	public IPersistenceService Build() => new PersistenceService(_sourceMap);
}

public interface IPersistenceService
{
	public IPersistableSource<T> Get<T>(string? key = null) where T : class;
}
internal class PersistenceService : IPersistenceService//, IMutablePersistenceService
{
	private readonly IReadOnlyDictionary<(Type Type, string? Key), IPersistableSource> _sourceMap;

	public PersistenceService(IReadOnlyDictionary<(Type Type, string? Key), IPersistableSource> sourceMap)
	{
		_sourceMap = sourceMap;
	}

	public IPersistableSource<T> Get<T>(string? key = null) where T : class
	{
		if (_sourceMap.TryGetValue((typeof(T), key), out var source))
		{
			return (IPersistableSource<T>)source;
		}

		throw new InvalidOperationException($"No persistable source for type '{typeof(T)}'(key={key}) has been registered.");
	}
}
public static class PersistenceServiceExtensions
{
	public static T? Read<T>(this IPersistenceService service, string? key = null) where T : class
	{
		return service.Get<T>(key).Read();
	}
	public static void Write<T>(this IPersistenceService service, T? value, string? key = null) where T : class
	{
		service.Get<T>(key).Write(value);
	}
}
#endregion

public static class DPAPI
{
	public static string? Encrypt(string? data, string? entropy = null, DataProtectionScope scope = DataProtectionScope.CurrentUser)
	{
		if (data == null) return null;

		var bytes = Encoding.UTF8.GetBytes(data);
		var entropyBytes = entropy?.Length > 0 ? Encoding.UTF8.GetBytes(entropy) : null;
		var encrypted = ProtectedData.Protect(bytes, entropyBytes, scope);
		var encryptedText = Convert.ToBase64String(encrypted);

		return encryptedText;
	}
	public static string? Decrypt(string? data, string? entropy = null, DataProtectionScope scope = DataProtectionScope.CurrentUser)
	{
		if (data == null) return null;

		var bytes = Convert.FromBase64String(data);
		var entropyBytes = entropy?.Length > 0 ? Encoding.UTF8.GetBytes(entropy) : null;
		var decrypted = ProtectedData.Unprotect(bytes, entropyBytes, scope);
		var decryptedText = Encoding.UTF8.GetString(decrypted);

		return decryptedText;
	}
}

public static class SecureStringExtensions
{
	public static SecureString? ToSecureString(this string? value)
	{
		if (value is null) return null;

		var result = new SecureString();
		foreach (var c in value)
		{
			result.AppendChar(c);
		}

		return result;
	}
	public static string? MarshalToString(this SecureString? value)
	{
		if (value is null) return null;
		if (value.Length == 0) return string.Empty;

		IntPtr handle = default;
		try
		{
			handle = Marshal.SecureStringToGlobalAllocUnicode(value);
			return Marshal.PtrToStringUni(handle);
		}
		finally
		{
			if (handle != default)
			{
				Marshal.ZeroFreeGlobalAllocUnicode(handle);
			}
		}
	}
}