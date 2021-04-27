using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.EntityFrameworkCore;


namespace evolib
{
    public static class GivenHistory
    {
        public class Model
        {
            public GiveAndTake.Type type { get; set; }
            public string id { get; set; }
            public Int64 amount { get; set; }
            public PresentBox.Type giveType { get; set; }
            public string text { get; set; }
        }

        public static int HistoryNum { get { return 100; } }

        static async Task<List<Databases.personal.GivenHistory>> UpdateAsync(long playerId, Databases.personal.PersonalDBContext db)
        {
            var changed = false;

            var source = await db.GivenHistorys.Where(r => r.playerId == playerId).ToListAsync();

            var list = source.OrderBy(x => x.obtainedDate).ThenBy(x => x.Id).ToList();

            var historyList = new List<Databases.personal.GivenHistory>();

            var count = list.Count;

            if (count > HistoryNum)
            {
                historyList.AddRange(list.Skip(count - HistoryNum));

                var removeList = new List<Databases.personal.GivenHistory>();
                removeList.AddRange(list.Take(count - HistoryNum));

                db.GivenHistorys.RemoveRange(removeList);

                changed = true;
            }
            else
            {
                historyList.AddRange(list);
            }

            if (changed)
            {
                await db.SaveChangesAsync();
            }

            return historyList;

        }

        public static async Task<List<Databases.personal.GivenHistory>> GetAsync(long playerId, Databases.personal.PersonalDBContext db)
        {
            return await UpdateAsync(playerId, db);
        }

        public static async Task AddAsync(long playerId, Databases.personal.PersonalDBContext db, List<Model> models)
        {
            var now = DateTime.UtcNow;

            foreach (var model in models)
            {
                var record = new Databases.personal.GivenHistory
                {
                    playerId = playerId,
                    obtainedDate = now,
                    type = model.type,
                    presentId = model.id,
                    amount = model.amount,
                    giveType = model.giveType,
                    text = model.text,
                };

                await db.GivenHistorys.AddAsync(record);
            }

            await db.SaveChangesAsync();

            // テーブル更新用にUpdate(件数制限制御)
            await UpdateAsync(playerId, db);
        }
    }
}