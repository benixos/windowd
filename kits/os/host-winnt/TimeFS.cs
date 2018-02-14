using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ClockFS : fileSystem
{
    FileNode myroot = null;

    public ClockFS() : base()
    {
        this.myroot = base.init();
    }

    public override fileSystem onmount(string mountPoint)
    {
        base.onmount(mountPoint);
        this.myroot.setName(getWorkPathArray(mountPoint)[getWorkPathArray(mountPoint).Length - 1]);

        //create sudo files attached to rootnode to present the files as attached to the node. we will never use .Data from these.
        this.myroot.addChild(new FileNode("time",fileTypes.Text));
        this.myroot.addChild(new FileNode("date", fileTypes.Text));

        return this;
    }

    public override FileNode read(string path, string flags, int offset, int length)
    {
        if(path == "")
        {
            return this.myroot;
        }
        else if (getWorkPathArray(path)[1] == "time")
        {
            FileNode timeNode = new FileNode(getNewfileName(path), fileTypes.Text);
            timeNode.putData(DateTime.Now.ToLongTimeString());
            return timeNode;
        }
        else if (getWorkPathArray(path)[1] == "date")
        {
            FileNode dateNode = new FileNode(getNewfileName(path), fileTypes.Text);
            dateNode.putData(DateTime.Now.ToLongDateString());
            return dateNode;
        }
        else
            return null;
    }

    public override int write(string path, int flags, string buffer, int offset, int length)
    {
        return errors.READ_ONLY;
    }
}