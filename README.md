# DNF.Projects Module

A sample module which tracks Github metrics for projects. It includes examples of how to use scheduled jobs, third party assemblies, JavaScript libraries, etc...

A series of video tutorials was recorded which explains the various aspects of this module:

[Oqtane Module Development Series](https://www.youtube.com/playlist?list=PLYhXmd7yV0elLNLfQwZBUlM7ZSMYPTZ_f)

You can interact with a live demonstration of the module here:

[.NET Foundation Project Activity Trends](https://www.dnfprojects.com/)

Note that you cannot run this module directly in your IDE. You need to ensure that the DNF.Projects folder is located within the same parent folder as the Oqtane framework:  

```
/username
  /DNF.Projects
  /oqtane.framework
```

Organizing the folders in this way allows the system to automatically deploy the module DLLs to the Oqtane framework when your build the module solution. Then you can run the Oqtane framework and it will dynamically load the module.

Please note that this module has a number of dependencies which need to be configured correctly in order for the full functionality to be enabled. You need to configure these dependencies in the Module Settings once an intance of the module has been added to a page:

1. You need to create your own Github Token: https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token .

2. You can specify the Notify Name and Notify Email as your name and preferred email address. These would be used to notify you if there is an issue accessing the Github API. However for this to work correctly, you need to ensure the Notification job is also enabled in your installation and the SMTP Settings are set properly in the Site Settings.

Example Screen Shots:

![Module](https://github.com/oqtane/dnf.projects/blob/master/Screenshot1.png?raw=true "Bar Chart")

![Module](https://github.com/oqtane/dnf.projects/blob/master/Screenshot2.png?raw=true "Line Chart")
