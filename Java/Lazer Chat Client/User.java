package gateley_stones_groupproject_client;

import java.util.ArrayList;

public class User
{    
    public String username, pictureURL;
    public boolean usingPicturePreset;
    public int picturePresetID;
    
    public static ArrayList<User> users = new ArrayList();
    
    public static void addUser(User user)
    {
        users.add(user);
    }
    
    public static void removeUser(User user)
    {
        users.remove(user);
    }

}