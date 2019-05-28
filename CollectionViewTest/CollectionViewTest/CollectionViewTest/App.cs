using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CollectionViewTest
{
    public class CellContent : Grid
    {
        public CellContent()
        {
            this.RowDefinitions = new RowDefinitionCollection()
            {
                new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)},
                new RowDefinition {Height = GridLength.Auto},
                new RowDefinition {Height = new GridLength(1, GridUnitType.Absolute)},
                new RowDefinition {Height = new GridLength(30, GridUnitType.Absolute)},
            };

            var autoHeightLabel = new Label();
            autoHeightLabel.SetBinding(Label.TextProperty, ".", stringFormat: "{0}, Auto");

            var absoluteHeightLabel = new Label();
            absoluteHeightLabel.SetBinding(Label.TextProperty, ".", stringFormat: "{0}, Abosolute");

            var startSeparator = new BoxView { BackgroundColor = Color.Green };
            var middleSeparator = new BoxView { BackgroundColor = Color.Black };

            this.Children.Add(startSeparator, 0, 0);
            this.Children.Add(autoHeightLabel, 0, 1); // This is not rendered when cell is inside CollectionView
            this.Children.Add(middleSeparator, 0, 2);
            this.Children.Add(absoluteHeightLabel, 0, 3); // This is rendered without problems 

            this.BackgroundColor = Color.Beige;
            this.Padding = new Thickness(6);
        }
    }

    public class App : Application
    {
        public App()
        {
            var itemsSource = Enumerable.Range(1, 100).Select(i => $"Item index: {i}").ToList();

            var collectionView = new CollectionView();
            collectionView.ItemsSource = itemsSource;
            collectionView.ItemTemplate = new DataTemplate(typeof(CellContent));

            var cellOutsideCollectionView = new CellContent(); // Lets creaate the same cell outside collection View and see if they differ (yes they do)
            cellOutsideCollectionView.BindingContext = itemsSource[0];

            var root = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                },
                RowDefinitions =
                {
                    new RowDefinition {Height = GridLength.Auto},
                    new RowDefinition {Height = GridLength.Star},
                }
            };
            root.Children.Add(new Label { Text = "Cell in CollectionView" }, 0, 0);
            root.Children.Add(new Label { Text = "Cell outside CollectionView" }, 1, 0);

            root.Children.Add(collectionView, 0, 1);
            root.Children.Add(cellOutsideCollectionView, 1, 1);

            this.MainPage = new ContentPage()
            {
                Content = root
            };

        }
    }
}
