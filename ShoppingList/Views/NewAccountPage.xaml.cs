using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShoppingList.Models;

namespace ShoppingList.Views;

public partial class NewAccountPage : ContentPage
{
    public NewAccountPage()
    {
        InitializeComponent();
        Title = "Create New Account";
    }

    private async void CreateAccount_OnClicked(object sender, EventArgs e)
    {
        // Check for blank or incorrectly formatted fields
        if (string.IsNullOrWhiteSpace(txtUser.Text) || 
            string.IsNullOrWhiteSpace(txtPassword1.Text) || 
            string.IsNullOrWhiteSpace(txtPassword2.Text) || 
            string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            await DisplayAlert("Error", "All fields must be filled in without spaces.","Ok");
            return;
        }
        
        // Check if passwords match
        if (txtPassword1.Text != txtPassword2.Text)
        {
            await DisplayAlert("Error", "Passwords have to match.","Ok");
            return;
        }
        
        // Regex for validating email addresses
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        // Check for valid email address ==    xxx@xxx.xxx
        if (!Regex.IsMatch(txtEmail.Text, emailPattern))
        {
            await DisplayAlert("Error", "Please enter a valid email.", "Ok");
            return;
        }
        
        // API stuff
        var data = JsonConvert.SerializeObject(new UserAccount(txtUser.Text, txtPassword1.Text, txtEmail.Text));
        var client = new HttpClient();
        var response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/createuser"), new StringContent(data,Encoding.UTF8,"application/json"));

        var AccountStatus = response.Content.ReadAsStringAsync().Result;
        
        
        // Check if the user exists
        if (AccountStatus == "user exists")
        {
            await DisplayAlert("Error", "This username is taken.","Ok");
            return;
        }
        
        // Check if the email is in use already
        if (AccountStatus == "email exists")
        {
            await DisplayAlert("Error", "This email is taken.","Ok");
            return;
        }
        
        if (AccountStatus == "complete")
        {
            response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/createuser"), new StringContent(data,Encoding.UTF8,"application/json"));

            var sKey = response.Content.ReadAsStringAsync().Result;

            if (!string.IsNullOrEmpty(sKey) && sKey.Length < 50)
            {
                App.SessionKey = sKey;
                Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "An unexpected error has occured while logging you in.","Ok");
                return;
            }
        }
        else
        {
            await DisplayAlert("Error", "An unexpected error has occured while trying to create your account.","Ok");
            return;
        }
        
        
        
    }
}