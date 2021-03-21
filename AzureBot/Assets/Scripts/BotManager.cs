using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Bot.Connector.DirectLine;
using System.Linq;
using UnityEngine.UI;

public class BotManager : MonoBehaviour
{
    private DirectLineClient client;
    private Conversation conversation;
    private string request;

    // Start is called before the first frame update
    void Start()
    {
        StartBotAsync();
    }

    // Update is called once per frame
    void Update()
    {


    }

    async System.Threading.Tasks.Task StartBotAsync()
    {
        client = new DirectLineClient("//your direct line key");
        conversation = await client.Conversations.StartConversationAsync();
    }

    public void invia()
    {
        request = GameObject.Find("InputFieldText").GetComponent<Text>().text;
        GameObject.Find("BotLogo").GetComponent<Animation>().Play();
        inviaAsync();
    }

    public async System.Threading.Tasks.Task inviaAsync()
    {
        Activity userMessage = new Activity
        {
            From = new ChannelAccount("your channel name"),
            Text = request,
            Type = ActivityTypes.Message
        };

        await client.Conversations.PostActivityAsync(conversation.ConversationId, userMessage);
        string watermark = null;


        var activitySet = await client.Conversations.GetActivitiesAsync(conversation.ConversationId, watermark);

        watermark = activitySet?.Watermark;

        var activities = from x in activitySet.Activities
                         select x;

        Activity[] activitiess = activities.ToArray<Activity>();
        GameObject.Find("Output").GetComponent<Text>().text= activitiess[activitiess.Length-1].Text;
        GameObject.Find("BotLogo").GetComponent<Animation>().Stop();
    }
}


