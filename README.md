## CloudBoost .NET SDK 

[![Slack Status](https://slack.cloudboost.io/badge.svg)](https://slack.cloudboost.io)   [![NuGet version](https://badge.fury.io/nu/Cloudboost.svg)](https://badge.fury.io/nu/Cloudboost)
This is the .NET SDK for CloudBoost written in C#. It is available both on NuGet. If you want to have a look into documentation, you can check them out here : [https://tutorials.cloudboost.io](https://tutorials.cloudboost.io) and API reference is available here : [https://docs.cloudboost.io](https://docs.cloudboost.io)
This SDK works with : 
- ASP.NET
- Windows Phone 
- Windows 10 apps. 
- Universal Apps
- WPF / Silverlight

## NuGet Installation
```
Install-Package cloudboost
```

### Getting Started

``` 

using CB;

```

### Sample Code

``` 

// AppID and AppKey are your App ID and key of the application created in CloudBoost Dashboard.

//Init your Application
CB.CloudApp.init('YourAppId','YourAppKey');

//Data Storage : Create a CloudObject of type 'Custom' (Note: You need to create a table 'Custom' on CloudBoost Dashboard)

CB.CloudObject obj = new CB.CloudObject('Custom');

//Set the property 'name' (Note: Create a column 'name' of type text on CloudBoost Dashboard)
obj.set('name','CloudBoost');

//Save the object
obj = await obj.SaveAsync();

```

<img align="right" height="150" src="https://cloud.githubusercontent.com/assets/5427704/7724257/b7f45d6c-ff0d-11e4-8f60-06024eaa1508.png">

#### Getting Started and Tutorials

Visit [Getting Started](https://tutorials.cloudboost.io) for tutorial and quickstart guide.


#### API Reference

Visit [CloudBoost Docs](http://docs.cloudboost.io) for API Reference.
