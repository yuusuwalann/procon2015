using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2015_2
{
    class Program
    {
        static void Main(string[] args)
        {
            String initFile = "app_config.txt";
            if(args.Length != 0)
            {
                initFile = args[0];
            }

            COMMON_MAP commonMap = new COMMON_MAP(initFile);

            Console.WriteLine("SEARCH_FOR=>" + commonMap.PROBLEM_FILE);

            GameInfo gi = new GameInfo();
            gi.GetProblem(commonMap);

            AdvanceGameInfo agi = new AdvanceGameInfo(gi);

            Search sch = new Search(agi, commonMap);
            int score = sch.SearchStart();

             Console.WriteLine("file:{0} score:{1}", commonMap.FILENAME, score.ToString());

        }
    }
}
