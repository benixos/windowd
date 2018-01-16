using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Window
{
    public string Name;
    public Window(string name)
    {
        this.Name = name;
    }
}

public class WinFS : fileSystem
{
    FileNode myroot = null;
    List<Window> myWindows = new List<Window>();
    public WinFS() : base()
    {
        this.myroot = base.init();
    }

    public override fileSystem onmount(string mountPoint)
    {
        base.onmount(mountPoint);
        this.myroot.setName(getWorkPathArray(mountPoint)[getWorkPathArray(mountPoint).Length - 1]);

        //create sudo files attached to rootnode to present the files as attached to the node. we will never use .Data from these.
        this.myroot.addChild(new FileNode("ctl", fileTypes.Text));
        this.myroot.addChild(new FileNode("new", fileTypes.Text));

        return this;
    }

    public override FileNode read(string path, string flags, int offset, int length)
    {
        if (path == "")
        {
            return this.myroot;
        }
        else if (getWorkPathArray(path)[1] == "ctl")
        {
            return null;
        }
        else if (getWorkPathArray(path)[1] == "new")
        {
            return null;
        }
        else
        {
            int count = 0;

            while(count < this.myWindows.Count && this.myWindows[count].Name != getWorkPathArray(path)[1])
            {
                count++;
            }

            if(this.myWindows[count].Name == getWorkPathArray(path)[1])
            {
                FileNode winNode = new FileNode(getWorkPathArray(path)[1], fileTypes.Directory);
                return winNode;
            }
            else
                return null;
        }

    }

    public override int write(string path, int flags, string buffer, int offset, int length)
    {
        Console.WriteLine("winfs write path "+path);

        if (getWorkPathArray(path)[1] == "new")
        {
            Console.WriteLine("adding window "+buffer);
            this.myroot.addChild(new FileNode(buffer, fileTypes.Directory));

            this.myWindows.Add(new Window(buffer));
            return 0;
        }
        else
            return -1;
    }
}
