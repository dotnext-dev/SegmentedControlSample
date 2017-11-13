using System;
using Xamarin.Forms;

namespace SegmentedControlSample
{
    public partial class SegmentedControlSamplePage : ContentPage
    {
        public SegmentedControlSamplePage()
        {
            InitializeComponent();

			this.BindingContext = new PageViewModel
			{
				CustomPointsSwitch = 1
			};
        }

		void OnValueChanged(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine(this.segControl.SelectedValue);
		}

		void Handle_Clicked(object sender, System.EventArgs e)
		{
			this.segControl.SelectedValue = "Two";
		}
    }

	public class PageViewModel : ObservableObject
	{
		int _customPointsSwitch;
		public int CustomPointsSwitch
		{
			get { return _customPointsSwitch; }
			set
			{
				SetProperty(ref _customPointsSwitch, value, nameof(CustomPointsSwitch));
			}
		}

		Command _selectCommand;
		public Command SelectCommand => _selectCommand ?? (_selectCommand = new Command((object param) =>
		{
			if (Int32.TryParse(param?.ToString(), out int index))
				CustomPointsSwitch = index;
		}));
	}
}
