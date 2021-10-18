using System;
using Xamarin.Forms;

namespace StudyBuddy.App.Controls
{
    public class ToggleButton : Button
    {
        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
                nameof(IsSelected),
                typeof(bool),
                typeof(ToggleButton),
                false);

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set
            {
                SetValue(IsSelectedProperty, value);
                ToggleColor();
            }
        }

        public void Toggle()
        {
            IsSelected = !IsSelected;
        }

        private void ToggleColor()
        {
            var bc = BackgroundColor;
            var tc = TextColor;
            TextColor = bc;
            BackgroundColor = tc;
        }

        public ToggleButton() : base()
        {
            Clicked += EmptyClass_Clicked;
        }

        private void EmptyClass_Clicked(object sender, EventArgs e)
        {
            Toggle();
        }
    }
}