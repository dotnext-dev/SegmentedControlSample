using System;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SegmentedControlSample.SegmentedControl), typeof(SegmentedControlSample.iOS.SegmentedControlRenderer))]
namespace SegmentedControlSample.iOS
{
	public class SegmentedControlRenderer : ViewRenderer<SegmentedControl, UISegmentedControl>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<SegmentedControl> e)
		{
			base.OnElementChanged(e);

			UISegmentedControl segmentedControl = null;
			if (Control == null)
			{
				segmentedControl = new UISegmentedControl();

				for (var i = 0; i < e.NewElement.Children.Count; i++)
				{
					segmentedControl.InsertSegment(Element.Children[i].Text, i, false);
				}

				SetNativeControl(segmentedControl);
				SetSelectedSegment();
			}

			if (e.OldElement != null)
			{
				// Unsubscribe from event handlers and cleanup any resources
				if (segmentedControl != null)
					segmentedControl.ValueChanged -= NativeValueChanged;
			}

			if (e.NewElement != null)
			{
				// Configure the control and subscribe to event handlers
				segmentedControl.ValueChanged += NativeValueChanged;
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == nameof(SegmentedControl.SelectedSegment))
				SetSelectedSegment();
		}

		void NativeValueChanged(object sender, EventArgs e)
		{
			if (Element is SegmentedControl formsElement)
			{
				formsElement.SelectedSegment = (int)Control.SelectedSegment;
			};
		}

		void SetSelectedSegment()
		{
			if (Element is SegmentedControl formsElement)
			{
				if (formsElement.SelectedSegment >= 0 && formsElement.SelectedSegment < Control.NumberOfSegments)
					Control.SelectedSegment = formsElement.SelectedSegment;
			}
		}
	}
}
