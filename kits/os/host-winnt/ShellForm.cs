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
    public partial class ShellForm : Form
    {
        List<Command> cmdList = new List<Command>();
        public bitboardWorld world;
        public ShellForm(bitboardWorld world)
        {
            InitializeComponent();
            this.world = world;
            this.world.setStdOut(richTextBox1);
            this.addCommand(new echo());
            this.addCommand(new write());
            this.addCommand(new cat());
            this.addCommand(new ls());
            this.addCommand(new cls());
            this.addCommand(new mkdir());
            this.addCommand(new dump());
            this.addCommand(new text());
            this.addCommand(new cp());
            Console.WriteLine("start path: "+ this.world.SysRoot);

        }

        public void Shutdown()
        {

        }

        public void print(string buffer)
        {
            richTextBox1.Text = richTextBox1.Text + buffer;
        }

        public void addCommand(Command cmd)
        {
            cmdList.Add(cmd);
        }

        private void enterCommand(string commandLine)
        {
            string[] commandString = commandLine.Split(' ');

            int count = 0; 

            while( cmdList != null &&  count < cmdList.Count)
            {
                if(cmdList[count].Name == commandString[0]) {
                    cmdList[count].Run(commandString, this.world);
                }
                count++;
            }
        }

        private void textBox1_TextChanged(object sender, KeyPressEventArgs e)
        {
            switch(e.KeyChar)
            {
                case '\r':
                this.enterCommand(textBox1.Text);
                break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.enterCommand(textBox1.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }


    public class text : Command
    {
        public text()
        {
            this.Name = "text";
            this.help = "text path";
        }

        public override int Run(string[] args, bitboardWorld world)
        {
            this.myWorld = world;
            return this.main(args);
        }
        public override string getName()
        {
            return this.Name;
        }
        public int main(string[] args)
        {
            BEdit myEdit = null;

            if (args.Length > 1)
                myEdit = new BEdit(this.myWorld, args[1]);
            else
                myEdit = new BEdit(this.myWorld, "");

            myEdit.Show();


            return 0;
        }
    }
}