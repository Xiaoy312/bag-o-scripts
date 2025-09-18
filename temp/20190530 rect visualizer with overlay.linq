<Query Kind="Program">
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Drawing2D</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

void Main()
{
	Util.AutoScrollResults = false;
	
	VisualizeRect();
}

void VisualizeRect(Image? reloadedSourceOverride = null)
{
	Util.HorizontalRun(true, "refresh: ",
		Clickable.Create("Image from Clipboard (use alt+printscn)", () => CacheAndRefresh(LoadClipboardImage())), // use alt+printscn
		"[fixme]~~Android Screen Capture~~" // Clickable.Create("[fixme]~~Android Screen Capture~~", () => CacheAndRefresh(AdbScreencap()))
	).Dump();
	
	var screenshot = reloadedSourceOverride ??
		Image.FromFile(GetCacheScreenshotPath());
		//Image.FromFile(@"C:\Users\Xiaoy312\Downloads\20250829 zcc 1.png");
		//new Bitmap(500, 300) as Image;
	var profile =
		//new DeviceProfile(screenshot.Size, 1); // raw, no scaling
		new DeviceProfile(screenshot.Size, 1).SetDownscaleFactor(1); // generic uwp/skia?
		//DeviceProfiles.iPhone14ProMax.SetL2PScaling(3).SetDisplayScaling(1 / 6.0); // L2P can be 3 or 4
		//DeviceProfiles.Android10Q.SetDisplayScaling(1.0 / 4);
		//new DeviceProfile(screenshot.Size, 3).SetDownscaleFactor(3); // ios
		//new DeviceProfile(screenshot.Size, 1).SetDownscaleFactor(3); // macos: variable window size
		//new DeviceProfile(screenshot.Size, 2.625/* Uno.UI.ViewHelper.Scale */).SetDownscaleFactor(2); // droid emulator

	// windows title bar crop and/or additional shrinking
	screenshot = screenshot.Crop(WindowCrop);
	screenshot = screenshot.Crop(x => new Rectangle(default, x.Size.Scale(0.75)));

	Func<int, int> scalePaint = x => (int)(x / 2 * profile.LogicalToPhysicalScaling);
	Func<Image, Point, Image> crosshair = (image, x) => image.Crosshair(x, scalePaint(200), Color.Red, scalePaint(5));
	Func<Image, Point, Image> crosshairBlack = (image, x) => image.Crosshair(x, scalePaint(200), Color.Black, scalePaint(5));
	Func<Image, Point, Image> crosshairThin = (image, x) => image.Crosshair(x, scalePaint(10), Color.Red, scalePaint(1));
	Func<Image, (Point, Point), Image> lineThin = (image, x) => image.Line(x.Item1, x.Item2, Color.Red, scalePaint(1));

	//screenshot = screenshot.Crop(1,1+30, screenshot.Width-2, screenshot.Height-2-30); // skia
	Util.HorizontalRun(true, [
		StackView("screencap", screenshot, (ss, x) => ss),
		//StackView("windowRect", new Rectangle(0, 0, 360, 760).Scale(profile.LogicalToPhysicalScaling), overlayPink),
		//StackView("visibleBounds", new Rectangle(0, 30, 360, 730).Scale(profile.LogicalToPhysicalScaling), x => HighlightArea(screenshot.Size, x)),
		" ", // separator

		//StackTreeGraphNode("RT_ZoomContainer // XY=0,0, Actual=373x601, AbsRect=[373x601@250,0]"),
		//StackTreeGraphNode("        Grid#PART_Viewport // XY=0,0, Actual=361x589, AbsRect=[361x589@250,0]"),
		..StackTreeGraphMulti("""
				ContentControl#PART_InnerContentControl // XY=20,83.3, Actual=810x1066, AbsRect=[321x422.5@270,83.3]
					ContentPresenter // XY=-0,0, Actual=810x1066, AbsRect=[321x422.5@270,83.3]
						Border#DeviceBorder // XY=-0,0, Actual=810x1066, AbsRect=[321x422.5@270,83.3]
						Border#DeviceReverseBorder // XY=20,20, Actual=770x1026, AbsRect=[305.1x406.6@277.9,91.2]
						Grid#PART_ApplicationArea // XY=1,1, Actual=768x1024, AbsRect=[304.4x405.8@278.3,91.6], Constraints=[0,768,∞]x[0,1024,∞]
						Grid#PART_ActualContent // XY=-0,-0, Actual=768x1024, AbsRect=[304.4x405.8@278.3,91.6], Constraints=[0,768,∞]x[0,1024,∞]
		"""),
		
		//StackView("ContentPresenter", StringParser.ParseRect("[601.3x801.7@113.4,233]").ScaleFor(profile), OverlayCrosshairEdge),
		//StackView("Window\\UIView[1]\\UIView[1]", FromDiagnostic("[Rect 430x863@0,69]").ScaleFor(profile), overlayCrosshairEdge),
		//StackView("ComboBox[1]", FromDiagnostic("[Rect 398x48@16,117]").ScaleFor(profile), overlayCrosshairEdge),
		//StackView("LVI#11\\Text actual", FromDiagnostic("[Rect 364x191@16,52]").ScaleFor(profile), overlayCrosshairEdge),
		//StackView("viewport", ParseRect("[365.7x459.8@23,201]").ScaleFor(profile), overlayCrosshairEdge),
		
		//StackView("Buttons",
		//	new[] { "[201x115@0,1470]", "[201x115@440,1470]", "[201x115@879,1470]" }.Select(x => FromDiagnostic(x).Translate(dy: 63+126)),
		//	(ss, ctx) => ctx.Aggregate(ss, overlayCrosshairEdge),
		//	rects => string.Join("\n", rects)
		//),
	]).Dump();

	object StackView<T>(string header, T value, Func<Image, T, Image> visualize, Func<T, string>? describe = null)
	{
		describe = describe ?? (x => GetCustomDescription(x) ?? x.ToString());

		return Util.VerticalRun(
			header,
			visualize(screenshot, value).Scale(profile.DisplayScaling),
			describe(value)
		);
	}
	object StackTreeGraphNode(string node)
	{
		var entry = TreeGraphEntry.Parse(node);

		return StackView(entry.Identifier, StringParser.ParseRect(entry.Descriptions["AbsRect"]!), OverlayCrosshairEdge);
	}
	object[] StackTreeGraphMulti(string tree)
	{
		return tree.Split('\n', StringSplitOptions.RemoveEmptyEntries)
			.Select(StackTreeGraphNode)
			.ToArray();
	}
	object StackViewEx(string header, Func<Image, Image> visualize)
	{
		return Util.VerticalRun(
			header,
			visualize(screenshot).Scale(profile.DisplayScaling),
			null
		);
	}
	string? GetCustomDescription(object? o)
	{
		switch (o)
		{
			case Image x: return GetPhysicalAndLogicalDescription(x.Size, y => y.Scale(profile.PhysicalToLogicalScaling), y => $"Size: w={y.Width}, h={y.Height}");
			case Rectangle x: return GetPhysicalAndLogicalDescription(x, y => y.Scale(profile.PhysicalToLogicalScaling), y => $"LTWH={y.X},{y.Y},{y.Width}x{y.Height}");
			case Point x: return GetPhysicalAndLogicalDescription(x, y => y.Scale(profile.PhysicalToLogicalScaling), y => $"Point: x={y.X}, y={y.Y}");

			case null: return null;
			default: return null;
		}

		string GetPhysicalAndLogicalDescription<T>(T value, Func<T, T> logicalConverter, Func<T, string> describe) => profile.LogicalToPhysicalScaling != 1
			? string.Concat("[L]", describe(logicalConverter(value)), "\n", "[P]", describe(value))
			: describe(value);
	}
	
	void CacheAndRefresh(Image image)
	{
		image.Save(GetCacheScreenshotPath(), ImageFormat.Jpeg);
		Util.ClearResults();
		VisualizeRect(image);
	}
	Rectangle WindowCrop(Image i) => new Rectangle(default, i.Size)
		.Shrink(new(0, 30, 0, 0)) // title bar
		.Shrink(new(1, 1, 1, 1)); // app border
		
	Image OverlayCrosshairEdge(Image image, Rectangle x) => image.Overlay(x, Color.Pink, 0.7).Crosshair(x.GetCenter(), scalePaint(50), Color.Red, scalePaint(5)).Edge(x, Color.Red, scalePaint(5));
}

string GetCacheScreenshotPath()
{
	return Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.Personal), 
		"Downloads",
		"rect-visualizer-cache.jpg"
	);
}
System.Drawing.Image AdbScreencap()
{
	// note 20230306: doesnt work anymore, just use the screenshot button of droid emulator, and look in the pc download folder
	// note: "A generic error occurred in GDI+." could just mean the folder doesnt exist... ffs
	throw new Exception();
	
	var process = new Process();
	process.StartInfo.FileName = "adb";
	process.StartInfo.Arguments = "exec-out screencap -p";
	process.StartInfo.UseShellExecute = false;
	process.StartInfo.RedirectStandardOutput = true;
	process.StartInfo.CreateNoWindow = true;

	process.Start();

	using (var stream = new MemoryStream())
	{
		process.StandardOutput.BaseStream.CopyTo(stream);
		process.WaitForExit();
		
		return Image.FromStream(stream);
	}
}
System.Drawing.Image LoadClipboardImage()
{
	return Clipboard.GetImage()!;
}

public class StringParser
{
	public static T Parse<T>(string input, params (Func<Match, T> Parser, string ShortRegex)[] parsers)
	{
		foreach (var item in parsers)
		{
			if (Regex.Match(input, ExpandPattern(item.ShortRegex)) is var m && m.Success)
				return item.Parser(m);
		}
		
		throw new FormatException("Unable to parse: " + input);
	}
	public static Size ParseSize(string s)
	{
		return Parse<Size>(s, [
			(FromWidthHeight, "&{width}x&{height}"), // 0x0
		]);
		
		Size FromWidthHeight(Match m) => new(m.Groups["width"].ParseDoubleAsInt(), m.Groups["height"].ParseDoubleAsInt());
	}
	public static Rectangle ParseRect(string s)
	{
		return Parse<Rectangle>(s, [
			(FromXYWH, "XY=&{x},&{y}, Actual=&{width}x&{height}"),		// XY=0,0, Actual=0x0 tree-graph
			(FromXYWH, "(&x &y; &width &height)"), 						// (0 0; 0 0)
			(FromXYWH, "(&{width}x&{height})@(&{x},&{y})"), 			// (0x0)@(0,0)
			(FromXYWH, "&{width}x&{height}@&{x},&{y}"), 				// 0x0@0,0
			(FromXYWH, "[Rect &{width}x&{height}@&{x},&{y}]"), 			// [Rect 0x0@0,0]
			(FromXYWH, "[Rect {&{width},&{height}}@{[&{x}, &{y}]}]"), 	// [Rect {0,0}@{[0, 0]}] uno4.x lunacy
		]);
		
		Rectangle FromXYWH(Match m) => new(
			m.Groups["x"].ParseDoubleAsInt(),
			m.Groups["y"].ParseDoubleAsInt(),
			m.Groups["width"].ParseDoubleAsInt(),
			m.Groups["height"].ParseDoubleAsInt()
		);
	}
	
	private static string ExpandPattern(string shortPattern) // expand &asd to named decimal capture, and regex-escape everything else
	{
		return shortPattern
			.Explode(@"[$&]({\w+}|\w+)") // ${x} $x &y &{y} // use brace when parsing connected format: 0x0 x,y
			.Select(x => x is Match m
				? Capture<Decimal>(m.Value[1..].StripPair("{}"))
				: Regex.Escape((string)x)
			)
			.Concat();
	}
	private static string Capture<T>(string name) => Type.GetTypeCode(typeof(T)) switch
	{
		TypeCode.Int32 => $"(?<{name}>{@"-?\d+"})",
		TypeCode.Decimal => $"(?<{name}>{@"-?\d+(\.\d+)?"})",
		_ => throw new ArgumentOutOfRangeException(),
	};
}

Rectangle FromLTRB(int left, int top, int right, int bottom) => new Rectangle(left, top, right - left, bottom - top);
Rectangle FromDiagnostic(string text)
{
	var match = Regex.Match(text, @"^\[(Rect )?(?<width>\d+(\.\d+)?)(x|,)(?<height>\d+(\.\d+)?)@(?<x>\d+(\.\d+)?), ?(?<y>\d+(\.\d+)?)\]$");
	if (!match.Success)
		throw new FormatException("Unable to parse: " + text);
	
	return new Rectangle
	(
		(int)double.Parse(match.Groups["x"].Value),
		(int)double.Parse(match.Groups["y"].Value),
		(int)double.Parse(match.Groups["width"].Value),
		(int)double.Parse(match.Groups["height"].Value)
	);
}
Rectangle ParseRect(string text)
{
	var patterns = new (string Pattern, Func<Match, Rectangle> Parser)[]
	{
		( $@"^\({Capture<decimal>("x")} {Capture<decimal>("y")}; {Capture<decimal>("width")} {Capture<decimal>("height")}\)", RectangleFromXYWH ), 		// (0 0; 414 896)
		( $@"^\({Capture<decimal>("width")}x{Capture<decimal>("height")}\)@\({Capture<decimal>("x")},{Capture<decimal>("y")}\)", RectangleFromXYWH ),	// (414x896)@(0,0) 
		( $@"^{Capture<decimal>("width")}x{Capture<decimal>("height")}@{Capture<decimal>("x")},{Capture<decimal>("y")}", RectangleFromXYWH ),			 // 414.00x896.00@0.00,0.00
		( $@"^\[(Rect )?{Capture<decimal>("width")}x{Capture<decimal>("height")}@{Capture<decimal>("x")},{Capture<decimal>("y")}\]", RectangleFromXYWH ), 	// [Rect 414x896@0,0]
		( $@"^\[Rect {{{Capture<decimal>("width")},{Capture<decimal>("height")}}}@{{\[{Capture<decimal>("x")}, ?{Capture<decimal>("y")}\]}}\]", RectangleFromXYWH ),	// [Rect {330,54}@{[35, 197]}]  // uno4.x ToString format...
	};
	foreach (var item in patterns)
	{
		if (Regex.Match(text, item.Pattern) is var m && m.Success)
			return item.Parser(m);
	}
	
	throw new FormatException("Unable to parse: " + text);
	
	string Capture<T>(string name) => Type.GetTypeCode(typeof(T)) switch
	{
		TypeCode.Int32 => $"(?<{name}>{@"-?\d+"})",
		TypeCode.Decimal => $"(?<{name}>{@"-?\d+(\.\d+)?"})",
		_ => throw new ArgumentOutOfRangeException(),
	};
	Rectangle RectangleFromXYWH(Match match) => new Rectangle
	(
		(int)double.Parse(match.Groups["x"].Value),
		(int)double.Parse(match.Groups["y"].Value),
		(int)double.Parse(match.Groups["width"].Value),
		(int)double.Parse(match.Groups["height"].Value)
	);
}

Image HighlightArea(Size screen, Rectangle rectangle)
{
	var image = new Bitmap(screen.Width, screen.Height);
	using (var g = Graphics.FromImage(image))
	{
		g.Clear(Color.Pink);
		g.FillRectangle(Brushes.Orange, rectangle);
	}

	return image;
}

public record DeviceProfile(
	Size PhysicalScreenSize, 
	double LogicalToPhysicalScaling, // ViewHelper.Scale
	double DisplayScaling = 1.0 / 6.0) // 
{
	public Size LogicalScreenSize => PhysicalScreenSize.Scale(PhysicalToLogicalScaling);

	// You should use the value of `` for log2phy
	public double PhysicalToLogicalScaling => Math.Pow(LogicalToPhysicalScaling, -1);
	
	public DeviceProfile(int physicalScreenWidth, int physicalScreenHeight, double logicalToPhysicalScaling, double displayScaling = 1.0 / 6.0)
		: this(new Size(physicalScreenWidth, physicalScreenHeight), logicalToPhysicalScaling, displayScaling)
	{
	}

	// fluent syntax
	public DeviceProfile SetDisplayScaling(double value)
	{
		return this with { DisplayScaling = value };
	}
	public DeviceProfile SetL2PScaling(double value)
	{
		return this with { LogicalToPhysicalScaling = value };
	}
	public DeviceProfile SetDownscaleFactor(double value) => SetDisplayScaling(1 / value);
}
public class DeviceProfiles
{
	public static readonly DeviceProfile Honor10 = new DeviceProfile(360 * 3, 760 * 3, 3, 1.0 / 6);
	public static readonly DeviceProfile iPhone12ProMax = new DeviceProfile(642, 1389, 1.5, 1.0 / 3);
	public static readonly DeviceProfile iPhone13ProMax = new DeviceProfile(1284, 2778, 3, 1.0 / 3);
	public static readonly DeviceProfile iPhone14ProMax = new DeviceProfile(1290, 2796, 3, 1.0 / 4);
	public static readonly DeviceProfile Android10Q = new DeviceProfile(1080, 1920, 2.625, 1.0 / 3);
}

public record Thickness(int Left, int Top, int Right, int Bottom);
public record TreeGraphEntry(string Identifier, Dictionary<string, string?> Descriptions)
{
	public static TreeGraphEntry Parse(string entry)
	{
		var parts = entry.Split(" // ", 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		//var (type, xname) = parts[0].Split('#', 2) is { Length: 2 } arr ? (arr[0], arr[1]) : (parts[0], null);
		var descriptions = parts.ElementAtOrDefault(1)?.Split(", ")
			.Select(x => x.Split('=', 2) is [var key, var value] ? (key, value) : (x, null))
			.ToDictionary(x => x.Item1, x => x.Item2) ??
			new();

		//return new(type, xname, descriptions);
		return new(parts[0], descriptions);
	}
}

public static class Clickable
{
	public static Hyperlinq Create(string header, Action action) => new Hyperlinq(action, header);
}

public static class StringExtensions
{
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
	public static IEnumerable<object> Explode(this string input, string pattern)
	{
		if (string.IsNullOrEmpty(input)) yield break;
		
		var cursor = 0;
		foreach (var match in Regex.Matches(input, pattern).OfType<Match>())
		{
			if (cursor < match.Index)
			{
				yield return input.Substring(cursor, match.Index - cursor);
			}
			
			yield return match;
			cursor = match.Index + match.Length;
		}
		yield return input.Substring(cursor);
	}
	public static string Concat(this IEnumerable<string> source) => string.Concat(source);
}
public static class RegexExtensions
{
	public static double ParseDouble(this Group group) => double.Parse(group.Value);
	public static int ParseDoubleAsInt(this Group group) => (int)group.ParseDouble();
}
public static class ImageExtensions
{
    public static Image Crop(this Image image, int width, int height) => image.Crop(0, 0, width, height);
    public static Image Crop(this Image image, int x, int y, int width, int height) => image.Crop(new Rectangle(x, y, width, height));
	public static Image Crop(this Image image, Rectangle rect)
	{
		return (image as Bitmap ?? new Bitmap(image)).Clone(rect, image.PixelFormat);
	}
	public static Image Crop(this Image image, Func<Image, Rectangle> getRect) => image.Crop(getRect(image));
	
	public static Bitmap Scale(this System.Drawing.Image image, double factor) => image.Scale((int)(image.Width * factor), (int)(image.Height * factor));
	public static Bitmap Scale(this System.Drawing.Image image, int width, int height)
	{
		var destRect = new Rectangle(0, 0, width, height);
		var destImage = new Bitmap(width, height);

		destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

		using (var graphics = Graphics.FromImage(destImage))
		{
			graphics.CompositingMode = CompositingMode.SourceCopy;
			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			using (var wrapMode = new ImageAttributes())
			{
				wrapMode.SetWrapMode(WrapMode.TileFlipXY);
				graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
			}
		}

		return destImage;
	}

	public static Image Overlay(this Image source, Rectangle rectangle, Color color, double opacity = 1.0) => source.Overlay(rectangle, new SolidBrush(Color.FromArgb((int)(255 * opacity), color.R, color.G, color.B)));
	public static Image Overlay(this Image source, Rectangle rectangle, Brush brush)
	{
		var image = new Bitmap(source);
		using (var g = Graphics.FromImage(image))
		{
			g.FillRectangle(brush, rectangle);
		}

		return image;
	}

	public static Image Edge(this Image source, Rectangle rectangle, Color color, int thickness = 5) => source.Edge(rectangle, new Pen(color, thickness){ Alignment = PenAlignment.Inset });
	public static Image Edge(this Image source, Rectangle rectangle, Pen pen)
	{
		var image = new Bitmap(source);
		using (var g = Graphics.FromImage(image))
		{
			g.DrawLines(pen, new[]
			{
				new Point(rectangle.Left, rectangle.Top),
				new Point(rectangle.Left, rectangle.Bottom),
				new Point(rectangle.Right, rectangle.Bottom),
				new Point(rectangle.Right, rectangle.Top),
				new Point(rectangle.Left, rectangle.Top),
			});
		}

		return image;
	}

	public static Image Line(this Image source, Point p0, Point p1, Color color, int thickness = 5) => source.Line(p0, p1, new Pen(color, thickness){ Alignment = PenAlignment.Inset });
	public static Image Line(this Image source, Point p0, Point p1, Pen pen)
	{
		var image = new Bitmap(source);
		using (var g = Graphics.FromImage(image))
		{
			g.DrawLines(pen, new[] { p0, p1 });
		}

		return image;
	}
	
	public static Image Crosshair(this Image source, Point location, int length, Color color, int thickness = 5) => source.Crosshair(location, length, new Pen(color, thickness){ Alignment = PenAlignment.Center });
	public static Image Crosshair(this Image source, Point location, int length, Pen pen)
	{
		var image = new Bitmap(source);
		using (var g = Graphics.FromImage(image))
		{
			g.DrawLine(pen, location.X - length, location.Y, location.X + length, location.Y);
			g.DrawLine(pen, location.X, location.Y - length, location.X, location.Y + length);
		}

		return image;
	}
	
	public static Image Rect(this Image source, Rectangle rect, Color color, int thickness = 5) => source.Rect(rect, new Pen(color, thickness){ Alignment = PenAlignment.Center });
	public static Image Rect(this Image source, Rectangle rect, Pen pen)
	{
		var image = new Bitmap(source);
		using (var g = Graphics.FromImage(image))
		{
			g.DrawRectangle(pen, rect);
		}

		return image;
	}
	
	public static Image Draw(this Image source, Action<Graphics> draw)
	{
		var image = new Bitmap(source);
		using (var g = Graphics.FromImage(image))
		{
			draw(g);
		}

		return image;
	}
}
public static class SyntaxExtensions
{
	public static T Apply<T>(this T target, Action<T> action) { action(target); return target; }
	public static T ApplyIf<T>(this T target, bool condition, Action<T> action) { if (condition) action(target); return target; }
	
	public static TResult Apply<T, TResult>(this T target, Func<T, TResult> selector) => selector(target);
	public static TResult Apply<T, TArgument, TResult>(this T target, Func<T, TArgument, TResult> selector, TArgument argument) => selector(target, argument);
	
}
public static class GeometryExtensions
{
	public static Point Scale(this Point point, double factor)
	{
		return new Point((int)(point.X * factor), (int)(point.Y * factor));
	}
	public static Size Scale(this Size size, double factor)
	{
		return new Size((int)(size.Width * factor), (int)(size.Height * factor));
	}
	public static Rectangle Scale(this Rectangle rect, double factor)
	{
		return new Rectangle((int)(rect.X * factor), (int)(rect.Y * factor), (int)(rect.Width * factor), (int)(rect.Height * factor));
	}
	
	public static Point Translate(this Point p, Point delta) => p.Translate(delta.X, delta.Y);
	public static Point Translate(this Point p, int dx, int dy) => new Point(p.X + dx, p.Y + dy);
	public static Rectangle Translate(this Rectangle rect, Point delta) => rect.Translate(delta.X, delta.Y);
	public static Rectangle Translate(this Rectangle rect, int dx = 0, int dy = 0) => new Rectangle(rect.X + dx, rect.Y + dy, rect.Width, rect.Height);
	public static Rectangle Translate(this Rectangle rect, double dx = 0, double dy = 0) => new Rectangle((int)(rect.X + dx), (int)(rect.Y + dy), rect.Width, rect.Height);
	
	public static Rectangle Shrink(this Rectangle r, Thickness thickness)
	{
		return new(
			r.Left + thickness.Left, 
			r.Top + thickness.Top, 
			r.Width - thickness.Left - thickness.Right,
			r.Height - thickness.Top - thickness.Bottom
		);
	}
	
	public static Point GetCenter(this Rectangle rect) => rect.Location.Translate(rect.Size.Scale(0.5).AsPoint());
	
	public static Point AsPoint(this Size size) => new Point(size.Width, size.Height);
}
internal static class CustomGeometryExtensions
{
	public static Rectangle ScaleFor(this Rectangle rect, DeviceProfile profile) => rect.Scale(profile.LogicalToPhysicalScaling);
	public static Point ScaleFor(this Point p, DeviceProfile profile) => p.Scale(profile.LogicalToPhysicalScaling);
}