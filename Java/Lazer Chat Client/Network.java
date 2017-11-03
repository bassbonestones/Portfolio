package gateley_stones_groupproject_client;

import gateley_stones_groupproject_client.messages.*;
import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStreamReader;
import java.net.Socket;
import java.util.Arrays;

public class Network
{
    private static String hostName;
    private static int port;
    
    private static Socket socket = null;
    private static DataOutputStream out;
    private static BufferedReader in;
    private static NetworkListener listener;
    
    private static class NetworkListener extends Thread
    {
        private String currentData = null;
        
        public boolean hasData()
        {
            return currentData != null;
        }
        
        public String getData()
        {
            String temp = currentData + "";
            currentData = null;
            return temp;
        }
        
        @Override
        public void run()
        {
            char[] buffer = new char[4096];
	    
            try
            {
                while((in.read(buffer)) != -1)
                {
                    currentData = Arrays.toString(buffer);
                    System.out.println(currentData);
                }
            }
            catch(Exception e)
            {
                e.printStackTrace();
            }
        }
    }
    
    public static void Init(String h, int p)
    {
        hostName = h;
        port = p;
        
        try
        {
            socket = new Socket(hostName, port);
            out = new DataOutputStream(socket.getOutputStream());
            in = new BufferedReader(new InputStreamReader(socket.getInputStream()));
            listener = new NetworkListener();
            listener.start();
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
    }
    
    public static String getHostName()
    {
        return hostName;
    }

    public static int getPort()
    {
	return port;
    }

    public static NetworkListener getListener()
    {
        return listener;
    }
    
    public static void Init()
    {
	MessageInit message = new MessageInit();
        message.username = Login.getUsername();
        message.usingPicturePreset = Login.getUsingPicturePreset();
        message.picturePresetID = Login.getFaceSelected();
        message.pictureURL = Login.getImgUrl();
        
	sendMessage(message);
    }

    public static void sendMessage(Message message)
    {
        try
        {
            out.writeBytes(message.toJSON());
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
    }

    public static void sendMessageAll(String text)
    {
        MessageAll message = new MessageAll();
        message.from = Login.getUsername();
        message.text = text;
        
        sendMessage(message);
    }
    
    public static void sendMessagePrivate(String text, String to)
    {
        MessagePrivate message = new MessagePrivate();
        message.from = Login.getUsername();
        message.text = text;
        message.to = to;
        
        sendMessage(message);
    }
}