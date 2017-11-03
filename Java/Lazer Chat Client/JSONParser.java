package gateley_stones_groupproject_client;

import gateley_stones_groupproject_client.messages.*;import java.util.ArrayList;
import org.json.JSONArray;
;
import org.json.JSONObject;

public class JSONParser
{
    public static Message getMessageFromJSON(String json)
    {
        Message message = null;
        
        JSONObject obj = new JSONObject(json);
        
        char type = obj.getString("type").charAt(0);
        
        switch(type)
        {
	    case 'A':
		MessageAll messageAll = new MessageAll();
                messageAll.from = obj.getString("from");
		messageAll.text = obj.getString("text");
		message = messageAll;
		break;
	    case 'P':
		MessagePrivate messagePrivate = new MessagePrivate();
		messagePrivate.from = obj.getString("from");
		messagePrivate.to = obj.getString("to");
		messagePrivate.text = obj.getString("text");
                message = messagePrivate;
		break;
	    case 'U':
		MessageUpdate tempMessage = new MessageUpdate();
		JSONArray userArray = obj.getJSONArray("users");
		
                User.users.clear();
                
		for(Object object : userArray)
		{
		    if(object instanceof JSONObject)
		    {
			JSONObject temp = (JSONObject)object;
			User user = new User();
			user.username = temp.getString("username");
			user.usingPicturePreset = temp.getBoolean("picturePreset");
			user.picturePresetID = temp.getInt("picturePresetID");
			user.pictureURL = temp.getString("pictureURL");
                        User.addUser(user);
		    }
		}
		break;
	    case 'I':
		MessageInit messageInit = new MessageInit();
		messageInit.username = "";
		messageInit.usingPicturePreset = false;
		messageInit.picturePresetID = 0;
		messageInit.pictureURL = "";
                message = messageInit;
                break;
		
        }
	
	return message;
    }
}
