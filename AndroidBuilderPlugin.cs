using AGS.Types;
using System.Drawing;

namespace AGS.Plugin.AndroidBuilder
{
    [RequiredAGSVersion("3.4.0.0")]
	public class AndroidBuilderPlugin : IAGSEditorPlugin
	{
        internal const string ICON_KEY = "AndroidBuilderPluginIcon";

        private static IAGSEditor _editor;
        private static ContentDocument _pane;

        public static IAGSEditor AGSEditor
        {
            get { return _editor; }
        }

        public static ContentDocument Pane
        {
            get { return _pane; }
        }

		public AndroidBuilderPlugin(IAGSEditor editor)
        {
            _editor = editor;
            _editor.AddComponent(AndroidBuilderComponent.Instance);
            _editor.GUIController.RegisterIcon(ICON_KEY, Properties.Resources.PluginIcon);
            _pane = new ContentDocument(AndroidBuilderPane.Instance, "Android", AndroidBuilderComponent.Instance, ICON_KEY);
            BuildTargetsInfo.RegisterBuildTarget(BuildTargetAndroid.Instance);
		}

		public void Dispose()
		{
			// We don't need any cleanup code
		}
	}
}
