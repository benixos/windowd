using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

public class HomeFS : fileSystem
{
    FileNode myroot = null;
    string sysHomePath = null;

    public HomeFS() : base()
    {
        this.myroot = base.init();
        this.sysHomePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        if (Directory.Exists(sysHomePath))
        {
            string[] fileEntries = Directory.GetFiles(sysHomePath);

            foreach (string fileName in fileEntries)
                this.myroot.addChild(new FileNode(getFileNameFromPath(fileName), fileTypes.Text));

            string[] subdirectoryEntries = Directory.GetDirectories(sysHomePath);
            foreach (string subdirectory in subdirectoryEntries)
                this.myroot.addChild(new FileNode(getFileNameFromPath(subdirectory), fileTypes.Directory));
        }
    }

    private string getFileNameFromPath(string rawPath)
    {
        string[] pathArray = rawPath.Split('\\');
        return pathArray.ElementAt(pathArray.Length - 1);
    }

    private string buildWindowsFilePath(string path)
    {
        string buffer = "";
        string[] pathArray = path.Split('/');

        int count = 1;
        while (count < pathArray.Length)
        {
            buffer = buffer + '\\' + pathArray[count];
            count++;
        }

        return buffer;
    }


    public override fileSystem onmount(string mountPoint)
    {
        base.onmount(mountPoint);
        this.myroot.setName(getWorkPathArray(mountPoint)[getWorkPathArray(mountPoint).Length - 1]);
        return this;
    }

    public override FileNode read(string path, string flags, int offset, int length)
    {
        Console.WriteLine("path = " + path);
        if (path == "")
        {
            return this.myroot;
        }
        else
        {
            string loadPath = sysHomePath + buildWindowsFilePath(path);

            if (Directory.Exists(loadPath))
            {
                Console.WriteLine("loading Directory");
                string[] fileEntries = Directory.GetFiles(loadPath);

                FileNode tempNode = new FileNode(path, fileTypes.Directory);

                foreach (string fileName in fileEntries)
                    tempNode.addChild(new FileNode(getFileNameFromPath(fileName), fileTypes.Text));

                string[] subdirectoryEntries = Directory.GetDirectories(loadPath);
                foreach (string subdirectory in subdirectoryEntries)
                    tempNode.addChild(new FileNode(getFileNameFromPath(subdirectory), fileTypes.Directory));

                return tempNode;

            }
            else if (File.Exists(loadPath))
            {
                FileNode tempNode = new FileNode(getFileNameFromPath(path), fileTypes.Text);

                string filebuffer = File.ReadAllText(loadPath);
                tempNode.putData(filebuffer);
                return tempNode;
            }
            else
                return null;
        }
    }

    public override int write(string path, int flags, string buffer, int offset, int length)
    {
        return -1;
    }
}
