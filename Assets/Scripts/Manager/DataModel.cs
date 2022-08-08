using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DataModel
{
    [System.Serializable]
    public class Data
    {
        public string video;
        public string target;
        public List<string> _follow_ups;
        public bool _force_open;
        public string _open_state;
        public string response_type;
    }
    [System.Serializable]
    public class PlaceholderAliases
    {
    }
    [System.Serializable]
    public class ResponseChannels
    {
        public string voice;
        public string frames;
        public string shapes;
    }
    [System.Serializable]
    public class VideoResponse
    {
        [SerializeField]
        public Data data;
        public string name;
        public int wait;
        public string title;
        public object whiteboard;
        public string placeholder;
        public bool show_feedback;
        public StateOptions state_options;
        public bool show_in_history;
        public ToStateFunction to_state_function;
        public bool maintain_whiteboard;
        public bool overwrite_whiteboard;
        public PlaceholderAliases placeholder_aliases;
        public ResponseChannels response_channels;
        public string whiteboard_title;
        public object options;
        public string engagement_id;
        public string customer_state;
        public string console;
        public string response_id;
        public string start_timestamp;
        public string timestamp;
    }
    [System.Serializable]
    public class StateOptions
    {
        public string cs_top_three;
        public string cs_must_have;
        public string cs_about_collection;
        public string cs_men_explore;
        public string cs_return;
        public string cs_enquiry;
    }
    [System.Serializable]
    public class ToStateFunction
    {
        public string function;
    }
}