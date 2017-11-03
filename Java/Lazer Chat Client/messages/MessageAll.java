package gateley_stones_groupproject_client.messages;

import org.json.JSONStringer;

public class MessageAll extends Message
{
    public String text;
    public String from; 
    
    @Override
    public String toJSON()
    {
	JSONStringer stringer = new JSONStringer();
	
	String ret = 
	    stringer.object()
		.key("type").value("A")
		.key("text").value(text)
		.key("from").value(from)
	    .endObject().toString();
	
	return ret;
    }
}
