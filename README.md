# Qvision poll-it

[![Build status](https://ci.appveyor.com/api/projects/status/i0rklyg43egdmkyb?svg=true)](https://ci.appveyor.com/project/JanvanHelvoort/qvision-poll-it)
[![NuGet release](http://img.shields.io/nuget/v/Qvision.PollIt.svg)](https://www.nuget.org/packages/Qvision.PollIt/)
[![MyGet Pre Release](https://img.shields.io/myget/janvanhelvoort/vpre/Qvision.PollIt.svg)](https://www.myget.org/feed/janvanhelvoort/package/nuget/Qvision.PollIt)

Umbraco package that let's create and manage polls - [Gallery](Documentation/gallery.md)

## Getting Started

### Installation

> *Note:* This package has been developed against **Umbraco v7.6.0** and will support that version and above.

#### NuGet package repository
To [install from NuGet](https://www.nuget.org/packages/Qvision.PollIt), you can run the following command from within Visual Studio:

	PM> Install-Package Qvision.PollIt

There is also a [MyGet build](https://www.myget.org/feed/janvanhelvoort/package/nuget/Qvision.PollIt) - for bleeding-edge / development releases.

#### After installation 
There will be a new section, 'Poll It'. If you can't see this section, you need to add the premisions for the users. Whitin this section, you can create and manage polls. 
![Dashboard](Documentation/Screenshots/Section%20Dashboard.png)

## Usage

### Creating your first poll
Within the new section, if you click on the `...` after Polls, you can select the `create` option. Now you are getting a blank edit screen.
![Create](Documentation/Screenshots/Question%20Edit.png)

### Propery editor
This package includes a custom propery editor for selecting a poll on a content page.
![Custom property](Documentation/Screenshots/Custom%20Property.png)

![Custom property](Documentation/Screenshots/Custom%20Property%20Editor.png)

## Developers Guide

For details on how to use the Doc Type Grid Editor package, please refer to our [Developers Guide](Documentation/developers-guide.md) documentation.

## License
Licensed under the [MIT License](LICENSE.md)
