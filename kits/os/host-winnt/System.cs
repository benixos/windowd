using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class bitboardWorld
{
    public AgI agi = new AgI();
    public string SysRoot = null;
    RichTextBox stdOut;

    public void setStdOut(RichTextBox box)
    {
        this.stdOut = box;
    }

    public void print(string buffer)
    {
        stdOut.Text = stdOut.Text + buffer + "\n";
    }

    public void flushScreenBuffer()
    {
        stdOut.Text = "";
    }
}

public abstract class Command
{
    public string Name;
    public string help;
    public bitboardWorld myWorld;
    public abstract int Run(string[] args, bitboardWorld world);
    public abstract string getName();
}

public class echo : Command
{
    public echo()
    {
        this.Name = "echo";
        this.help = "echo string";
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
        myWorld.print(args[1]);
        return 0;
    }
}

public class cat : Command
{
    public cat()
    {
        this.Name = "cat";
        this.help = "cat path";
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
        if (args.Length < 1)
            return -1;

        FileNode file = myWorld.agi.read(args[1], "", 0, 0);

        if (file.getType() == fileTypes.Text)
            myWorld.print(file.getData());
        else
            myWorld.print("Can Not Read File");
        return 0;
    }
}

public class ls : Command
{
    public ls()
    {
        this.Name = "ls";
        this.help = "ls path";
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
        if (args.Length < 1)
            return -1;

        FileNode file = myWorld.agi.read(args[1], "", 0, 0);

        int count = 0;

        if (file == null)
        {
            myWorld.print("File Not found");
            return -1;
        }

        if (file.getDirList().Count == 0)
        {
            myWorld.print("Empty Directory");
            return 0;
        }

        while (count < file.getDirList().Count)
        {
            myWorld.print(file.getDirList()[count].getName());
            count++;
        }
        return 0;
    }
}

public class dump : Command
{
    public dump()
    {
        this.Name = "dump";
        this.help = "dump path";
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
        FileNode file = myWorld.agi.read(args[1], "", 0, 0);

        if (file != null)
        {
            myWorld.print(file.getName() + ":");
            if (file.getNext() != null)
                myWorld.print("next" + file.getNext().ToString() + ":");
            else
                myWorld.print("next == null");
            if (file.getPrev() != null)
                myWorld.print("prev" + file.getPrev().ToString() + ":");
            else
                myWorld.print("prev == null");

            if (file.getParent() != null)
                myWorld.print("parent" + file.getParent().ToString() + ":");
            else
                myWorld.print("parent == null");

            if (file.getType() == fileTypes.Directory)
                myWorld.print("Child count:" + file.getDirList().Count);

            myWorld.print("type" + file.getType().ToString() + ":");
            return 0;
        }
        else
            return -1;
    }
}

public class cls : Command
{
    public cls()
    {
        this.Name = "cls";
        this.help = "cls";
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
        myWorld.flushScreenBuffer();
        return 0;
    }
}

public class write : Command
{
    public write()
    {
        this.Name = "write";
        this.help = "write buffer";
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
        string writeBuffer = "";

        if (args.Length < 4)
            myWorld.print(this.help);

        if (args.Length > 4)
        {
            int count = 2;

            while (count < args.Length)
            {
                writeBuffer = writeBuffer + " " + args[count];
                count++;
            }
        }
        //else
          //  writeBuffer = args[2];

        myWorld.print("writing " + writeBuffer + " to file:" + args[1]);
        myWorld.agi.write(args[1], 2, writeBuffer, 0, 0);
        return 0;
    }
}

public class mkdir : Command
{
    public mkdir()
    {
        this.Name = "mkdir";
        this.help = "mkdir path";
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
        myWorld.print("Creating directory:" + args[1]);
        myWorld.agi.write(args[1], writeFlags.CREATEDIR, "1", 0, 0);
        return 0;
    }
}

public class cp : Command
{
    public cp()
    {
        this.Name = "cp";
        this.help = "cp source dest";
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
        myWorld.print("Cp file:" + args[1] + " to "  +  args[2] );

        FileNode readBuffer = myWorld.agi.read(args[1],"",0,0);

        myWorld.agi.write(args[2], writeFlags.CREATEDATAFILE,readBuffer.toJson(),0,0);
        return 0;
    }
}

