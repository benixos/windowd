using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bitboard
{
    public partial class BEdit : Form
    {
        bitboardWorld myWorld;
        string currentPath = null;
        FileNode editFile = null;

        public BEdit(bitboardWorld world, string path)
        {
            this.myWorld = world;

            InitializeComponent();

            if (path != "" && path != null)
            {
                this.currentPath = path;
                this.Name = path;
                editFile = myWorld.agi.read(path, "", 0, 0);
                if (editFile != null && editFile.getType() == fileTypes.Text)
                    richTextBox1.Text = editFile.getData();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.currentPath != null)
                myWorld.agi.write(currentPath, 2, richTextBox1.Text, 0, 0);
            else
                myWorld.print("no save path set");

        }
    }
}
