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

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using NClass.Translations;

namespace NClass.Core
{
  public sealed class Project : IModifiable
  {
    public event EventHandler BeginUndoableOperation;
    public event EventHandler Modified;
    public event EventHandler Renamed;
    public event EventHandler FileStateChanged;
    public event ProjectItemEventHandler ItemAdded;
    public event ProjectItemEventHandler ItemRemoved;

    private bool Loading { get; set; } = false;

    public Project()
    {
      _name = Strings.Untitled;
    }

    public Project(string name)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      if (name.Length == 0)
        throw new ArgumentException("Name cannot empty string.");

      _name = name;
    }

    private string _name;
    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        if (_name != value && value != null && value.Length > 0)
        {
          OnBeginUndoableOperation(EventArgs.Empty);
          _name = value;
          IsUntitled = false;
          OnRenamed(EventArgs.Empty);
          OnModified(EventArgs.Empty);
        }
      }
    }

    public bool IsUntitled { get; private set; } = true;

    public bool IsReadOnly { get; private set; } = false;

    private FileInfo _projectFile = null;
    public string FilePath
    {
      get
      {
        return _projectFile?.FullName;
      }
      private set
      {
        if (value != null)
        {
          try
          {
            FileInfo file = new FileInfo(value);

            if (_projectFile == null || _projectFile.FullName != file.FullName)
            {
              _projectFile = file;
              OnFileStateChanged(EventArgs.Empty);
            }
          }
          catch
          {
            if (_projectFile != null)
            {
              _projectFile = null;
              OnFileStateChanged(EventArgs.Empty);
            }
          }
        }
        else if (_projectFile != null) // value == null
        {
          _projectFile = null;
          OnFileStateChanged(EventArgs.Empty);
        }
      }
    }

    public string FileName
    {
      get
      {
        if (_projectFile != null)
          return _projectFile.Name;
        else
          return Name + ".ncp";
      }
    }

    public bool IsDirty { get; private set; } = false;

    private readonly List<IProjectItem> _items = new List<IProjectItem>();
    public IEnumerable<IProjectItem> Items
    {
      get { return _items; }
    }

    public int ItemCount
    {
      get { return _items.Count; }
    }

    public bool IsEmpty
    {
      get { return ItemCount == 0; }
    }

    public void Clean()
    {
      foreach (IProjectItem item in Items)
      {
        item.Clean();
      }
      if (IsDirty)
      {
        IsDirty = false;
        OnFileStateChanged(EventArgs.Empty);
      }
    }

    public void CloseItems()
    {
      foreach (IProjectItem item in Items)
      {
        item.Close();
      }
    }

    public void Add(IProjectItem item)
    {
      if (item == null)
        throw new ArgumentNullException("item");
      if (_items.Contains(item))
        throw new ArgumentException("The project already contains this item.");

      OnBeginUndoableOperation(EventArgs.Empty);
      item.Project = this;
      item.BeginUndoableOperation += delegate { OnBeginUndoableOperation(EventArgs.Empty); };
      item.Modified += new EventHandler(Item_Modified);
      _items.Add(item);

      OnItemAdded(new ProjectItemEventArgs(item));
      OnModified(EventArgs.Empty);
    }

    public void Remove(IProjectItem item)
    {
      if (_items.Remove(item))
      {
        OnBeginUndoableOperation(EventArgs.Empty);
        item.Close();
        item.BeginUndoableOperation -= delegate { OnBeginUndoableOperation(EventArgs.Empty); };
        item.Modified -= new EventHandler(Item_Modified);
        OnItemRemoved(new ProjectItemEventArgs(item));
        OnModified(EventArgs.Empty);
      }
    }

    private void Item_Modified(object sender, EventArgs e)
    {
      IsDirty = true;
      OnModified(EventArgs.Empty);
    }

    public string GetProjectDirectory()
    {
      if (_projectFile != null)
        return _projectFile.DirectoryName;
      else
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    /// <exception cref="IOException">
    /// Could not load the project.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The save file is corrupt and could not be loaded.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="fileName"/> is empty string.
    /// </exception>
    public static Project Load(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentException(Strings.ErrorBlankFilename, "fileName");

      if (!File.Exists(fileName))
        throw new FileNotFoundException(Strings.ErrorFileNotFound);

      XmlDocument document = new XmlDocument();
      try
      {
        document.Load(fileName);
      }
      catch (Exception ex)
      {
        throw new IOException(Strings.ErrorCouldNotLoadFile, ex);
      }

      XmlElement root = document["Project"];
      Project project = new Project();
      project.Loading = true;
      try
      {
        project.Deserialize(root);
      }
      catch (Exception ex)
      {
        throw new InvalidDataException(Strings.ErrorCorruptSaveFile, ex);
      }
      project.Loading = false;
      project.FilePath = fileName;
      project.IsReadOnly = project._projectFile.IsReadOnly;

      return project;
    }

    /// <exception cref="IOException">
    /// Could not save the project.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The project was not saved before by the <see cref="Save(string)"/> method.
    /// </exception>
    public void Save()
    {
      if (_projectFile == null)
        throw new InvalidOperationException(Strings.ErrorCannotSaveFile);

      Save(FilePath);
    }

    /// <exception cref="IOException">
    /// Could not save the project.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="fileName"/> is null or empty string.
    /// </exception>
    public void Save(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentException(Strings.ErrorBlankFilename, "fileName");

      XmlDocument document = new XmlDocument();
      XmlElement root = document.CreateElement("Project");
      document.AppendChild(root);

      Serialize(root);
      try
      {
        document.Save(fileName);
      }
      catch (Exception ex)
      {
        throw new IOException(Strings.ErrorCouldNotSaveFile, ex);
      }

      IsReadOnly = false;
      FilePath = fileName;
      Clean();
    }

    private void Serialize(XmlElement node)
    {
      XmlElement nameElement = node.OwnerDocument.CreateElement("Name");
      nameElement.InnerText = Name;
      node.AppendChild(nameElement);

      foreach (IProjectItem item in Items)
      {
        XmlElement itemElement = node.OwnerDocument.CreateElement("ProjectItem");
        item.Serialize(itemElement);

        Type type = item.GetType();
        XmlAttribute typeAttribute = node.OwnerDocument.CreateAttribute("type");
        typeAttribute.InnerText = type.FullName;
        itemElement.Attributes.Append(typeAttribute);

        XmlAttribute assemblyAttribute = node.OwnerDocument.CreateAttribute("assembly");
        assemblyAttribute.InnerText = type.Assembly.FullName;
        itemElement.Attributes.Append(assemblyAttribute);

        node.AppendChild(itemElement);
      }
    }

    /// <exception cref="InvalidDataException">
    /// The save format is corrupt and could not be loaded.
    /// </exception>
    private void Deserialize(XmlElement node)
    {
      IsUntitled = false;

      XmlElement nameElement = node["Name"];
      if (nameElement == null || nameElement.InnerText == "")
        throw new InvalidDataException("Project's name cannot be empty.");
      _name = nameElement.InnerText;

      foreach (XmlElement itemElement in node.GetElementsByTagName("ProjectItem"))
      {
        XmlAttribute typeAttribute = itemElement.Attributes["type"];
        XmlAttribute assemblyAttribute = itemElement.Attributes["assembly"];

        if (typeAttribute == null || assemblyAttribute == null)
          throw new InvalidDataException("ProjectItem's type or assembly name is missing.");

        string typeName = typeAttribute.InnerText;
        string assemblyName = assemblyAttribute.InnerText;

        try
        {
          Assembly assembly = Assembly.Load(assemblyName);
          IProjectItem projectItem = (IProjectItem)assembly.CreateInstance(
            typeName, false,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null, null, null, null);

          projectItem.Deserialize(itemElement);
          projectItem.Clean();
          Add(projectItem);
        }
        catch (InvalidDataException)
        {
          throw;
        }
        catch (Exception ex)
        {
          throw new InvalidDataException("Invalid type or assembly of ProjectItem.", ex);
        }
      }
    }

    private void OnBeginUndoableOperation(EventArgs e)
    {
      if (!Loading)
      {
        BeginUndoableOperation?.Invoke(this, e);
      }
    }

    private void OnModified(EventArgs e)
    {
      if (!Loading)
      {
        IsDirty = true;
        Modified?.Invoke(this, e);
      }
    }

    private void OnRenamed(EventArgs e)
    {
      Renamed?.Invoke(this, e);
    }

    private void OnItemAdded(ProjectItemEventArgs e)
    {
      ItemAdded?.Invoke(this, e);
    }

    private void OnItemRemoved(ProjectItemEventArgs e)
    {
      ItemRemoved?.Invoke(this, e);
    }

    private void OnFileStateChanged(EventArgs e)
    {
      FileStateChanged?.Invoke(this, e);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;

      if (GetType() != obj.GetType())
        return false;

      Project project = (Project)obj;

      if (_projectFile == null && project._projectFile == null)
        return object.ReferenceEquals(this, obj);

      return (
        _projectFile != null && project._projectFile != null &&
        _projectFile.FullName == project._projectFile.FullName
      );
    }

    public override int GetHashCode()
    {
      if (_projectFile != null)
        return _projectFile.GetHashCode();
      else
        return Name.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format("{0} [{1}]", Name, FilePath);
    }
  }
}