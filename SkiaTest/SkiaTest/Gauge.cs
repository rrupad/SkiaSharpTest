using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SkiaTest
{
	static class Constants
	{
		public const int IDLE = 97;
		public const int NOFLOW = 99;
		public const int FAULT = 99;
		public const int RUNNING = 96;
		public const int NORMAL = 0;
	}

	public class Gauge : SKCanvasView
	{
		//		public Gauge()
		//		{
		//			WidthRequest = 500;
		//			HeightRequest = 250;
		//		}






		#region Properties
		// Properties for the Values
		public static readonly BindableProperty ValProperty =
			BindableProperty.Create("Value", typeof(float), typeof(Gauge), 0.0f);

		public float Val
		{
			get { return (float)GetValue(ValProperty); }
			set { SetValue(ValProperty, value); }
		}

		public static readonly BindableProperty StartValueProperty =
			BindableProperty.Create("StartValue", typeof(float), typeof(Gauge), 0.0f);

		public float StartValue
		{
			get { return (float)GetValue(StartValueProperty); }
			set { SetValue(StartValueProperty, value); }
		}

		public static readonly BindableProperty EndValueProperty =
			BindableProperty.Create("EndValue", typeof(float), typeof(Gauge), 100.0f);

		public float EndValue
		{
			get { return (float)GetValue(EndValueProperty); }
			set { SetValue(EndValueProperty, value); }
		}

		public static readonly BindableProperty HighlightRangeStartValueProperty =
			BindableProperty.Create("HighlightRangeStartValue", typeof(float), typeof(Gauge), 70.0f);

		public float HighlightRangeStartValue
		{
			get { return (float)GetValue(HighlightRangeStartValueProperty); }
			set { SetValue(HighlightRangeStartValueProperty, value); }
		}

		public static readonly BindableProperty HighlightRangeEndValueProperty =
			BindableProperty.Create("HighlightRangeEndValue", typeof(float), typeof(Gauge), 100.0f);

		public float HighlightRangeEndValue
		{
			get { return (float)GetValue(HighlightRangeEndValueProperty); }
			set { SetValue(HighlightRangeEndValueProperty, value); }
		}

		// Properties for the Colors
		public static readonly BindableProperty GaugeLineColorProperty =
			BindableProperty.Create("GaugeLineColor", typeof(Color), typeof(Gauge), Color.FromHex("#70CBE6"));

		public Color GaugeLineColor
		{
			get { return (Color)GetValue(GaugeLineColorProperty); }
			set { SetValue(GaugeLineColorProperty, value); }
		}

		public static readonly BindableProperty ValueColorProperty =
			BindableProperty.Create("ValueColor", typeof(Color), typeof(Gauge), Color.FromHex("FF9A52"));

		public Color ValueColor
		{
			get { return (Color)GetValue(ValueColorProperty); }
			set { SetValue(ValueColorProperty, value); }
		}

		public static readonly BindableProperty RangeColorProperty =
			BindableProperty.Create("RangeColor", typeof(Color), typeof(Gauge), Color.FromHex("#E6F4F7"));

		public Color RangeColor
		{
			get { return (Color)GetValue(RangeColorProperty); }
			set { SetValue(RangeColorProperty, value); }
		}

		public static readonly BindableProperty NeedleColorProperty =
		   BindableProperty.Create("NeedleColor", typeof(Color), typeof(Gauge), Color.FromRgb(252, 18, 30));

		public Color NeedleColor
		{
			get { return (Color)GetValue(NeedleColorProperty); }
			set { SetValue(NeedleColorProperty, value); }
		}

		// Properties for the Units

		public static readonly BindableProperty UnitsTextProperty =
			BindableProperty.Create("UnitsText", typeof(string), typeof(Gauge), "");

		public string UnitsText
		{
			get { return (string)GetValue(UnitsTextProperty); }
			set { SetValue(UnitsTextProperty, value); }
		}

		public static readonly BindableProperty ValueFontSizeProperty =
		   BindableProperty.Create("ValueFontSize", typeof(float), typeof(Gauge), 33f);

		public float ValueFontSize
		{
			get { return (float)GetValue(ValueFontSizeProperty); }
			set { SetValue(ValueFontSizeProperty, value); }
		}
		#endregion

// =================   Status information from the sensors ================================================================

		public static readonly BindableProperty SensorStateProperty =
		   BindableProperty.Create("SensorState", typeof(char), typeof(Gauge));

		public char SensorState
		{
			get { return (char)GetValue(SensorStateProperty); }
			set { SetValue(ValueFontSizeProperty, value); }
		}
// --------------------------------------------------------------------------------------------------------------------------
/*
		public static readonly BindableProperty PMP1StateProperty =
		   BindableProperty.Create("PMP1State", typeof(char), typeof(Gauge), null);

		public char PMP1State
		{
			get { return (char)GetValue(PMP1StateProperty); }
			set { SetValue(ValueFontSizeProperty, value); }
		}
// --------------------------------------------------------------------------------------------------------------------------

		public static readonly BindableProperty PMP2StateProperty =
		   BindableProperty.Create("PMP2State", typeof(char), typeof(Gauge), null);

		public char PMP2State
		{
			get { return (char)GetValue(PMP2StateProperty); }
			set { SetValue(ValueFontSizeProperty, value); }
		}
// --------------------------------------------------------------------------------------------------------------------------
		public static readonly BindableProperty P1PWRStateProperty =
		   BindableProperty.Create("P1PWRState", typeof(char), typeof(Gauge), null);

		public char P1PWRState
		{
			get { return (char)GetValue(P1PWRStateProperty); }
			set { SetValue(ValueFontSizeProperty, value); }
		}

// --------------------------------------------------------------------------------------------------------------------------
		public static readonly BindableProperty P2PWRStateProperty =
		   BindableProperty.Create("P2PWRState", typeof(char), typeof(Gauge), null);

		public char P2PWRState
		{
			get { return (char)GetValue(P2PWRStateProperty); }
			set { SetValue(ValueFontSizeProperty, value); }
		}

// --------------------------------------------------------------------------------------------------------------------------
*/




		protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
		{
			base.OnPaintSurface(e);

			var canvas = e.Surface.Canvas;
			canvas.Clear();

			int width = e.Info.Width;
			int height = e.Info.Height;


			Console.WriteLine($"height : {height} width: {width}");

			SKPaint backPaint = new SKPaint
			{
				Style = SKPaintStyle.Fill,
				Color = SKColors.WhiteSmoke,
			};

			canvas.DrawRect(new SKRect(0, 0, width, height), backPaint);

			canvas.Save();

			canvas.Translate(width / 2, height / 3);
			canvas.Scale(Math.Min(width / 210f, height / 410f));

			SKPoint center = new SKPoint(0, 0);

			var rect = new SKRect(-100, -100, 100, 100);

			// Add a buffer for the rectangle
//			rect.Inflate(-10, -10);


			SKPaint GaugePointPaint = new SKPaint
			{
				IsAntialias = true,
				Style = SKPaintStyle.Fill,
				Color = ValueColor.ToSKColor()
			};

			SKPaint HighlightRangePaint = new SKPaint
			{
				IsAntialias = true,
				Style = SKPaintStyle.Fill,
				Color = RangeColor.ToSKColor()
			};


			// Draw the range of values

			var rangeStartAngle = AmountToAngle(HighlightRangeStartValue);
			var rangeEndAngle = AmountToAngle(HighlightRangeEndValue);
			var angleDistance = rangeEndAngle - rangeStartAngle;

			using (SKPath path = new SKPath())
			{
				path.AddArc(rect, rangeStartAngle, angleDistance);
				path.LineTo(center);
				canvas.DrawPath(path, HighlightRangePaint);
			}

			// Draw the main gauge line/arc
			SKPaint GaugeMainLinePaintP1 = new SKPaint
			{
				IsAntialias = true,
				Style = SKPaintStyle.Stroke,
				Color = SKColors.Red,
				StrokeWidth = 30
			};
			var startAngle = 135; var sweepAngle = 45.0f;

			using (SKPath path = new SKPath())
			{
				path.AddArc(rect, startAngle, sweepAngle);
				canvas.DrawPath(path, GaugeMainLinePaintP1);
			}

			//Sector2
			SKPaint GaugeMainLinePaintP2 = new SKPaint
			{
				IsAntialias = true,
				Style = SKPaintStyle.Stroke,
				Color = SKColors.Orange,
				StrokeWidth = 30
			};

			var startAngleP2 = 180.0f; sweepAngle = 195.0f;
			using (SKPath path = new SKPath())
			{
				path.AddArc(rect, startAngleP2, sweepAngle);
				canvas.DrawPath(path, GaugeMainLinePaintP2);
			}

			//Sector3
			SKPaint GaugeMainLinePaintP3 = new SKPaint
			{
				IsAntialias = true,
				Style = SKPaintStyle.Stroke,
				Color = SKColors.Green,
				StrokeWidth = 30
			};

			var startAngleP3 = 375.0f; sweepAngle = 30.0f;
			using (SKPath path = new SKPath())
			{
				path.AddArc(rect, startAngleP3, sweepAngle);
				canvas.DrawPath(path, GaugeMainLinePaintP3);
			}


			SKPaint sensorLED = new SKPaint
			{
				IsAntialias = true,
				Style = SKPaintStyle.Fill,
				Color = SKColors.Green,
				StrokeWidth = 15
			};

			using (SKPath path = new SKPath())
			{
				path.AddCircle(-100, 120, 20);
				canvas.DrawPath(path, sensorLED);
			}

			SKPaint p1LED = new SKPaint
			{
				IsAntialias = true,
				Style = SKPaintStyle.Fill,
				Color = SKColors.Green,
				StrokeWidth = 15
			};

			using (SKPath path = new SKPath())
			{
				path.AddCircle(-35, 120, 20);
				canvas.DrawPath(path, p1LED);
			}

			SKPaint p2LED = new SKPaint
			{
				IsAntialias = true,
				Style = SKPaintStyle.Fill,
				Color = SKColors.Green,
				StrokeWidth = 15
			};

			using (SKPath path = new SKPath())
			{
				path.AddCircle(35, 120, 20);
				canvas.DrawPath(path, p2LED);
			}


			SKPaint pwrLED = new SKPaint
			{
				IsAntialias = true,
				Style = SKPaintStyle.Fill,
				StrokeWidth = 15
			};

			pwrLED.Color = SKColors.Red;

			using (SKPath path = new SKPath())
			{
				path.AddCircle(100, 120, 20);
				canvas.DrawPath(path, pwrLED);
			}

			//===============================  Draw Text or below the dots ==============================

			SKPaint textFmt = new SKPaint
			{
				Color = SKColors.Black,
				TextSize = 20
			};

			canvas.DrawText("SNSR", -125, 160, textFmt);
			canvas.DrawText("PMP1", -55, 160, textFmt);
			canvas.DrawText("PMP2", 10, 160, textFmt);
			canvas.DrawText("PWR", 75, 160, textFmt);



			DrawNeedle(canvas, Val);                                      //Draw Needle

			SetSensorState(canvas, SensorState);
//			SetPMP1State(canvas, PMP1State);
//			SetPMP2State(canvas, PMP2State);
//			SetPWRState(canvas, P1PWRState, P2PWRState);

			SKPaint NeedleScrewPaint = new SKPaint()						//Draw Screw
			{
				IsAntialias = true,
//				Shader = SKShader.CreateRadialGradient(center, width / 60, new SKColor[]
//			   { new SKColor(171, 171, 171), SKColors.White }, new float[] { 0.05f, 0.9f }, SKShaderTileMode.Mirror)
			};

			canvas.DrawCircle(center, width / 130, NeedleScrewPaint);

			SKPaint paint = new SKPaint
			{
				IsAntialias = true,
				Style = SKPaintStyle.Stroke,
				Color = new SKColor(81, 84, 89).WithAlpha(100),
				StrokeWidth = 5f
			};

			canvas.DrawCircle(center, width / 100, paint);

			// Draw the Units of Measurement Text on the display
			SKPaint textPaint = new SKPaint
			{
				IsAntialias = true,
				Color = SKColors.Black
			};

			float textWidth = textPaint.MeasureText(UnitsText);
			textPaint.TextSize = 30f;

			SKRect textBounds = SKRect.Empty;
			textPaint.MeasureText(UnitsText, ref textBounds);

			float xText = -1 * textBounds.MidX - 25;
			float yText = 95 - textBounds.Height;

			// And draw the text
			canvas.DrawText(UnitsText, xText, yText, textPaint);

			// Draw the Value on the display
			var valueText = Val.ToString("F0"); //You can set F1 or F2 if you need float values
			float valueTextWidth = textPaint.MeasureText(valueText);
			textPaint.TextSize = ValueFontSize;

			textPaint.MeasureText(valueText, ref textBounds);

			xText = -1 * textBounds.MidX + 5;
			yText = 85 - textBounds.Height + 15;

			// And draw the text
			canvas.DrawText(valueText, xText, yText, textPaint);
			canvas.Restore();
		}

		float AmountToAngle(float value)
		{
			return 135f + (value / (EndValue - StartValue)) * 270f;
		}

		void DrawNeedle(SKCanvas canvas, float value)
		{
			float angle = 45f + (value / (100 - 0)) * 270f;
			canvas.Save();
			canvas.RotateDegrees(angle);

			float needleWidth = 6f;
			float needleHeight = 110f;
			float x = 0f, y = 0f;

			SKPaint paint = new SKPaint
			{
				IsAntialias = true,
				Color = NeedleColor.ToSKColor()
			};

			SKPath needleRightPath = new SKPath();

			needleRightPath.MoveTo(x, y);
			needleRightPath.LineTo(x + needleWidth, y);
			needleRightPath.LineTo(x + needleWidth, y + needleHeight);
			needleRightPath.LineTo(x, y + needleHeight);
			needleRightPath.LineTo(x,  y);

			SKPath needleLeftPath = new SKPath();
			needleRightPath.MoveTo(x, y);
			needleRightPath.LineTo(x - needleWidth, y);
			needleRightPath.LineTo(x - needleWidth, y + needleHeight);
			needleRightPath.LineTo(x, y + needleHeight);
			needleRightPath.LineTo(x, y);

			canvas.DrawPath(needleRightPath, paint);
			canvas.DrawPath(needleLeftPath, paint);
			canvas.Restore();
		}


		void SetSensorState(SKCanvas canvas, char SensorState)
		{
//			sensorLED.Color = SKColors.Green;
//			if (SensorState == Constants.FAULT) sensorLED.Color = SKColors.Red;
		}

		void SetPMP1State(SKCanvas canvas, char PMP1State)
		{

		}

		void SetPMP2State(SKCanvas canvas, char PMP2State)
		{

		}

		void SetPWRState(SKCanvas canvas, char P1PWRState, char P2PWRState)
		{

		}



		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			// Determine when to change. Basically on any of the properties that we've added that affect
			// the visualization, including the size of the control, we'll repaint
			if (propertyName == HighlightRangeEndValueProperty.PropertyName
				|| propertyName == HighlightRangeStartValueProperty.PropertyName
				|| propertyName == ValProperty.PropertyName
				|| propertyName == WidthProperty.PropertyName
				|| propertyName == HeightProperty.PropertyName
				|| propertyName == StartValueProperty.PropertyName
				|| propertyName == EndValueProperty.PropertyName
				|| propertyName == GaugeLineColorProperty.PropertyName
				|| propertyName == ValueColorProperty.PropertyName
				|| propertyName == RangeColorProperty.PropertyName
				|| propertyName == UnitsTextProperty.PropertyName)
			{
				InvalidateSurface();
			}
		}
	}

}
