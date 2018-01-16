using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;


using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;

public class jsLoadable 
{
    private WebBrowser web;
    private bitboardWorld world;

    public jsLoadable(bitboardWorld currentWorld)
    {
        this.world = currentWorld;
        this.web = new WebBrowser(); 
    }

    private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
        Console.WriteLine("doc complete");
    }

    public void init()
    {
    }

    public void tput()
    {
    }

    public void rput()
    {
        Console.WriteLine("::rput");
    }

    public void tget() { }

    public void rget() { }

    public void tattch() { }

    public void rattach() { }

    public void tremove() { }

    public void fremove() { }

    public void tflush() { }

    public void rflush() { }
}

public class Module 
{
    public string Name;
    public jsLoadable myTask;

    public Module(string name, string sourcefile, bitboardWorld host)
    {
        this.Name = name;
        this.myTask = new jsLoadable(host);
    }
}

public class App
{
    public string Name;
    public App(string name, string sourcefile)
    {
        this.Name = name;
    }
}

public class ProcFS : fileSystem
{
    FileNode myroot = null;
    List<Module> myModules = new List<Module>();
    List<App> myApps= new List<App>();
    FileNode appsNode;
    FileNode moduleFS;
    bitboardWorld myworld;
    public jsLoadable testApp;
    WebBrowser webItem = new WebBrowser();

    public ProcFS() : base()
    {
        this.myroot = base.init();
    }

    public override fileSystem onmount(string mountPoint)
    {
        base.onmount(mountPoint);
        this.myroot.setName(getWorkPathArray(mountPoint)[getWorkPathArray(mountPoint).Length - 1]);

        this.appsNode = new FileNode("apps", fileTypes.Directory);
        this.appsNode.addChild(new FileNode("ctl", fileTypes.Text));
        this.appsNode.addChild(new FileNode("new", fileTypes.Text));
        this.myroot.addChild(this.appsNode);

        this.moduleFS = new FileNode("modules", fileTypes.Directory);
        this.moduleFS.addChild(new FileNode("ctl", fileTypes.Text));
        this.moduleFS.addChild(new FileNode("new", fileTypes.Text));
        this.myroot.addChild(this.moduleFS);

        return this;
    }

    public override FileNode init(bitboardWorld world)
    {
        this.myworld = world;
        Console.WriteLine("procfs init");
        this.myModules.Add(new Module("test", "", world));

        return this.myroot;
    }


    private void callModule()
    {
        Console.WriteLine("testing stuff");

        Console.WriteLine("item count"+ this.myModules.Count);

        this.myModules.ElementAt(0).myTask.init();
        this.myModules.ElementAt(0).myTask.tput();

    }

    public override FileNode read(string path, string flags, int offset, int length)
    {
        if (path == "")
        {
            return this.myroot;
        }
        else if (getWorkPathArray(path)[1] == "apps")
        {
            if (path == "/apps")
            {
                Console.WriteLine("returning this.appsNode"+appsNode.countChildren());
                return this.appsNode;
            }
            else
            {
                int count = 0;

                while (count < this.myApps.Count && this.myApps[count].Name != getWorkPathArray(path)[1])
                {
                    count++;
                }

                if (this.myApps[count].Name == getWorkPathArray(path)[1])
                {
                    FileNode winNode = new FileNode(getWorkPathArray(path)[1], fileTypes.Directory);
                    return winNode;
                }

                return null;
            }
        }
        else if (getWorkPathArray(path)[1] == "modules")
        {
            if (path == "/modules")
                return this.moduleFS;
            else
            {
                int count = 0;

                while (count < this.myModules.Count && this.myModules[count].Name != getWorkPathArray(path)[1])
                {
                    count++;
                }

                if (this.myModules[count].Name == getWorkPathArray(path)[1])
                {
                    FileNode winNode = new FileNode(getWorkPathArray(path)[1], fileTypes.Directory);
                    return winNode;
                }

                return null;
            }
        }
        else
            return null;
    }

    public override int write(string path, int flags, string buffer, int offset, int length)
    {
        if (getWorkPathArray(path)[1] == "apps")
        {
            string[] args = buffer.Split(':');

            if (getWorkPathArray(path)[2] == "new")
            {
                this.appsNode.addChild(new FileNode(args[0], fileTypes.Directory));

                this.myApps.Add(new App(args[0], args[1]));
                return 0;
            }
            else
                return -1;
        }
        else if (getWorkPathArray(path)[1] == "modules")
        {
            string[] args = buffer.Split(':');

            if (getWorkPathArray(path)[2] == "new")
            {
                this.moduleFS.addChild(new FileNode(args[0], fileTypes.Directory));

                this.myModules.Add(new Module(args[0], args[1], this.myworld));
                return 0;
            }
            else
                return -1;
        }
        else
            return -1;
    }
}
