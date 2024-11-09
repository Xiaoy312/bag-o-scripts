<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>MoreLinq</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Web</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Dynamic</Namespace>
</Query>

#nullable enable
#load ".\duration calculator.config"
#define INJECT_EOD_FOR_CURRENT_DATETIME
//#define SORT_JSON_PROPERTY
#define DUMP_JSON_AS_OBJECT

public partial class TimesheetScript
{
	public static async Task Main() =>
		//LegacyMain();
		await NewMain();

	public static async Task NewMain()
	{
		//InitializeJsonSerializer();
		//TimesheetParsingTests();
		//TimesheetProcessingTests();
		//await HarvestApiTest();
		//ProcessTimesheet($@"D:\documents\timesheets\{DateHelper.ThisMonday.AddDays(-7):yyyyMMdd}");
		ProcessTimesheet($@"D:\documents\timesheets\{DateHelper.ThisMonday.AddDays(+0):yyyyMMdd}");
	}

	public static void LegacyMain()
	{
		var thisMonday = DateTime.Today.AddDays(DateTime.Today.DayOfWeek switch
		{
			DayOfWeek.Sunday => -6,
			_ => -(int)DateTime.Today.DayOfWeek + 1
		});

		//ProcessWeeklyTimesheet($@"D:\documents\timesheets\{thisMonday.AddDays(-14):yyyyMMdd}");
		//ProcessWeeklyTimesheet2($@"D:\documents\timesheets\{thisMonday.AddDays(-7):yyyyMMdd}", "0000000", "**************************************************************************************");
		//ProcessWeeklyTimesheet($@"D:\documents\timesheets\{thisMonday:yyyyMMdd}");
		//ProcessWeeklyTimesheet(@"D:\documents\timesheets\20221205");
		//ProcessWeeklyTimesheet($@"D:\documents\timesheets\{thisMonday.AddDays(-7):yyyyMMdd}", (category, raw) => "#13680".Split(',').Any(raw.Contains));
		//ProcessWeeklyTimesheet(@"D:\documents\timesheets\20231016", (category, raw) => "datacontext".Split(',').Any(raw.Contains));
		//ProcessWeeklyTimesheet(@"D:\documents\timesheets\20240729", (category, raw) => raw.Contains("17695"));
		//ProcessWeeklyTimesheet($@"D:\documents\timesheets\{thisMonday.AddDays(-7):yyyyMMdd}", (c, r) => c == "uno#w901");
		//ProcessWeeklyTimesheet($@"D:\documents\timesheets\{thisMonday:yyyyMMdd}", (c, r) => c == "kahua#199");
		//ProcessWeeklyTimesheet(@"D:\documents\timesheets\time-archives\20211213");
	}
}
/* todo:
 * [x] !!! force github item syntax, "unoplatform/uno#123: description..."
 * [x] support for '^' and '^n'
 * [x] in day/week report: % of an item
 * cross weeks search function
 * [x] refactor
 * [x] harvest api integration
 *		[x] auto linking issue
 * [ ] extract project and task id, and add mappings for them
 * [ ] view & delete existing timesheet from here
 * [x] add option to add single day update
 * [ ] discord linking
 * [ ] document timesheet syntax
 * [ ] harvest setup guide
 */

public partial class TimesheetScript // new
{
	static partial void InitializeAdditionalServices(IServiceCollection builder)
	{
		builder
			.AddSingleton<HarvestApiEndpoint>();
	}

	private static void TimesheetParsingTests()
	{
		Timesheet.Load($@"D:\documents\timesheets\{DateHelper.ThisMonday.AddDays(0):yyyyMMdd}");
		//Timesheet.Parse("""
		//	@20240101
		//	@asd
		//	@20231332
		//	1234 qwe
		//	2456 qwe
		//	1234
		//	1234 
		//	1260 asd
		//""");
	}
	private static void TimesheetProcessingTests()
	{
		var services = BuildServiceProvider();
		var config = services.GetRequiredService<ScriptConfig>();

		var sheet = Timesheet.Load($@"D:\documents\timesheets\{DateHelper.ThisMonday.AddDays(-7):yyyyMMdd}");
		sheet = sheet.ResolveRelativePointers();

		sheet.Dump(null, 0, exclude: "RawData");

		var report = TimeReport.GenerateFrom(sheet)
			.ExtractEventSources(config.GithubMappings);

		report.DumpSummary();
	}
	private static async Task HarvestApiTest()
	{
		var services = BuildServiceProvider();

		var harvest = services.GetRequiredService<HarvestApiEndpoint>();
		//await harvest.GetCurrentUserAsync().DumpJson(0).DumpContent();
		//await harvest.AddTimeEntry(new HarvestNewTimeEntry(00000000, 00000000, DateHelper.TodayOnly, 0.1, "test", HarvestExternalReference.FromUrl("https://help.getharvest.com/api-v2/timesheets-api/timesheets/time-entries/"))).Dump();

		var results = await harvest.GetProjectAssignmentsAsync().Unwrap();
		results.ProjectAssignments
			.SelectMany(pa => pa.TaskAssignments.Select(t => new
			{
				ProjectId = pa.Project.Id,
				ProjectCode = pa.Project.Code,
				ProjectName = pa.Project.Name,
				TaskId = t.Task.Id,
				TaskName = t.Task.Name,
			}))
			.Dump();

		//var persistence = services.GetRequiredService<IPersistenceService>();
		//(persistence.Read<ScriptSecrets>().Dump()?.Declassify()).Dump();
		
		//var settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
		//JsonConvert.SerializeObject(new ScriptSecrets("qwe", "asd".ToSecureString()), settings).DumpTell();
	}
	private static void ProcessTimesheet(string path)
	{
		var services = BuildServiceProvider();
		var config = services.GetRequiredService<ScriptConfig>();

		Util.Metatext($"loading timesheet: {path}").Dump();
		var sheet = Timesheet.Load(path)
			.ResolveRelativePointers()
			.Dump(null, 0);
		var report = TimeReport.GenerateFrom(sheet)
			.ExtractEventSources(config.GithubMappings)
			.Dump(null, 0);

		report.DumpSummary();

		var date = report.Items.Min(x => x.Date);
		var relativeTime = date.GetWeeksFrom(DateHelper.TodayOnly) switch
		{
			var n when n < -1 => $"{-n} weeks ago",
			-1 => "last week",
			0 => "this week",
			1 => "next week",
			var n when n > 1 => $"{n} weeks from now",
			_ => "?",
		};
		
		Util.VerticalRun(
			Util.HorizontalRun(true, report.Items
				.Select(x => x.Date)
				.Distinct()
				.Select(x => BuildHarvestSyncClickable($"{x:dddd MMdd}", y => y.Date == x))
			),
			BuildHarvestSyncClickable("Everything", x => true)
		).Dump($"Sync this timesheet to harvest?: {path} (from {relativeTime})");
		
		Hyperlinq BuildHarvestSyncClickable(string header, Func<TimeReport.TaskItem, bool> filter)
		{
			return Clickable.Create(header, async () =>
			{
				if (SafetyCheck($"Sync {header.RegexReplace("^E", "e")} to harvest?"))
				{
					var harvest = services.GetRequiredService<HarvestApiEndpoint>();
					foreach (var item in report.Items.Where(filter))
					{
						await harvest.AddTimeEntry(new HarvestNewTimeEntry(
							34393204, // extract to config or add mapping
							19505120, // extract to config or add mapping
							item.Date,
							Math.Max(0.01, item.Duration.TotalHours), // 0 or unspecified will immediately start an timer
							$"{item.EventSource?.ToString() ?? "no-event-source"}: {(item.EventSource?.Source == item.Category ? item.Text.Substring(item.Category.Length + 1).Trim() : item.Text)}",
							item.EventSource?.ToHarvestExternalReference()
						));
					}
				}
			});
		}
	}

	private static bool SafetyCheck(string prompt, bool @throw = true)
	{
		var random = new Random();

		var operands = new int[] { random.Next(1, 9), random.Next(1, 9) };
		var answer = Util.ReadLine($"Answer ({operands.JoinBy('+')}) to continue: {prompt}".Dump()).Dump();
		var result = answer == operands.Sum().ToString();
		if (!result)
		{
			Util.HorizontalRun(true,
				"> Invalid Answer: (╯°□°）╯︵",
				Util.WithStyle(answer, "transform: scale(-1, 1)")
			).Dump();
			if (@throw) throw new InvalidOperationException();
		}

		return result;
	}
}
public partial class TimesheetScript // legacy
{
	static void ProcessWeeklyTimesheet(string path, Func<string, string, bool> sumFilter = null)
	{
		var filename = Path.GetFileName(path);
		var content = File.ReadAllText(path);
		Util.OnDemand("click to expand", () => content).Dump($"Raw Input: {path} =============================================================================", 0);

		var data = Regex.Matches(content, @"^@20\d{6}", RegexOptions.Multiline)
			.Cast<Match>()
			.Select(m => new
			{
				Date = DateTime.ParseExact(m.Value.Substring(1), "yyyyMMdd", CultureInfo.InvariantCulture),
				Data = content.Substring(m.Index + m.Length, (m.NextMatch() is var next && next.Success ? next.Index : content.Length) - m.Index - m.Length).Trim()
			})
			.Select(x => new { x.Date, Data = string.IsNullOrWhiteSpace(x.Data) ? null : x.Data })
			.Where(x => x.Date <= DateTime.Today)
			.Select(y => new
			{
				Date = y.Date,
				Data = y.Data
					// processing
					?.Split('\n')
					.Where(x => !string.IsNullOrWhiteSpace(x) && !x.TrimStart().StartsWith("//"))
					.Select(x => x.Split(" ".ToArray(), 2))
					.Select(x => new { Start = ParseTime(x[0]), Reason = x.Length == 2 ? x[1] : throw new FormatException($"Invalid reason for: {x[0]}") })
					// zip last
					//.Append(new { Start = DateTime.Now.TimeOfDay, Reason = "EOD // auto-inserted with current time" })
					.ZipLast(x => new
					{
						x.Last.Start,
						End = x.Current.Start,
						Duration = x.Current.Start - x.Last.Start,
						x.Last.Reason
					})
					.Where(x => !Regex.IsMatch(x.Reason, "^EO[DFW]$"))
			})
			.Dump($"{filename}: Parsed Data", 0)
			.Where(x => x.Data != null)
			.SelectMany(x => x.Data.Where(x => x.Reason != "afk").Select(y => new
			{
				x.Date,
				y.Start,
				y.End,
				y.Duration,
				y.Reason
			}))
			.GroupBy(x => x.Date, (key, group) => new
			{
				Date = $"{key:yyyy-MM-dd dddd}",
				Time = group
					.GroupBy(x => CategoryNameFix(x.Reason), (k, g) => new
					{
						Reason = k,
						Duration = TimeSpan.FromSeconds(g.Sum(x => x.Duration.TotalSeconds)),
						DurationHours = TimeSpan.FromSeconds(g.Sum(x => x.Duration.TotalSeconds)).TotalHours,
						Blocks = g.Select(x => new { x.Reason, x.Duration.TotalHours, x.Date.DayOfWeek, x.Start, x.End })
					})
					.Where(x => x.Reason != "afk")
					.OnDemand($"{group.Where(x => CategoryNameFix(x.Reason) != "afk").Sum(x => x.Duration).TotalHours:F2} hrs"),
				RawValues = group,
			})
			.Dump($"{filename}: Grouped", 1, exclude: "RawValues");

		if (sumFilter != null)
		{
			data.SelectMany(x => x.RawValues)
				.Where(x => sumFilter(CategoryNameFix(x.Reason), x.Reason))
				// dont need to filter by not-afk, the sumFilter should be limiting enough
				.Sum(x => x.Duration).TotalHours
				.Dump("Filtered Sum");
		}

		var total = data
			.SelectMany(x => x.RawValues)
			.Where(x => CategoryNameFix(x.Reason) != "afk")
			.Sum(x => x.Duration).TotalHours;
		var wipSum = 7.5 * (total / 7.5 - Math.Floor(total / 7.5));

		Util.Metatext($"total: {total:0.##} hrs ({total / 7.5:0.##} days)").Dump();
		Util.Metatext($"missing: {7.5 * 5 - total:0.##} hrs ({5 - total / 7.5:0.##} days)").Dump();
		Util.Metatext($"current wip: {wipSum:0.##}...{7.5 - wipSum:0.##} hrs // (done...missing)").Dump();
	}
	static void ProcessWeeklyTimesheet2(string path, string userId, string token)
	{
		var filename = Path.GetFileName(path);
		var content = File.ReadAllText(path);
		Util.OnDemand("click to expand", () => content).Dump($"Raw Input: {path} =============================================================================", 0);

		var data = Regex.Matches(content, @"^@20\d{6}", RegexOptions.Multiline)
			.Cast<Match>()
			.Select(m => new
			{
				Date = DateTime.ParseExact(m.Value.Substring(1), "yyyyMMdd", CultureInfo.InvariantCulture),
				Data = content.Substring(m.Index + m.Length, (m.NextMatch() is var next && next.Success ? next.Index : content.Length) - m.Index - m.Length).Trim()
			})
			.Select(x => new { x.Date, Data = string.IsNullOrWhiteSpace(x.Data) ? null : x.Data })
			.Where(x => x.Date <= DateTime.Today)
			.Select(y => new
			{
				Date = y.Date,
				Data = y.Data
					// processing
					?.Split('\n')
					.Where(x => !string.IsNullOrWhiteSpace(x) && !x.TrimStart().StartsWith("//"))
					.Select(x => x.Split(" ".ToArray(), 2))
					.Select(x => new { Start = ParseTime(x[0]), Reason = x.Length == 2 ? x[1] : throw new FormatException($"Invalid reason for: {x[0]}") })
					// zip last
					//.Append(new { Start = DateTime.Now.TimeOfDay, Reason = "EOD // auto-inserted with current time" })
					.ZipLast(x => new
					{
						x.Last.Start,
						End = x.Current.Start,
						Duration = x.Current.Start - x.Last.Start,
						x.Last.Reason
					})
					.Where(x => !Regex.IsMatch(x.Reason, "^EO[DFW]$"))
			})
			.Dump($"{filename}: Parsed Data", 0)
			.Where(x => x.Data != null)
			.SelectMany(x => x.Data.Where(x => x.Reason != "afk").Select(y => new
			{
				x.Date,
				y.Start,
				y.End,
				y.Duration,
				y.Reason
			}))
			.ToArray();

		data
			.GroupBy(x => x.Date, (key, group) => new
			{
				Date = $"{key:yyyy-MM-dd dddd}",
				Time = group
					.GroupBy(x => CategoryNameFix(x.Reason), (k, g) => new
					{
						Reason = k,
						Duration = TimeSpan.FromSeconds(g.Sum(x => x.Duration.TotalSeconds)),
						DurationHours = TimeSpan.FromSeconds(g.Sum(x => x.Duration.TotalSeconds)).TotalHours,
						Blocks = g.Select(x => new { x.Reason, x.Duration.TotalHours, x.Date.DayOfWeek, x.Start, x.End })
					})
					.Where(x => x.Reason != "afk")
					.OnDemand($"{group.Where(x => CategoryNameFix(x.Reason) != "afk").Sum(x => x.Duration).TotalHours:F2} hrs"),
				RawValues = group,
			})
			.Dump($"{filename}: Grouped", 1, exclude: "RawValues");

		foreach (var day in data.GroupBy(x => x.Date))
		{
			var script = day
				.GroupBy(x => CategoryNameFix(x.Reason), (k, g) => new
				{
					Reason = k,
					Title = g.OrderBy(x => x.Reason.TrimEnd().EndsWith('^')).FirstOrDefault()?.Reason,
					DurationHours = TimeSpan.FromSeconds(g.Sum(x => x.Duration.TotalSeconds)).TotalHours
				})
				.Where(x => x.Reason != "afk")
				.Select(x => $$"""
				fetch("https://uno21.harvestapp.com/time/api", {
					"headers": {
						"accept": "application/json, text/javascript, */*",
						"content-type": "application/json",
						"x-csrf-token": "{{token}}",
						"x-requested-with": "XMLHttpRequest",
					},
					"body": JSON.stringify({
						project_id: 34393204, task_id: 19505120, user_id: {{userId}},
						spent_at: '{{day.Key:yyyy-MM-dd}}',
						hours: '{{x.DurationHours:0.00}}',
						notes: '{{HttpUtility.JavaScriptStringEncode(x.Title)}}',
					}),
					"method": "POST"
				});
			""")
				.JoinBy('\n');

			Clickable.CopyText($"{day.Key:yyyy-MM-dd dddd}", script).Dump();
		}
	}

	static TimeSpan ParseTime(string value)
	{
		var result = default(TimeSpan?);
		try
		{
			if (Regex.Match(value, @"^(?<hours>\d{1,2})(?<minutes>\d{2})$") is var match && match.Success)
			{
				var hours = int.Parse(match.Groups["hours"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture);
				var minutes = int.Parse(match.Groups["minutes"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture);

				result = new TimeSpan(hours, minutes, 0);
			}
		}
		catch (Exception ex)
		{
			ex.Dump();
			throw new FormatException($"Invalid time: {value}", ex);
		}

		return result ?? throw new FormatException($"Invalid time: {value}");
	}
	static string CategoryNameFix(string name)
	{
		return name
			.Split("//", 2)[0]
			.Split(":", 2)[0]
			.Trim()
			//.RegexReplace(@"#\d+", "") // uncomment to group by project only instead of project and issue
			;
	}
}

public record Timesheet(string? Path, string RawData, Timesheet.Timeline[] Timelines)
{
	public static Timesheet Load(string path) => Parse(File.ReadAllText(path)) with { Path = path };
	public static Timesheet Parse(string raw)
	{
		var lines = LineToken.Tokenize(raw);
		var error = default(string);
		error ??= lines.OfType<InvalidLineToken>().ToArray() is { Length: > 0 } errors ? $"Invalid syntax on line(s): {string.Join(", ", errors.Select(x => x.Line))}" : null;
		error ??= lines.Where(x => x is not EmptyToken).FirstOrDefault() is not DateToken ? "Expected date token not found at the start of timesheet." : null;
		if (error is { })
		{
			throw new Exception(error)
				.With("lines", lines.Select(x => new { x.Line, x.Raw, (x as InvalidLineToken)?.Error }));
		}

		var timelines = lines
			.Segment(x => x is DateToken)
			.Select(Timeline.Parse)
			.ToArray();

		return new Timesheet(null, raw, timelines);
	}

	public Timesheet ResolveRelativePointers() // resolve ^ and ^n pointers
	{
		return this with { Timelines = Timelines.Select(ResolveRelativePointers).ToArray() };

		Timeline ResolveRelativePointers(Timeline timeline)
		{
			var entries = new List<TimeEntry>();
			foreach (var entry in timeline.Entries)
			{
				if (entry.Text.StartsWith('^'))
				{
					// (^ or ^1) points to -2, ^2 points -3
					var offset = entry.Text.Length > 1
						? int.TryParse(entry.Text[1..], NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
							? value + 1 : throw new Exception($"Invalid relative pointer '{entry.Text}' on line {entry.Line}: a positive number is expected after caret '^'.")
						: 2;
					if (entries.Count < offset) throw new IndexOutOfRangeException($"Relative pointer '{entry.Text}' on line {entry.Line} doesnt point to a line within the date {timeline.Date}.");

					entries.Add(entry with { ResolvedText = entries[^offset].Apply(x => x.ResolvedText ?? x.Text) });
				}
				else
				{
					entries.Add(entry);
				}
			}

			return timeline with { Entries = entries.ToArray() };
		}
	}

	private object ToDump() => new { Path, RawData = Util.OnDemand("Click to expand", () => RawData), Timelines };

	internal abstract record LineToken(int Line, string Raw)
	{
		public static LineToken[] Tokenize(string raw)
		{
			return raw.Split('\n')
				.Select(LineToken.Parse)
				.ToArray();
		}
		public static LineToken Parse(string raw, int line)
		{
			var text = raw.Split("//", 2)[0].Trim();

			if (string.IsNullOrEmpty(text) || text.StartsWith("//")) return new EmptyToken(line, raw);
			if (text.StartsWith('@'))
			{
				if (text.Length != 9) return Error("Invalid date marker syntax");

				var datePart = text[1..9];
				if (!DateTime.TryParseExact(datePart, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
					return Error($"Invalid date marker: {datePart} is not a valid date");

				return new DateToken(line, raw, date);
			}
			if (text.Length >= 4 && text[0..4] is { } timePart && timePart.All(char.IsAsciiDigit))
			{
				// TimeSpan parsing cant handle than more 2359. But that is accepted due to legacy reason...
				// Simply check the minute parts is < 60.
				var time = int.Parse(timePart, NumberStyles.Integer, CultureInfo.InvariantCulture);
				if (time % 100 >= 60) return Error($"Invalid time marker: {timePart} is not a valid time");
				if (text.Length < 6 || text[4] != ' ') return Error($"Invalid time marker: description missing");

				return new TimeToken(line, raw, new TimeSpan(time / 100, time % 100, 0), text[5..]);
			}

			return Error("Invalid syntax");

			InvalidLineToken Error(string error) => new InvalidLineToken(line, raw, error);
		}
	}
	internal record DateToken(int Line, string Raw, DateTime Date) : LineToken(Line, Raw);
	internal record TimeToken(int Line, string Raw, TimeSpan Time, string Description) : LineToken(Line, Raw);
	internal record EmptyToken(int Line, string Raw) : LineToken(Line, Raw);
	internal record InvalidLineToken(int Line, string Raw, string Error) : LineToken(Line, Raw);

	public record Timeline(int Line, DateTime Date, TimeEntry[] Entries)
	{
		internal static Timeline Parse(IEnumerable<LineToken> tokens)
		{
			var lines = tokens.ToArray();
			if (!lines.Any()) throw new ArgumentException("Tokens are empty.");

			if (lines[0] is not DateToken date) throw new Exception("Expected date token not found.");
			if (lines.Skip(1).FirstOrDefault(x => x is not (TimeToken or EmptyToken)) is { } unexpected)
				throw new Exception($"Unexpected {unexpected.GetType().Name} found at line {unexpected.Line}");

			var times = lines.Skip(1).OfType<TimeToken>().ToList();
			if (!times.Any()) return new Timeline(date.Line, date.Date, Array.Empty<TimeEntry>());
			if (times[^1] is { } eod && !Regex.IsMatch(times[^1].Description, "^EO[DWF]", RegexOptions.IgnoreCase))
			{
#if INJECT_EOD_FOR_CURRENT_DATETIME
				var now = DateTime.Now;
				if (date.Date == now.Date && eod.Time < now.TimeOfDay)
				{
					times.Add(new TimeToken(-1, null!, new TimeSpan(now.Hour, now.Minute, 00), "EOD"));
				}
				else
#endif
				{
					throw new Exception($"Expected EOD not found from line {eod.Line}");
				}
			}

			var entries = times
				.Pairwise((current, next) => new TimeEntry(current.Line, current.Time, next.Time, current.Description))
				.ToArray();
			if (entries.FirstOrDefault(x => (x.End - x.Start) < TimeSpan.Zero) is { } negative)
			{
				throw new Exception($"Invalid time entry on line {negative.Line}: the duration ({negative.Start}~{negative.End}) is negative");
			}

			return new(date.Line, date.Date, entries);
		}
	}
	public record TimeEntry(int Line, TimeSpan Start, TimeSpan End, string Text, string? ResolvedText = null);
}
public record TimeReport(Timesheet Timesheet, TimeReport.TaskItem[] Items)
{
	public static TimeReport GenerateFrom(Timesheet sheet)
	{
		var items = sheet.Timelines
			.SelectMany(timeline => timeline.Entries.Select(x => new { timeline.Date, Task = x }))
			.GroupBy(x => new { x.Date, Category = (x.Task.ResolvedText ?? x.Task.Text).Split(':', 2)[0] }, (k, g) =>
				new TaskItem(DateOnly.FromDateTime(k.Date), k.Category, g.Sum(x => x.Task.End - x.Task.Start))
				{
					Entries = g.Select(y => y.Task).ToArray()
				})
			.Where(x => x.Category.ToLower() != "afk")
			.ToArray();

		return new(sheet, items);
	}

	public TimeReport ExtractEventSources(IReadOnlyDictionary<string, string>? githubMappings = null)
	{
		return this with
		{
			Items = Items
				.Select(x => x with { EventSource = RemapSource(FindEventSource(x)) })
				.ToArray()
		};

		EventSource? FindEventSource(TaskItem x) => EventSource.TryExtractFrom(x.Category) ?? EventSource.TryExtractFrom(x.Text);
		EventSource? RemapSource(EventSource? source)
		{
			if (githubMappings is { } &&
				source is { Type: EventSource.EventSourceType.Github } github &&
				github.Source.Split('#', 2) is [var prefix, var suffix] &&
				githubMappings.TryGetValue(prefix.ToLowerInvariant(), out var mapped))
			{
				source = source with { ExpandedSource = $"{mapped}#{suffix}" };
			}

			return source;
		}
	}

	public void DumpSummary()
	{
		Items
			.GroupBy(x => x.Date, (k, g) => new
			{
				Date = k,
				Duration = g.Sum(x => x.Duration),
				Blocks = g
			})
			.Dump("Summary", 0);

		Clickable.Create("Pie Chart", () => Items.GroupBy(x => x.Category)
			.Chart(g => g.Key, g => g.Sum(x => x.Duration).TotalHours, LINQPad.Util.SeriesType.Pie)
			.ToWindowsChart()
			.Apply(chart =>
			{
				chart.Legends.Add("Default");
				chart.ChartAreas[0].Area3DStyle.Enable3D = true;
				chart.ChartAreas[0].Area3DStyle.Inclination = 50;
				chart.Series[0].SetCustomProperty("PieLabelStyle", "Outside");

				var total = Items.Sum(x => x.Duration).TotalHours;
				foreach (var p in chart.Series[0].Points)
				{
					var value = p.YValues[0];
					var ratio = value / total;

					p.LegendToolTip =
					p.ToolTip = $"{p.AxisLabel}: {value:F2}hrs ({ratio:P2})";
				}
			})
			.Dump()
		).Dump();

		var total = Items.Sum(x => x.Duration).TotalHours;
		var wipSum = 7.5 * (total / 7.5 - Math.Floor(total / 7.5));

		Util.Metatext($"total: {total:0.##} hrs ({total / 7.5:0.##} days)").Dump();
		Util.Metatext($"missing: {7.5 * 5 - total:0.##} hrs ({5 - total / 7.5:0.##} days)").Dump();
		Util.Metatext($"wip2next: {wipSum:0.##}...{7.5 - wipSum:0.##} hrs // (done...missing)").Dump();
	}

	public record TaskItem(DateOnly Date, string Category, TimeSpan Duration)
	{
		public EventSource? EventSource { get; init; }
		public required Timesheet.TimeEntry[] Entries { get; init; }

		internal string Text => Entries.FirstOrDefault()?.Text ?? string.Empty;
	}
}
public record EventSource(EventSource.EventSourceType Type, string Source, string? ExpandedSource = null)
{
	public static EventSource? TryExtractFrom(string text)
	{
		if (Pattern.Match(text) is not { Success: true } match) return null;

		var type = Enum.GetValues<EventSourceType>().FirstOrDefault(x => match.Groups[x.ToString().ToLowerInvariant()].Success);
		return new EventSource(type, match.Value);
	}

	public HarvestExternalReference? ToHarvestExternalReference()
	{
		return Type switch
		{
			EventSourceType.Github when ExpandedSource?.Match(@"^(.+)\#(\d+)$") is { } m => HarvestExternalReference.FromUrl(m.Result("https://github.com/$1/issues/$2")),
			// EventSourceType.Discord when (is #message_id and not @username)... => link to chat
			_ => null,
		};
	}

	public override string ToString() => ExpandedSource ?? Source;
	private object ToDump() => Util.OnDemand(Source, () => this);

	public enum EventSourceType { Unknown, Github, Discord, }
	private static readonly Regex Pattern = new("""
		(?<=^|\W)(
			(?<github>[\w\-\.]*\#\d+) |
			(?<discord>discord@[A-z0-9_\.]+)
		)\b
	""", RegexOptions.IgnorePatternWhitespace);
}

public partial class HarvestApiEndpoint(ScriptConfig config) : ApiEndpointBase
{
	public async Task Test()
	{
		(await QueryJson<HarvestProjectAssignments>(q => q
			.Get().AppendPath("users/me/project_assignments")
		)).DumpJson(0).Dump(0);
	}

	public async Task<Json<HarvestUser>> GetCurrentUserAsync()
	{
		return await QueryJson<HarvestUser>(q => q
			.Get().AppendPath("users/me")
		);
	}

	public async Task<Json<object>> AddTimeEntry(HarvestNewTimeEntry entry)
	{
		return await QueryJson<object>(q => q
			.Post()
			.AppendPath("time_entries")
			.JsonPayload(entry)
		);
	}

	public async Task<Json<HarvestProjectAssignments>> GetProjectAssignmentsAsync()
	{
		return await QueryJson<HarvestProjectAssignments>(q => q
			.Get().AppendPath("users/me/project_assignments")
		);
	}
}
public partial class HarvestApiEndpoint
{
	// https://help.getharvest.com/api-v2/
	public readonly Uri BaseAddress = new("https://api.harvestapp.com/v2/");

	protected override HttpClient CreateHttpClient()
	{
		var client = new HttpClient();
		client.BaseAddress = BaseAddress;
		client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.Secrets?.HarvestPAT.MarshalToString()}");
		client.DefaultRequestHeaders.Add("Harvest-Account-Id", config.Secrets?.HarvestAccountID);
		client.DefaultRequestHeaders.Add("User-agent", "duration-calculator");

		return client;
	}
	protected override JsonSerializerSettings? CreateJsonSerializerSettings()
	{
		var settings = new JsonSerializerSettings()
		{
			ContractResolver = new DefaultContractResolver
			{
				NamingStrategy = new SnakeCaseNamingStrategy(),
			},
		};
		
		return settings;
	}
}
public record HarvestUser(int Id, string Email); // https://help.getharvest.com/api-v2/users-api/users/users/
public record HarvestNewTimeEntry(/*string? UserId = null,*/ int ProjectId, int TaskId, DateOnly SpentDate, double Hours, string? Notes = null, HarvestExternalReference? ExternalReference = null); // https://help.getharvest.com/api-v2/timesheets-api/timesheets/time-entries/#create-a-time-entry-via-duration
public record HarvestExternalReference(string? Id = null, string? GroupId = null, /*string? AccountId = null,*/ string? Permalink = null)
{
	public static HarvestExternalReference FromUrl(string url) => new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), url);
}
public record HarvestProjectAssignments(HarvestProjectAssignment[] ProjectAssignments);
public record HarvestProjectAssignment(int Id, bool IsActive, HarvestProject Project, HarvestTaskAssignment[] TaskAssignments);
public record HarvestProject(int Id, string Name, string Code);
public record HarvestTaskAssignment(int Id, HarvestTask Task);
public record HarvestTask(int Id, string Name);

public abstract class ApiEndpointBase
{
	protected virtual HttpClient CreateHttpClient() => new();
	protected virtual JsonSerializerSettings? CreateJsonSerializerSettings() => null;

	protected async Task<HttpResponseMessage> Query(Func<QueryBuilder, QueryBuilder> builder)
	{
		using var client = CreateHttpClient();

		return await builder(new QueryBuilder(CreateJsonSerializerSettings())).Query(client);
	}
	protected Task<Json<T>> QueryJson<T>(Func<QueryBuilder, QueryBuilder> builder) => Query(builder).ReadAsJson<T>(CreateJsonSerializerSettings());
	protected Task<JsonArray<T>> QueryJsonArray<T>(Func<QueryBuilder, QueryBuilder> builder) => Query(builder).ReadAsJsonArray<T>(CreateJsonSerializerSettings());
}
public class QueryBuilder
{
	private readonly HttpRequestMessage request = new HttpRequestMessage();
	private readonly JsonSerializerSettings? jsonSerializerSettings = null;
	
	private Dictionary<string, string> query = new Dictionary<string, string>();
	private Dictionary<string, string> payload = new Dictionary<string, string>();
	private (string ContentType, string Json)? jsonPayload;
	
	public QueryBuilder(JsonSerializerSettings? jsonSerializerSettings = null)
	{
		this.jsonSerializerSettings = jsonSerializerSettings;
	}

	public QueryBuilder Get() => Do(() => request.Method = HttpMethod.Get);
	public QueryBuilder Put() => Do(() => request.Method = HttpMethod.Put);
	public QueryBuilder Put(string contentType /*ignored*/) => Do(() => request.Method = HttpMethod.Put);
	public QueryBuilder Post() => Do(() => request.Method = HttpMethod.Post);
	public QueryBuilder PostMultipart() => throw new NotImplementedException();
	public QueryBuilder Part(string name, Stream stream, string mimeType, string filename, Func<bool> condition) => throw new NotImplementedException();
	public QueryBuilder Delete() => Do(() => request.Method = HttpMethod.Delete);

	public QueryBuilder AppendPath(string path) => Do(() => request.RequestUri = new Uri(path, UriKind.Relative));
	public QueryBuilder FromUrl(string uri) => Do(() => request.RequestUri = new Uri(uri));

	public QueryBuilder Param(string name, string value) => Do(() => query.Add(name, value));
	public QueryBuilder Param(string name, string value, Func<bool> condition) => Do(() => query.Add(name, value), condition);
	public QueryBuilder Param(string name, Func<string> value, Func<bool> condition) => Do(() => query.Add(name, value()), condition);

	public QueryBuilder JsonPayload(string json, string contentType = "application/json") => Do(() => jsonPayload = (contentType, json));
	public QueryBuilder JsonPayload<T>(T value, string contentType = "application/json", JsonSerializerSettings? settings = null) => Do(() => jsonPayload = (contentType, JsonConvert.SerializeObject(value, jsonSerializerSettings ?? settings)));
	public QueryBuilder PayloadParam(string name, string value) => Do(() => payload.Add(name, value));
	public QueryBuilder PayloadParam(string name, string value, Func<bool> condition) => Do(() => payload.Add(name, value), condition);
	public QueryBuilder PayloadParam(string name, Func<string> value, Func<bool> condition) => Do(() => payload.Add(name, value()), condition);

	private QueryBuilder Do(Action action)
	{
		action();
		return this;
	}
	private QueryBuilder Do(Action action, Func<bool> condition)
	{
		if (condition()) action();
		return this;
	}

	public Task<HttpResponseMessage> Query(HttpClient httpClient, bool ensureSuccess = true, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead)
	{
		if (query.Any() && payload.Any())
			throw new InvalidOperationException("Param and PayloadParam should not be used together");

		if (query.Any())
			request.RequestUri = new Uri(request.RequestUri.OriginalString + "?" + GetQueryString(), request.RequestUri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative);

		if (jsonPayload is { } json)
		{
			request.Content = new StringContent(json.Json);
			request.Content.Headers.ContentType = new MediaTypeHeaderValue(json.ContentType);
		}
		else if (payload.Any())
			request.Content = new FormUrlEncodedContent(payload);

		return httpClient
			.SendAsync(request, option)
			.Apply(x => ensureSuccess ? x.EnsureSuccessStatusCode() : x);

		string GetQueryString(bool filterEmptyValue = true, bool addQueryIndicator = false)
		{
			using (var content = new FormUrlEncodedContent(query.Where(x => !filterEmptyValue || !string.IsNullOrEmpty(x.Value))))
			{
				var query = content.ReadAsStringAsync().Result;
				if (addQueryIndicator && !string.IsNullOrEmpty(query))
					query = "?" + query;

				return query;
			};
		}
	}
}

public class Json<T> : JObject
{
	public T? Content { get; }

	public Json(string json, JsonSerializerSettings? settings = null) : base(JObject.Parse(json))
	{
		Content = ToObject<T>(JsonSerializer.Create(settings));
	}
	public static new Json<T> Parse(string json, JsonSerializerSettings? settings = null) => new Json<T>(json, settings);

	private object? ToDump() => Content;
	public override string ToString()
	{
		// hide default ToString
		return Content is not { } && typeof(T).GetMethod(nameof(ToString))?.DeclaringType == typeof(object)
			? string.Empty : Content.ToString();
	}
}
public class JsonArray<T> : JArray
{
	public T[] Items { get; }

	public JsonArray(string json, JsonSerializerSettings? settings = null) : base(JArray.Parse(json))
	{
		Items = JsonConvert.DeserializeObject<T[]>(json, settings);
	}
	public static new JsonArray<T> Parse(string json, JsonSerializerSettings? settings = null) => new JsonArray<T>(json, settings);

	private object ToDump() => Items;
	public override string ToString()
	{
		// hide default ToString
		return typeof(T).GetMethod(nameof(ToString)).DeclaringType != typeof(object)
			? Items.ToString()
			: string.Empty;
	}
}
public static class JsonExtensions
{
	public static object ToObject(this JToken token)
	{
		if (token == null) return null;

		switch (token.Type)
		{
			case JTokenType.Object:
				return token.Children<JProperty>()
#if SORT_JSON_PROPERTY
					.OrderBy(prop => prop.Name)
#endif
#if DUMP_JSON_AS_OBJECT
					.Aggregate(
						(IDictionary<string, object>)new ExpandoObject(),
						(obj, prop) =>
						{
							obj.Add(prop.Name, ToObject(prop.Value));
							return obj;
						});
#else
					.ToDictionary(prop => prop.Name, prop => ToObject(prop.Value));
#endif

			case JTokenType.Array:
				return token.Select(ToObject).ToList();

			default:
				return ((JValue)token).Value;
		}
	}
	public static async Task<object> ToObject<T>(this Task<T> task) where T : JToken
	{
		return (await task).ToObject();
	}
	public static async Task<T> Unwrap<T>(this Task<Json<T>> task)
	{
		return (await task).Content;
	}
	public static T DumpJson<T>(this T token, int depth = 1) where T : JToken
	{
		token.ToObject().Dump(depth);
		return token;
	}
	public static async Task<T> DumpJson<T>(this Task<T> task, int depth = 1) where T : JToken
	{
		var token = await task;

		token.ToObject().Dump(depth);
		return token;
	}
	public static async Task<T> DumpContent<T>(this Task<Json<T>> task, int depth = 1)
	{
		var json = await task;

		json.Content.Dump(depth);
		return json.Content;
	}
	public static async Task<T> DumpContent<T, TDump>(this Task<Json<T>> task, Func<T, TDump> dumpSelector, int depth = 1)
	{
		var json = await task;

		dumpSelector(json.Content).Dump(depth);
		return json.Content;
	}
	public static async Task<T> DumpAs<T, TDump>(this Task<T> task, Func<JToken, TDump> selector) where T : JToken
	{
		var token = await task;

		token
			.Select(selector)
			.Dump();
		return token;
	}
}

public static class Clickable
{
	public static Hyperlinq CopyText(string text) => CopyText(text, text);
	public static Hyperlinq CopyText(string header, string text)
	{
		return Create(header, () => Clipboard.SetText(text));
	}

	public static Hyperlinq Create(string header, Action action) => new Hyperlinq(action, header);
}
public static class DateHelper
{
	public static DateOnly TodayOnly => DateOnly.FromDateTime(DateTime.Today);

	public static DateTime ThisMonday => DateTime.Today.FindMonday();

	public static int GetWeeksFrom(this DateOnly date, DateOnly reference) => (date.FindMonday().DayNumber - reference.FindMonday().DayNumber) / 7;

	public static DateTime FindMonday(this DateTime date) => date.AddDays(date.DayOfWeek.GetMondayOffset());
	public static DateOnly FindMonday(this DateOnly date) => date.AddDays(date.DayOfWeek.GetMondayOffset());
	internal static int GetMondayOffset(this DayOfWeek day) => -(day == DayOfWeek.Sunday ? 7 : (int)day) + 1;
}

public static class DumpExtensions
{
	public static T DumpTo<T>(this T obj, DumpContainer container)
	{
		container.Content = obj;

		return obj;
	}
	public static T DumpTo<T>(this T obj, DumpContainer container, int depth)
	{
		container.Content = obj;
		container.DumpDepth = depth;

		return obj;
	}
}
public static class EnumerableExtensions
{
	// ignore first and last item, since they dont have a pair
	public static IEnumerable<(T Current, T Last)> ZipSelf<T>(this IEnumerable<T> source)
	{
		var first = true;
		var last = default(T);
		foreach (var item in source)
		{
			if (!first) yield return (item, last);
			else first = false;

			last = item;
		}
	}
	public static IEnumerable<TZipped> ZipLast<T, TZipped>(this IEnumerable<T> source, Func<(T Current, T Last), TZipped> zip)
	{
		foreach (var item in source.ZipSelf())
			yield return zip(item);
	}
	public static TimeSpan Sum<T>(this IEnumerable<T> source, Func<T, TimeSpan> selector) => source.Select(selector).Sum();
	public static TimeSpan Sum(this IEnumerable<TimeSpan> source) => TimeSpan.FromTicks(source.Sum(x => x.Ticks));
	public static IEnumerable<T> TrimNull<T>(this IEnumerable<T?> source) => source.OfType<T>();
}
public static class ExceptionExtensions
{
	public static TException With<TException>(this TException exception, object key, object value) where TException : Exception
	{
		exception.Data[key] = value;
		return exception;
	}
}
public static class FluentExtensions
{
	public static TResult Apply<T, TResult>(this T target, Func<T, TResult> resultSelector) => resultSelector(target);
	public static T Apply<T>(this T target, Action<T> action)
	{
		action(target);
		return target;
	}
}
public static class HttpClientExtensions
{
	public static async Task<HttpResponseMessage> EnsureSuccessStatusCode(this Task<HttpResponseMessage> task, Func<HttpResponseMessage, object> contextProvider = null)
	{
		var response = await task;

		//response.EnsureSuccessStatusCode();
		if (!response.IsSuccessStatusCode)
		{
			throw new HttpResponseException2(
				response.StatusCode,
				$"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase}).");
		}

		return response;
	}

	public static async Task<Stream> ReadAsStreamAsync(this Task<HttpResponseMessage> task)
	{
		var response = await task;

		return await response.Content.ReadAsStreamAsync();
	}

	public static async Task<JObject> ReadAsJObject(this Task<HttpResponseMessage> task)
	{
		var response = await task;
		var content = await response.Content.ReadAsStringAsync();

		try
		{
			return JObject.Parse(content);
		}
		catch (Exception e)
		{
			//typeof(HttpResponseMessage).Log().Error("Failed to parse response content", e);
			throw;
		}
	}
	public static async Task<Json<T>> ReadAsJson<T>(this Task<HttpResponseMessage> task, JsonSerializerSettings? settings = null)
	{
		var response = await task;
		var content = await response.Content.ReadAsStringAsync();

		try
		{
			return Json<T>.Parse(content, settings);
		}
		catch (Exception e)
		{
			//typeof(HttpResponseMessage).Log().Error("Failed to parse response content", e);
			throw;
		}
	}
	public static async Task<JsonArray<T>> ReadAsJsonArray<T>(this Task<HttpResponseMessage> task, JsonSerializerSettings? settings = null)
	{
		var response = await task;
		var content = await response.Content.ReadAsStringAsync();

		try
		{
			return JsonArray<T>.Parse(content, settings);
		}
		catch (Exception e)
		{
			//typeof(HttpResponseMessage).Log().Error("Failed to parse response content", e);
			throw;
		}
	}
}
public static class StringExtensions
{
	public static string RegexReplace(this string input, string pattern, string replacement) => Regex.Replace(input, pattern, replacement);
	public static string RegexReplace(this string input, string pattern, string replacement, RegexOptions options) => Regex.Replace(input, pattern, replacement, options);

	public static Match Match(this string input, string pattern) => Regex.Match(input, pattern);

	public static string JoinBy<T>(this IEnumerable<T> source, char separator) => string.Join(separator, source);
	public static string JoinBy<T>(this IEnumerable<T> source, char separator, Func<T, string> selector) => string.Join(separator, source.Select(selector));
	public static string JoinBy<T>(this IEnumerable<T> source, string separator) => string.Join(separator, source);
	public static string JoinBy<T>(this IEnumerable<T> source, string separator, Func<T, string> selector) => string.Join(separator, source.Select(selector));
}

public sealed class HttpResponseException2 : HttpRequestException
{
	public HttpStatusCode StatusCode { get; init; }

	public HttpResponseException2(HttpStatusCode statusCode, string message)
		: base(message)
		=> StatusCode = statusCode;
}