// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// 
// This program is free software; you can redistribute it and/or modify it under 
// the terms of the GNU General Public License as published by the Free Software 
// Foundation; either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with 
// this program; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using NClass.Translations;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace NClass.GUI
{
  public static class UpdatesChecker
  {
    private const string VersionUrl = "https://raw.githubusercontent.com/TrevorDArcyEvansBJSS/nERD/master/version.xml";

    private class VersionInfo
    {
      public VersionInfo(string version, string translationVersion, string versionName, string downloadPageUrl, string notes)
      {
        MainVersion = new Version(version);
        TranslationVersion = translationVersion;
        VersionName = versionName;
        DownloadPageUrl = downloadPageUrl;
        Notes = notes;
      }

      public Version MainVersion { get; }

      public string TranslationVersion { get; }

      public string VersionName { get; }

      public string DownloadPageUrl { get; }

      public string Notes { get; }

      public bool IsUpdated
      {
        get
        {
          return (IsMainProgramUpdated || IsTranslationUpdated);
        }
      }

      public bool IsMainProgramUpdated
      {
        get
        {
          return (MainVersion.CompareTo(Program.CurrentVersion) > 0);
        }
      }

      public bool IsTranslationUpdated
      {
        get
        {
          string currentTranslationVersion = Strings.TranslationVersion;
          return (TranslationVersion.CompareTo(currentTranslationVersion) > 0);
        }
      }

      public override string ToString()
      {
        if (VersionName == null)
          return MainVersion.ToString();
        else
          return string.Format("{0} ({1})", VersionName, MainVersion);
      }
    }

    private static VersionInfo GetVersionManifestInfo()
    {
      using (var downLoadUrl = new HttpRequestMessage(HttpMethod.Get, VersionUrl))
      {
        downLoadUrl.Headers.Add("Authorization",
            "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", Settings.Default.GitHubToken, "x-oauth-basic"))));
        downLoadUrl.Headers.Add("User-Agent", "nERD-github-client");

        using (var client = new HttpClient())
        {
          using (var contentResponse = client.SendAsync(downLoadUrl).Result)
          {
            var content = contentResponse.Content.ReadAsStringAsync().Result;
            var document = new XmlDocument();
            document.LoadXml(content);
            var root = document.DocumentElement;

            // Get main version information
            var versionElement = root["Version"];
            var version = versionElement.InnerText;

            // Get translation version information
            XmlNodeList translationElements = root.SelectNodes("TranslationVersions/" + Strings.TranslationName);
            string translationVersion;
            if (translationElements.Count == 0)
              translationVersion = Strings.TranslationVersion;
            else
              translationVersion = translationElements[0].InnerText;

            // Get other informations
            var name = root["VersionName"].InnerText;
            var url = root["DownloadPageUrl"].InnerText;
            var notes = root["Notes"].InnerText.Trim();

            return new VersionInfo(version, translationVersion, name, url, notes);
          }
        }
      }
    }

    private static void OpenUrl(string url)
    {
      System.Diagnostics.Process.Start(url);
    }

    public static void CheckForUpdates()
    {
      try
      {
        VersionInfo info = GetVersionManifestInfo();
        ShowNewVersionInfo(info);
      }
      catch (WebException)
      {
        MessageBox.Show(Strings.ErrorConnectToServer, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      catch (InvalidDataException)
      {
        MessageBox.Show(Strings.ErrorReadVersionData, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static void ShowNewVersionInfo(VersionInfo info)
    {
      if (info.IsUpdated)
      {
        string text = GetVersionDescription(info);
        string caption = Strings.CheckingForUpdates;

        DialogResult result = MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

        if (result == DialogResult.Yes)
          OpenUrl(info.DownloadPageUrl);
      }
      else
      {
        MessageBox.Show(Strings.NoUpdates, Strings.CheckingForUpdates, MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
    }

    private static string GetVersionDescription(VersionInfo info)
    {
      StringBuilder builder = new StringBuilder(512);

      if (info.IsMainProgramUpdated)
      {
        // Header text
        builder.AppendFormat("{0}: {1}\n\n", Strings.NewVersion, info.VersionName);

        // Main program's changes
        builder.Append(info.Notes);
        builder.Append("\n\n");
      }
      else if (info.IsTranslationUpdated)
      {
        builder.AppendFormat("{0}\n\n", Strings.TranslationUpdated);
      }

      // Download text
      builder.Append(Strings.ProgramDownload);

      return builder.ToString();
    }
  }
}
