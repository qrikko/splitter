using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace speedrun
{   
    [System.Serializable]
    public class Split {
        public int attempt_index;
        public long split_time;
        public long split_duration;
    }

    [System.Serializable]
    public class RunAttempt {
        public int attempt_index;
        public string start_datetime;
        public string end_datetime;
        public bool finished;
        //public List<Split> splits = new List<Split>();
    }

    [System.Serializable]
    public class GameMeta {
        public string thumb_path;
        public string name;
        public string catergory;
        public string start_offset;
        public int attempts_count;
        public int finished_count;
        public int pb_attempt_index;
        public bool limerick = false;
    }

    [System.Serializable]
    public class SplitMeta { // should probably be renamed segment
        public string thumb_path;
        public string name;
        public long pb = 0;
        public int pb_index = 0;
        public long gold = 0;
        public List<Split> history = new List<Split>();
        public bool pause_state = false;
    }

    [System.Serializable]
    public class Run
    {
        public GameMeta game_meta = new GameMeta();
        public List<SplitMeta> split_meta = new List<SplitMeta>();
        public List<RunAttempt> attempts = new List<RunAttempt>();
    }

    [System.Serializable]
    public class RunModel
    {
        public Run run = new Run();

        public void save(string path)
        {
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            FileStream fs = new FileStream(path, FileMode.Create);

            string json = JsonConvert.SerializeObject(this);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(json);
            sw.Close();
            fs.Close();
        }
    }
}