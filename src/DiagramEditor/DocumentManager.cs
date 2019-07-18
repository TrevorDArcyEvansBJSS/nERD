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
using System.Collections.Generic;

namespace NClass.DiagramEditor
{
  public sealed class DocumentManager
  {
    public event DocumentEventHandler ActiveDocumentChanged;
    public event DocumentEventHandler DocumentAdded;
    public event DocumentEventHandler DocumentRemoved;
    public event DocumentMovedEventHandler DocumentMoved;

    private LinkedListNode<IDocument> _switchingNode = null;

    private readonly List<IDocument> _documents = new List<IDocument>();
    public IEnumerable<IDocument> Documents
    {
      get { return _documents; }
    }

    private readonly OrderedList<IDocument> _documentHistory = new OrderedList<IDocument>();
    public IEnumerable<IDocument> DocumentHistory
    {
      get { return _documentHistory; }
    }

    public int DocumentCount
    {
      get { return _documents.Count; }
    }

    public bool HasDocument
    {
      get { return (_documents.Count > 0); }
    }

    private IDocument _activeDocument = null;
    public IDocument ActiveDocument
    {
      get
      {
        return _activeDocument;
      }
      set
      {
        if (_activeDocument != value && _documents.Contains(value))
        {
          IDocument oldDocument = _activeDocument;
          if (!SwitchingTabs)
            _documentHistory.ShiftToFirstPlace(value);
          _activeDocument = value;
          OnActiveDocumentChanged(new DocumentEventArgs(oldDocument));
        }
      }
    }

    public bool SwitchingTabs
    {
      get { return (_switchingNode != null); }
    }

    private void Document_Closing(object sender, EventArgs e)
    {
      IDocument document = (IDocument)sender;
      Close(document);
    }

    public void AddOrActivate(IDocument document)
    {
      if (document == null)
        throw new ArgumentNullException("document");

      EndSwitching();

      IDocument oldDocument = _activeDocument;
      if (_documents.Contains(document))
      {
        if (_activeDocument != document)
        {
          _activeDocument = document;
          _documentHistory.ShiftToFirstPlace(document);
          OnActiveDocumentChanged(new DocumentEventArgs(oldDocument));
        }
      }
      else
      {
        _documents.Add(document);
        _activeDocument = document;
        _documentHistory.AddFirst(document);
        document.Closing += new EventHandler(Document_Closing);
        OnDocumentAdded(new DocumentEventArgs(document));
        OnActiveDocumentChanged(new DocumentEventArgs(oldDocument));
      }
    }

    public void MoveDocument(IDocument document, int places)
    {
      int index = _documents.IndexOf(document);

      if (index >= 0 && places != 0)
      {
        int position = index;
        if (places > 0)
        {
          while (position < index + places && position < DocumentCount - 1)
          {
            _documents[position] = _documents[position + 1];
            position++;
          }
        }
        else // places < 0
        {
          while (position > index + places && position > 0)
          {
            _documents[position] = _documents[position - 1];
            position--;
          }
        }

        _documents[position] = document;
        OnDocumentMoved(new DocumentMovedEventArgs(document, index, position));
      }
    }

    public bool Close(IDocument document)
    {
      EndSwitching();

      if (_documents.Remove(document))
      {
        _documentHistory.Remove(document);
        document.Closing -= new EventHandler(Document_Closing);
        OnDocumentRemoved(new DocumentEventArgs(document));
        if (_activeDocument == document)
        {
          IDocument oldDocument = _activeDocument;
          if (HasDocument)
            _activeDocument = _documentHistory.FirstValue;
          else
            _activeDocument = null;
          OnActiveDocumentChanged(new DocumentEventArgs(oldDocument));
        }
        return true;
      }
      else
      {
        return false;
      }
    }

    public void CloseAll()
    {
      EndSwitching();

      if (HasDocument)
      {
        while (_documents.Count > 0)
        {
          IDocument document = _documents[_documents.Count - 1];
          _documents.RemoveAt(_documents.Count - 1);
          document.Closing -= new EventHandler(Document_Closing);
          OnDocumentRemoved(new DocumentEventArgs(document));
        }

        _documentHistory.Clear();

        if (_activeDocument != null)
        {
          IDocument oldDocument = _activeDocument;
          _activeDocument = null;
          OnActiveDocumentChanged(new DocumentEventArgs(oldDocument));
        }
      }
    }

    public void CloseAllOthers(IDocument exception)
    {
      EndSwitching();

      if (HasDocument && _documents.Count >= 2)
      {
        while (_documents.Count >= 2)
        {
          IDocument document = _documents[_documents.Count - 1];
          if (document != exception)
          {
            _documents.RemoveAt(_documents.Count - 1);
          }
          else
          {
            document = _documents[_documents.Count - 2];
            _documents.RemoveAt(_documents.Count - 2);
          }
          document.Closing -= new EventHandler(Document_Closing);
          OnDocumentRemoved(new DocumentEventArgs(document));
        }

        _documentHistory.Clear();
        _documentHistory.Add(exception);

        if (_activeDocument != exception)
        {
          IDocument oldDocument = _activeDocument;
          _activeDocument = exception;
          OnActiveDocumentChanged(new DocumentEventArgs(oldDocument));
        }
      }
    }

    public void SwitchDocument()
    {
      if (DocumentCount >= 2)
      {
        if (_switchingNode == null)
          _switchingNode = _documentHistory.First;

        _switchingNode = _switchingNode.Next;
        if (_switchingNode == null)
          _switchingNode = _documentHistory.First;

        ActiveDocument = _switchingNode.Value;
      }
    }

    public void EndSwitching()
    {
      if (SwitchingTabs)
      {
        _switchingNode = null;
        if (HasDocument)
          _documentHistory.ShiftToFirstPlace(ActiveDocument);
      }
    }

    private void OnActiveDocumentChanged(DocumentEventArgs e)
    {
      ActiveDocumentChanged?.Invoke(this, e);
    }

    private void OnDocumentAdded(DocumentEventArgs e)
    {
      DocumentAdded?.Invoke(this, e);
    }

    private void OnDocumentRemoved(DocumentEventArgs e)
    {
      DocumentRemoved?.Invoke(this, e);
    }

    private void OnDocumentMoved(DocumentMovedEventArgs e)
    {
      DocumentMoved?.Invoke(this, e);
    }
  }
}
