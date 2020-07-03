using CollectionViewGroupedSelectedItemBug.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CollectionViewGroupedSelectedItemBug.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        public class MenuItem
        {
            public string IconName { get; set; }
            public string Name { get; set; }
            public MenuItemType Id { get; set; }
        }

        public class MenuItemGroup : ObservableCollection<MenuItem>
        {
            public string Name { get; private set; }

            public MenuItemGroup(string name, List<MenuItem> menuItems) : base(menuItems)
            {
                Name = name;
            }
        }
        public ObservableCollection<MenuItemGroup> MenuItems { get; private set; } = new ObservableCollection<MenuItemGroup>();
        public MenuItem SelectedMenuItem { get; set; }

        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        public MenuPage()
        {
            InitializeComponent();

            MenuItems = new ObservableCollection<MenuItemGroup>
            {
                new MenuItemGroup(
                    "First Section",
                    new List<MenuItem> {
                        new MenuItem { Id = MenuItemType.Browse, Name = "Browse" }
                    }
                ),
                new MenuItemGroup(
                    "Second Section",
                    new List<MenuItem> {
                        new MenuItem { Id = MenuItemType.About, Name = "About" }
                    }
                )
            };

            collectionView.ItemsSource = MenuItems;
            var firstItem = MenuItems.Select(x => x.FirstOrDefault())?.FirstOrDefault(); 
            collectionView.SelectedItem = firstItem;
            collectionView.SelectionChanged += CollectionView_SelectionChanged;
        }

        async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var previous = (e.PreviousSelection.FirstOrDefault() as MainPageViewModel.MenuItem);
            var current = (e.CurrentSelection.FirstOrDefault() as MenuItem);

            if (current == null)
                return;

            var id = (int)current.Id;
            await RootPage.NavigateFromMenu(id);
        }
    }
}