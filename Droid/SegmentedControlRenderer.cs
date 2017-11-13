using System;

using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SegmentedControlSample.SegmentedControl), typeof(SegmentedControlSample.Droid.SegmentedControlRenderer))]
namespace SegmentedControlSample.Droid
{
	public class SegmentedControlRenderer : ViewRenderer<SegmentedControl, RadioGroup>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<SegmentedControl> e)
		{
			base.OnElementChanged(e);

			RadioGroup nativeControl = null;
			if (Control == null)
			{
				// Instantiate the native control and assign it to the Control property with the SetNativeControl method
				var layoutInflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
				nativeControl = new RadioGroup(Context)
				{
					Orientation = Orientation.Horizontal
				};

				for (var i = 0; i < e.NewElement.Children.Count; i++)
				{
					var o = e.NewElement.Children[i];
					var v = (SegmentedControlButton)layoutInflater.Inflate(Resource.Layout.SegmentedControl, null);
					v.Text = o.Text;
					if (i == 0)
						v.SetBackgroundResource(Resource.Drawable.segmented_control_first_background);
					else if (i == e.NewElement.Children.Count - 1)
						v.SetBackgroundResource(Resource.Drawable.segmented_control_last_background);
					nativeControl.AddView(v);
				}

				SetNativeControl(nativeControl);
				SetSelectedSegment();
			}

			if (e.OldElement != null)
			{
				// Unsubscribe from event handlers and cleanup any resources
				if (nativeControl != null)
					nativeControl.CheckedChange -= NativeCheckedChanged;
			}

			if (e.NewElement != null)
			{
				nativeControl.CheckedChange += NativeCheckedChanged;
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == nameof(SegmentedControl.SelectedSegment))
				SetSelectedSegment();
		}

		void NativeCheckedChanged(object sender, EventArgs e)
		{
			if (Element is SegmentedControl formsElement)
			{
				var rg = (RadioGroup)sender;
				if (rg.CheckedRadioButtonId != -1)
				{
					var id = rg.CheckedRadioButtonId;
					var radioButton = rg.FindViewById(id);
					var radioIndex = rg.IndexOfChild(radioButton);
					formsElement.SelectedSegment = radioIndex;
				}
			};
		}

		void SetSelectedSegment()
		{
			if (Element is SegmentedControl formsElement)
			{
				if (formsElement.SelectedSegment >= 0 && formsElement.SelectedSegment < Control.ChildCount)
				{
					var radioBtn = (RadioButton)Control.GetChildAt(formsElement.SelectedSegment);
					radioBtn.Checked = true;
				}
			}
		}
	}

	public class SegmentedControlButton : RadioButton
	{
		private int lineHeightSelected;
		private int lineHeightUnselected;
		private Paint linePaint;

		public SegmentedControlButton(Context context, IAttributeSet attributes) : this(context, attributes, Resource.Attribute.segmentedControlOptionStyle)
		{
		}

		public SegmentedControlButton(Context context, IAttributeSet attributes, int defStyle) : base(context, attributes, defStyle)
		{
			Initialize(attributes, defStyle);
		}

		private void Initialize(IAttributeSet attributes, int defStyle)
		{
			var a = this.Context.ObtainStyledAttributes(attributes, Resource.Styleable.SegmentedControlOption, defStyle, Resource.Style.SegmentedControlOption);

			var lineColor = a.GetColor(Resource.Styleable.SegmentedControlOption_lineColor, 0);
			linePaint = new Paint();
			linePaint.Color = lineColor;

			lineHeightUnselected = a.GetDimensionPixelSize(Resource.Styleable.SegmentedControlOption_lineHeightUnselected, 0);
			lineHeightSelected = a.GetDimensionPixelSize(Resource.Styleable.SegmentedControlOption_lineHeightSelected, 0);

			a.Recycle();
		}

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);

			if (linePaint.Color != 0 && (lineHeightSelected > 0 || lineHeightUnselected > 0))
			{
				var lineHeight = Checked ? lineHeightSelected : lineHeightUnselected;

				if (lineHeight > 0)
				{
					var rect = new Rect(0, Height - lineHeight, Width, Height);
					canvas.DrawRect(rect, linePaint);
				}
			}
		}
	}
}
