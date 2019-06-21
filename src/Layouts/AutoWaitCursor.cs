using System;
using System.Windows.Forms;

namespace Layouts
{
  public sealed class AutoWaitCursor : IDisposable
  {
    private readonly Cursor _oldCursor;

    public AutoWaitCursor()
    {
      _oldCursor = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
    }

    public void Dispose()
    {
      Cursor.Current = _oldCursor;
    }
  }
}
