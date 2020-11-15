using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;

using System.Timers;

public class ImportExportSplits : MonoBehaviour
{
    [SerializeField] private FileBrowser _filepicker = null;
    public void on_click() {
        FileBrowser filepicker = Instantiate(_filepicker);
        filepicker.gameObject.SetActive(true);

        string[] filters = {".lss"};
        filepicker.show((string path) => {
            Debug.Log(path);
            create_splits_from_xml(path);
        }, filters);
    }

    private void create_splits_from_xml(string xml_path) {
        speedrun.RunModel run = new speedrun.RunModel();

        XmlDocument doc = new XmlDocument();
        doc.Load(xml_path);

        ///////////////////
        // Run meta-data
        run.run.game_meta.name = doc.DocumentElement.SelectSingleNode("/Run/GameName").InnerText;
        run.run.game_meta.catergory = doc.DocumentElement.SelectSingleNode("/Run/CategoryName").InnerText;
        System.TimeSpan ts = System.TimeSpan.Parse(doc.DocumentElement.SelectSingleNode("/Run/Offset").InnerText);
        //run.run.game_meta.start_offset = string.Format("{0}.{0}", ts.Seconds, ts.Milliseconds);
        run.run.game_meta.start_offset = ts.Seconds + "." + ts.Milliseconds;
        
        run.run.game_meta.attempts_count = System.Int32.Parse(doc.DocumentElement.SelectSingleNode("/Run/AttemptCount").InnerText);

        ////////////////
        // Split meta-data
        XmlNode segment_nodelist = doc.DocumentElement.SelectSingleNode("/Run/Segments");
        foreach(XmlNode node in segment_nodelist) {
            speedrun.SplitMeta m = new speedrun.SplitMeta();
            
            m.name = node.SelectSingleNode("Name").InnerText;
            m.thumb_path = node.SelectSingleNode("Icon").InnerText;
            // need to take the info which is formated in hh:mm:ss.fffffff in livesplit
            // and make it a timestamp-friendly time..(ms I think I use..)
            string pb_time = node.SelectSingleNode("SplitTimes/SplitTime/RealTime").InnerText;
            System.TimeSpan pb_dt = System.TimeSpan.Parse(pb_time);
            m.pb = (long)pb_dt.TotalMilliseconds;
            // need to find the pb-index when parsing history!
            string glod_time = node.SelectSingleNode("BestSegmentTime/RealTime").InnerText;
            System.TimeSpan glod_dt = System.TimeSpan.Parse(glod_time);
            m.gold = (long)glod_dt.TotalMilliseconds;

            /////////////////////////////////
            // parse the history for the run!
            XmlNode history = node.SelectSingleNode("SegmentHistory");
            foreach (XmlNode entry in history) {
                speedrun.Split split = new speedrun.Split();
                
                split.attempt_index = System.Int32.Parse(entry.Attributes.GetNamedItem("id").InnerText);
                System.TimeSpan entry_ts = System.TimeSpan.Parse(entry.SelectSingleNode("RealTime").InnerText);
                split.split_time = (long)entry_ts.TotalMilliseconds;
                split.split_duration = (long)entry_ts.TotalMilliseconds;
                // duration is tricky it is what livesplit actually store..
                // I am not sure we'll have to try it out and figure out as we go.. do we just skip duration, or do we try to calculate it?

                m.history.Add(split);
            }
            run.run.split_meta.Add(m);
        }
        
        ///////////
        // Attempts
        XmlNode attempts = doc.DocumentElement.SelectSingleNode("/Run/AttemptHistory");
        foreach(XmlNode attempt_node in attempts) {
            speedrun.RunAttempt attempt = new speedrun.RunAttempt();
            attempt.attempt_index = System.Int32.Parse(attempt_node.Attributes.GetNamedItem("id").InnerText);
            attempt.start_datetime = attempt_node.Attributes.GetNamedItem("started").InnerText;
            attempt.end_datetime = attempt_node.Attributes.GetNamedItem("ended").InnerText;
            attempt.finished = attempt_node.HasChildNodes;

            run.run.attempts.Add(attempt);
        }

        string json_path = System.IO.Path.GetFileName(xml_path);
        json_path = json_path.Replace(".lss", ".json");

        string id = PlayerPrefs.GetString("active_game");
        string path = Application.persistentDataPath + "/" + id + "/splits/" + json_path;

        run.save(path);
        
        //run.run.split_meta
    }
}
