using NClass.DiagramEditor;
using System.ComponentModel;
using System.Windows.Forms;

namespace NClass.GUI
{
  public partial class TabbedWindow : UserControl
  {
    public TabbedWindow()
    {
      InitializeComponent();
    }

    private DocumentManager _docManager = null;
    [Browsable(false)]
    public DocumentManager DocumentManager
    {
      get
      {
        return _docManager;
      }
      set
      {
        if (_docManager != value)
        {
          _docManager = value;

          if (_docManager != null)
            _docManager.ActiveDocumentChanged -= docManager_ActiveDocumentChanged;
          _docManager = value;

          if (_docManager != null)
          {
            _docManager.ActiveDocumentChanged += docManager_ActiveDocumentChanged;
            canvas.Document = _docManager.ActiveDocument;
          }
          else
          {
            canvas.Document = null;
          }
          tabBar.DocumentManager = value;
        }
      }
    }

    [Browsable(false)]
    public TabBar TabBar
    {
      get { return tabBar; }
    }

    [Browsable(false)]
    public Canvas Canvas
    {
      get { return canvas; }
    }

    private void docManager_ActiveDocumentChanged(object sender, DocumentEventArgs e)
    {
      canvas.Document = _docManager.ActiveDocument;
    }
  }
}
