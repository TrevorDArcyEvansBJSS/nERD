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

namespace NClass.Core
{
  public abstract class Element : IModifiable
  {
    private int dontRaiseRequestCount = 0;
    private int dontRaisePreRequestCount = 0;

    public event EventHandler BeginUndoableOperation;
    public event EventHandler Modified;

    public bool IsDirty { get; private set; } = false;

    public virtual void Clean()
    {
      IsDirty = false;
      //TODO: tagok tisztítása
    }

    protected bool Initializing { get; set; } = false;

    protected bool RaisePreChangedEvent
    {
      get
      {
        return (dontRaisePreRequestCount == 0);
      }
      set
      {
        if (!value)
          dontRaisePreRequestCount++;
        else if (dontRaisePreRequestCount > 0)
          dontRaisePreRequestCount--;

        if (RaisePreChangedEvent)
          OnBeginUndoableOperation(EventArgs.Empty);
      }
    }

    protected bool RaiseChangedEvent
    {
      get
      {
        return (dontRaiseRequestCount == 0);
      }
      set
      {
        if (!value)
          dontRaiseRequestCount++;
        else if (dontRaiseRequestCount > 0)
          dontRaiseRequestCount--;

        if (RaiseChangedEvent && IsDirty)
          OnModified(EventArgs.Empty);
      }
    }

    protected void OnBeginUndoableOperation()
    {
      if (!Initializing)
      {
        if (RaisePreChangedEvent)
        {
          OnBeginUndoableOperation(EventArgs.Empty);
        }
      }
    }

    protected void Changed()
    {
      if (!Initializing)
      {
        if (RaiseChangedEvent)
          OnModified(EventArgs.Empty);
        else
          IsDirty = true;
      }
    }

    private void OnBeginUndoableOperation(EventArgs e)
    {
      if (BeginUndoableOperation != null)
        BeginUndoableOperation(this, e);
    }

    private void OnModified(EventArgs e)
    {
      IsDirty = true;
      if (Modified != null)
        Modified(this, e);
    }
  }
}