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
    public event EventHandler BeginUndoableOperation;
    public event EventHandler Modified;

    public bool IsDirty { get; private set; } = false;
    public Guid Id { get; } = Guid.NewGuid();

    private int _dontRaiseRequestCount = 0;
    private int _dontRaisePreRequestCount = 0;

    public virtual void Clean()
    {
      IsDirty = false;
    }

    protected bool Initializing { get; set; } = false;

    protected bool RaisePreChangedEvent
    {
      get
      {
        return (_dontRaisePreRequestCount == 0);
      }
      set
      {
        if (!value)
        {
          _dontRaisePreRequestCount++;
        }
        else if (_dontRaisePreRequestCount > 0)
        {
          _dontRaisePreRequestCount--;
        }

        if (RaisePreChangedEvent)
        {
          OnBeginUndoableOperation(EventArgs.Empty);
        }
      }
    }

    protected bool RaiseChangedEvent
    {
      get
      {
        return (_dontRaiseRequestCount == 0);
      }
      set
      {
        if (!value)
        {
          _dontRaiseRequestCount++;
        }
        else if (_dontRaiseRequestCount > 0)
        {
          _dontRaiseRequestCount--;
        }

        if (RaiseChangedEvent && IsDirty)
        {
          OnModified(EventArgs.Empty);
        }
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
        {
          OnModified(EventArgs.Empty);
        }
        else
        {
          IsDirty = true;
        }
      }
    }

    private void OnBeginUndoableOperation(EventArgs e)
    {
      BeginUndoableOperation?.Invoke(this, e);
    }

    private void OnModified(EventArgs e)
    {
      IsDirty = true;
      Modified?.Invoke(this, e);
    }
  }
}