using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SegmentedControlSample
{
	public class SegmentedControl : View, IViewContainer<SegmentedControlOption>
	{
		public IList<SegmentedControlOption> Children { get; set; }

		public SegmentedControl()
		{
			Children = new List<SegmentedControlOption>();
		}

		public event EventHandler ValueChanged;
		public static readonly BindableProperty SelectedValueProperty =
			BindableProperty.Create(
				"SelectedValue", typeof(string), typeof(SegmentedControl),
				defaultBindingMode: BindingMode.TwoWay,
				defaultValue: default(string), propertyChanged: OnSelectedValueChanged);

		public string SelectedValue
		{
			get { return (string)GetValue(SelectedValueProperty); }
			set { SetValue(SelectedValueProperty, value); }
		}

		static void OnSelectedValueChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((SegmentedControl)bindable).OnSelectedValueChangedImpl((string)oldValue, (string)newValue);
		}

		protected virtual void OnSelectedValueChangedImpl(string oldValue, string newValue)
		{
			ValueChanged?.Invoke(this, EventArgs.Empty);
			SelectedSegment = GetSelectedIndex(SelectedValue);
		}

		public static readonly BindableProperty SelectedSegmentProperty =
			BindableProperty.Create(
				"SelectedSegment", typeof(int), typeof(SegmentedControl),
				defaultBindingMode: BindingMode.TwoWay,
				defaultValue: default(int), propertyChanged: OnSelectedSegmentChanged);

		public int SelectedSegment
		{
			get { return (int)GetValue(SelectedSegmentProperty); }
			set { SetValue(SelectedSegmentProperty, value); }
		}

		private static void OnSelectedSegmentChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((SegmentedControl)bindable).OnSelectedSegmentChangedImpl((int)oldValue, (int)newValue);
		}

		protected virtual void OnSelectedSegmentChangedImpl(int oldValue, int newValue)
		{
			SelectedValue = GetSelectedValue(SelectedSegment);
		}

		int GetSelectedIndex(object selectedItem)
		{
			if (selectedItem == null)
				return -1;

			if (selectedItem is string optionText)
				return Children.IndexOf(Children.FirstOrDefault(x => Equals(x.Text, optionText)));

			return -1;
		}

		string GetSelectedValue(int index)
		{
			if (index >= 0 && index < Children.Count)
				return Children[index].Text;

			return null;
		}
	}

	public class SegmentedControlOption : View
	{
		public static readonly BindableProperty TextProperty =
			BindableProperty.Create("Text", typeof(string), typeof(SegmentedControlOption), string.Empty);

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
	}
}
