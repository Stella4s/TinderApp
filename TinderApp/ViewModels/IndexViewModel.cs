﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using TinderApp.Models;
using TinderApp.Views;
using System.Linq;
using System.Collections.Generic;

namespace TinderApp.ViewModels
{
    public class IndexViewModel : BaseViewModel
    {
        private int CurrentNumber;
        private string SwipeDirection;
        public Contact Item { get; set; }
        public Command LoadItemsCommand { get; }

        private string fullName;
        private int age;
        private string city;

        public string FullName
        {
            get => fullName;
            set => SetProperty(ref fullName, value);
        }

        public int Age
        {
            get => age;
            set => SetProperty(ref age, value);
        }
        public string City
        {
            get => city;
            set => SetProperty(ref city, value);
        }

        public IndexViewModel()
        {
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                var Item = await DataStore.GetItemAsync(CurrentNumber.ToString());
                if (Item != null)
                {
                    switch (SwipeDirection)
                    {
                        case "Left":
                            Item.SwipeState = SwipeStates.Accepted;
                            CurrentNumber++;
                            SwipeDirection = "None";
                            break;
                        case "Right":
                            Item.SwipeState = SwipeStates.Denied;
                            CurrentNumber++;
                            SwipeDirection = "None";
                            break;
                        default:
                            break;
                    }
                }

                var NewItem = await DataStore.GetItemAsync(CurrentNumber.ToString());

                FullName = NewItem.FullName;
                Age = NewItem.Age;
                City = NewItem.City;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        public async void OnSwipedLeft(object Sender, EventArgs e)
        {
            SwipeDirection = "Left";
            await ExecuteLoadItemsCommand();
        }
        public async void OnSwipedRight(object Sender, EventArgs e)
        {
            SwipeDirection = "Right";
            await ExecuteLoadItemsCommand();
        }
    }
}