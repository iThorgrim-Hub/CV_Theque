namespace CV_Theque
{
    public sealed class PanelBuilder : Panel
    {
        private static FormBuilder? parent;
        private static Panel? _panel;
        public static Dictionary<string, Panel>? _panels = new Dictionary<string, Panel>();

        public Dictionary<int, Tuple<Panel, PanelBuilder>>? _panel54 = new Dictionary<int, Tuple<Panel,PanelBuilder>>();

        public static Dictionary<string, Size> panelCoord;

        private static new FormBuilder Parent
        {
            get => parent;
            set => parent = value;
        }

        private PanelBuilder(string name)
        {
            panelCoord = new Dictionary<string, Size>()
            {
                {"MainMenu", new Size((int)(parent.ClientSize.Width * .2), parent.ClientSize.Height)},
                {"MainPanel", new Size((int)(parent.ClientSize.Width * .8), parent.ClientSize.Height)}
            };

            // dgv builder
            _panel = new Panel();

            _panel.Name = name;

            _panel.Size = panelCoord[name];
            parent.Controls.Add(_panel);
            _panel.Dock = DockStyle.Left;

            if (name == "MainPanel")
            {
                _panel.BackColor = Color.Black;
            }
        }

        public static Panel GetPanel(string name, FormBuilder parent)
        {
            if (Parent != parent) { Parent = parent; };

            if (!_panels.ContainsKey(name))
            {
                _panels.Add(name, new PanelBuilder(name));
            }

            return _panels[name];
        }
    }
}
