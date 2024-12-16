using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingList.Models;
using Newtonsoft.Json;

namespace ShoppingList.Views;

public partial class MainPage : ContentPage
{
    private LoginPage LP = new LoginPage();
    public MainPage()
    {
        InitializeComponent();
        Title = "Shopping List Pro";
        this.Loaded += MainPage_Loaded;
        LP.Unloaded += LP_Unloaded;
    }

    private void MainPage_Loaded(object sender, EventArgs e)
    {
        OnAppearing1();
    }

    private void LP_Unloaded(object sender, EventArgs e)
    {
        OnAppearing1();
    }

    public void OnAppearing1()
    {
        if (string.IsNullOrEmpty(App.SessionKey))
        {
            Navigation.PushModalAsync(new NavigationPage(LP)); // New window on top
        }
        else
        {
            txtInput.Text = App.SessionKey;
        }
        
    }

    private async void Logout_OnClicked(object sender, EventArgs e)
    {
        var data = JsonConvert.SerializeObject(new UserAccount(App.SessionKey));
        
        var client = new HttpClient();
        
        await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/logout"), 
            new StringContent(data,Encoding.UTF8,"application/json"));
        
        
        App.SessionKey = "";
        OnAppearing1();
    }
}