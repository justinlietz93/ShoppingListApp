using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShoppingList.Models;

namespace ShoppingList.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        Title = "Login";
    }

    private async void Login_OnClicked(object sender, EventArgs e)
    {
        // User info testing
        //u: Jlietz93
        //p: aaa
        var data = JsonConvert.SerializeObject(new UserAccount(txtUser.Text, txtPassword.Text));
        
        var client = new HttpClient();
        
        var response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/login"), 
            new StringContent(data,Encoding.UTF8,"application/json"));

        var sKey = response.Content.ReadAsStringAsync().Result;

        if (!string.IsNullOrEmpty(sKey) && sKey.Length < 50)
        {
            App.SessionKey = sKey;
            Navigation.PopModalAsync();
        }
        else
        {
            await DisplayAlert("Error", "Invalid username or password.","Ok");
            return;
        }
        Navigation.PopModalAsync(); // Removes login window
    }

    private void CreateAccount_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new NewAccountPage());
    }
}