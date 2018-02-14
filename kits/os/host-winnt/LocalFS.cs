using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 public class LocalFS : fileSystem
{
    FileNode myroot = null;

    public LocalFS() : base()
    {
        this.myroot = base.init();
    }

    public override fileSystem onmount(string mountPoint)
    {
        base.onmount(mountPoint);
        this.myroot.setName(getWorkPathArray(mountPoint)[getWorkPathArray(mountPoint).Length-1]);           
         return this;
    }

    public override FileNode read(string path, string flags, int offset, int length)
    {
        if (path == "")
            return this.myroot;

        if (path == "/dev" || getWorkPathArray(path)[1] == "dev" )
        {
            //Console.WriteLine("looking for local device name:" + getNewfileName(path) + " ::  " + getWorkPathArray(path)[2]);
            return new FileNode(getNewfileName(path), fileTypes.SPECIALDATA);
        }
       // else
       //     Console.WriteLine("looking for localfs path:" + path);

        FileNode aNode = base.read(path, flags, offset, length);
        return aNode;
    }

    public override int write(string path, int flags, string buffer, int offset, int length)
    {
        if (getWorkPathArray(path)[0] == "/dev")
        { 
            Console.WriteLine("looking for local device name:" + getNewfileName(path));
            return 0;
        }
        else
            Console.WriteLine("looking for localfs path:" + path);

        return  base.write(path, flags, buffer, offset, length); ;
    }
}

