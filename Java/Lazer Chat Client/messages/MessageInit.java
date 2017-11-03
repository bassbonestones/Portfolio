package gateley_stones_groupproject_client.messages;

import org.json.JSONStringer;

public class MessageInit extends Message
{
    public String username;
    public boolean usingPicturePreset;
    public int picturePresetID;
    public String pictureURL;
    
    @Override
    public String toJSON()
    {
        JSONStringer stringer = new JSONStringer();
        
        String ret =
                stringer.object()
		    .key("type").value("I")
		    .key("username").value(username)
		    .key("picturePreset").value(usingPicturePreset)
		    .key("picturePresetID").value(picturePresetID)
		    .key("pictureURL").value(pictureURL)
		.endObject().toString();
	
	return ret;
    }
}
