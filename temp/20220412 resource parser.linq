<Query Kind="Program">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Logging.Console</NuGetReference>
  <NuGetReference>TextCopy</NuGetReference>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>System.Dynamic</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>TextCopy</Namespace>
</Query>

// todo: nullable enable

#define ENABLE_GENERIC_VALUE_OBJECT_PARSING
#define SKIP_ALL_NOTIMPLEMENTED_OBJECT // comment out to throw on missing parser
#define SKIP_ALL_NOTIMPLEMENTED_OBJECT_NO_LOG_WARN // hide "Ignoring '{...}', as there is no parser implemented for it" msg
#define REPLACE_UNO_PLATFORM_XMLNS
#define ALLOW_DUPLICATED_KEYS // temp workaround for platform specifics
#define ALLOW_DUPLICATED_KEYS_WITHOUT_WARNING
#define PARSE_VISUALELEMENT_CHILD // comment this out for performance, if we arent working with visual-tree/state
#define DUMP_RESOURCEREF_WITH_KEY_INLINE // comment for `{StaticResource KEY}`, vs `StaticResourceRef` commented out

using static UserQuery.Global;
using static UserQuery.ValueSimplifier;

public partial class Script
{
	public static void Main()
	{
		ResourceDictionary.ThemeMapping = new Dictionary<string, string>
		{
			["Light"] = "Light",
			["Dark"] = "Default,Dark" // Default=Dark is a weird concept introduced by lightweight styling...
		};
		//Specialized.ListThemes();
		//Specialized.ListExposedThemeV2Styles();
		//Specialized.ListExposedCupertinoStyles();
		//Specialized.ListExposedToolkitV2Styles();
		//Specialized.DiffThemeToolkitV2InnerResources();
		//Specialized.CheckLightWeightResourceParity(@"D:\code\uno\platform\Uno.Themes\src/library/Uno.Material/Styles/Controls/v2/NavigationView.xaml");
		LightWeightSourceGen.GenerateThemeCsMarkup();
		//LightWeightSourceGen.GenerateToolkitCsMarkup();

		/*var additionalResources = new[]
		{
			@"D:\code\uno\framework\Uno\src\Uno.UI\UI\Xaml\Style\Generic\SystemResources.xaml",
			@"D:\code\uno\framework\Uno\src\Uno.UI.FluentTheme.v2\themeresources_v2.xaml",
			@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\Common\TextBoxVariables.xaml",
			@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\Common\Fonts.xaml",
			@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\Typography.xaml",
			@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\SharedColors.xaml",
			@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\SharedColorPalette.xaml",
			@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\_Resources.xaml",
			@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\TextBlock.xaml"
		}.Aggregate(new ResourceDictionary(), (acc, file) => acc.Merge((ResourceDictionary)ScuffedXamlParser.Load(file)));*/
		//Specialized.CheckLightWeightResourceParity(@"D:\code\uno\platform\Uno.Themes\src/library/Uno.Material/Styles/Controls/v2/ToggleSwitch.xaml");
		//Specialized.ExtractLightWeightResources(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\Button.xaml", additionalResources);
		//Specialized.ExtractLightWeightResources(@"D:\code\uno\framework\Uno\src\Uno.UI.FluentTheme.v2\themeresources_v2.xaml", new ResourceDictionary());
		//string.Join("\n\n", Directory.GetFiles(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\", "*.xaml")
		//	.Prepend(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\Typography.xaml")
		//	.Select(x => string.Join("\n", $"# {Path.GetFileName(x)}", Specialized.ExtractLightWeightResources(x, additionalResources)))
		//).OnDemand("Click to expand").Dump("All in one");
		//foreach (var control in Directory.GetFiles(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\", "*.xaml"))
		//	Specialized.ExtractLightWeightResources(control, additionalResources);

		//ListColors(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v1\ColorPalette.xaml");
		//ListColors(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\SharedColorPalette.xaml");
		//ListColors(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\SharedColors.xaml");
		////ListColors(@"D:\code\uno\platform\Uno.Todo\src\ToDo.UI\Styles\ColorPaletteOverride.xaml");
		//ListColors(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v1\MaterialColors.xaml");
		/*SpecializedListColorTheme(
			@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\SharedColorPalette.xaml",
			@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\SharedColors.xaml",
			generateBrushesBasedOnColorAndOpacity: false);*/

		//foreach (var control in Directory.GetFiles(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\", "*.xaml").Where(x => !Path.GetFileName(x).Contains('_')))
		//	ListStyles(control.Dump());

		//ListStyles(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v1\Button.xaml");
		//ListStyles(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\FloatingActionButton.xaml");
		//ListStyles(@"D:\code\uno\platform\Uno.UI.Toolkit\src\library\Uno.Toolkit.Material\Styles\Controls\v2\ChipGroup.xaml");
		//CompareStyles(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v1\TextBlock.xaml", x => x.BasedOn != null);
		//CompareStyles(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\TextBlock.xaml", x => true || x.BasedOn != null, x => Regex.Replace(x, @"^{(Static|Theme)Resource (?<key>\w+)}$", "(*${key})"));
		//CompareStyles(@"D:\code\uno\platform\Uno.Toolkit\src\library\Uno.Toolkit.Material\Styles\Controls\v2\Chip.xaml", x => x.BasedOn != null, SimplifyReference);
		//CompareStyles(@"D:\code\uno\platform\Uno.Toolkit\src\library\Uno.Toolkit.Material\Styles\Controls\v2\ChipGroup.xaml", x => x.BasedOn != null, SimplifyReference);
		/*var typography = (ResourceDictionary)ScuffedXamlParser.Load(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\Typography.xaml").Dump("Typography", 0);
		CompareStyles(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\TextBlock.xaml", x => true || x.BasedOn != null);
		CompareStyles(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\TextBlock.xaml", 
			x => true || x.BasedOn != null,
			resolveResource: x => ResolveResource(typography, x, 3)
		);*/

		Match TryMatch(string input, string pattern) => Regex.Match(input, pattern) is { Success: true } m ? m : null;
		string SimplifyReference(string value) => TryMatch(value, @"^{StaticResource (?<key>\w+)}$")?.Result("*${key}") ?? value;
		string ResolveResource(ResourceDictionary resources, string value, int maxDepth = 1)
		{
			for (int depth = 0; depth < maxDepth && value != null && TryMatch(value, @"^{StaticResource (?<key>\w+)}$") is { Success: true } match; depth++)
			{
				value = match.Groups["key"].Value?.Apply(y => (resources[y] as StaticResource)?.Value?.ToString());
			}
			return value;
		}
	}

	private static void ListColors(string path)
	{
		Util.Metatext($"==================== {path}").Dump();
		var document = XDocument.Load(path);
		var root = (ResourceDictionary)ScuffedXamlParser.Parse(document.Root);

		object FormatColor(Color x) => Util.HorizontalRun(true, x.ToColoredBlock(), x.ToRgbText());

		root.Values
			.Where(x => x.IsReferenceFor<Color>())
			.Select(x => x switch
			{
				StaticResource sr => new { x.Key, Value = FormatColor((Color)sr.Value) },
				ThemeResource tr => new { x.Key, LightValue = FormatColor((Color)tr.LightValue), DarkValue = FormatColor((Color)tr.DarkValue) },

				_ => (object)null,
			})
			.Dump("Colors", 0);
		root.Values
			.Where(x => x.IsReferenceFor<Double>())
			.Dump("Doubles", 0);
		var brushes = root.Values
			.Where(x => x.IsStaticResourceFor<SolidColorBrush>())
			.Select(x => new { x.Key, Value = (x as StaticResource).Value as SolidColorBrush })
			.Select(x => new
			{
				x.Key,
				//Color = IKeyedResource.GetKeyFromMarkup(x.Value.GetDP("Color")),
				//Opacity = IKeyedResource.GetKeyFromMarkup(x.Value.GetDP("Opacity")),
				Color = x.Value.GetDP("Color"),
				Opacity = x.Value.GetDP("Opacity"),
			})
			.OrderBy(x => x.Color)
			.ThenBy(x => x.Opacity)
			.Dump("Brushes", 0);

		brushes
			.GroupBy(x => x.Color, (g, k) => Util.OnDemand($"{g}[{k.Count()}]", () => k))
			.Dump("Brushes by Color", 0);
	}
	private static void SpecializedListColorTheme(string paletteFile, string brushFile, bool generateBrushesBasedOnColorAndOpacity = false)
	{
#if false
		var paletteRD = (ResourceDictionary)ScuffedXamlParser.Parse(XDocument.Load(paletteFile).Root);
		var brushRD = (ResourceDictionary)ScuffedXamlParser.Parse(XDocument.Load(brushFile).Root);

		var colors = paletteRD.Values
			.Where(x => x.IsReferenceFor<Color>())
			.Select(x => x switch
			{
				StaticResource sr => new { x.Key, Value = new[] { (Color)sr.Value } },
				ThemeResource tr => new { x.Key, Value = new[] { (Color)tr.LightValue, (Color)tr.DarkValue } },

				_ => throw new Exception(),
			})
			.Dump("Colors", 0);
		var opacities = paletteRD.Values
			.Where(x => x.IsReferenceFor<Double>())
			.Dump("Opacities", 0);
		var brushes = brushRD.Values
			.Where(x => x.IsReferenceFor<SolidColorBrush>())
			//.Select(x => new { x.Key, Value = (x as StaticResource).Value as SolidColorBrush })
			.Select(x => new { x.Key, Value = (x as ThemeResource).LightValue as SolidColorBrush }) // both light and default(dark) are just duplicated for lightweight styling
			.Select(x => new
			{
				x.Key,
				//Color = IKeyedResource.GetKeyFromMarkup(x.Value.GetDP("Color")),
				//Opacity = IKeyedResource.GetKeyFromMarkup(x.Value.GetDP("Opacity")),
				//ColorValue = paletteRD.TryGetValue(IKeyedResource.GetKeyFromMarkup(x.Value.GetDP("Color")), out var color) ? color : default,
				//OpacityValue = paletteRD.TryGetValue(IKeyedResource.GetKeyFromMarkup(x.Value.GetDP("Opacity")), out var opacity) ? opacity : default,
			})
			.OrderBy(x => x.Color)
			.ThenBy(x => x.Opacity)
			.Dump("Brushes");

		colors.Select(color => new
		{
			BaseColor = color.Key,
			Brushes = from opacity in opacities.Prepend(null)
					  let key = color.Key.Key[0..^5] + opacity?.Key.Key[0..^7] + "Brush"
					  select new
					  {
						  Key = key,
						  Opacity = (opacity as StaticResource)?.Value as double? ?? 1,
						  Defined = Util.HighlightIf(brushes.Any(x => x.Key.Key == key), x => !x),
						  Copy = Util.HorizontalRun(true,
							  Clickable.CopyText("Key", key),
							  Clickable.CopyText("Ref", $"{{StaticResource {key}}}"),
							  Clickable.CopyText("Fwd", $"<StaticResource x:Key=\"\" ResourceKey=\"{key}\" />")
						  ),
					  }
		}).Dump("Quick Lookup", 0);

		if (generateBrushesBasedOnColorAndOpacity)
		{
			var crossProducts = (
				from color in colors
				from opacity in opacities.Prepend(null)
				let sanity0 = color.Key.Key.EndsWith("Color") ? true : throw new NotImplementedException()
				let sanity1 = opacity?.Key.Key.EndsWith("Opacity") != false ? true : throw new NotImplementedException()
				let key = color.Key.Key[0..^5] + opacity?.Key.Key[0..^7] + "Brush"
				select new
				{
					CK = color.Key,
					OK = opacity?.Key,
					Key = key,
					Defined = brushes.Any(x => x.Key.Key == key)
				}
			).ToArray();

			crossProducts.Dump("cross-products: Colors x Doubles", 0);
			crossProducts.GroupBy(x => x.CK).SelectMany(g => g.Select(x =>
				$@"<SolidColorBrush x:Key='{x.Key}'
					Color='{{ThemeResource {x.CK.Key}}}'
					{(x.OK?.Apply(y => $"Opacity='{{StaticResource {y.Key}}}'"))}
				/>"
				.RegexReplace(@"\s+", " ")
				.Replace('\'', '"')
			)
				.Prepend($"<!--#region {g.Key.Key} -->")
				.Append($"<!--#endregion {g.Key.Key}-->")
			)
			.JoinBy("\n").Dump("all brushes");
			brushes.Select(x => x.Key.Key).Except(crossProducts.Select(x => x.Key)).Dump("missing brushes", 0);
		}
#endif
	}
	private static void ListStyles(string path)
	{
		var document = XDocument.Load(path);
		var root = (ResourceDictionary)ScuffedXamlParser.Parse(document.Root);

		root.Values
			.Select(x => (x as StaticResource)?.Value)
			.OfType<Style>()
			.Dump(path, 1);
	}
	private static void CompareStyles(string path, Func<Style, bool> predicate, Func<string, string> resolveResource = null)
	{
		// ^resolveResource: return null if unresolvable

		var document = XDocument.Load(path);
		var root = (ResourceDictionary)ScuffedXamlParser.Parse(document.Root);

		var styles = root.Values
			.Where(x => x is StaticResource { Value: Style style } && predicate(style));

		PivotHelper.Pivot(styles,
			x => x.Key,
			x => x.StaticValue<Style>().Setters.ToDictionary(
				x => x.Property,
				x => (object)x.Value?.ToString()?.Apply(resolveResource ?? (_ => null)) ?? x.Value
			)
		).Dump(path);
	}

	private static partial class Specialized // legacy code from pre-uno5 era
	{
		public static void ListExposedThemeV2Styles()
		{
			var resources = new ResourceDictionary();
			var controls = Directory.GetFiles(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\", "*.xaml")
				.Where(x => !Path.GetFileName(x).Contains('_'));
			foreach (var control in controls)
			{
				//Util.Metatext($"Processing: {control}").Dump();
				var filename = Path.GetFileNameWithoutExtension(control);
				var document = XDocument.Load(control);
				var root = (ResourceDictionary)ScuffedXamlParser.Parse(document.Root);

				var filter = (filename switch
				{
					"ContentDialog" => "Button",
					"DatePicker" => "Button",
					"NavigationView" => "Button,SplitView,TextBlock,ContentControl",
					"PasswordBox" => "Button",
					"PipsPager.Base" => "Button",
					"Slider" => "Thumb",
					"TextBox" => "Button",

					_ => "",
				});
				var filtered = root.Values
					.Where(x =>
						x is StaticResource { Value: Style { TargetType: string type } } &&
						!filter.Split(',').Contains(type)
					);
				//filtered
				//	.OfStaticResourceType<Style>()
				//	.Select(x => new { x.Key, x.TargetType })
				//	.Dump($"{Path.GetFileNameWithoutExtension(control)}: {filtered.OfStaticResourceType<Style>().Count()} styles", 0);

				resources.AddRange(filtered);
			}

			var _resources = (ResourceDictionary)ScuffedXamlParser.Load(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\_Resources.xaml");
			var implicitStyles = _resources.Values.OfStaticResourceOf<Style>()
				.Select(x => x.BasedOn)
#if true // Themes 3.0
				.Select(x => ((resources[x] as StaticResource)?.Value as Style)?.BasedOn)
#endif
				.ToArray()
				.Dump("ImplicitStyles", 0);
			var aliasMap = _resources.Values.Where(x => x.IsStaticResourceFor<StaticResourceRef>())
				.Select(x => new { x.Key, Key2 = ((x as StaticResource)?.Value as StaticResourceRef)?.ResourceKey })
				.ToDictionary(x => x.Key2, x => x.Key)
				.Dump("Aliases", 0);

			//resources.Values.OfStaticResourceOf<Style>()
			//	.Where(x => x.Key != null)
			//	.Where(x => x.Key is string k && !@"
			//			MaterialDefault, MaterialBase, 
			//			BaseStyle, BaseMaterial, BaseTextBlockStyle,
			//			MUX_
			//		".Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
			//		.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
			//		.Any(k.Contains)
			//	)
			//	.Select(x => new
			//	{
			//		x.Key,
			//		AliasedKey = aliasMap.TryGetValue(x.Key, out var key) ? key : "",
			//		x.TargetType,
			//		ImplicitStyle = implicitStyles.Contains(x.Key) ? "true" : "",
			//	})
			//	.Dump("=== Style Exports ===", 0)
			//	.ToCopyableMarkdownTable()
			//	.Dump();
		}
		/*not updated since uno5*/
		public static void ListExposedCupertinoStyles()
		{
			var styles = new ResourceDictionary();
			var controls = Directory.GetFiles(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Cupertino\Styles\Controls\", "*.xaml")
				.Where(x => !Path.GetFileName(x).Contains('_'));
			foreach (var control in controls)
			{
				//Util.Metatext($"Processing: {control}").Dump();
				var document = XDocument.Load(control);
				var root = (ResourceDictionary)ScuffedXamlParser.Parse(document.Root);

				styles.Merge(root);
			}

			//var styleInfos = ParseGetStyleInfos(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\MaterialResourcesV2.cs");
			//styles.Values.OfStaticResourceOf<Style>()
			//	.OrderBy(x => x.TargetType)
			//	//.Join(styleInfos, style => style.Key, info => info.ResourceKey, (style, info) => new { Style = style, Info = info })
			//	.Where(x => x.Key.StartsWith("Cupertino"))
			//	.Select(x => new
			//	{
			//		x.TargetType,
			//		Key = x.Key
			//	})
			//	//.OrderBy(x => x.TargetType)
			//	//.GroupBy(x => x.TargetType, (k, g) => $"`{k}`|" + string.Join("<br/>", g.Select(x => x.Key)))
			//	.Dump("CupertinoStyles", 0);
		}
		public static void ListExposedToolkitV2Styles()
		{
			var resources = new ResourceDictionary();
			var controls = Directory.GetFiles(@"D:\code\uno\platform\Uno.Toolkit\src\library\Uno.Toolkit.Material\Styles\Controls\v2\", "*.xaml")
				.Where(x => !Path.GetFileName(x).Contains('_'));

			foreach (var control in controls)
			{
				//Util.Metatext($"Processing: {control}").Dump();
				var filename = Path.GetFileNameWithoutExtension(control);
				var document = XDocument.Load(control);
				var root = (ResourceDictionary)ScuffedXamlParser.Parse(document.Root);

				var filter = (filename switch
				{
					"Chip" => "Button",
					"NavigationBar" => "utu:NavigationBarPresenter",
					"TabBar" => "utu:TabBarSelectionIndicatorPresenter",

					_ => "",
				});
				//var filtered = root.Values
				//	.Where(x =>
				//		x is StaticResource { Value: Style { Key: string key, TargetType: string type } }
				//			? !filter.Split(',').Contains(type)
				//			: true
				//	);
				//filtered
				//	.OfStaticResourceOf<Style>()
				//	.Select(x => new { x.Key, x.TargetType })
				//	.Dump($"{Path.GetFileNameWithoutExtension(control)}: {filtered.OfStaticResourceOf<Style>().Count()} styles", 0);

				//resources.AddRange(filtered);
			}

			var _common = (ResourceDictionary)ScuffedXamlParser.Load(@"D:\code\uno\platform\Uno.Toolkit\src\library\Uno.Toolkit.Material\Styles\Controls\v2\_Common.xaml");
			var implicitStyles = _common.Values.OfStaticResourceOf<Style>()
				.Select(x => x.BasedOn)
#if true // Toolkit 4.2
				//.Select(x => ((resources[x] as StaticResource)?.Value as Style)?.BasedOn)
#endif
				.ToArray()
				.Dump("ImplicitStyles", 0);
			var aliasMap = _common.Values.Where(x => x.IsStaticResourceFor<StaticResourceRef>())
				.Select(x => new { x.Key, Key2 = ((x as StaticResource)?.Value as StaticResourceRef)?.ResourceKey })
				.ToDictionary(x => x.Key2, x => x.Key)
				.Dump("Aliases", 0);

			//resources.Values.OfStaticResourceOf<Style>()
			//	.Where(x => x.Key != null)
			//	.Where(x => x.Key is string k && !@"
			//			MaterialDefault, MaterialBase, 
			//			BaseStyle, BaseMaterial, BaseTextBlockStyle,
			//		".Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
			//		.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
			//		.Any(k.Contains)
			//	)
			//	.Select(x => new
			//	{
			//		x.Key,
			//		AliasedKey = aliasMap.TryGetValue(x.Key, out var key) ? key : "",
			//		x.TargetType,
			//		ImplicitStyle = implicitStyles.Contains(x.Key) ? "true" : "",
			//	})
			//	.Dump("=== Style Exports ===", 0)
			//	.ToCopyableMarkdownTable()
			//	.Dump();
		}

		[Obsolete]
		private static IEnumerable<(string ResourceKey, string SharedKey, bool IsDefaultStyle)> GetThemeV2StyleInfos()
		{
			const string StylePrefix = "M3Material";
			var result = new List<(string ResourceKey, string SharedKey, bool IsDefaultStyle)>();

			Add("M3MaterialCheckBoxStyle", isImplicit: true);
			Add("M3MaterialAppBarButtonStyle", isImplicit: true);
			Add("M3MaterialCommandBarStyle", isImplicit: true);
			Add("M3MaterialRadioButtonStyle", isImplicit: true);
			Add("M3MaterialDisplayLarge");
			Add("M3MaterialDisplayMedium");
			Add("M3MaterialDisplaySmall");
			Add("M3MaterialHeadlineLarge");
			Add("M3MaterialHeadlineMedium");
			Add("M3MaterialHeadlineSmall");
			Add("M3MaterialTitleLarge");
			Add("M3MaterialTitleMedium");
			Add("M3MaterialTitleSmall");
			Add("M3MaterialLabelLarge");
			Add("M3MaterialLabelMedium");
			Add("M3MaterialLabelSmall");
			Add("M3MaterialBodyLarge");
			Add("M3MaterialBodyMedium", isImplicit: true);
			Add("M3MaterialBodySmall");
			Add("M3MaterialOutlinedTextBoxStyle");
			Add("M3MaterialFilledTextBoxStyle", isImplicit: true);
			Add("M3MaterialOutlinedPasswordBoxStyle");
			Add("M3MaterialFilledPasswordBoxStyle", isImplicit: true);
			Add("M3MaterialElevatedButtonStyle");
			Add("M3MaterialFilledButtonStyle", isImplicit: true);
			Add("M3MaterialFilledTonalButtonStyle");
			Add("M3MaterialOutlinedButtonStyle");
			Add("M3MaterialTextButtonStyle");
			Add("M3MaterialIconButtonStyle");
			Add("M3MaterialCalendarViewStyle", isImplicit: true);
			Add("M3MaterialCalendarDatePickerStyle", isImplicit: true);
			Add("M3MaterialFlyoutPresenterStyle", isImplicit: true);
			Add("M3MaterialMenuFlyoutPresenterStyle", isImplicit: true);
			Add("M3MaterialNavigationViewStyle", isImplicit: true);
			Add("M3MaterialNavigationViewItemStyle", isImplicit: true);
			Add("M3MaterialListViewStyle", isImplicit: true);
			Add("M3MaterialListViewItemStyle", isImplicit: true);
			Add("M3MaterialTextToggleButtonStyle", isImplicit: true);
			Add("M3MaterialIconToggleButtonStyle");
			Add("M3MaterialDatePickerStyle", isImplicit: true);

			return result;

			void Add(string key, string alias = null, bool isImplicit = false) =>
				result.Add((key, alias ?? key.Substring(StylePrefix.Length), isImplicit));
		}
		[Obsolete]
		private static IEnumerable<(string ResourceKey, string SharedKey, bool IsDefaultStyle)> GetToolkitV2StyleInfos()
		{
			const string StylePrefix = "M3Material";
			var result = new List<(string ResourceKey, string SharedKey, bool IsDefaultStyle)>();

			Add("M3MaterialDividerStyle", isImplicit: true);
			Add("M3MaterialNavigationBarStyle", isImplicit: true);
			Add("M3MaterialModalNavigationBarStyle");
			Add("M3MaterialMainCommandStyle", isImplicit: true);
			Add("M3MaterialModalMainCommandStyle");
			Add("M3MaterialTopTabBarStyle");
			Add("M3MaterialColoredTopTabBarStyle");
			Add("M3MaterialElevatedSuggestionChipStyle");
			Add("M3MaterialSuggestionChipStyle");
			Add("M3MaterialInputChipStyle");
			Add("M3MaterialElevatedFilterChipStyle");
			Add("M3MaterialFilterChipStyle");
			Add("M3MaterialElevatedAssistChipStyle");
			Add("M3MaterialAssistChipStyle");
			Add("M3MaterialElevatedSuggestionChipGroupStyle");
			Add("M3MaterialSuggestionChipGroupStyle");
			Add("M3MaterialInputChipGroupStyle");
			Add("M3MaterialElevatedFilterChipGroupStyle");
			Add("M3MaterialFilterChipGroupStyle");
			Add("M3MaterialElevatedAssistChipGroupStyle");
			Add("M3MaterialAssistChipGroupStyle");
			return result;

			void Add(string key, string? alias = null, bool isImplicit = false) =>
				result.Add((key, alias ?? key.Substring(StylePrefix.Length), isImplicit));
		}
		[Obsolete]
		private static IEnumerable<(string ResourceKey, string SharedKey, bool IsDefaultStyle)> ParseGetStyleInfos(string path)
		{
			var source = File.ReadAllText(path);
			var tree = CSharpSyntaxTree.ParseText(source)/*.DumpSyntaxTree()*/;

			var getStyleInfos = tree.GetRoot()
				.DescendantNodes().OfType<MethodDeclarationSyntax>()
				.FirstOrDefault(x => x.Identifier.Text == "GetStyleInfos");

			return getStyleInfos.Body
				//.DumpSyntaxNode()
				.ChildNodes().OfType<ExpressionStatementSyntax>()
				.Select(x => x.Expression)
				.OfType<InvocationExpressionSyntax>()
				.Where(x => (x.Expression as IdentifierNameSyntax)?.Identifier.Text == "Add")
				.Select(x => new
				{
					Key = x.ArgumentList.Arguments[0].Expression.Cast<LiteralExpressionSyntax>().Token.ValueText,
					Implicit = x.ArgumentList.Arguments
						.FirstOrDefault(y => y.NameColon?.Name?.Identifier.ValueText == "isImplicit")
						?.Expression.Cast<LiteralExpressionSyntax>().Token.Value
						as bool? ?? false
				})
				.Select(x => (x.Key, x.Key.RegexReplace("^M3Material", ""), x.Implicit));
		}
	}
	private static partial class Specialized
	{
		public static void CheckLightWeightResourceParity(string path)
		{
			var document = XDocument.Load(path);
			var root = (ResourceDictionary)ScuffedXamlParser.Parse(document.Root);

			var themeResources = root.Values.OfType<ThemeResource>()
				.Select(x => new
				{
					x.Key,
					x.LightValue,
					x.DarkValue,
					Parity = x.AreThemeDefinitionEqual(),
				})
				.ToArray()
				.Dump("ThemeResources", 0);
			var staticResources = root.Values.OfType<StaticResource>()
				.ToArray()
				.Dump("StaticResources", 0);

			Regex.Matches(document.ToString(), @"\{StaticResource (\w+)\}").Cast<Match>()
				.Select(x => x.Groups[1].Value)
				.Where(x => themeResources.Select(x => x.Key).Contains(x))
				.GroupBy(x => x, (k, g) => $"{k} x{g.Count()}")
				.Dump();

			if (themeResources.Count(x => !x.Parity) is { } count && count != 0)
				Util.WithStyle($"{count} of the {themeResources.Length} theme-resources are in disparity", $"color: red").Dump();
			else
				Util.WithStyle($"All {themeResources.Length} theme-resources are in parity", $"color: green").Dump();
		}
		public static string ExtractLightWeightResources(string inspectFile, ResourceDictionary additionalResources)
		{
			var root = (ResourceDictionary)ScuffedXamlParser.Load(inspectFile).Dump(inspectFile, 0);
			var resources = new ResourceDictionary(additionalResources).Merge(root);

			var table = root.Values
				.Where(x => !x.IsStaticResourceFor<Style>())
				.Where(x => x is not StaticResource) // only theme resource should be included for whats considered LightWeight
				.Select(x =>
					x is StaticResource sr ? new { x.Key, RefValue = GetResource(sr.Value) } :
					x is ThemeResource tr ? new { x.Key, RefValue = GetResource(tr.DarkValue) } :
					throw new ArgumentOutOfRangeException()
				)
				.Select(x => new
				{
					x.Key.Key,
					x.RefValue.Type,
					Value = FormatValue(x.RefValue.Value),
				})
				.ToArray()
				.Dump();
			var markdown = table.ToMarkdownTable();
			Clickable.CopyText("Copy as markdown table", markdown).Dump();

			return markdown;

			(string Type, object Value) GetResource(object value)
			{
				if (!(value is StaticResourceRef or ThemeResource))
				{
					return (GetTypename(value), value);
				}

				var innerKey = default(string);
				while (true)
				{
					if (value is StaticResourceRef srr)
					{
						innerKey = srr.ResourceKey;
						if (resources[srr.ResourceKey] is { } mapped)
						{
							value = mapped;
						}
						else
						{
							return (InferTypenameFromSystemKey(srr.ResourceKey), srr.ResourceKey);
						}
					}
					else if (value is ThemeResource tr)
					{
						innerKey = tr.Key.Key;
						value = tr.DarkValue;
					}
					else break;
				}

				return (GetTypename(value), innerKey);
			}
			string InferTypenameFromSystemKey(string key)
			{
				var mappings = new (string Pattern, string Replacement)?[]
				{
					("Brush$",  "Brush"),
					("FontFamily$",  "FontFamily"),
					("FontSize$",  "Double"),
					("CornerRadius$",  "CornerRadius"),
				};
				var result = mappings
					.FirstOrDefault(x => Regex.IsMatch(key, x.Value.Pattern))
					?.Replacement;
				if (result != null)
					Util.WithStyle($"Inferring '{key}' as type '{result}'", "color: orange").Dump();

				return result;
			}
			string GetTypename(object value) => value switch
			{
				GenericValueObject gvo => gvo.Typename,
				StaticResource sr => sr.Value?.GetType().Name,

				_ => value?.GetType().Name,
			};
			string FormatValue(object value) => value switch
			{
				GenericValueObject gvo => gvo.Value,

				_ => value?.ToString(),
			};
		}

		public static void ListThemes() // Colors,Opacities,Brushes
		{
			var palette = (ResourceDictionary)ScuffedXamlParser.Load(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\SharedColorPalette.xaml");
			palette.Values.OfThemeResource<Color>()
				.Dump("Colors", 0)
				.ToPaddedMarkdownTable(separatorPadding: 1, separatorOnSides: true)
				.ToCopyable()
				.Dump();


			palette.Values.OfStaticResource<double>()
				.Dump("Opacities", 0)
				.ToPaddedMarkdownTable(separatorPadding: 1, separatorOnSides: true)
				.ToCopyable()
				.Dump();

			var colors = (ResourceDictionary)ScuffedXamlParser.Load(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\SharedColors.xaml");
			colors.Values.OfThemeResource<SolidColorBrush>()
				.MustAll(x => x.AreThemeDefinitionEqual())
				.Select(x => new
				{
					x.Key,
					Value = (x.LightValue as SolidColorBrush)
				})
				.Select(x => new
				{
					x.Key,
					Color = x.Value.GetDP("Color") is StaticResourceRef srColor ? (object)srColor.ResourceKey : x.Value.Color,
					Opacity = x.Value.GetDP("Opacity") is StaticResourceRef srOpacity ? (object)srOpacity.ResourceKey : x.Value.Opacity,
				})
				.Dump("Brushes", 0)
				.ToPaddedMarkdownTable(separatorPadding: 1, separatorOnSides: true)
				.ToCopyable()
				.Dump();
		}
		public static void DiffThemeToolkitV2InnerResources()
		{
#if false // THEMES
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "Button.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "CalendarDatePicker.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "CalendarView.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "CheckBox.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "ComboBox.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "CommandBar.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "ContentDialog.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "DatePicker.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "FloatingActionButton.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "Flyout.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "HyperlinkButton.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "ListView.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "NavigationView.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "PasswordBox.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "PipsPager.xaml", "PipsPager.UWP.xaml", "PipsPager.Base.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "ProgressBar.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "ProgressRing.xaml", "ProgressRingWinUI.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "RadioButton.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "Ripple.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "Slider.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "TextBlock.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "TextBox.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "ToggleButton.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\themes@{2.6,3.0}\Styles\Controls\v2", "ToggleSwitch.xaml");
#elif true // TOOLKIT
			//DiffResources(@"D:\code\temp\diff_projects\toolkit@{3.0,4.2}\src\library\Uno.Toolkit.Material\Styles\Controls\v2\", "Card.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\toolkit@{3.0,4.2}\src\library\Uno.Toolkit.Material\Styles\Controls\v2\", "CardContentControl.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\toolkit@{3.0,4.2}\src\library\Uno.Toolkit.Material\Styles\Controls\v2\", "Chip.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\toolkit@{3.0,4.2}\src\library\Uno.Toolkit.Material\Styles\Controls\v2\", "ChipGroup.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\toolkit@{3.0,4.2}\src\library\Uno.Toolkit.Material\Styles\Controls\v2\", "Divider.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\toolkit@{3.0,4.2}\src\library\Uno.Toolkit.Material\Styles\Controls\v2\", "NavigationBar.xaml");
			//DiffResources(@"D:\code\temp\diff_projects\toolkit@{3.0,4.2}\src\library\Uno.Toolkit.Material\Styles\Controls\v2\", "TabBar.xaml");
#endif
			//ExtractInnerResources((ResourceDictionary)ScuffedXamlParser.Load(@"D:\code\temp\diff_projects\toolkit@4.2\src\library\Uno.Toolkit.Material\Styles\Controls\v2\NavigationBar.xaml")).Dump();

			(string Key, bool Themed, string Value)[] ExtractInnerResources(ResourceDictionary rd)
			{
				return rd
					.OrderByDescending(x => x.Value is ThemeResource)
					.Select(x => ((string Key, bool Themed, string Value))(
						x.Key.ToString(),
						x.Value is ThemeResource,
						x.Value switch
						{
							StaticResource sr => FormatValue(sr.Value),
							ThemeResource tr => tr.AreThemeDefinitionEqual()
								? FormatValue(tr.LightValue)
								: throw new Exception($"Polarized resource: key={x.Dump().Key}"),
							_ => throw new ArgumentOutOfRangeException($"Invalid resource dictionary value type: {x.Value.GetType().Name} (key={x.Key})"),
						}
					))
					.ToArray();
			}
			void DiffResources(string basePattern, params string[] files)
			{
				if (Regex.Match(basePattern, "{.+}") is not { Success: true } match ||
					!match.Value.TryStripPair("{}", out var args) ||
					args.Split(',') is not [var oldArg, var newArg])
				{
					throw new ArgumentException($"Invalid base pattern: {basePattern}");
				}

				var oldBase = basePattern[..match.Index] + oldArg + basePattern[(match.Index + match.Length)..];
				var newBase = basePattern[..match.Index] + newArg + basePattern[(match.Index + match.Length)..];

				var oldResources = ExtractInnerResources(GetResources(oldBase, files));
				var newResources = ExtractInnerResources(GetResources(newBase, files));
				var keys = oldResources.Concat(newResources).Select(x => x.Key).ToList();

				var table = Pair(oldResources, newResources, x => x.Key)
					.OrderByDescending(x => x.Old?.Themed ?? x.New?.Themed)
					.ThenBy(x => keys.IndexOf(x.Old?.Key ?? x.New?.Key))
					.Select(x => new
					{
						OldKey = x.Old?.Key ?? "- NEWLY ADDED -",
						NewKey = x.New?.Key ?? "- REMOVED -",
						Themed = CompareValue(x.Old?.Themed.ToString(), x.New?.Themed.ToString()),
						Value = CompareValue(x.Old?.Value, x.New?.Value),
					})
					//.Dump(Path.GetFileNameWithoutExtension(files.First()))
					//.Where(x => x.OldKey != "- NEWLY ADDED -")
					//.ToCopyableMarkdownTable().Dump()
					;

				$"# {Path.GetFileNameWithoutExtension(files[0])}".Dump();
				table.ToMarkdownTable().Dump();

				ResourceDictionary GetResources(string basePath, string[] files)
				{
					return files.Select(x => Path.Combine(basePath, x))
						.Where(x => File.Exists(x))
						.Aggregate(new ResourceDictionary(), (acc, x) => acc.Merge((ResourceDictionary)ScuffedXamlParser.Load(x)));
				}
				IEnumerable<(T? Old, T? New)> Pair<T>(IEnumerable<T> oldSource, IEnumerable<T> newSource, Func<T, string> keySelector) where T : struct
				{
					var oldMap = oldSource.ToDictionary(keySelector);
					var newMap = newSource.ToDictionary(keySelector);

					foreach (var (o, n) in oldMap.Join(newMap, o => o.Key, n => n.Key, Tuple.Create).ToArray()) // MIDDLE
					{
						oldMap.Remove(o.Key);
						newMap.Remove(n.Key);

						yield return (o.Value, n.Value);
					}

					var knownPairs = new List<(string OldKey, string NewKey)>()
					{
						("MaterialComboBoxItemSelectedBackgroundThemeBrush", "ComboBoxItemBackgroundSelected"),
						("MaterialComboBoxArrowForegroundThemeBrush", "ComboBoxArrowForeground"),
						("MaterialComboBoxPlaceholderFocusedThemeBrush", "ComboBoxUpperPlaceHolderForeground"),
						("MaterialComboBoxPlaceholderForegroundThemeBrush", "ComboBoxPlaceHolderForeground"),
						("MaterialDateTimeFlyoutBorderThickness", "DatePickerFlyoutBorderThickness"),
						("MaterialDatePickerFlyoutPresenterBackgroundBrush", "DatePickerFlyoutPresenterBackground"),
						("MaterialDatePickerBackgroundColorBrush", "DatePickerButtonBackground"),
						("M3MateriaChipCheckGlyphSize", "ChipCheckGlyphSize"), // typo'd
						("MaterialChipSelectedForeground", "ChipForegroundChecked"),
						("MaterialChipSelectedBackground", "ChipBackgroundChecked"),
						("_____", "_____"),
						("_____", "_____"),
					};
					foreach (var knownPair in knownPairs) // MIDDLE'
					{
						if (oldMap.TryGetValue(knownPair.OldKey, out var oldValue) && newMap.TryGetValue(knownPair.NewKey, out var newValue))
						{
							oldMap.Remove(knownPair.OldKey);
							newMap.Remove(knownPair.NewKey);

							yield return (oldValue, newValue);
						}
					}

					var mutationsT1 = new List<(Func<string, string> OldKeyMutator, Func<string, string> NewKeyMutator)>
					{
						(o => o.Replace("PathData", "Data"), n => n),
						(o => o.RegexReplace("GlyphPathStyle", "GlyphPathData"), n => n),
						(o => Path.GetFileNameWithoutExtension(files[0]) + o, n => n),
						(o => Path.GetFileNameWithoutExtension(files[0]) + o.RegexReplace("^(M3)?Material", ""), n => n),
						(o => o.RegexReplace("BackgroundBrush$", "Brush"), n => n),
						(o => o.RegexReplace("(Theme|Color)Brush$", ""), n => n),
						(o => o.RegexReplace("(Theme|Color)Brush$", "Brush"), n => n),
						(o => o.RegexReplace("(Theme|Color)Brush$", "").RegexReplace("(Selected)?(PointerOver|Pressed|Focused|Unfocused|Disabled)(.+)$", "$3$1$2"), n => n),
						(o => o.RegexReplace("(Theme|Color)Brush$", "").RegexReplace("(Selected)?(PointerOver|Pressed|Focused|Unfocused|Disabled)(.+)$", "$3$1$2"), n => n.Replace("Checked", "Selected")),
						(o => o.RegexReplace("(Theme|Color)Brush$", "").RegexReplace("(Selected)?(PointerOver|Pressed|Focused|Unfocused|Disabled)(.+)$", "$3$1$2"), n => n.Replace("Background", "")),
					};
					var mutationsT2 = new List<(Func<string, string> OldKeyMutator, Func<string, string> NewKeyMutator)>
					{
						(o => o, n => n),
						(o => o.RegexReplace("^(M3)?Material", ""), n => n),
						(o => o.Replace(Path.GetFileNameWithoutExtension(files[0]), ""), n => n.Replace(Path.GetFileNameWithoutExtension(files[0]), "")),
						
						//(o => o.Replace("Selected", "Checked"), n => n),
						
						(o => o.Replace("SurfaceFab", "FabSurface"), n => n),
						(o => o.Replace("SecondaryFab", "FabSecondary"), n => n),
						(o => o.Replace("TertiaryFab", "FabTertiary"), n => n),
					};
					foreach (var t2 in mutationsT2)
						foreach (var t1 in mutationsT1)
						{
							(Func<string, string> OldKeyMutator, Func<string, string> NewKeyMutator) mutation = (
								OldKeyMutator: (string x) => t1.OldKeyMutator(t2.OldKeyMutator(x)),
								NewKeyMutator: (string x) => t1.NewKeyMutator(t2.NewKeyMutator(x)));
							foreach (var (o, n) in oldMap.Join(newMap, o => mutation.OldKeyMutator(o.Key), n => mutation.NewKeyMutator(n.Key), Tuple.Create).ToArray()) // MIDDLE'
							{
								oldMap.Remove(o.Key);
								newMap.Remove(n.Key);

								yield return (o.Value, n.Value);
							}
						}
					foreach (var o in oldMap) // LEFT
					{
						yield return (o.Value, default);
					}
					// we dont care above new values for migration reference
					//foreach (var n in newMap) // RIGHT
					//{
					//	yield return (default, n.Value);
					//}
				}
			}

			string FormatValue(object value)
			{
				return value switch
				{
					StaticResourceRef srr => srr.ResourceKey,
					Style style => $"Style@{style.TargetType}",
					GenericValueObject gvo when gvo.Typename == "LottieVisualSource" => gvo.Value,
					GenericValueObject gvo when gvo.Typename == "GridLength" => gvo.Value,
					GenericValueObject gvo when gvo.Typename == "ControlTemplate" => null,

					_ => value?.ToString(),
				};
			}
			string CompareValue(string o, string n)
			{
				if (o == null) return n;
				if (n == null) return o;
				if (o == n) return o;

				return o.Length + n.Length > 100
					? string.Join("\n", o, "->", n)
					: string.Join(" ", o, "->", n);
			}
		}
	}
}
public partial class Script
{
	private static partial class LightWeightSourceGen
	{
		public static void GenerateThemeCsMarkup()
		{
			var context = new[]
			{
				@"D:\code\uno\framework\Uno\src\Uno.UI\UI\Xaml\Style\Generic\SystemResources.xaml",
				@"D:\code\uno\framework\Uno\src\Uno.UI.FluentTheme.v2\themeresources_v2.xaml",
				@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\Common\TextBoxVariables.xaml",
				@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\Common\Fonts.xaml",
				@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\Typography.xaml",
				@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\SharedColors.xaml",
				@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\SharedColorPalette.xaml",
			}.Aggregate(new ResourceDictionary(), (acc, file) => acc.Merge((ResourceDictionary)ScuffedXamlParser.Load(file)));
			Util.Metatext("=== finished parsing context").Dump();
			Console.WriteLine();
			//Util.ClearResults();

			Directory.GetFiles(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\", "*.xaml")
				.Where(x => x != @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\_Resources.xaml")
				//.Where(x => x == @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\Button.xaml")
				.Where(x => x == @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\CheckBox.xaml") // fixme: CheckBoxCheckBackgroundFillUnchecked
				//.Where(x => x == @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\ComboBox.xaml") // fixme?: dupes ComboBoxBackgroundDisabled,ComboBoxBackgroundUnfocused
				//.Where(x => x == @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\ContentDialog.xaml")
				//.Where(x => x == @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\CalendarView.xaml")
				//.Where(x => x == @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\DatePicker.xaml") // fixme: Button prefix not trimming properly
				//.Where(x => x == @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\FloatingActionButton.xaml")
				//.Where(x => x == @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\HyperlinkButton.xaml")
				//.Where(x => x == @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\PasswordBox.xaml")
				//.Where(x => x == @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\RatingControl.xaml") // fixme: RatingControlForegroundSelected, RatingControlForegroundSelected.Set
				//.Where(x => x == @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\Slider.xaml")
				//.Where(x => x == @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\TextBlock.xaml")
				//.Where(x => x == @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\ToggleSwitch.xaml")
				.ForEach(x => InnerImpl(x, context, GetOptionsFor(x)));
			//InnerImpl(/* OUTLIER */ @"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\TextBlock.xaml");

			void InnerImpl(string path, ResourceDictionary context, SourceGenOptions options = default)
			{
				var outputPath = path.RegexReplace(@"\.xaml$", ".cs");
				if (options?.Production == true)
				{
					outputPath = Path.Combine(@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Themes.WinUI.Markup\Theme\", Path.GetFileName(outputPath));
				}
				if (options?.Skip == true)
				{
					Util.WithStyle($"skipped: {path}", "color: orange").Dump();
					File.Delete(outputPath);
					return;
				}

				var source = GenerateCsMarkup(path, context, options);

				//Util.VerticalRun(Util.OnDemand("source", () => source), source.ToCopyable()).Dump("source");
				File.WriteAllText(outputPath, source);
				$"lines: {source.Count(x => x == '\n')}, length: {source.Length} => {outputPath}".Dump();
			}

			SourceGenOptions GetOptionsFor(string path)
			{
				var options = (SourceGenOptions)(Path.GetFileNameWithoutExtension(path) switch
				{
					"FloatingActionButton" => new() { TargetType = "Button" },
					"Slider" => new() { ForcedGroupings = "TickBar,Track".Split(',').ToDictionary(x => x, x => default(string)) },
					"ToggleSwitch" => new() { ForcedGroupings = "Knob,Thumb".Split(',').ToDictionary(x => x, x => default(string)) },

					"CommandBar" or
					"MediaPlayerElement" or
					"PipsPager.Base" or // todo
					"RatingControl" or
					"Flyout" or // todo
					"ListView" or // todo
					"NavigationView" or // todo
					"PipsPager" or
					"PipsPager.Base" or
					"Ripple"
						=> new() { Skip = true },

					_ => new(),
				});
				options.ForcedGroupings["Typography"] = "CharacterSpacing,FontFamily,FontSize,FontWeight";
				options.PromoteDefaultStyleResources = true;
				options.IgnoredResourceTypes = "ControlTemplate,LottieVisualSource".Split(',');
				options.NamespaceImports = new[]
				{
					"System",
					"Windows.UI",
					"Microsoft.UI.Text",
					"Microsoft.UI.Xaml",
					"Microsoft.UI.Xaml.Media",
					"Uno.Extensions.Markup",
					"Uno.Extensions.Markup.Internals",
				};
				options.Namespace = "Uno.Themes.Markup";
				options.Production = false;

				return options;
			}
		}
		public static void GenerateToolkitCsMarkup()
		{
			var context = new[]
			{
				@"D:\code\uno\framework\Uno\src\Uno.UI\UI\Xaml\Style\Generic\SystemResources.xaml",
				@"D:\code\uno\framework\Uno\src\Uno.UI.FluentTheme.v2\themeresources_v2.xaml",
				@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\Common\TextBoxVariables.xaml",
				@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\Common\Fonts.xaml",
				@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\Typography.xaml",
				@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\SharedColors.xaml",
				@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\SharedColorPalette.xaml",
			}.Aggregate(new ResourceDictionary(), (acc, file) => acc.Merge((ResourceDictionary)ScuffedXamlParser.Load(file)));
			Util.Metatext("=== finished parsing context").Dump();
			//Console.WriteLine();
			Util.ClearResults();

			Directory.GetFiles(@"D:\code\uno\platform\Uno.Toolkit\src\library\Uno.Toolkit.Material\Styles\Controls\v2", "*.xaml")
				.Where(x => x != @"D:\code\uno\platform\Uno.Toolkit\src\library\Uno.Toolkit.Material\Styles\Controls\v2\_Common.xaml")
				//.Select(x => $".Where(x => x == @\"{x}\")").Dump();
				//.Where(x => x == @"D:\code\uno\platform\Uno.Toolkit\src\library\Uno.Toolkit.Material\Styles\Controls\v2\Card.xaml")					// fixme: vsg via overlay are not grouped
				//.Where(x => x == @"D:\code\uno\platform\Uno.Toolkit\src\library\Uno.Toolkit.Material\Styles\Controls\v2\CardContentControl.xaml")		// fixme: vsg via overlay are not grouped
				//.Where(x => x == @"D:\code\uno\platform\Uno.Toolkit\src\library\Uno.Toolkit.Material\Styles\Controls\v2\Chip.xaml")					// OK
				//.Where(x => x == @"D:\code\uno\platform\Uno.Toolkit\src\library\Uno.Toolkit.Material\Styles\Controls\v2\ChipGroup.xaml")				// OK
				//.Where(x => x == @"D:\code\uno\platform\Uno.Toolkit\src\library\Uno.Toolkit.Material\Styles\Controls\v2\Divider.xaml")				// OK
				//.Where(x => x == @"D:\code\uno\platform\Uno.Toolkit\src\library\Uno.Toolkit.Material\Styles\Controls\v2\NavigationBar.xaml")			// skipped; fixme: BaseMaterial, messy generation
				//.Where(x => x == @"D:\code\uno\platform\Uno.Toolkit\src\library\Uno.Toolkit.Material\Styles\Controls\v2\TabBar.xaml")					// fixme: hide base styles
				.ForEach(x => InnerImpl(x, context, GetOptionsFor(x)));

			void InnerImpl(string path, ResourceDictionary context, SourceGenOptions options = default)
			{
				var outputPath = path.RegexReplace(@"\.xaml$", ".cs");
				if (options?.Production == true)
				{
					outputPath = Path.Combine(@"D:\code\uno\platform\Uno.Toolkit\src\library\Uno.Toolkit.WinUI.Material.Markup\Theme\", Path.GetFileName(outputPath));
				}
				if (options?.Skip == true)
				{
					Util.WithStyle($"skipped: {path}", "color: orange").Dump();
					File.Delete(outputPath);
					return;
				}

				var source = GenerateCsMarkup(path, context, options);

				//Util.VerticalRun(Util.OnDemand("source", () => source), source.ToCopyable()).Dump("source");
				File.WriteAllText(outputPath, source);
				//$"lines: {source.Count(x => x == '\n')}, length: {source.Length} => {outputPath}".Dump();
			}

			SourceGenOptions GetOptionsFor(string path)
			{
				var options = (SourceGenOptions)(Path.GetFileNameWithoutExtension(path) switch
				{
					"Card" or
					"CardContentControl" or
					"Chip" or
					"ChipGroup" or
					"Divider" or
					//"NavigationBar" or
					"TabBar" or
					"asd" => new() { },

					//_ => new() { },
					_ => new() { Skip = true },
				});
				options.ForcedGroupings["Typography"] = "CharacterSpacing,FontFamily,FontSize,FontWeight";
				options.PromoteDefaultStyleResources = true;
				options.IgnoredResourceTypes = "DataTemplate,ControlTemplate,ItemsPanelTemplate,LottieVisualSource".Split(',');
				options.NamespaceImports = new[]
				{
					"System",
					"Windows.UI",
					"Microsoft.UI.Text",
					"Microsoft.UI.Xaml",
					"Microsoft.UI.Xaml.Media",
					"Uno.Extensions.Markup",
					"Uno.Extensions.Markup.Internals",
					null,
					"ResourceKeyDefinitionAttribute = Uno.Toolkit.WinUI.Material.Markup.ResourceKeyDefinitionAttribute",
				};
				options.Namespace = "Uno.Toolkit.Markup";
				options.UseFileScopedNamespace = true;
				options.XamlControlTypeResolver = type => type.Split(':', 2) switch
				{
					["utu", var typename] => $"global::Uno.Toolkit.UI.{typename}",
					_ => type,
				};
				options.Production = true;

				return options;
			}
		}

		private static string GenerateCsMarkup(string file, ResourceDictionary context, SourceGenOptions options)
		{
			return InnerImpl(file, context, options);

			string InnerImpl(string path, ResourceDictionary context, SourceGenOptions options = default)
			{
				if (options?.Skip == true) return null;

				options ??= new();
				options.TargetType ??= Path.GetFileNameWithoutExtension(path);

				var rd = ScuffedXamlParser.Load<ResourceDictionary>(path);
				rd.ResolveWith(context);

				//rd.Values.Dump("ResourceDictionary", 0);
				//options.Dump("options", 0);

				var resources = new NamedResourceBag { Name = "Resources" };
				var styles = new NamedResourceBag { Name = "Styles", Sortable = false };
				foreach (var resource in rd)
				{
					if (resource.Value is StaticResource sr &&
						sr.Value is Style style &&
						style.TargetType?.Split(':', 2).LastOrDefault() == options.TargetType &&
						style.Setters.Any())
					{
						//Util.Metatext($"Style: {sr.Key}").Dump();
						//style.Dump($"Style: {sr.Key}", 0);
						var name = sr.Key.Key
							.RemoveHead(options.TrimMaterialPrefix ? "Material" : null)
							.RemoveTail("Style")
							.RemoveTail(options.TargetType);
						if (name != "Base")
						{
							var srr = new StaticResourceRef(sr.Key.Key);
							styles.Resources.Add(new StaticResourceRef(sr.Key.Key));
							styles.ContextedResources.Add((srr, name.EmptyAsNull() ?? "Default"));
						}

						var wip = ExtractRelationship(options, rd, style);
						var styleResources = Rehierarchy(options, rd, style, wip);

						styleResources.Name = name;
						{
							// we need to remove the extra prefix from its resources+descendents before the context is lost.
							//styleResources.Dump("styleResources", 0);
							var prefixes = sr.Key.Key.RemoveTail("Style")
								.PermuteWords()
								.Concat(options.TrimMaterialPrefix && name?.Length > 0 ? name.PermuteWords() : Array.Empty<string>())
								.Concat(options.TrimMaterialPrefix ? new[] { "Material" } : Array.Empty<string>())
								.ToArray();
							styleResources.Flatten(x => x.Children).ForEach(item =>
							{
								//item.Dump(item.Name, 0);
								if (item != styleResources)
								{
									item.Name = UpdateName(item.Name);
								}
								item.ContextedResources = item.ContextedResources
									.Select(x => x with { Context = UpdateName(x.Context) })
									.ToHashSet();
							});

							string UpdateName(string name)
							{
								foreach (var prefix in prefixes)
								{
									if (name.StartsWith(prefix))
									{
										return name[prefix.Length..];
									}
								}

								return name;
							}
						}

						if (options.PromoteDefaultStyleResources && ",Base,Default".Split(',').Contains(styleResources.Name))
						{
							resources.Merge(styleResources);
						}
						else
						{
							resources.Children.Add(styleResources);
						}
					}
				}

				PreGenProcessing(options, resources);

				var control = new NamedResourceBag { Name = options.TargetType, Sortable = false, Children = { resources, styles } };
				var theme = new NamedResourceBag { Name = "Theme", Sortable = false, Children = { control } };
				var source = Generate(options, rd, theme);

				return source;
			}
		}
		private static NamedResourceBag ExtractRelationship(SourceGenOptions options, ResourceDictionary rd, Style style)
		{
			var templateRoot = style.GetTemplateRoot();
			var vsgs = (templateRoot?.GetVisualStateGroups())
				.Safe()
				.ToArray()
				//.Dump("VisualStateGroups", 0)
				;

			// Visual Tree
			templateRoot
				?.TreeGraph(x => x.Children, x => x.Name is { Length: > 0 } name ? $"{x.Type}#{name}" : x.Type)
				.OnDemand("Click to expand")
				//.Dump("Visual Tree")
				;
			templateRoot
				?.Flatten()
				//.Dump("Flattened VisualTree", 0)
				;

			var propertiesContainingReferences = Enumerable
				.Concat(
					style.Setters.Select(x => new { Path = "", x.Property, x.Value }),
					(templateRoot ?? new VisualTreeElement("empty"))
						.Flatten()
						.SelectMany(x => Enumerable
							.Concat(x.Element.Properties, x.Element.AttachedProperties)
							.Select(y => new { x.Path, Property = y.Key, y.Value })
						)
				)
				.Where(y => y.Value is IResourceRef)
				.Where(y => y.Property != "Style")
				.OrderBy(y => y.Property)
				.Select(x => new
				{
					Path = $"{x.Path}/@{x.Property}",
					TargetName = x.Path.Split('/').Last().Split('#') is [var type, var name] ? name : null,
					PropertyName = x.Property,
					//Value = y.Value,
					Reference = (IResourceRef)x.Value,
				})
				//.GroupBy(x => x.Value, (k, g) => new { Key = k, Properties = g.Select(x => x.Path).JoinBy("\n") })
				.Where(x => rd.ContainsKey(x.Reference.ResourceKey))
				//.Dump("Referential Properties by XPath", 0)
				;

			var propertiesAffectedByReferentialVSGs = vsgs
				.SelectMany(vsg => new[]
					{
						vsg.VisualStates.SelectMany(vs => vs.Setters.Select(x => new { Source = vs.Name, x.Target, x.Value, IsTimeline = false })),
						vsg.VisualStates.SelectMany(vs => (vs.Storyboard?.Children).Safe().Select(x => new { Source = vs.Name, Target = x.TargetName + x.TargetProperty?.Prefix("."), Value = (object)x.Value, IsTimeline = true })),
						vsg.Transitions.SelectMany(t => (t.Storyboard?.Children).Safe().Select(x => new { Source = $"{t.From}->{t.To}", Target = x.TargetName + x.TargetProperty?.Prefix("."), Value = (object)x.Value, IsTimeline = true })),
					}
					.SelectMany(row => row.Safe().Select(x => new { Source = $"{vsg.Name}\\{x.Source}", x.Target, x.Value, x.IsTimeline }))
				)
				// Timelines have simplified value, its references are expressed as S[Key] or T[Key]
				.Where(x => !x.IsTimeline ? x.Value is IResourceRef : x.Value is string simplified && Regex.IsMatch(simplified, @"\[\w+\]"))
				// extract references used
				.Select(x => new
				{
					x.Source,
					//x.Target, 
					TargetName = x.Target.Split('.')[0],
					PropertyName = x.Target.Split('.', 2).LastOrDefault().StripPair("()"),
					x.Value,
					References = (x.Value switch
					{
						IResourceRef rr => new[] { rr },
						string simplified when x.IsTimeline => Regex.Matches(simplified, @"(S|T)\[(\w+)\]").Cast<Match>()
							.Select(x => (IResourceRef)(x.Groups[1].Value switch
							{
								"S" => new StaticResourceRef(x.Groups[2].Value),
								"T" => new ThemeResourceRef(x.Groups[2].Value),
								_ => throw new ArgumentOutOfRangeException(),
							}))
							.ToArray(),
						_ => Array.Empty<IResourceRef>(),
					}).Where(y => rd.ContainsKey(y.ResourceKey))
				})
				.Where(x => x.References?.Any() ?? false)
				.ToArray()
				//.Dump("Properties affected by visual-state (that also reference local resources)", 0)
				;

			// note: since we did filter both left and right sides by "containing reference",
			// here we need to do a full-join
			// fixme: perhaps it is better to not prefilter sources, and perform a inner-join and then filter?
			//		^ we will still need to left/right-join, since VSG can modified a locally undefined property...
			var tmp = (templateRoot ?? new VisualTreeElement("empty"))
				.Flatten()
				.Select(x => new
				{
					x.Path,
					TargetName = x.Path.Split('/').Last().Split('#') is [var type, var name] ? name : null,
				})
				.Prepend(new { Path = "", TargetName = default(string) })
				.Select(x => new
				{
					x.Path,
					Properties = Enumerable.Concat(
						propertiesContainingReferences
							.Where(y => y.Path.Split("/@", 2)[0] == x.Path) // note: we can have referential properties on xname-less element, and they need to be preserved
							.Select(y => new { y.PropertyName, y.Reference, Context = "Default" }),
						propertiesAffectedByReferentialVSGs
							.Where(y => y.TargetName == x.TargetName)
							.SelectMany(y => y.References.Select(z => new { y.PropertyName, Reference = z, Context = y.Source }))
					)
				})
				.SelectMany(x => x.Properties.Select(y => new { x.Path, y.PropertyName, y.Reference, y.Context }))
				.GroupBy(x => $"{x.Path}/@{x.PropertyName}", (k, g) => new { Path = k, References = g.Select(x => (x.Reference, x.Context)).ToArray() })
				//.Dump("query: pre-group 1", 0)
				// ---
				.GroupBy(x => x.References.OrderByDescending(x => x.Context == "Default").FirstOrDefault(), (k, g) => new
				{
					//k.Reference,
					Reference = k.Reference.ResourceKey,
					k.Context,
					//ResourceGroupAndPaths = g.GroupBy(x => x.References, new SequentialEqualityComparer<(IResourceRef, string)>()),
					//ResourceGroupAndPaths2 = g.GroupBy(x => x.References, (k, g) => (Keys: k, Paths: g.Select(x => x.Path)), new SequentialEqualityComparer<(IResourceRef Reference, string Context)>()).SingleOrDefault(),
					ResourceGroupAndPaths = (
						Keys: g.SelectMany(x => x.References),
						Paths: g.Select(x => x.Path)
					)
				})
				// consolidate by key, favoring non-visualstate-based value
				.OrderByDescending(x => x.Context == "Default").DistinctBy(x => x.Reference)
				.OrderBy(x => x.Reference)
				.OrderByDescending(x => x.ResourceGroupAndPaths.Keys.Count()).ThenBy(x => x.Reference) // debug ordering
				.Select(x => new
				{
					x.Reference,
					x.Context,
					ResourceGroup = x.ResourceGroupAndPaths.Keys,
					Paths = x.ResourceGroupAndPaths.Paths,
				})
				.ToArray()
				//.Dump("tmp", 0)
				;

			var result = new NamedResourceBag();
			result.Children.AddRange(tmp.Select(x => new NamedResourceBag
			{
				Name = x.Reference,
				Resources = x.ResourceGroup.Select(y => y.Reference).ToHashSet(),
				ContextedResources = x.ResourceGroup.ToHashSet(),
				Paths = x.Paths.ToHashSet(),
			}));

			return result;
		}
		private static NamedResourceBag Rehierarchy(SourceGenOptions options, ResourceDictionary rd, Style style, NamedResourceBag wip)
		{
			// inject context
			wip.Children.ForEach(x => InferRawBagContext(options, style.TargetType, x));
			//if (wip.Children.Any(x => x.Resources.Any(y => y.ResourceKey.StartsWith(""))))
			//{
			//	wip.Children
			//		.OrderBy(x => x.Name)
			//		//.Where(x => x.Name.Contains("Elevation"))
			//		.Dump("wip.Children (pre-hierarchy)", 1, exclude: "Resources,Children,Sortable"); // note: depth=0 will not snapshot the state
			//	//throw new NotImplementedException();
			//}

			// reduce perfect resource subset
			foreach (var item in wip.Children.ToArray())
			{
				if (wip.Children.FirstOrDefault(x => x != item && item.Resources.IsSubsetOf(x.Resources)) is { } superset)
				{
					wip.Children.Remove(item);
					superset.Merge(item);
				}
			}
			//if (wip.Children.SelectMany(x => x.Resources).Count() != wip.Children.SelectMany(x => x.Resources).Distinct().Count())
			//{
			//	wip.Children.SelectMany(x => x.Resources).Select(x => x.ResourceKey).Dump().Distinct().Dump();
			//	Debug.Assert(wip.Children.SelectMany(x => x.Resources).Count() == wip.Children.SelectMany(x => x.Resources).Distinct().Count(), "from here on, there is should be no duplicate");
			//}

			var resources = new NamedResourceBag { Name = "Resources" };
			//wip.Children.OrderBy(x => x.Name).ToArray().Dump("wip.Children", 0);

			IEnumerable<NamedResourceBag> CombineByLeftNameIfPossible(IEnumerable<NamedResourceBag> bags)
			{
				var results = new List<NamedResourceBag>();
				foreach (var g in bags.GroupBy(x => x.LeftName))
				{
					var children = g.ToList();

					if (children is [var singleEntry])
					{
						// linear
						results.Add(singleEntry);
					}
					else if (string.IsNullOrEmpty(g.Key) ||
						style.Setters.Any(x => x.Property == g.Key))
					{
						// linear
						results.AddRange(children);
					}
					else
					{
						// hierarchical
						var bag = new NamedResourceBag { Name = g.Key, Children = children };
						foreach (var item in bag.Children)
						{
							item.Name = item.Name.RemoveHead(g.Key);
						}

						results.Add(bag);
					}
				}

				// setter property and vsg will not be grouped by .LeftName, so we have to handle them separately at the end
				foreach (var setterProperty in results.Where(x => x.Paths.JustOneOrDefault()?.StartsWith("/@") == true).ToArray())
				{
					if (results
						.Flatten(x => x.Children)
						.FirstOrDefault(x => x != setterProperty && setterProperty.Name == (x.LeftName + x.RightName)) is { } matchingVsg)
					{
						results.Remove(setterProperty);
						matchingVsg.Merge(setterProperty);
					}
				}

				//results.Dump("results (after setters re-insert)", 1);
				return results;
			}

			// 1. extract forced groups
			// we are extracting forced groups before setter properties,
			// because otherwise it could result in resources belonging to the same property
			// placed under different nodes (setter vs visual-state-group), thus prevent them from being combined.
			wip.Children.GroupBy(x => options.ForcedGroupings?.FirstOrNull(kvp => (kvp.Value ?? kvp.Key).Split(',').Any(y => x.Name.Contains(y)))?.Key)
				//.OrderBy(x => x.Key).Dump("ForcedGroupings", 1)
				.Where(g => g.Key != null)
				.ForEach(g =>
				{
					foreach (var item in g)
					{
						wip.Children.Remove(item);
						item.Name = item.Name.RemoveOnce(g.Key);
						item.LeftName = item.LeftName.RemoveOnce(g.Key);
					}

					var bag = new NamedResourceBag
					{
						Name = g.Key,
						Children = CombineByLeftNameIfPossible(g).ToList(),
					};
					resources.Children.Add(bag);
				});

			// ~~2. extract setter(control-level) properties~~ merged into next part
			//wip.Children.Where(x => x.DebugPaths.All(y => y.StartsWith("/@")))
			//	.ToArray()
			//	.ForEach(x =>
			//	{
			//		wip.Children.Remove(x);
			//		resources.Children.Add(x);
			//	});

			// 3. extract remainders
			resources.Children.AddRange(CombineByLeftNameIfPossible(wip.Children));

			//resources.Dump("resources", 0);
			//resources.Children.Dump("resources.Children", 1);
			//resources.Flatten(x => x.Children).MustAll(x => x.Resources.Count * x.Children.Count == 0, x => $"{x.Name} contains both resource-group and children");

			return resources;
		}
		private static void InferRawBagContext(SourceGenOptions options, string targetType, NamedResourceBag bag)
		{
			bag.Name = bag.Name.RemoveOnce(options.TrimControlName ? targetType : null);
			bag.RightName = InferVisualStateBagName(bag);
			bag.LeftName = InferLeftName(bag, bag.RightName);

			string InferVisualStateBagName(NamedResourceBag bag)
			{
				var properties = bag.Paths.Select(x => x.Split("/@", 2).ElementAtOrDefault(1)).Distinct().ToArray();
				if (properties.Length == 1)
				{
					return properties[0];
				}
				// exceptions...
				if (properties.Length == 2)
				{
					var exceptions = new List<(string Properties, string MergedNames)>
					{
						("Width,Height", "Length,Thickness,Size"),
						("RadiusX,RadiusY", "Radius"),
					};
					foreach (var (propertyGroup, aliases) in exceptions)
					{
						if (properties.IsSubset(propertyGroup.Split(',')) && aliases.Split(',').FirstOrDefault(bag.Name.EndsWith) is { } property)
						{
							return property;
						}
					}
				}

				return null;
			}
			static string InferLeftName(in NamedResourceBag bag, string rightName) // 2nd argument used to suggest order
			{
				var visualStateNames = bag.ContextedResources
					.Select(x => x.Context?.Split('\\', 2) is [var vsg, var state] ? state : null)
					.Where(x => x is { })
					.ToArray();
				// suffixes options are grouped by suffix of same category
				// to avoid repeat lookup, eg: 
				// once "Pressed" is found, there is no need to check "Disabled" anymore
				var suffixesOptions = new List<string[]>();
				suffixesOptions.Add(visualStateNames);
				if (rightName != null)
					suffixesOptions.Add(new[] { rightName });

				// repeatly remove every suffixes options
				var result = bag.Name;
				var hit = true;
				do
				{
					hit = false;
					foreach (var options in suffixesOptions)
					{
						if (options.FirstOrDefault(result.EndsWith) is { } tail)
						{
							result = result.RemoveTail(tail);

							suffixesOptions.Remove(options);
							hit = true;
							break;
						}
					}
				} while (hit && suffixesOptions.Count > 0);

				return result;
			}
		}
		private static NamedResourceBag PreGenProcessing(SourceGenOptions options, NamedResourceBag resources)
		{
			//resources.Flatten(x => x.Children).Where(x => x.Name == "Thumb").ForEach(bag => bag.Children.Dump(bag.Name ?? "<null>", 1));
			resources.Flatten(x => x.Children).ForEach(AdjustVisualStateGroupedResources);
			TrimAndPromote();
			PromoteSingleResource(resources);

			void AdjustVisualStateGroupedResources(NamedResourceBag bag)
			{
				// flag vsg-bag, so they are non-sortable
				if (bag.ContextedResources.Select(x => x.Context).Where(x => x != "Default").ToArray() is { Length: > 0 } contexts &&
					contexts.All(x => x.Contains(@"\")))
				{
					bag.IsVSG = true;
				}

				// remove vsg name from vs name
				bag.ContextedResources = bag.ContextedResources
					.Select(x => x with { Context = x.Context.Split('\\', 2)[^1] })
					.ToHashSet();

				// correct vsg names that doesnt have normal/default state
				if (bag.Resources.Count > 1 &&
					bag.ContextedResources.Select(x => x.Context).FirstOrDefault(bag.Name.Contains) is { } stateName)
				{
					bag.Name = bag.Name.RegexReplace(Regex.Unescape(stateName), "");
				}
			}
			void PromoteSingleResource(NamedResourceBag node)
			{
				foreach (var item in node.Children)
				{
					PromoteSingleResource(item);
				}

				foreach (var item in node.Children.Where(x => x.Resources.Count == 1 && x.Children.Count == 0).ToArray())
				{
					node.Children.Remove(item);

					if (item.Paths.FirstOrDefault().StartsWith("/@") &&
						node.Children.FirstOrDefault(x => x != item && (x.Name == item.Name)) is { } vsg)
					{
						// merge with relevant visual state group
						vsg.Merge(item);
					}
					else
					{
						// otherwise promote
						if (item.Resources.FirstOrDefault() is { } resource)
						{
							node.Resources.Add(resource);
							node.ContextedResources.Add((resource, Context: item.Name));
							node.Paths.AddRange(item.Paths);
						}
					}
				}
			}
			void TrimAndPromote()
			{
				foreach (var style in resources.Children)
				{
					style.ContextedResources = style.ContextedResources.Select(x => x with { Context = UpdateName(x.Context) }).ToHashSet();
					style.Children.ForEach(x => x.Name = UpdateName(x.Name));

					string UpdateName(string name) => name
						.RemoveHead(style.Name)
						.RemoveHead(options.TargetType);
				}

				TryPromote(resources);

				void TryPromote(NamedResourceBag bag)
				{
					foreach (var item in bag.Children.ToArray())
					{
						TryPromote(item);

						// if we fully trimmed the name of a bag or if its name is same as parent's, it should just be merged into its parent
						if (string.IsNullOrEmpty(item.Name) || item.Name.PermuteWords().Contains(bag.Name))
						{
							// prevent generation of .Default property when its Name is fully trimmed
							if (string.IsNullOrEmpty(item.Name) && item.Resources.Count == 1)
							{
								item.ContextedResources = item.ContextedResources
									.Select(x => x with { Context = item.Paths.FirstOrDefault().Split("/@", 2).LastOrDefault() })
									.ToHashSet();
							}

							bag.Children.Remove(item);
							bag.Merge(item);
						}
					}
				}
			}

			return resources;
		}
		private static string Generate(SourceGenOptions options, ResourceDictionary rd, NamedResourceBag theme)
		{
			//theme.Dump("theme bag", 0);
			//theme.Flatten(x => x.Children).FirstOrDefault(x => x.Name == "Resources").Dump("Resources bag", 0);
			//theme.Flatten(x => x.Children).FirstOrDefault(x => x.Name == "Styles").Dump("Styles bag", 0);
			
			theme.Flatten(x => x.Children)
				.Where(x => x.IsVSG)
				.Dump("VSG bags", 1, exclude: "Resources, Sortable");

			var buffer = new IndentedTextBuilder("\t") { BlockConsecutiveEmptyLines = true };

			using (WriteFileHeader())
			{
				WriteBag(theme);
			}

			return buffer.ToString();

			bool WriteBag(NamedResourceBag bag)
			{
				//bag.Dump(bag.Name ?? "<null>", 0);
				var children = GetChildrenSorted(bag).ToArray();
				var resources = bag.ContextedResources
					.Where(x => !(options.IgnoredResourceTypes?.Contains(ResolveResourceNiceTypeName(x.Resource)) ?? false))
					// trim duplicate resource keys
					.OrderByDescending(y => y.Context == "Default")
					.ThenByDescending(y => y.Context == "Normal")
					.DistinctBy(y => y.Context)
					.DistinctBy(y => y.Resource)
					.Apply(sequence =>
						!bag.Sortable ? sequence :
						!bag.IsVSG ? sequence.OrderBy(x => x.Context) : sequence
							.OrderByDescending(y => y.Context == "Default")
							.ThenByDescending(y => y.Context == "Normal")
					)
					.ToArray();

				if (children.Length == 0 && resources.Length == 0) return false;

				var hadSibling = false;
				// .default syntax: // used to skip 2nd part of... (Background.Default/Normal)
				//		public static readonly BackgroundVSG Background = new();
				//		public class BackgroundVSG
				//			public ThemeResourceKey<Brush> Default { get; } = ...;
				//			public ThemeResourceKey<Brush> PointerOver { get; } = ...;
				//			public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Default;
				var vsgDefaultState = options.Production && bag.IsVSG
					? resources.FirstOrNull(x => x.Context is "Default" or "Normal")
					: null;
				var useDefaultShortcutSyntax = vsgDefaultState is { };
				var implClassName = useDefaultShortcutSyntax ? $"{bag.Name}VSG" : bag.Name;

				// write .default syntax alias
				if (useDefaultShortcutSyntax)
				{
					// public static readonly BackgroundVSG Background = new();
					buffer.AppendLine($"public static readonly {implClassName} {bag.Name} = new();");
					// alias and the class should be grouped, without empty line inbetween
				}

				// write class header
				buffer.AppendLine(options.Production
					? $"public {(!useDefaultShortcutSyntax ? "static " : "")}partial class {implClassName}"
					: $"class {implClassName} // Sortable={bag.Sortable}, IsVSG={bag.IsVSG}");
				using var _ = options.Production ? buffer.Block("{", "}") : buffer.Block();

				// write nested
				foreach (var item in children)
				{
					if (options.Production && hadSibling) buffer.AppendEmptyLine();
					if (WriteBag(item))
					{
						hadSibling = true;
					}
				}

				// write resources
				foreach (var item in resources)
				{
					var type = ResolveResourceNiceTypeName(item.Resource);
					if (bag.Name != "Styles" && type == "Style") continue; // ignore nested style unless we are under 'Styles'

					if (options.Production && hadSibling) buffer.AppendEmptyLine();

					if (options.Production) buffer
						.AppendLine($"[ResourceKeyDefinition(typeof({type}), \"{item.Resource.ResourceKey}\"{(type == "Style" && ResolveStyleTargetType(item.Resource) is { } targetType ? $", TargetType = typeof({ResolveStyleTargetType(item.Resource)})" : "")})]");
					buffer
						   .AppendLine(options.Production
							? $"public {(!useDefaultShortcutSyntax ? "static " : "")}{item.Resource.GetTypename()}Key<{type}> {item.Context} {(useDefaultShortcutSyntax ? "=" : "=>")} new(\"{item.Resource.ResourceKey}\");"
							: $"{item.Resource.GetTypename()}<{type}> {item.Context} => new(\"{item.Resource.ResourceKey}\");"
						);

					hadSibling = true;
				}

				// write .default syntax implicit operator
				if (useDefaultShortcutSyntax)
				{
					if (options.Production && hadSibling) buffer.AppendEmptyLine();

					// public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Default;
					var type = ResolveResourceNiceTypeName(vsgDefaultState?.Resource);
					buffer.AppendLine($"public static implicit operator ThemeResourceKey<{type}>({implClassName} self) => self.{vsgDefaultState?.Context};");

					hadSibling = true;
				}

				return true;
			}
			IDisposable WriteFileHeader()
			{
				if (!options.Production) return Disposable.Empty;

				foreach (var import in options.NamespaceImports)
				{
					if (!string.IsNullOrEmpty(import))
					{
						buffer.AppendLine($"using {import};");
					}
					else
					{
						buffer.AppendEmptyLine();
					}
				}
				buffer.AppendEmptyLine();

				if (options.UseFileScopedNamespace)
				{
					buffer.AppendLine($"namespace {options.Namespace};");
					buffer.AppendEmptyLine();
					return Disposable.Empty;
				}
				else
				{
					buffer.AppendLine("namespace Uno.Themes.Markup");
					return buffer.Block("{", "}");
				}
			}

			IEnumerable<NamedResourceBag> GetChildrenSorted(NamedResourceBag bag)
			{
				var results = bag.Children
					.OrderByDescending(x => x.Resources.Any() || x.Children.Any());

				if (bag.Sortable)
				{
					results = results.ThenBy(x => x.Name);
				}

				return results;
			}
			string ResolveStyleTargetType(IResourceRef rr)
			{
				return (rd.TryGetValue(rr.ResourceKey, out var resolved) ? resolved.GetResourceValue() : null) switch
				{
					Style style => ResolveXamlControlType(style.TargetType),
					_ => null,
				};

				string ResolveXamlControlType(string xamltype)
				{
					return //xamltype?.Split(':', 2)[^1];
						options.XamlControlTypeResolver?.Invoke(xamltype) ??
					 	xamltype?.Split(':', 2) switch // guestimate
						 {
							 [.., var typename] when "Popup,ToggleButton".Split(',').Contains(typename)
								 => $"global::Microsoft.UI.Xaml.Controls.Primitives.{typename}",
							 [var typename] => $"global::Microsoft.UI.Xaml.Controls.{typename}",
							 ["muxc", var typename] => $"global::Microsoft.UI.Xaml.Controls.{typename}",
							 [var xmlns, var typename] => $"{xmlns}:{typename}",

							 _ => xamltype,
						 };
				}
			}
			string ResolveResourceType(IResourceRef rr)
			{
				return (rd.TryGetValue(rr.ResourceKey, out var resolved) ? resolved.GetResourceValue() : null) switch
				{
					GenericValueObject npo => npo.Typename,
					VisualTreeElement vte => vte.Type,
					var x => x.GetType().Name,
				};
			}
			string ResolveResourceNiceTypeName(IResourceRef rr)
			{
				return ResolveResourceType(rr) switch
				{
					"Double" => "double",
					"Int32" => "int",
					"String" => "string",
					"Boolean" => "bool",

					"SolidColorBrush" => "Brush",

					var x => x,
				};
			}
		}

		public class SourceGenOptions
		{
			// parser options
			public bool TrimControlName { get; set; } = true;
			public bool TrimMaterialPrefix { get; set; } = true;
			public bool PromoteDefaultStyleResources { get; set; } = true;
			public string TargetType { get; set; } = null;
			public string[] IgnoredResourceTypes { get; set; }
			public Dictionary<string, string?> ForcedGroupings { get; set; } = new();

			// generator options
			public string[] NamespaceImports { get; set; }
			public string Namespace { get; set; }
			public bool UseFileScopedNamespace { get; set; } = false;
			public Func<string, string> XamlControlTypeResolver { get; set; }

			// options
			public bool Skip { get; set; }
			public bool Production { get; set; }
		}
		public class NamedResourceBag
		{
			public string Name { get; set; }
			public string LeftName { get; set; }
			public string RightName { get; set; }
			public HashSet<IResourceRef> Resources { set; get; } = new();
			public HashSet<(IResourceRef Resource, string Context)> ContextedResources { set; get; } = new();
			public HashSet<string> Paths { set; get; } = new();
			public List<NamedResourceBag> Children { set; get; } = new();

			public bool IsVSG { get; set; }
			public bool Sortable { get; set; } = true;

			public void Merge(NamedResourceBag other)
			{
				Resources.AddRange(other.Resources);
				ContextedResources.AddRange(other.ContextedResources);
				Paths.AddRange(other.Paths);
				Children.AddRange(other.Children);
			}
		}
	}
}

public static class Global
{
	public static readonly XNamespace Presentation = XNamespace.Get(NSPresentation);
	public static readonly XNamespace X = XNamespace.Get(NSX);

	public const string NSXmlns = "http://www.w3.org/2000/xmlns/";
	public const string NSX = "http://schemas.microsoft.com/winfx/2006/xaml";
	public const string NSPresentation = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
}
public static class ScuffedXamlParser
{
	public static readonly string[] IgnoredXmlnsPrefixes = "todo,void".Split(',');

	private static readonly ILogger _logger = typeof(ScuffedXamlParser).Log();

	public static T Load<T>(string path)
	{
		Util.Metatext($"=== parsing: {path}").Dump();
		//_logger.LogInformation($"loading: {path}");
		var document = XDocument.Load(path);
		return ScuffedXamlParser.Parse<T>(document.Root);
	}
	public static object Load(string path) => Load<object>(path);
	public static T Parse<T>(XElement e) => (T)Parse(e);
	public static object Parse(XElement e)
	{
		if (IsExplicitlyIgnored(e)) return new Ignorable(e.Name.LocalName);
		if (TryParseSimpleValue(e, out var sv)) return sv;
		if (TryParseGenericValueObject(e, out var gvo)) return gvo;
		if (VisualTreeElement.TryParse(e, out var vte)) return vte;

		return e.GetNameParts() switch
		{
			(NSPresentation, nameof(ResourceDictionary)) => ResourceDictionary.Parse(e),
			(NSPresentation, nameof(Thickness)) => ParseThickness(e.Value),
			(NSPresentation, nameof(CornerRadius)) => ParseCornerRadius(e.Value),
			(NSPresentation, nameof(Color)) => ParseColor(e.Value),
			(NSPresentation, nameof(SolidColorBrush)) => ParseSolidColorBrush(e),
			(NSPresentation, nameof(StaticResource)) => ParseStaticResource(e),
			(NSPresentation, nameof(ThemeResource)) => ParseThemeResource(e),

			// common platform-conditional offenders
			(_/*NSPresentation*/, nameof(Style)) => Style.Parse(e),
			(_/*NSPresentation*/, nameof(Setter)) => Setter.Parse(e),
			(_/*NSPresentation*/, nameof(ControlTemplate)) => ControlTemplate.Parse(e),

			(NSPresentation, nameof(VisualStateGroup)) => VisualStateGroup.Parse(e),
			(NSPresentation, nameof(VisualTransition)) => VisualTransition.Parse(e),
			(NSPresentation, nameof(VisualState)) => VisualState.Parse(e),
			(NSPresentation, nameof(Storyboard)) => Storyboard.Parse(e),

			_ when e.Name.LocalName.EndsWith("Converter") => null,
			_ => UnhandledResult(),
		};

		object UnhandledResult()
		{
#if SKIP_ALL_NOTIMPLEMENTED_OBJECT
#if !SKIP_ALL_NOTIMPLEMENTED_OBJECT_NO_LOG_WARN
			Util.Metatext($"Ignoring '{e.Name.LocalName}', as there is no parser implemented for it").Dump();
#endif
			return new NotParsedObject(e.Name.LocalName.ToString());
#else
			throw new NotImplementedException(e.Name.ToString());
#endif
		}
	}
	public static object ParseInlineValue(string value)
	{
		if (IKeyedResource.TryParseResource(value) is { } markup)
		{
			return markup;
		}

		// just returning raw text for now, since we dont have the type plugged in here
		return value;
	}

	private static bool TryParseSimpleValue(XElement e, out object result) // for primitive, struct, poco types, excluding DependencyObject
	{
		(var success, result) = e.GetNameParts() switch
		{
			(NSX, nameof(Boolean)) => (true, bool.Parse(e.Value)),
			(NSX, nameof(Int32)) => (true, int.Parse(e.Value)),
			(NSX, nameof(Double)) => (true, double.Parse(e.Value)),
			(NSX, nameof(String)) => (true, e.Value),

			_ => (false, default(object)),
		};
		return success;
	}
	private static bool TryParseGenericValueObject(XElement e, out object result) // for types that we are too lazy to define
	{
		result = e.GetNameParts() switch
		{
#if ENABLE_GENERIC_VALUE_OBJECT_PARSING
			(NSPresentation, "FontFamily") => new GenericValueObject("FontFamily", e.Value),
			(NSPresentation, "Visibility") => new GenericValueObject("Visibility", e.Value),
			(NSPresentation, "FontWeight") => new GenericValueObject("FontWeight", e.Value),
			(NSPresentation, "GridLength") => new GenericValueObject("GridLength", e.Value),
			//(NSPresentation, "ControlTemplate") => new GenericValueObject("ControlTemplate", "parser-not-implemented"),
			(_, "LottieVisualSource") => new GenericValueObject("LottieVisualSource", e.Attribute("UriSource")?.Value),
#endif

			_ => null,
		};
		return result != null;
	}

	public static Thickness ParseThickness(string raw)
	{
		try
		{
			return raw.Split(',') switch
			{
				[var uniform] when double.TryParse(uniform, out var u) => new(u, u, u, u),
				[var lr, var tb] when
					double.TryParse(lr, out var lrv) &&
					double.TryParse(tb, out var tbv)
					=> new(lrv, tbv, lrv, tbv),
				[var l, var t, var r, var b] when
					double.TryParse(l, out var lv) &&
					double.TryParse(t, out var tv) &&
					double.TryParse(r, out var rv) &&
					double.TryParse(b, out var bv)
					=> new(lv, tv, rv, bv),

				_ => throw new FormatException("Invalid corner-radius syntax"),
			};
		}
		catch (Exception ex)
		{
			throw new ArgumentException("Invalid corner-radius literal: " + raw, ex);
		}
	}
	public static CornerRadius ParseCornerRadius(string raw)
	{
		try
		{
			return raw.Split(',') switch
			{
				[var uniform] when double.TryParse(uniform, out var u) => new(u, u, u, u),
				[var tl, var tr, var br, var bl] when
					double.TryParse(tl, out var tlv) &&
					double.TryParse(tr, out var trv) &&
					double.TryParse(br, out var brv) &&
					double.TryParse(bl, out var blv)
					=> new(tlv, trv, brv, blv),

				_ => throw new FormatException("Invalid corner-radius syntax"),
			};
		}
		catch (Exception ex)
		{
			throw new ArgumentException("Invalid corner-radius literal: " + raw, ex);
		}
	}
	public static Color ParseColor(string raw)
	{
		// #rgb (need to check definition...), #rrggbb, #aarrggbb
		if (raw.StartsWith("#") && raw[1..] is { Length: /*3 or*/ 6 or 8 } hex && Regex.IsMatch(hex, "^[a-f0-9]+$", RegexOptions.IgnoreCase))
		{
			var parts = Enumerable.Range(0, hex.Length / 2).Select(x => byte.Parse(hex.Substring(x * 2, 2), NumberStyles.HexNumber)).ToArray();
			return parts.Length switch
			{
				3 => new Color(byte.MaxValue, parts[0], parts[1], parts[2]),
				4 => new Color(parts[0], parts[1], parts[2], parts[3]),

				_ => throw new ArgumentException("Invalid color literal: " + raw),
			};
		}

		uint color = raw.ToLowerInvariant() switch
		{
			"transparent" => 0x00FFFFFF,
			"aliceblue" => 0xFFF0F8FF,
			"antiquewhite" => 0xFFFAEBD7,
			"aqua" => 0xFF00FFFF,
			"aquamarine" => 0xFF7FFFD4,
			"azure" => 0xFFF0FFFF,
			"beige" => 0xFFF5F5DC,
			"bisque" => 0xFFFFE4C4,
			"black" => 0xFF000000,
			"blanchedalmond" => 0xFFFFEBCD,
			"blue" => 0xFF0000FF,
			"blueviolet" => 0xFF8A2BE2,
			"brown" => 0xFFA52A2A,
			"burlywood" => 0xFFDEB887,
			"cadetblue" => 0xFF5F9EA0,
			"chartreuse" => 0xFF7FFF00,
			"chocolate" => 0xFFD2691E,
			"coral" => 0xFFFF7F50,
			"cornflowerblue" => 0xFF6495ED,
			"cornsilk" => 0xFFFFF8DC,
			"crimson" => 0xFFDC143C,
			"cyan" => 0xFF00FFFF,
			"darkblue" => 0xFF00008B,
			"darkcyan" => 0xFF008B8B,
			"darkgoldenrod" => 0xFFB8860B,
			"darkgray" => 0xFFA9A9A9,
			"darkgreen" => 0xFF006400,
			"darkkhaki" => 0xFFBDB76B,
			"darkmagenta" => 0xFF8B008B,
			"darkolivegreen" => 0xFF556B2F,
			"darkorange" => 0xFFFF8C00,
			"darkorchid" => 0xFF9932CC,
			"darkred" => 0xFF8B0000,
			"darksalmon" => 0xFFE9967A,
			"darkseagreen" => 0xFF8FBC8F,
			"darkslateblue" => 0xFF483D8B,
			"darkslategray" => 0xFF2F4F4F,
			"darkturquoise" => 0xFF00CED1,
			"darkviolet" => 0xFF9400D3,
			"deeppink" => 0xFFFF1493,
			"deepskyblue" => 0xFF00BFFF,
			"dimgray" => 0xFF696969,
			"dodgerblue" => 0xFF1E90FF,
			"firebrick" => 0xFFB22222,
			"floralwhite" => 0xFFFFFAF0,
			"forestgreen" => 0xFF228B22,
			"fuchsia" => 0xFFFF00FF,
			"gainsboro" => 0xFFDCDCDC,
			"ghostwhite" => 0xFFF8F8FF,
			"gold" => 0xFFFFD700,
			"goldenrod" => 0xFFDAA520,
			"gray" => 0xFF808080,
			"green" => 0xFF008000,
			"greenyellow" => 0xFFADFF2F,
			"honeydew" => 0xFFF0FFF0,
			"hotpink" => 0xFFFF69B4,
			"indianred" => 0xFFCD5C5C,
			"indigo" => 0xFF4B0082,
			"ivory" => 0xFFFFFFF0,
			"khaki" => 0xFFF0E68C,
			"lavender" => 0xFFE6E6FA,
			"lavenderblush" => 0xFFFFF0F5,
			"lawngreen" => 0xFF7CFC00,
			"lemonchiffon" => 0xFFFFFACD,
			"lightblue" => 0xFFADD8E6,
			"lightcoral" => 0xFFF08080,
			"lightcyan" => 0xFFE0FFFF,
			"lightgoldenrodyellow" => 0xFFFAFAD2,
			"lightgray" => 0xFFD3D3D3,
			"lightgreen" => 0xFF90EE90,
			"lightpink" => 0xFFFFB6C1,
			"lightsalmon" => 0xFFFFA07A,
			"lightseagreen" => 0xFF20B2AA,
			"lightskyblue" => 0xFF87CEFA,
			"lightslategray" => 0xFF778899,
			"lightsteelblue" => 0xFFB0C4DE,
			"lightyellow" => 0xFFFFFFE0,
			"lime" => 0xFF00FF00,
			"limegreen" => 0xFF32CD32,
			"linen" => 0xFFFAF0E6,
			"magenta" => 0xFFFF00FF,
			"maroon" => 0xFF800000,
			"mediumaquamarine" => 0xFF66CDAA,
			"mediumblue" => 0xFF0000CD,
			"mediumorchid" => 0xFFBA55D3,
			"mediumpurple" => 0xFF9370DB,
			"mediumseagreen" => 0xFF3CB371,
			"mediumslateblue" => 0xFF7B68EE,
			"mediumspringgreen" => 0xFF00FA9A,
			"mediumturquoise" => 0xFF48D1CC,
			"mediumvioletred" => 0xFFC71585,
			"midnightblue" => 0xFF191970,
			"mintcream" => 0xFFF5FFFA,
			"mistyrose" => 0xFFFFE4E1,
			"moccasin" => 0xFFFFE4B5,
			"navajowhite" => 0xFFFFDEAD,
			"navy" => 0xFF000080,
			"oldlace" => 0xFFFDF5E6,
			"olive" => 0xFF808000,
			"olivedrab" => 0xFF6B8E23,
			"orange" => 0xFFFFA500,
			"orangered" => 0xFFFF4500,
			"orchid" => 0xFFDA70D6,
			"palegoldenrod" => 0xFFEEE8AA,
			"palegreen" => 0xFF98FB98,
			"paleturquoise" => 0xFFAFEEEE,
			"palevioletred" => 0xFFDB7093,
			"papayawhip" => 0xFFFFEFD5,
			"peachpuff" => 0xFFFFDAB9,
			"peru" => 0xFFCD853F,
			"pink" => 0xFFFFC0CB,
			"plum" => 0xFFDDA0DD,
			"powderblue" => 0xFFB0E0E6,
			"purple" => 0xFF800080,
			"red" => 0xFFFF0000,
			"rosybrown" => 0xFFBC8F8F,
			"royalblue" => 0xFF4169E1,
			"saddlebrown" => 0xFF8B4513,
			"salmon" => 0xFFFA8072,
			"sandybrown" => 0xFFF4A460,
			"seagreen" => 0xFF2E8B57,
			"seashell" => 0xFFFFF5EE,
			"sienna" => 0xFFA0522D,
			"silver" => 0xFFC0C0C0,
			"skyblue" => 0xFF87CEEB,
			"slateblue" => 0xFF6A5ACD,
			"slategray" => 0xFF708090,
			"snow" => 0xFFFFFAFA,
			"springgreen" => 0xFF00FF7F,
			"steelblue" => 0xFF4682B4,
			"tan" => 0xFFD2B48C,
			"teal" => 0xFF008080,
			"thistle" => 0xFFD8BFD8,
			"tomato" => 0xFFFF6347,
			"turquoise" => 0xFF40E0D0,
			"violet" => 0xFFEE82EE,
			"wheat" => 0xFFF5DEB3,
			"white" => 0xFFFFFFFF,
			"whitesmoke" => 0xFFF5F5F5,
			"yellow" => 0xFFFFFF00,
			"yellowgreen" => 0xFF9ACD32,

			_ => throw new ArgumentException("Invalid color literal: " + raw),
		};
		if (BitConverter.GetBytes(color) is [var a, var r, var g, var b])
		{
			return new Color(a, r, g, b);
		}
		else
		{
			// how?
			throw new InvalidOperationException();
		}
	}
	public static SolidColorBrush ParseSolidColorBrush(XElement e)
	{
		var result = new SolidColorBrush();
		if (result.MapDP(e, x => x.Color, out var color)) result = result with { Color = ParseColor(color) };
		if (result.MapDP(e, x => x.Opacity, out var opacity)) result = result with { Opacity = double.Parse(opacity) };

		return result;
	}
	private static object ParseStaticResource(XElement e)
	{
		return new StaticResourceRef(e.Attribute("ResourceKey")?.Value);
	}
	private static object ParseThemeResource(XElement e)
	{
		return new ThemeResourceRef(e.Attribute("ResourceKey")?.Value);
	}

	// we dont actually want to just take Ignorable as is (due to uno specific xmlns), so we will use a custom list here
	// we dont want to introduce the notion of "platform" yet...
	public static bool IsExplicitlyIgnored(XElement e)
	{
		var prefix = e.GetPrefixOfNamespace(e.Name.NamespaceName);
		return IgnoredXmlnsPrefixes.Contains(prefix);
	}
	public static bool IsExplicitlyIgnored(XAttribute attribute)
	{
		var prefix = attribute.Parent.GetPrefixOfNamespace(attribute.Name.NamespaceName);
		return IgnoredXmlnsPrefixes.Contains(prefix);
	}

	private static bool MapDP<T, TProperty>(this T obj, XElement e, Expression<Func<T, TProperty>> memberSelector, out string value) where T : DependencyObject
	{
		var property = memberSelector.Body switch
		{
			MemberExpression m => m.Member.Name,

			_ => throw new ArgumentOutOfRangeException(memberSelector.Body.Type.ToString()),
		};
		if (e.Attribute(property) is { } attribute)
		{
			if (IKeyedResource.TryParseResource(attribute.Value) is { } resRef)
			{
				obj.SetDP(property, resRef);
			}
			else
			{
				value = attribute.Value;
				return true;
			}
		}

		value = default;
		return false;
	}
}
public static class XamlObjectHelper
{
	public static bool IsCollectionType(XElement e)
	{
		var (xmlns, name) = e.GetNameParts();
		return (xmlns, name) switch
		{
			(_, "VisualStateManager.VisualStateGroups") => true,
			(_, "Grid.RowDefinitions" or "Grid.ColumnDefinitions") => true,
			(_, _) when name.EndsWith(".PrimaryCommands") => true,
			(_, _) when name.EndsWith(".Resources") => true,

			_ => false,
		};
	}
}
public static class ValueSimplifier
{
	public static string SimplifyMarkup(string value)
	{
		if (string.IsNullOrEmpty(value)) return value;
		if (!ShouldSimplify(value)) return value;

		if (Regex.Match(value, @"^{(?<type>Static|Theme)Resource (?<key>\w+)}$") is { Success: true } resourceMarkup)
		{
			return
				resourceMarkup.Groups["type"].Value[0] + // prefix with S/T, so we can still trace its original definition
				resourceMarkup.Result("[${key}]");
		}
		// fixme: naive parser
		if (Regex.Match(value, "{Binding(?<vargs>.+)?}") is { Success: true } bindingMarkup)
		{
			return bindingMarkup.Groups["vargs"].Value.Split(',')
				.Select(x => x.Trim().Split('=', 2))
				.ToDictionary(x => x.Length > 1 ? x[0] : "Path", x => x.Last())
				.TryGetValue("Path", out var path)
					? $"{{{path}}}" : "{this}";
		}

		return value;
	}
	public static string SimplifyTimeSpan(string value)
	{
		if (!ShouldSimplify(value)) return value;

		return Regex.Replace(value, "^(00?:)+", "");
	}

	private static bool ShouldSimplify(string value) => true;
}

public record Ignorable(string Typename); // mc:Ignorable
public record NotParsedObject(string Typename);
public record GenericValueObject(string Typename, string Value);
public record DependencyObject
{
	private EquitableDictionary<string, object> _properties = new();

	public object GetDP(string dp) => _properties.TryGetValue(dp, out var value) ? value : default;
	public void SetDP(string dp, object value) => _properties[dp] = value;
}

public record Thickness(double Left, double Top, double Right, double Bottom)
{
	public override string ToString()
	{
		// format: uniform, [same-left-right,same-top-bottom], [left,top,right,bottom]
		if (Left == Top && Top == Right && Right == Bottom) return $"{Left:0.#}";
		if (Left == Right && Top == Bottom) return $"{Left:0.#},{Top:0.#}";
		return $"{Left:0.#},{Top:0.#},{Right:0.#},{Bottom:0.#}";
	}
}
public record CornerRadius(double TopLeft, double TopRight, double BottomRight, double BottomLeft)
{
	public override string ToString()
	{
		// format: uniform, [left,top,right,bottom]
		if (TopLeft == TopRight && TopRight == BottomRight && BottomRight == BottomLeft) return $"{TopLeft:0.#}";
		return $"{TopLeft:0.#},{TopRight:0.#},{BottomRight:0.#},{BottomLeft:0.#}";
	}
}
public record Color(byte A, byte R, byte G, byte B)
{
	public override string ToString() => "#" + this.ToRgbText();
}
public record SolidColorBrush(Color Color = default, double Opacity = 1) : DependencyObject
{
	public override string ToString()
	{
		var color = GetDP(nameof(Color)) switch
		{
			IResourceRef rf => rf.ResourceKey,
			null => Color.ToString(),
			_ => throw new ArgumentOutOfRangeException(),
		};
		var opacity = GetDP(nameof(Opacity)) switch
		{
			IResourceRef rf => $"*{rf.ResourceKey}",
			null when Opacity != 1 => $"*{Opacity}",
			null => "",
			_ => throw new ArgumentOutOfRangeException(),
		};

		return $"{color}{opacity}";
	}
}

public record Style(string TargetType = null, string BasedOn = null)
{
	public List<Setter> Setters = new();

	private object ToDump() => new { TargetType, BasedOn, Setters };

	public static Style Parse(XElement e)
	{
		var result = new Style(
			e.Attribute("TargetType")?.Value,
			IKeyedResource.GetKeyFromMarkup(e.Attribute("BasedOn")?.Value)
		);
		ParseSetters(e.Elements(), wrapped: false);

		return result;

		void ParseSetters(IEnumerable<XElement> children, bool wrapped)
		{
			foreach (var child in children)
			{
				if (child.Name.IsMemberOf<Style>(nameof(Setters)) && !wrapped)
				{
					ParseSetters(child.Elements(), wrapped: true);
					continue;
				}

				var parsed = ScuffedXamlParser.Parse(child);
				if (parsed is Ignorable) continue;
				if (parsed is Setter setter)
				{
					result.Setters.Add(setter);
				}
				else
				{
					throw new NotImplementedException($"{(wrapped ? "Style > Style.Setters" : "Style > ")}{child.Name.Pretty()}").PreDump(child);
				}
			}
		}
	}

	public VisualTreeElement GetTemplateRoot()
	{
		return Setters
			.FirstOrDefault(x => x.Property == "Template")
			?.Value.As<ControlTemplate>()
			?.TemplateRoot;
	}
}
public record Setter(string Target, string Property, object Value)
{
	public static Setter Parse(XElement e)
	{
		return new Setter
		(
			Property: e.Attribute("Property")?.Value,
			Target: e.Attribute("Target")?.Value,
			Value: GetDirectOrNestedValue()
		);

		object GetDirectOrNestedValue()
		{
			if (e.Element(Presentation + "Setter.Value") is { } valueMember && valueMember.HasElements)
			{
				return ScuffedXamlParser.Parse(valueMember.Elements().Single());
			}
			if (e.Attribute("Value") is { Value: { Length: > 0 } value })
			{
				return ScuffedXamlParser.ParseInlineValue(value);
			}

			return null;
		}
	}
}

public record ControlTemplate(string TargetType = null)
{
	public VisualTreeElement TemplateRoot { get; private set; }

	public static ControlTemplate Parse(XElement e)
	{
		var result = new ControlTemplate(e.Attribute("TargetType")?.Value);
		var elements = e.Elements().ToArray();
		result.TemplateRoot = elements.Length switch
		{
			0 => null,
			1 => ScuffedXamlParser.Parse(elements[0]) is { } parsed
				? parsed is VisualTreeElement vte
					? vte : throw new InvalidOperationException($"ControlTemplate can only accept a child of VisualTreeElement: parsed type is {parsed?.GetType().Name ?? "<null>"}").PreDump(parsed)
				: throw new InvalidOperationException(),
			_ => throw new InvalidOperationException($"ControlTemplate > multiple children are present").PreDump(e),
		};

		return result;
	}
}
public record VisualTreeElement(string Type)
{
	private static readonly string[] WhiteListedTypes = @"
		// winui
		Border, Grid, StackPanel, StackLayout, PivotPanel, WrapPanel, GridView, 
		ScrollViewer, ScrollContentPresenter, ScrollBar
		ItemsControl, ItemsPresenter, ItemsRepeater, ItemsRepeaterScrollHost, ListView, Pivot, ListViewItemPresenter,
		ItemsStackPanel, PivotHeaderPanel, CalendarPanel,
		Control, ContentControl, ContentPresenter, ScrollViewer, Button, RepeatButton, TextBox, TextBlock, Slider, RadioButton, ToggleButton, ToggleSwitch, ProgressBar, NumberBox
		CheckBox, ComboBox, ComboBoxItem, 
		CalendarView, MediaPlayerPresenter, 
		NavigationView, NavigationViewList, NavigationViewItemPresenter,
		AppBar, AppBarButton, CommandBar, CommandBarOverflowPresenter,
		Path, Rectangle, Ellipse, Polyline, Canvas, Image,
		PathIcon, FontIcon, BitmapIcon, SymbolIcon
		TickBar, Thumb,
		ColorSpectrum, ColorPickerSlider,
		MonochromaticOverlayPresenter,
		AnimatedIcon,
		AnimatedVisual,AnimatedAcceptVisualSource,AnimatedChevronDownSmallVisualSource,AnimatedChevronUpDownSmallVisualSource,AnimatedBackVisualSource,AnimatedGlobalNavigationButtonVisualSource
		InfoBarPanel
		TabViewListView, TreeViewList
		CarouselPanel,
		DataTemplate, ItemsPanelTemplate
		
		// winui\DependencyObjects... just lumping them here for now
		RowDefinition, ColumnDefinition

		// winui\etc containers/wrappers
		Flyout, Popup,
		SplitView, NavigationView, NavigationViewItem, NavigationViewItemSeparator,
		Viewbox, ElevatedView,
		
		// uno
		NativeCommandBarPresenter
		
		// themes, toolkit
		Ripple,
		Chip, Drawer, ShadowContainer, 
		NativeNavigationBarPresenter, NavigationBarPresenter,
		TabBar, TabBarSelectionIndicatorPresenter, TabBarListPanel, 
	".Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
			.Where(x => !x.StartsWith("//"))
			.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
			.ToArray();

	public string Name { get; private set; }
	public Dictionary<string, object> Properties { get; private set; } = new();
	public Dictionary<string, object> AttachedProperties { get; private set; } = new();
	public List<VisualTreeElement> Children { get; private set; } = new();

	public static bool TryParse(XElement e, out object result) // generic parser for all visual tree elements or DependencyObjects
	{
		if (WhiteListedTypes.Contains(e.Name.LocalName))
		{
			result = Parse(e);
			return true;
		}

		result = default;
		return false;
	}
	public static VisualTreeElement Parse(XElement e)
	{
		var result = new VisualTreeElement(e.Name.LocalName);
		foreach (var attribute in e.Attributes())
		{
			ParseAttributeMember(attribute, result);
		}
		foreach (var child in e.Elements())
		{
			ParseChildMember(child, result);
		}

		return result;
	}
	public static void ParseAttributeMember(XAttribute attribute, VisualTreeElement target)
	{
		if (ScuffedXamlParser.IsExplicitlyIgnored(attribute)) return;

		var (xmlns, name) = attribute.GetNameParts();
		if (xmlns == string.Empty)
		{
			xmlns = attribute.Parent.GetDefaultNamespace().NamespaceName;
		}

		switch ((xmlns, name))
		{
			case (NSXmlns, _): // inline xmlns definition
				return;

			case (NSX, "Name"): target.Name = attribute.Value; return;

			case (NSPresentation, _):
			case (NSX, _):
			case ("using:Microsoft.UI.Xaml.Controls", _):
			case ("using:Windows.UI.Xaml.Controls.Primitives", _):
			case ("using:Microsoft.UI.Xaml.Controls.Primitives", _):
				target.Properties[name] = ScuffedXamlParser.ParseInlineValue(attribute.Value); return;

			case (_, _) when name.Contains('.'):
				target.AttachedProperties[name] = ScuffedXamlParser.ParseInlineValue(attribute.Value); return;

			default: //target.Properties.Add(name, attribute.Value); return;
				attribute.Dump((xmlns, name).ToString());
				throw new NotImplementedException();
		}
	}
	public static void ParseChildMember(XElement child, VisualTreeElement target)
	{
		//if (ScuffedXamlParser.IsExplicitlyIgnored(child)) return;

		var (xmlns, name) = child.GetNameParts();
		if (xmlns == string.Empty)
		{
			xmlns = child.GetDefaultNamespace().NamespaceName;
		}

		/* we have 3 cases here: // for child element only (not applicable to attribute)
			1. member property whose local name is $"{Parent.ClassName or its upperclass name}.{PropertyName}"
			2. attached property whose local name is $"{DeclaringType}.{PropertyName}"
			3. any remaining should be direct content
		*/

		var parts = name.Split('.', 2);
		var className = parts[0];
		var memberName = parts.ElementAtOrDefault(1);
		if (memberName != null && target.IsMatchingClass(className)) // case 1
		{
			if (ParseValue() is { } value /*&& value is not IScriptIgnorable*/)
			{
				target.Properties[memberName] = ParseValue();
			}
		}
		else if (memberName != null) // case 2
		{
			if (ParseValue() is { } value /*&& value is not IScriptIgnorable*/)
			{
				target.AttachedProperties[name] = ParseValue();
			}
		}
#if PARSE_VISUALELEMENT_CHILD
		else // case 3
		{
			var parsed = ScuffedXamlParser.Parse(child);
			if (parsed is Ignorable) return;
			if (parsed is VisualTreeElement vte)
			{
				target.Children.Add(vte);
			}
			else
			{
				child.Dump((xmlns, name).ToString());
				throw new NotImplementedException();
			}
		}
#endif

		object ParseValue()
		{
			var elements = child.Elements().ToArray();
			if (XamlObjectHelper.IsCollectionType(child))
			{
				return elements.Select(ScuffedXamlParser.Parse)/*.Where(x => x is not IScriptIgnorable)*/.ToArray();
			}
			else
			{
				if (elements.Length == 0) return null;
				else if (elements.Length == 1) return ScuffedXamlParser.Parse(elements[0]);
				throw new Exception($"{child.Name} is not a collection type, but nests more than one element.");
			}
		}
	}

	public IEnumerable<VisualStateGroup> GetVisualStateGroups()
	{
		if (AttachedProperties.TryGetValue("VisualStateManager.VisualStateGroups", out var boxedVsgs) &&
			boxedVsgs.As<object[]>().Cast<VisualStateGroup>() is { } vsgs)
		{
			return vsgs;
		}
		else
		{
			return null;
		}
	}
	public IEnumerable<(string Path, VisualTreeElement Element)> Flatten()
	{
		return YieldNodeWalk(this, "");

		IEnumerable<(string Path, VisualTreeElement Element)> YieldNodeWalk(VisualTreeElement element, string path)
		{
			path += '/' + Describe(element);

			yield return (path, element);
			foreach (var property in element.Properties)
			{
				// control-template
				if (property.Key == "Template" &&
					property.Value is ControlTemplate { TemplateRoot: { } templateRoot })
				{
					foreach (var item in YieldNodeWalk(templateRoot, (path + "/.Template")))
					{
						yield return (item.Path, item.Element);
					}
				}

				// todo: item-template
			}
			foreach (var child in element.Children)
			{
				// nested children
				foreach (var item in YieldNodeWalk(child, path))
				{
					yield return (item.Path, item.Element);
				}
			}
		}
		static string Describe(object node)
		{
			return node switch
			{
				VisualTreeElement vte => string.IsNullOrEmpty(vte.Name) ? vte.Type : $"#{vte.Name}",

				_ => throw new NotImplementedException().PreDump(node),
			};
		}
	}
	private bool IsMatchingClass(string className)
	{
		// todo: check through class hierarchy too
		return Type == className;
	}
}

public record VisualStateGroup(string Name)
{
	public List<VisualState> VisualStates { get; } = new();
	public List<VisualTransition> Transitions { get; } = new();

	public static VisualStateGroup Parse(XElement e)
	{
		var result = new VisualStateGroup(e.Attribute(X + "Name")?.Value);
		foreach (var child in e.Elements())
		{
			if (child.Name.Is<VisualState>())
			{
				result.VisualStates.Add(VisualState.Parse(child));
			}
			else if (child.Name.IsMemberOf<VisualStateGroup>(nameof(Transitions)))
			{
				result.Transitions.AddRange(ParseTransitions(child));
			}
			else
			{
				throw new NotImplementedException($"Unexpected {nameof(VisualStateGroup)} child element: {child.Name}").PreDump(child);
			}
		}

		return result;
	}
	public static IEnumerable<VisualTransition> ParseTransitions(XElement transitions)
	{
		foreach (var child in transitions.Elements())
		{
			var item = ScuffedXamlParser.Parse(child);
			if (item is VisualTransition transition)
			{
				yield return transition;
			}
			else
			{
				throw new NotImplementedException($"Unexpected {transitions.Name.LocalName} child element: {child.Name}").PreDump(child);
			}
		}
	}
}
public record VisualState(string Name)
{
	public Storyboard Storyboard { get; private set; }
	public List<Setter> Setters { get; private set; } = new();

	public static VisualState Parse(XElement e)
	{
		var result = new VisualState(e.Attribute(X + "Name")?.Value);

		foreach (var child in e.Elements())
		{
			if (child.Name.IsMemberOf<VisualState>(out var memberName))
			{
				if (memberName == "Setters")
				{
					result.Setters.AddRange(ParseSetters(child));
				}
			}
			else if (child.Name.Is("Storyboard"))
			{
				result.Storyboard = Storyboard.Parse(child);
			}
			else
			{
				throw new NotImplementedException($"Unexpected {nameof(VisualState)} child element: {child.Name}").PreDump(child);
			}
		}

		return result;
	}
	public static IEnumerable<Setter> ParseSetters(XElement setters)
	{
		foreach (var child in setters.Elements())
		{
			var item = ScuffedXamlParser.Parse(child);
			if (item is Ignorable) continue;
			if (item is Setter setter)
			{
				yield return setter;
			}
			else
			{
				throw new NotImplementedException($"Unexpected {setters.Name.LocalName} child element: {child.Name}").PreDump(child);
			}
		}
	}
}
public record VisualTransition(string Name, string From, string To)
{
	public Storyboard Storyboard { get; private set; }

	public static VisualTransition Parse(XElement e)
	{
		var result = new VisualTransition(e.Attribute(X + "Name")?.Value, e.Attribute("From")?.Value, e.Attribute("To")?.Value);

		foreach (var child in e.Elements())
		{
			if (child.Name.Is("Storyboard"))
			{
				result.Storyboard = Storyboard.Parse(child);
			}
			else
			{
				throw new NotImplementedException($"Unexpected {nameof(VisualTransition)} child element: {child.Name}").PreDump(child);
			}
		}

		return result;
	}
}
public record Storyboard // doesnt make sense to inherit from Timeline here (while it was the case in uwp)
{
	public List<Timeline> Children { get; } = new();

	public static Storyboard Parse(XElement e)
	{
		var result = new Storyboard();
		foreach (var child in e.Elements())
		{
			if (Timeline.Parse(child) is { } timeline)
			{
				result.Children.Add(timeline);
			}
			else if (child.Name.Is<Storyboard>()) // hack: quick workaround for doubly-nested Storyboard (eg: Uno\Fluent\ProgressBar...)
			{
				return Parse(child);
			}
			else
			{
				throw new NotImplementedException($"Storyboard > {child.Name.Pretty()}").PreDump(child);
			}
		}
		return result;
	}

	public object ToDump() => Children;
}
public record Timeline(string Type)
{
	public string TargetName { get; private set; }
	public string TargetProperty { get; private set; }
	public string Value { get; private set; }
	//public string ValueTimeline { get; private set; }

	public static Timeline Parse(XElement e)
	{
		return e.GetNameParts() switch
		{
			(_, "ColorAnimation") => ParseSimpleAnimation(),
			(_, "ColorAnimationUsingKeyFrames") => ParseKeyFrames(),
			(_, "DoubleAnimation") => ParseSimpleAnimation(),
			(_, "DoubleAnimationUsingKeyFrames") => ParseKeyFrames(),
			(_, "DragItemThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "DragOverThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "DrillInThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "DrillOutThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "DropTargetItemThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "FadeInThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "FadeOutThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "ObjectAnimationUsingKeyFrames") => ParseKeyFrames(),
			(_, "PointAnimation") => ParseSimpleAnimation(),
			(_, "PointAnimationUsingKeyFrames") => ParseKeyFrames(),
			(_, "PointerDownThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "PointerUpThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "PopInThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "PopOutThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "RepositionThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "SplitCloseThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "SplitOpenThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "SwipeBackThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "SwipeHintThemeAnimation") => ParsePreconfiguredAnimation(),

			_ => null,
		};

		Timeline ParseCommon()
		{
			var result = new Timeline(e.Name.LocalName);
			result.TargetName = e.Attribute("Storyboard.TargetName")?.Value;
			result.TargetProperty = e.Attribute("Storyboard.TargetProperty")?.Value;

			return result;
		}
		Timeline ParseKeyFrames()
		{
			var result = ParseCommon();
			result.Value = string.Join("\n", e.Elements().Select(ParseKeyFrame));

			return result;
		}
		string ParseKeyFrame(XElement frame)
		{
			string Value(string key = "Value") => SimplifyMarkup(frame.Attribute(key)?.Value ?? frame.GetMember("Value")?.Value);
			string KeyTime(string key = "KeyTime") => SimplifyMarkup(SimplifyTimeSpan(frame.Attribute(key)?.Value));
			string Raw(string key) => frame.Attribute(key)?.Value;

			return frame.Name.LocalName switch
			{
				"DiscreteObjectKeyFrame" => $"{Value()} @{KeyTime()}",

				// DoubleKeyFrame
				"DiscreteDoubleKeyFrame" => $"{Value()} @{KeyTime()}",
				"EasingDoubleKeyFrame" => $"{Value()} @{KeyTime()} f={frame.Attribute("EasingFunction")}",
				"LinearDoubleKeyFrame" => $"{Value()} @{KeyTime()} f=Linear",
				"SplineDoubleKeyFrame" => $"{Value()} @{KeyTime()} f=Spline",

				"LinearColorKeyFrame" => $"{Value()} @{KeyTime()} f=Linear",

				_ => throw new NotImplementedException($"{e.Name.LocalName} > {frame.Name.Pretty()}").PreDump(frame),
			};
		}
		Timeline ParseSimpleAnimation()
		{
			var result = ParseCommon();
			result.Value = e.Name.LocalName switch
			{
				"DoubleAnimation" => FormatAnimation(),
				"ColorAnimation" => FormatAnimation(),

				_ => throw new NotImplementedException(e.Name.Pretty()).PreDump(e),
			};

			return result;

			string FormatAnimation()
			{
				var from = SimplifyMarkup(e.Attribute("From")?.Value);
				var by = SimplifyMarkup(e.Attribute("By")?.Value);
				var to = SimplifyMarkup(e.Attribute("To")?.Value);
				var duration = SimplifyMarkup(SimplifyTimeSpan(e.Attribute("Duration")?.Value));

				return (from, by, to) switch
				{
					(_, null, null) => $"{from}->$base in {duration}",
					(null, _, null) => $"$current->{by} in {duration}",
					(null, null, _) => $"$current->{to} in {duration}",
					(_, null, _) => $"{from}->{to} in {duration}",
					(_, _, null) => $"{from}->{by} in {duration}",

					// -- https://learn.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.animation.doubleanimation?view=winrt-22621#remarks
					_ => throw new InvalidOperationException($"A DoubleAnimation typically has at least one of the From, By or To properties set, but never all three: from={from}, by={by}, to={to}"),
				};
			}
		}
		Timeline ParsePreconfiguredAnimation()
		{
			var result = ParseCommon();
			result.TargetName ??= e.Attribute("TargetName")?.Value;
			result.Value = result.Type;

			return result;
		}
	}
}

// Static/ThemeResource indicates the resources was stored whether under RD or under RD.ThemeDictionaries
// Static/ThemeResourceRef indicates the type of resource alias(<StaticResource ResourceKey="..." />) or markup({StaticResource ...})
public record ResourceKey(string Key, string TargetType = null) : IComparable
{
	public static implicit operator ResourceKey(string key) => new(key);

	private object ToDump() => ToString();
	public override string ToString() => Key?.ToString() ?? $"TargetType={TargetType}";
	public static explicit operator string(ResourceKey x) => x.ToString();

	public int CompareTo(object obj)
	{
		if (obj is ResourceKey other)
		{
			int? Compare(string a, string b) => (a, b) switch
			{
				(null, null) => null,
				(_, null) => 1,
				(null, _) => -1,
				(_, _) => a.CompareTo(b),
			};

			return Compare(TargetType, other.TargetType) ?? Compare(Key, other.Key) ?? 0;
		}

		return 1;
	}
}
public interface IKeyedResource
{
	ResourceKey Key { get; }

	public static string GetKeyFromMarkup(string markup)
	{
		if (markup is null) return null;

		return TryGetKeyFromMarkup(markup, out var key)
			? key
			: throw new ArgumentException("Invalid resource markup: " + markup);
	}
	public static bool TryGetKeyFromMarkup(string maybeMarkup, out string key)
	{
		if (maybeMarkup is not null && Regex.Match(maybeMarkup, @"^{(?<type>(Static|Theme|Dynamic)Resource) (?<key>\w+)}$") is { Success: true } match)
		{
			key = match.Groups["key"].Value;
			return true;
		}
		else
		{
			key = default;
			return false;
		}
	}
	public static IResourceRef TryParseResource(string maybeMarkup)
	{
		if (maybeMarkup is not null && Regex.Match(maybeMarkup, @"^{(?<type>(Static|Theme|Dynamic)Resource) (?<key>\w+)}$") is { Success: true, Groups: var g })
		{
			return g["type"].Value switch
			{
				"StaticResource" => new StaticResourceRef(g["key"].Value),
				"ThemeResource" => new ThemeResourceRef(g["key"].Value),
				"DynamicResource" => throw new NotImplementedException("DynamicResource"),

				_ => throw new ArgumentOutOfRangeException($"Invalid resource markup: {g["type"].Value}"),
			};
		}

		return default;
	}

	public object GetResourceValue()
	{
		// favoring Light value when resolving for simplicity
		return this switch
		{
			StaticResource sr => sr.Value,
			ThemeResource tr => tr.LightValue,

			_ => throw new ArgumentOutOfRangeException(),
		};
	}
}
public interface IResourceRef : IComparable
{
	string ResourceKey { get; }

	int IComparable.CompareTo(object obj)
	{
		if (obj is IResourceRef rr)
		{
			return ResourceKey.CompareTo(rr.ResourceKey);
		}

		return 1;
	}
	public string GetTypename() => GetType().Name.RemoveTail("Ref");
}
public record StaticResource(ResourceKey Key, object Value) : IKeyedResource;
public record ThemeResource(ResourceKey Key, object LightValue, object DarkValue) : IKeyedResource // naive impl only, it should be a dict<TKey, object>
{
	public bool AreThemeDefinitionEqual()
	{
		return LightValue?.Equals(DarkValue) == true;
	}
}
public record StaticResourceRef(string ResourceKey) : IResourceRef
{
#if DUMP_RESOURCEREF_WITH_KEY_INLINE
	private object ToDump() => Util.OnDemand($"{{StaticResource {ResourceKey}}}", () => this);
#endif
}
public record ThemeResourceRef(string ResourceKey) : IResourceRef
{
#if DUMP_RESOURCEREF_WITH_KEY_INLINE
	private object ToDump() => Util.OnDemand($"{{ThemeResource {ResourceKey}}}", () => this);
#endif
}
public class ResourceDictionary : Dictionary<ResourceKey, IKeyedResource>
{
	public static Dictionary<string, string> ThemeMapping = new()
	{
		["Light"] = "Light",
		["Dark"] = "Dark",
	};

	// fixme: do not merge themed-values into values, instead keep them inside ThemeDictionaries, and concat them when looking for expansion (debug enumerating, DO NOT concat into to enumerable)
	// also remove Merge() and use MergedDictionaries
	// that way we can preserve the windows hierarchy

	public ResourceDictionary() { }
	public ResourceDictionary(ResourceDictionary rd) : base(rd) { }

	public void Add(IKeyedResource resource)
	{
		if (!TryAdd(resource.Key, resource))
		{
#if ALLOW_DUPLICATED_KEYS
#if !ALLOW_DUPLICATED_KEYS_WITHOUT_WARNING
			Util.WithStyle($"Duplicated key: {resource.Key}", "color: orange").Dump();
#endif
#else
			throw new ArgumentException($"Duplicated key: {resource.Key}");
#endif
		}
	}
	public void AddRange(IEnumerable<IKeyedResource> resources)
	{
		foreach (var resource in resources)
			Add(resource);
	}
	public ResourceDictionary Merge(ResourceDictionary other)
	{
		AddRange(other.Values);
		return this;
	}

	public object ToDump() => Values;

	public IKeyedResource this[string Key = null, string TargetType = null]
	{
		get => this.TryGetValue(new(Key, TargetType), out var value) ? value : default;
	}

	public static string GetKey(XElement element)
	{
		// https://docs.microsoft.com/en-us/windows/apps/design/style/xaml-resource-dictionary
		// x:Name can be used instead of x:Key. However, [...less optimal].
		return
			element.Attribute(X + "Key")?.Value ??
			element.Attribute(X + "Name")?.Value;
	}
	private static ResourceKey GetResourceKey(XElement element)
	{
		if (GetKey(element) is string key)
		{
			return key;
		}
		if (element.Attribute("TargetType")?.Value is { } targetType)
		{
			return new(null, targetType);
		}

		throw new KeyNotFoundException().PreDump(element);
	}

	public static ResourceDictionary Parse(XElement e)
	{
		var result = new ResourceDictionary();
		foreach (var child in e.Elements())
		{
			if (child.Name.IsMemberOf<ResourceDictionary>(out var property))
			{
				_ = property switch
				{
					"ThemeDictionaries" => ParseThemeDictionaries(result, child),
					"MergedDictionaries" => ParseMergedDictionaries(result, child),

					_ => throw new NotImplementedException(property),
				};
			}
			else
			{
				result.Add(new StaticResource(ResourceDictionary.GetResourceKey(child), ScuffedXamlParser.Parse(child)));
			}
		}

		return result;
	}
	protected static ResourceDictionary[] ParseThemeDictionaries(ResourceDictionary rd, XElement e)
	{
		var themes = Parse(e);

		var light = (themes.FirstOrDefault(x => ThemeMapping["Light"].Split(',').Contains(x.Key.Key)).Value as StaticResource)?.Value as ResourceDictionary;
		var dark = (themes.FirstOrDefault(x => ThemeMapping["Dark"].Split(',').Contains(x.Key.Key)).Value as StaticResource)?.Value as ResourceDictionary;

		object GetResourceUnwrapped(ResourceDictionary rd, ResourceKey key)
			//=> ((StaticResource)rd[key]).Value;
			=> rd?.TryGetValue(key, out var value) == true && value is StaticResource sr ? sr.Value : default;
		foreach (var key in Enumerable.Union(light?.Keys.Safe(), dark?.Keys.Safe()))
			rd.Add(new ThemeResource(key, GetResourceUnwrapped(light, key), GetResourceUnwrapped(dark, key)));

		return new[] { light, dark };
	}
	protected static ResourceDictionary[] ParseMergedDictionaries(ResourceDictionary rd, XElement e)
	{
		var buffer = new List<ResourceDictionary>();
		foreach (var item in e.Elements())
		{
			var nested = Parse(item);
			rd.Merge(nested);
		}

		return buffer.ToArray();
	}

	/// <summary>Resolve or unwrap static/theme-resources with the provided context.</summary>
	internal void ResolveWith(ResourceDictionary context, int maxDepth = 100)
	{
		context = new ResourceDictionary(context).Merge(this);

		foreach (var key in Keys.ToArray())
		{
			var value = this[key];
			for (int i = 0; i < maxDepth; i++)
			{
				var referenced = value.GetResourceValue();
				if (referenced is IResourceRef rr && context.TryGetValue(rr.ResourceKey, out var result))
				{
					value = result;
				}
				else break;
			}

			this[key] = value;
		}
	}
}

public class EquitableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
	public override int GetHashCode()
	{
		var hash = new HashCode();
		foreach (var pair in this)
		{
			hash.Add(pair);
		}

		return hash.ToHashCode();
	}
	public override bool Equals(object obj)
	{
		if (obj == null) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj is EquitableDictionary<TKey, TValue> other)
		{
			return
				this.Count == other.Count &&
				this.GetHashCode() == other.GetHashCode();
		}

		return false;
	}
}
public class SequentialEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
{
	public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
	{
		return GetHashCode(x) == GetHashCode(y);
	}

	public int GetHashCode(IEnumerable<T> obj)
	{
		if (obj is null) return 0;

		// note: Enumerable.Aggregate(new HashCode(), ...) is passed around by value...
		var hash = new HashCode();
		foreach (var item in obj.Order())
		{
			hash.Add(item);
		}

		return hash.ToHashCode();
	}
}
public class IndentedTextBuilder
{
	public bool BlockConsecutiveEmptyLines { get; set; }
	public int IndentLevel { get; set; }

	private readonly string padding;
	private readonly StringBuilder builder = new();
	private bool wroteEmptyLinePreviously = false;

	public IndentedTextBuilder(string padding) => this.padding = padding;

	private IndentedTextBuilder AppendJoinLine(string separator, string[] items)
	{
		builder
			.Append(GetCurrentIndentation())
			.AppendJoin(separator, items.Where(x => x != null))
			.AppendLine();
		return this;
	}
	public IndentedTextBuilder AppendLine(string line)
	{
		//Util.WithStyle(GetCurrentIndentation() + line, "font-family: 'Lucida Console', Monaco, monospace").Dump();
		builder
			.Append(GetCurrentIndentation())
			.AppendLine(line);

		wroteEmptyLinePreviously = false;
		return this;
	}
	public IndentedTextBuilder AppendEmptyLine()
	{
		if (!BlockConsecutiveEmptyLines || !wroteEmptyLinePreviously)
		{
			//Console.WriteLine("empty-line");
			builder.AppendLine();

			wroteEmptyLinePreviously = true;
		}
		return this;
	}

	private string GetCurrentIndentation() => string.Concat(Enumerable.Repeat(padding, IndentLevel));
	public IDisposable Block()
	{
		IndentLevel++;
		return Disposable.Create(() => IndentLevel--);
	}
	public IDisposable Block(string opening, string closing)
	{
		AppendLine(opening);
		IndentLevel++;

		return Disposable.Create(() =>
		{
			IndentLevel--;
			AppendLine(closing);
		});
	}
	public IndentedTextBuilder Indent()
	{
		IndentLevel++;
		return this;
	}
	public IndentedTextBuilder Unindent()
	{
		IndentLevel--;
		return this;
	}

	public override string ToString() => builder.ToString();
}
public class Disposable : IDisposable
{
	public static IDisposable Empty => Create(null);
	public static IDisposable Create(Action dispose) => new Disposable(dispose);

	private readonly Action dispose;
	private Disposable(Action dispose) => this.dispose = dispose;

	public void Dispose() => dispose?.Invoke();
}

public static class EnumerableExtensions
{
	public static IEnumerable<T> Safe<T>(this IEnumerable<T> source) => source is not null ? source : Array.Empty<T>();
	public static T JustOneOrDefault<T>(this IEnumerable<T> source) where T : class
	{
		T first = null;
		foreach (var item in source)
		{
			if (first != null) return null;
			first = item;
		}

		return first;
	}
	public static T? JustOneOrNull<T>(this IEnumerable<T> source) where T : struct
	{
		T? first = null;
		foreach (var item in source)
		{
			if (first != null) return null;
			first = item;
		}

		return first;
	}
	public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
	{
		foreach (var item in source)
		{
			action(item);
		}
	}
	public static string JoinBy<T>(this IEnumerable<T> source, string separator)
	{
		return string.Join(separator, source);
	}
	public static IEnumerable<T> MustAll<T>(this IEnumerable<T> source, Func<T, bool> predicate, Func<T, string> formatError = null)
	{
		foreach (var item in source)
		{
			if (!predicate(item))
				throw new ArgumentOutOfRangeException(nameof(item), item, formatError?.Invoke(item) ?? "Invalid value").PreDump(item);

			yield return item;
		}
	}
	public static IEnumerable<T> EmptyAsNull<T>(this IEnumerable<T> source) => source.ToArray() is { Length: > 0 } array ? array : null;
	public static bool IsSubsetBy<T, TKey>(this IEnumerable<T> source, IEnumerable<T> superset, Func<T, TKey> keySelector)
	{
		return !source.Select(keySelector).IsSubset(superset.Select(keySelector));
	}
	public static bool IsSubset<T>(this IEnumerable<T> source, IEnumerable<T> superset)
	{
		return !source.Except(superset).Any();
	}
	public static T? FirstOrNull<T>(this IEnumerable<T> source, Func<T, bool> predicate) where T : struct
	{
		foreach (var element in source)
		{
			if (predicate(element)) return element;
		}

		return null;
	}
}
public static class ExceptionExtensions
{
	/// <summary>Used to dump an object inside a throw-expression</summary>
	public static TException PreDump<TException>(this TException exception, object dump) where TException : Exception
	{
		dump.Dump();
		return exception;
	}
	/// <summary>Used to dump an object inside a throw-expression</summary>
	public static TException PreDump<TException>(this TException exception, object dump, string description) where TException : Exception
	{
		dump.Dump(description);
		return exception;
	}
}
public static class StringExtensions
{
	public static string Prefix(this string value, string prefix) => prefix + value;
	public static string Suffix(this string value, string suffix) => value + suffix;
	public static string RegexReplace(this string input, string pattern, string replacement) => Regex.Replace(input, pattern, replacement);
	public static string RemoveOnce(this string s, string value)
	{
		return value.Length > 0 && s.IndexOf(value) is var index && index != -1
			? s.Remove(index, value.Length)
			: s;
	}
	public static string RemoveHead(this string value, string head) => head?.Length > 0 && value.StartsWith(head) ? value[head.Length..] : value;
	public static string RemoveTail(this string value, string tail) => tail?.Length > 0 && value.EndsWith(tail) ? value[..^tail.Length] : value;
	public static string Trim(this string value, string trimChars) => value.Trim(trimChars.ToArray());
	public static string StripPair(this string value, string pair) => TryStripPair(value, pair, out var result) ? result : value;
	public static bool TryStripPair(this string value, string pair, out string result)
	{
		if (pair is not [var left, var right]) throw new ArgumentException($"invalid pair: {pair}");

		value = value.Trim();
		if (value.StartsWith(left) && value.EndsWith(right))
		{
			result = value[1..^1];
			return true;
		}
		else
		{
			result = null;
			return false;
		}
	}
	public static string EmptyAsNull(this string value) => value == string.Empty ? null : value;

	public static Hyperlinq ToCopyable(this string value, string header = "Copy") => Clickable.CopyText(header, value);
}
public static class XNameExtensions
{
	public static (string Xmlns, string LocalName) GetNameParts(this XElement e, bool trimApiContract = true) => e.Name.GetNameParts(trimApiContract);
	public static (string Xmlns, string LocalName) GetNameParts(this XAttribute e, bool trimApiContract = true) => e.Name.GetNameParts(trimApiContract);
	public static (string Xmlns, string LocalName) GetNameParts(this XName name, bool trimApiContract = true)
	{
		var xmlns = name.NamespaceName;
		if (trimApiContract && xmlns.IndexOf('?') is var index && index > 0)
		{
			xmlns = xmlns[..(index)];
		}
#if REPLACE_UNO_PLATFORM_XMLNS
		if (xmlns.StartsWith("http://uno.ui/"))
		{
			xmlns = NSPresentation;
		}
#endif

		return (xmlns, name.LocalName);
	}
	public static string Pretty(this XName name)
	{
		var (xmlns, localName) = name.GetNameParts();

		return $"{{{xmlns}}}{localName}";
	}

	public static bool Match(this XName x, string xmlns, string localName) => x.GetNameParts() == (xmlns, localName);

	public static bool Is<T>(this XName x) => x.LocalName == typeof(T).Name;
	public static bool Is(this XName x, string name) => x.LocalName == name;
	public static bool IsAnyOf(this XName x, params string[] names) => names.Contains(x.LocalName);

	public static bool IsMemberOf<T>(this XName x) => x.IsMemberOf(typeof(T));
	public static bool IsMemberOf(this XName x, Type type) => x.IsMemberOf(type.Name);
	public static bool IsMemberOf(this XName x, string typeName) => x.IsMemberOf(typeName, out var _);
	public static bool IsMemberOf<T>(this XName x, string memberName) => x.IsMemberOf(typeof(T), memberName);
	public static bool IsMemberOf(this XName x, Type type, string memberName) => x.IsMemberOf(type.Name, memberName);
	public static bool IsMemberOf(this XName x, string typeName, string memberName) => x.LocalName == $"{typeName}.{memberName}";
	public static bool IsMemberOf<T>(this XName x, out string memberName) => x.IsMemberOf(typeof(T), out memberName);
	public static bool IsMemberOf(this XName x, Type type, out string memberName) => x.IsMemberOf(type.Name, out memberName);
	public static bool IsMemberOf(this XName x, string typeName, out string memberName)
	{
		memberName = x.LocalName.Split(".") is { Length: 2 } parts && parts[0] == typeName
			? parts[1]
			: default;

		return memberName != default;
	}

	public static XElement GetMember(this XElement e, string memberName) => e.Element(e.Name.Namespace + $"{e.Name.LocalName}.{memberName}");
}
public static class XAttributeExtensions
{
	public static string ResourceKey(this XAttribute attribute)
	{
		var match = Regex.Match(attribute.Value, @"^{(StaticResource|ThemeResource) (?<key>.+)}$");

		return match.Success ? match.Groups["key"].Value : null;
	}
}
public static class ColorExtensions
{
	public static string ToRgbText(this Color color) => $"{color.R:X2}{color.G:X2}{color.B:X2}";
	public static string ToRgbText(this SolidColorBrush brush) => brush.Color.ToRgbText();

	public static object ToColoredBlock(this Color color)
	{
		var background = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
		return Util.RawHtml($"<div style='height: 20px; width: 20px; background: #{color.ToRgbText()};' />");
	}
	public static object ToColoredBlock(this SolidColorBrush brush) => brush.Color.ToColoredBlock();

	public static string ToPrettyString(this Color color)
	{
		return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2} (rgb: {color.R} {color.G} {color.B}, alpha: {color.A})";
	}
	public static string ToPrettyString(this SolidColorBrush brush) => brush.Color.ToPrettyString();
}
public static class MarkdownHelper
{
	public static string ToMarkdownBlock(this Color color) => $"![](http://via.placeholder.com/50x25/{color.ToRgbText()}/{color.ToRgbText()})";
	public static string ToMarkdownBlock(this SolidColorBrush brush) => brush.Color.ToMarkdownBlock();

	public static string ToMarkdownTable<T>(this IEnumerable<T> source)
	{
		var properties = typeof(T).GetProperties();
		var buffer = new StringBuilder();

		// header
		buffer
			.AppendLine(string.Join("|", properties.Select(x => x.Name)))
			.AppendLine(string.Join("|", Enumerable.Repeat("-", properties.Length)));

		// content
		foreach (var item in source)
		{
			buffer.AppendLine(string.Join("|", properties
				.Select(p => p.GetValue(item))
			));
		}

		return buffer.ToString();
	}
	public static string ToPaddedMarkdownTable<T>(
		this IEnumerable<T> source,
		Func<PropertyInfo, string> headerFormatter = null,
		Func<object, string> objectFormatter = null,
		int separatorPadding = 0,
		bool separatorOnSides = false)
	{
		var properties = typeof(T).GetProperties();
		var buffer = new List<string[]>();

		headerFormatter ??= x => x.Name;
		objectFormatter ??= x => x?.ToString();

		buffer.Add(properties.Select(headerFormatter).ToArray());
		foreach (var item in source)
		{
			buffer.Add(properties.Select(p => objectFormatter(p.GetValue(item))).ToArray());
		}

		var columnWidths = properties.Select((_, i) => buffer.Max(x => x[i].Length)).ToArray();

		var builder = new StringBuilder();
		var separator = string.Concat(new string(' ', separatorPadding), "|", new string(' ', separatorPadding));

		void WriteRow(IEnumerable<string> values)
		{
			if (separatorOnSides) builder.Append('|').Append(' ', separatorPadding);
			builder.AppendJoin(separator, values.Select((x, i) => x.PadRight(columnWidths[i])));
			if (separatorOnSides) builder.Append(' ', separatorPadding).Append('|');
			builder.AppendLine();
		}

		WriteRow(buffer[0]);
		WriteRow(columnWidths.Select(x => new string('-', x)));
		foreach (var row in buffer.Skip(1))
		{
			WriteRow(row);
		}

		return builder.ToString();
	}
}
public static class Logger
{
	private static ILoggerFactory? _factory;
	private static ILoggerFactory Factory => _factory ?? throw new InvalidOperationException("The factory is not initialized.");

	static Logger()
	{
		InitializeFactory(builder => builder.SetMinimumLevel(LogLevel.Debug));
	}
	public static void InitializeFactory(Action<ILoggingBuilder> configure)
	{
		if (_factory is { }) throw new InvalidOperationException("The factory was already initialized.");

		_factory = LoggerFactory.Create(b => b
			.Apply(configure)
			.AddConsole(c =>
			{
				c.FormatterName = "Systemd"; // one-liner output
			})
		);
	}

	public static ILogger<T> For<T>() => Factory.CreateLogger<T>();
	public static ILogger For(Type type) => Factory.CreateLogger(type);
	public static ILogger Log(this Type type) => Factory.CreateLogger(type);
}
public static class KeyedResourceExtensions
{
	public static bool IsReferenceFor<T>(this IKeyedResource x)
	{
		return x switch
		{
			StaticResource sr => sr.Value is T,
			ThemeResource tr => tr.LightValue is T && tr.DarkValue is T,

			_ => false,
		};
	}
	public static bool IsStaticResourceFor<T>(this IKeyedResource x) => (x as StaticResource)?.Value is T;
	public static bool IsThemeResourceFor<T>(this IKeyedResource x) => x is ThemeResource tr && tr.LightValue is T && tr.DarkValue is T;
	public static T StaticValue<T>(this IKeyedResource x) => (T)((StaticResource)x).Value;

	public static bool IsUnresolved(this IKeyedResource x)
	{
		return x switch
		{
			StaticResource sr when sr.Value is StaticResourceRef or ThemeResourceRef => true,
			ThemeResource tr when
				(tr.LightValue is StaticResourceRef or ThemeResourceRef) &&
				(tr.DarkValue is StaticResourceRef or ThemeResourceRef) => true,

			_ => false,
		};
	}

	public static IEnumerable<T> OfStaticResourceOf<T>(this IEnumerable<IKeyedResource> source)
	{
		return source
			.Select(x => (x as StaticResource)?.Value)
			.OfType<T>();
	}
	public static IEnumerable<StaticResource> OfStaticResource<T>(this IEnumerable<IKeyedResource> source)
	{
		return source.OfType<StaticResource>()
			.Where(x => x.IsStaticResourceFor<T>());
	}
	public static IEnumerable<ThemeResource> OfThemeResource<T>(this IEnumerable<IKeyedResource> source)
	{
		return source.OfType<ThemeResource>()
			.Where(x => x.IsThemeResourceFor<T>());
	}
}
public static class FluentExtensions
{
	public static T As<T>(this object x) where T : class => x as T;
	public static TResult Apply<T, TResult>(this T value, Func<T, TResult> selector)
	{
		return selector(value);
	}
	public static T Apply<T>(this T value, Action<T> apply)
	{
		apply(value);
		return value;
	}
}
public static class PivotHelper
{
	public static IEnumerable<ExpandoObject> Pivot<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, Func<T, IDictionary<string, object>> columnSelectors)
	{
		var results = new Dictionary<TKey, ExpandoObject>();
		foreach (var item in source)
		{
			var key = keySelector(item);
			if (!results.TryGetValue(key, out var data))
			{
				results[key] = data = new ExpandoObject();
				(data as dynamic).Key = key;
			}

			var columns = columnSelectors(item);
			columns.Populate(data);
		}

		return results.Values;
	}

	private static ExpandoObject Populate(this IDictionary<string, object> source, ExpandoObject target)
	{
		var targetImpl = target as IDictionary<string, object>;
		foreach (var kvp in source)
		{
			targetImpl[kvp.Key] = kvp.Value;
		}

		return target;
	}
}
public static class CodeAnalysisExtensions
{
	public static T As<T>(this SyntaxNode x) where T : SyntaxNode => x as T;
	public static T Cast<T>(this SyntaxNode x) where T : SyntaxNode => (T)x;
}
public static class Clickable
{
	public static Hyperlinq CopyText(string text) => CopyText(text, text);
	public static Hyperlinq CopyText(string header, string text)
	{
		return Create(header, () => ClipboardService.SetText(text));
	}

	public static Hyperlinq Create(string header, Action action) => new Hyperlinq(action, header);
}
public static class HierarchyExtensions
{
	public static string TreeGraph<T>(this T node, Func<T, IEnumerable<T>> childrenSelector, Func<T, string> formatter)
	{
		return node.TreeGraph(childrenSelector, (x, i) => new string(' ', i * 4) + formatter);
	}
	public static string TreeGraph<T>(this T node, Func<T, IEnumerable<T>> childrenSelector, Func<T, int, string> formatter)
	{
		return node.TreeGraph(childrenSelector, seq => seq, formatter);
	}
	public static string TreeGraph<T>(this T node, Func<T, IEnumerable<T>> childrenSelector, Func<IEnumerable<T>, IEnumerable<T>> sorter, Func<T, string> formatter)
	{
		return node.TreeGraph(childrenSelector, sorter, (x, i) => new string(' ', i * 4) + formatter);
	}
	public static string TreeGraph<T>(this T node, Func<T, IEnumerable<T>> childrenSelector, Func<IEnumerable<T>, IEnumerable<T>> sorter, Func<T, int, string> formatter)
	{
		var buffer = new StringBuilder();
		WriteNode(node, 0);

		void WriteNode(T node, int depth)
		{
			buffer.AppendLine(formatter(node, depth));
			foreach (var child in sorter(childrenSelector(node)))
			{
				WriteNode(child, depth + 1);
			}
		}

		return buffer.ToString();
	}

	public static IEnumerable<T> Flatten<T>(this T element, Func<T, IEnumerable<T>> childrenSelector)
	{
		yield return element;
		foreach (var child in childrenSelector(element) ?? Enumerable.Empty<T>())
		{
			foreach (var item in Flatten(child, childrenSelector))
			{
				yield return item;
			}
		}
	}
	public static IEnumerable<(string Path, T Node)> Flatten<T>(this T element, Func<T, IEnumerable<T>> childrenSelector, Func<T, string> nodeDescriptor, string separator = @"/") => Flatten(element, childrenSelector, nodeDescriptor, separator, null);
	private static IEnumerable<(string Path, T Node)> Flatten<T>(this T element, Func<T, IEnumerable<T>> childrenSelector, Func<T, string> nodeDescriptor, string separator, string path)
	{
		path += separator + nodeDescriptor(element);

		yield return (path, element);
		foreach (var child in childrenSelector(element) ?? Enumerable.Empty<T>())
		{
			foreach (var item in Flatten(child, childrenSelector, nodeDescriptor, separator, path))
			{
				yield return (item.Path, item.Node);
			}
		}
	}
	public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector)
	{
		return source.SelectMany(x => Flatten(x, childrenSelector));
	}
}
public static class HashSetExtensions
{
	public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> items)
	{
		foreach (var item in items)
		{
			set.Add(item);
		}
	}
}
public static class PermutationHelper
{
	public static IEnumerable<string> PermuteWords(this string text)
	{
		return Regex.Split(text, "(?<!^)(?=[A-Z])")
			.Permute()
			.Select(arr => string.Concat(arr));
	}
	public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> source)
	{
		if (source == null || !source.Any())
			throw new ArgumentNullException("source");

		var array = source.ToArray();

		return Permute(array, 0, array.Length - 1);
	}
	private static IEnumerable<IEnumerable<T>> Permute<T>(T[] array, int i, int n)
	{
		if (i == n)
			yield return array.ToArray();
		else
		{
			for (int j = i; j <= n; j++)
			{
				array.Swap(i, j);
				foreach (var permutation in Permute(array, i + 1, n))
					yield return permutation.ToArray();
				array.Swap(i, j); //backtrack
			}
		}
	}
	private static void Swap<T>(this T[] array, int i, int j)
	{
		T temp = array[i];

		array[i] = array[j];
		array[j] = temp;
	}
}