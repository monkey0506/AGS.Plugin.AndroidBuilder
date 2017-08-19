using AGS.Types;

namespace AGS.Plugin.AndroidBuilder
{
    [RequiredAGSVersion("3.4.0.0")]
	public class AndroidBuilderPlugin : IAGSEditorPlugin
	{
        internal const string ICON_KEY = "AndroidBuilderPluginIcon";

        internal static readonly AGSVersion AGS_VERSION_341 = new AGSVersion(3, 4, 1);

        internal static AGSVersion AGS_VERSION_CURRENT
        {
            get;
            private set;
        }

        private static IAGSEditor _editor;
        private static string _editorDirectoryPath;
        private static ContentDocument _pane;

        public static IAGSEditor AGSEditor
        {
            get { return _editor; }
        }

        // This is mirrored off of how the AGS editor gets its own path.
        public static string AGSEditorDirectory
        {
            get { return _editorDirectoryPath ?? (_editorDirectoryPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)); }
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
            AGS_VERSION_CURRENT = new AGSVersion(_editor.Version);
		}

		public void Dispose()
		{
			// We don't need any cleanup code
		}
	}
}
