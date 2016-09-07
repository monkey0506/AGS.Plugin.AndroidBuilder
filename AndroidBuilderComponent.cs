using AGS.Types;
using System.Collections.Generic;
using System.Xml;

namespace AGS.Plugin.AndroidBuilder
{
    /// <summary>
    /// Editor component for the plugin.
    /// </summary>
    public class AndroidBuilderComponent : IEditorComponent
	{
		private const string COMPONENT_ID = "AndroidBuilderComponent";
        private const string CONTROL_ID_ROOT_NODE = "AndroidBuilderRoot";

        private static AndroidBuilderComponent _instance;

        public static AndroidBuilderComponent Instance
        {
            get { return _instance == null ? _instance = new AndroidBuilderComponent() : _instance; }
        }

		private AndroidBuilderComponent()
		{
            AndroidBuilderPlugin.AGSEditor.GUIController.ProjectTree.AddTreeRoot(this, CONTROL_ID_ROOT_NODE, "Android", AndroidBuilderPlugin.ICON_KEY);
		}

		string IEditorComponent.ComponentID
		{
			get { return COMPONENT_ID; }
		}

		IList<MenuCommand> IEditorComponent.GetContextMenu(string controlID)
		{
			List<MenuCommand> contextMenu = new List<MenuCommand>();
			return contextMenu;
		}

		void IEditorComponent.CommandClick(string controlID)
		{
            if (controlID == CONTROL_ID_ROOT_NODE)
            {
                AndroidBuilderPlugin.AGSEditor.GUIController.AddOrShowPane(AndroidBuilderPlugin.Pane);
            }
        }

		void IEditorComponent.PropertyChanged(string propertyName, object oldValue)
		{
		}

		void IEditorComponent.BeforeSaveGame()
		{
            AndroidBuilderPane.Instance.SaveMetadata();
		}

		void IEditorComponent.RefreshDataFromGame()
        {
            // A new game has been loaded, so remove the existing pane
            AndroidBuilderPlugin.AGSEditor.GUIController.RemovePaneIfExists(AndroidBuilderPlugin.Pane);
        }

		void IEditorComponent.GameSettingsChanged()
		{
		}

		void IEditorComponent.ToXml(XmlTextWriter writer)
		{
            AndroidMetadata.ToXml(writer);
		}

		void IEditorComponent.FromXml(XmlNode node)
		{
            AndroidMetadata.FromXml(node);
		}

		void IEditorComponent.EditorShutdown()
		{
		}
	}
}
