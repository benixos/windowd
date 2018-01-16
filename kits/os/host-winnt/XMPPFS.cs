using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.Collections;
using agsXMPP.protocol.iq.roster;
using agsXMPP.Xml.Dom;
using agsXMPP.Factory;


public class AgIXML : Element
{
    public AgIXML()
    {
        this.TagName = "agixml";
        this.Namespace = "softsurve:agi";
    }

    public AgIXML(string Type, string path) : this()
    {
        this.ID = DateTime.Now.Ticks.GetHashCode(); 
        this.Type = Type;
        this.Path = path;
        this.SessionID = "null";
        this.SessionKey = "null";
        this.Data = "null";
        this.Flags = "null";
    }

    public int ID
    {
        get { return GetTagInt("id"); }
        set { SetTag("id", value.ToString()); }
    }

    public string Type
    {
        get { return GetTag("type"); }
        set { SetTag("type", value.ToString()); }
    }

    public string SessionID
    {
        get { return GetTag("session"); }
        set { SetTag("session", value.ToString()); }
    }

    public string SessionKey
    {
        get { return GetTag("key"); }
        set { SetTag("key", value.ToString()); }
    }

    public string Path
    {
        get { return GetTag("path"); }
        set { SetTag("path", value.ToString()); }
    }

    public string Data
    {
        get { return GetTag("path"); }
        set { SetTag("path", value.ToString()); }
    }

    public string Flags
    {
        get { return GetTag("path"); }
        set { SetTag("path", value.ToString()); }
    }
}


public class contact
{
    public string userID = "";
    public string locationID = "";
    public string status = "";
}

public class messages
{
    public string from = "";
    public string recevied = "";
    public string body = "";
}

public class chat
{
    public contact chatWith = new contact();
    public List<messages> msgChain = new List<messages>();
}

public static class xmppManager
{
    public static XmppClientConnection xmpp;

    public static List<contact> contacts = new List<contact>();
    public static List<chat> chats = new List<chat>();
    public static bitboardWorld world;
    public static void login(string username, string password)
    {
        Jid jidSender = new Jid(username);
        XmppClientConnection xmpp = new XmppClientConnection(jidSender.Server);
        xmppManager.xmpp = xmpp;

        agsXMPP.Factory.ElementFactory.AddElementType("agixml", "softsurve:agi", typeof(AgIXML));

        xmpp.Open(jidSender.User, password);
        xmpp.OnLogin += new ObjectHandler(xmpp_OnLogin);
        xmpp.OnPresence += new PresenceHandler(xmpp_OnPresence);
        xmpp.OnMessage += Xmpp_OnMessage;
    }

    static void xmpp_OnLogin(object sender)
    {  
        Console.WriteLine("Logged In");

        Presence p = new Presence(ShowType.chat, "Online");
        p.Type = PresenceType.available;
        xmpp.Send(p);
    }

    static void xmpp_OnPresence(object sender, Presence pres)
    {
        //Console.WriteLine("Available Contacts: ");
        //Console.WriteLine( pres.From.User+ " : " + pres.From.Server + " : "  + pres.Type);

        int count = 0;
        //check to see if we just update the pres
        while (contacts.Count != 0 &&  count < contacts.Count )
        {
            if(contacts.ElementAt(count).userID == pres.From.User)
            {
                contacts.ElementAt(count).status = pres.Type.ToString();
                return;
            }
            count++;
        }

        count = 0;
        //if now, we add to the list
        contact info = new contact();
        info.userID = pres.From.User;
        info.status = pres.Type.ToString();
        contacts.Add(info);
    }

    static void Xmpp_OnMessage(object sender, agsXMPP.protocol.client.Message msg)
    {
        if (msg.HasTag(typeof(AgIXML)))
        {
            AgIXML agiMsg = msg.SelectSingleElement(typeof(AgIXML)) as AgIXML;
            agiMsg newMsg = new agiMsg();
            newMsg.ID = agiMsg.ID;
            newMsg.Type = agiMsg.Type;
            newMsg.Path = agiMsg.Path;
            newMsg.SessionID = agiMsg.SessionID;
            newMsg.SessionKey = agiMsg.SessionKey;
            newMsg.Data = agiMsg.Data;
            newMsg.Flags = agiMsg.Flags;

            switch (newMsg.Type)
            {
                case "tput":
                    xmppManager.world.agi.tput(newMsg);
                    break;
                case "rput":
                    xmppManager.world.agi.rput(newMsg);
                    break;
                case "tget":
                    xmppManager.world.agi.tget(newMsg);
                    break;
                case "rget":
                    xmppManager.world.agi.rget(newMsg);
                    break;
                case "tattach":
                    xmppManager.world.agi.tattach(newMsg);
                    break;
                case "rattach":
                    xmppManager.world.agi.rattach(newMsg);
                    break;
                case "tremove":
                    xmppManager.world.agi.tremove(newMsg);
                    break;
                case "rremove":
                    xmppManager.world.agi.rremove(newMsg);
                    break;
                case "tflush":
                    xmppManager.world.agi.tflush(newMsg);
                    break;
                case "rflush":
                    xmppManager.world.agi.rflush(newMsg);
                    break;
            }
        }

        if (msg.Body != null)
        {
             messages newMsg = new messages();
            newMsg.body = msg.Body;
            newMsg.from = msg.From.User;

            int count = 0;

            while (chats.Count != 0 && count < chats.Count)
            {
                if (chats.ElementAt(count).chatWith.userID == msg.From.User)
                {
                    chats.ElementAt(count).msgChain.Add(newMsg);
                    return;
                }
                count++;
            }

            chat newchat = new chat();
            newchat.msgChain.Add(newMsg);
            newchat.chatWith.userID = msg.From.User.ToString();
            chats.Add(newchat);
            Console.WriteLine(newMsg.body.ToString());
        }
    }
}

public class XMPPFS : fileSystem
{
    FileNode myroot = null;

    public XMPPFS() : base()
    {
        this.myroot = base.init();
    }

    public override fileSystem onmount(string mountPoint)
    {
        base.onmount(mountPoint);
        this.myroot.setName(getWorkPathArray(mountPoint)[getWorkPathArray(mountPoint).Length - 1]);
        this.myroot.addChild(new FileNode("ctl", fileTypes.Text));
        this.myroot.addChild(new FileNode("new", fileTypes.Text));
        this.myroot.addChild(new FileNode("contacts", fileTypes.Directory));
        this.myroot.addChild(new FileNode("chats", fileTypes.Directory));
        return this;
    }

    public override FileNode read(string path, string flags, int offset, int length)
    {
        if (path == "")
            return this.myroot;

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
        else if (getWorkPathArray(path)[1] == "contacts")
        {
            if (xmppManager.contacts.Count == 0)
            {
                FileNode contactListNode = new FileNode(path, fileTypes.Directory);
                return contactListNode;
            }
            else if(getWorkPathArray(path).Length > 2)
            {
                if (getWorkPathArray(path).Length == 3)
                {
                    int count = 0;

                    while (count < xmppManager.contacts.Count && xmppManager.contacts[count].userID != getWorkPathArray(path)[2])
                    {
                        count++;
                    }
                    //check list for username
                    if (xmppManager.contacts[count].userID == getWorkPathArray(path)[1])
                    {
                        //found it, return ./ctl ./new
                        FileNode jidNode = new FileNode(getWorkPathArray(path)[1], fileTypes.Directory);

                        jidNode.addChild(new FileNode("ctl",fileTypes.Text));
                        jidNode.addChild(new FileNode("new", fileTypes.Text));

                        return jidNode;
                    }
                      //file not found
                    return null;
                }
                else
                {
                    int count = 0;

                    while (count < xmppManager.contacts.Count && xmppManager.contacts[count].userID != getWorkPathArray(path)[1])
                    {
                        count++;
                    }

                    if (xmppManager.contacts[count].userID == getWorkPathArray(path)[1])
                    {
                        FileNode jidNode = new FileNode(getWorkPathArray(path)[1], fileTypes.Directory);
                        return jidNode;
                    }
                }
            }
            else
            {
                FileNode contactListNode = new FileNode(path, fileTypes.Directory);

                foreach (contact user in xmppManager.contacts)
                {
                    contactListNode.addChild(  new FileNode(user.userID, fileTypes.Directory));
                }

                return contactListNode;
            }
        }
        else if (getWorkPathArray(path)[1] == "chats")
        {
            if (xmppManager.chats.Count == 0)
                return new FileNode(path,fileTypes.Directory);
            else if (getWorkPathArray(path).Length > 2)
            {
                if (getWorkPathArray(path).Length > 3)
                {
                    if (getWorkPathArray(path).Length == 3)
                    {
                        int count = 0;

                        while (count < xmppManager.chats.Count && xmppManager.chats[count].chatWith.userID != getWorkPathArray(path)[2])
                        {
                            count++;
                        }
                        //check list for username
                        if (xmppManager.chats[count].chatWith.userID == getWorkPathArray(path)[2])
                        {
                            //found it, return ./ctl ./new
                            FileNode jidNode = new FileNode(getWorkPathArray(path)[1], fileTypes.Directory);

                            jidNode.addChild(new FileNode("ctl", fileTypes.Text));
                            jidNode.addChild(new FileNode("new", fileTypes.Text));

                            return jidNode;
                        }
                    }
                    if (getWorkPathArray(path).Length == 4)
                    {
                        Console.WriteLine("here i am"+ getWorkPathArray(path)[3]);
                        int count = 0;

                        while (count < xmppManager.chats.Count && xmppManager.chats[count].chatWith.userID != getWorkPathArray(path)[3])
                        {
                            count++;
                        }
                        //check list for username
                        if (xmppManager.chats[count-1].chatWith.userID == getWorkPathArray(path)[2])
                        {
                            //found usrname
                            if (xmppManager.chats[count-1].chatWith.userID == getWorkPathArray(path)[2])
                            {
                                //Console.WriteLine("returning msg"+ getWorkPathArray(path)[3]);
                                chat currentChat = xmppManager.chats[count-1];

                                FileNode jidNode = new FileNode(getWorkPathArray(path)[3], fileTypes.Text);
                                //Console.WriteLine("looking for chat " + getWorkPathArray(path)[3]);
                                //Console.WriteLine("AKA: " + Convert.ToInt32(getWorkPathArray(path)[3]));
                                jidNode = new FileNode(getWorkPathArray(path)[3], fileTypes.Text);
                                jidNode.putData(currentChat.msgChain.ElementAt(Convert.ToInt32(getWorkPathArray(path)[3])).body);
                                return jidNode;
                            }
                            return null;
                        }
                    }
                    //file not found
                    return null;
                }
                else
                {
                    int count = 0;

                    while (count < xmppManager.chats.Count && xmppManager.chats[count].chatWith.userID != getWorkPathArray(path)[1])
                    {
                        count++;
                    }

                    if (xmppManager.chats.ElementAt(count-1).chatWith.userID == getWorkPathArray(path)[2])////
                    {
                        chat currentChat = xmppManager.chats.ElementAt(count - 1);

                        FileNode jidNode = new FileNode(getWorkPathArray(path)[2], fileTypes.Directory);
                        int msgcount = 0;

                        foreach (messages msg in currentChat.msgChain)
                        {
                            jidNode.addChild(new FileNode(msgcount.ToString(), fileTypes.Directory));
                            msgcount++;
                        }

                        return jidNode;
                    }
                    else
                        return new FileNode(getWorkPathArray(path)[1],fileTypes.Directory);
                }
            }
            else
            {
                FileNode contactListNode = new FileNode(path, fileTypes.Directory);

                foreach (chat chat in xmppManager.chats)
                {
                    contactListNode.addChild(new FileNode(chat.chatWith.userID, fileTypes.Directory));
                }

                return contactListNode;
            }
        }
        return null;
    }

    public override int write(string path, int flags, string buffer, int offset, int length)
    {
        if (getWorkPathArray(path)[1] == "contacts")
        {
            Console.WriteLine("looking for contacts:" + getNewfileName(path));
            return 0;
        }
        else if (getWorkPathArray(path)[1] == "chats")
        {
            Console.WriteLine("looking for chats:" + getNewfileName(path)+" : " + buffer);
            xmppManager.xmpp.Send(new agsXMPP.protocol.client.Message("fucifer@softsurve.com", MessageType.chat, buffer));
            return 0;
        }
        else if (getWorkPathArray(path)[1] == "nodes")
        {
            Console.WriteLine("write received:" + buffer);

            AgIXML agiMsg = new AgIXML("tput", "/home");
            Jid to = new Jid("dlockamy@softsurve.com");
            Message msg = new Message();
            msg.To = to;
            msg.AddChild(agiMsg);

            xmppManager.xmpp.Send(msg);

            return 0;
        }
        else
           return errors.READ_ONLY;
    }
}