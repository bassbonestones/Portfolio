package gateley_stones_groupproject_client.messages;

import org.json.JSONStringer;

public class MessagePrivate extends Message
{
    public String text;
    public String from;
    public String to;
    
    @Override
    public String toJSON()
    {
	JSONStringer stringer = new JSONStringer();
	
	String ret = 
	    stringer.object()
		.key("type").value("P")
		.key("text").value(text)
		.key("from").value(from)
		.key("to").value(to)
	    .endObject().toString();
	
	return ret;
    }
}
