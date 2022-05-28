using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;




namespace ms
{
    // Class that stores information on the quest log of an individual character
    public class QuestLog
    {
        public QuestLog()
        {
            Started = new ReadOnlyDictionary<short, string>(started);
        }
        public void add_started(short qid, string qdata)
        {
            started[qid] = qdata;
        }
        public void add_in_progress(short qid, short qidl, string qdata)
        {
            in_progress[qid] = new Tuple<short, string>(qidl, qdata);
        }
        public void add_completed(short qid, long time)
        {
            completed[qid] = time;
        }
        public bool is_started(short qid)
        {
            return started.Any(pair => pair.Key == qid);
        }
        public short get_last_started()
        {
            return started.Last().Key;
        }

        private SortedDictionary<short, string> started = new SortedDictionary<short, string>();
        private SortedDictionary<short, System.Tuple<short, string>> in_progress = new SortedDictionary<short, System.Tuple<short, string>>();
        private SortedDictionary<short, long> completed = new SortedDictionary<short, long>();

        public ReadOnlyDictionary<short, string> Started;

    }
}


