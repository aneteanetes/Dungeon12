using Dungeon.Varying;

namespace Dungeon.VariableEditor
{
    public partial class VariablesForm : Form
    {
        public VariablesForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Variables.Values;
        }
    }
}