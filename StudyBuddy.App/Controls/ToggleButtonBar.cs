using System;
using System.Collections;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace StudyBuddy.App.Controls
{
    public class ToggleButtonBar : StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IList),
                typeof(ToggleButtonBar),
                null,
                propertyChanged: (bo, ov, nv) => (bo as ToggleButtonBar).SetItemsSource((IList)nv));

        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
                nameof(SelectedIndex),
                typeof(int),
                typeof(ToggleButtonBar),
                0,
                BindingMode.TwoWay,
                propertyChanged: (bo, ov, nv) => (bo as ToggleButtonBar).SelectedIndex = Convert.ToInt32(nv));

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
                nameof(SelectedItem),
                typeof(int),
                typeof(ToggleButtonBar),
                0,
                BindingMode.TwoWay,
                propertyChanged: (bo, ov, nv) => (bo as ToggleButtonBar).SelectedIndex = Convert.ToInt32(nv));

        public IList ItemsSource
        {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set
            {
                if (value >= ItemsSource.Count)
                    throw new Exception("Index not allowed!");

                SetValue(SelectedIndexProperty, value);

                for (int i = 0; i < Children.Count; i++)
                {
                    var button = Children[i] as ToggleButton;
                    if (i != value && button.IsSelected)
                        button.IsSelected = false;

                    if (i == value && !button.IsSelected)
                        button.IsSelected = true;
                }
            }
        }

        public object SelectedItem
        {
            get => ItemsSource[SelectedIndex];
            set
            {
                for (int i=0; i<ItemsSource.Count; i++)
                {
                    if (ItemsSource[i] == value)
                        SelectedIndex = i;
                }
            }
        }

        private void SetItemsSource(IList value)
        {
            UpdateEvents(value);
            UpdateView();
        }

        private void UpdateEvents(IList value)
        {
            if (value is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged -= MultiSelectionView_CollectionChanged;
                notifyCollectionChanged.CollectionChanged += MultiSelectionView_CollectionChanged;
            }
        }

        private void MultiSelectionView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            if (ItemsSource == null)
                return;

            this.Children.Clear();
            //SetValue(SelectedItemProperty, null);

            for (int i=0; i<ItemsSource.Count; i++)
            {
                var b = new ToggleButton();
                b.Text = ItemsSource[i].ToString();
                b.Clicked += B_Clicked;

                if (i == SelectedIndex)
                    b.IsSelected = true;

                Children.Add(b);
            }
        }

        private void B_Clicked(object sender, EventArgs e)
        {
            for (int i=0; i<Children.Count; i++)
            {
                var button = Children[i] as ToggleButton;
                if (button == sender)
                {
                    SelectedIndex = i;
                    return;
                }
            }
        }

        public ToggleButtonBar() : base()
        {
            Orientation = StackOrientation.Horizontal;
        }
    }
}