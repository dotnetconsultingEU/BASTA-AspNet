﻿// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

#nullable disable

using dotnetconsulting;

namespace dotnetconsulting.UserManagement.Infrastructure.Settings;

public class EnvironmentSettings
{
    public string[] Plants { get; set; } = Array.Empty<string>();
    public string DefaultCulture { get; set; } = "en-US";
}