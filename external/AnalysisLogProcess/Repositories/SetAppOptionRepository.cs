using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using LogFile.Models.ApiServer;

namespace AnalysisLogProcess.Repositories
{
    class SetAppOptionRepository : BaseRepository<Log>
    {
        private IList<BigQueryInsertRow> RowsGraphic { get; } = new List<BigQueryInsertRow>();
        private IList<BigQueryInsertRow> RowsVoiceChat { get; } = new List<BigQueryInsertRow>();

        public SetAppOptionRepository(BigQueryClient client, string datasetId)
            : base(client, datasetId)
        { }

        public override void Add(Log log, ref int insertCount)
        {
            var app = log.App.SetAppOption;

            if (app.Category == 1) // todo: const? enum?
            {
                var row = new BigQueryInsertRow
                {
                    { "player_id",                   app.PlayerId.Hash() },
                    { "change_datetime",             DateTimeParseToUtc(app.Date) },
                    { "viewing_angle_value",         app.Values[20] }, // todo: const? enum?
                    { "gamma_correction",            app.Values[5] },
                    { "max_framerate",               app.Values[19] },
                    { "vsync_flag",                  app.Values[21] },
                    { "graphic_quality_cd",          app.Values[3] },
                    { "render_scale_cd",             app.Values[11] },
                    { "effect_quality_cd",           app.Values[14] },
                    { "texture_filtering_cd",        app.Values[15] },
                    { "shadow_cd",                   app.Values[16] },
                    { "anti_alias_cd",               app.Values[4] },
                    { "hdr_flag",                    app.Values[8] },
                    { "color_blind_filter",          app.Values[9] },
                    { "color_blind_filter_strength", app.Values[10] },
                };

                RowsGraphic.Add(row);
                insertCount++;
            }
            else if (app.Category == 3)
            {
                var row = new BigQueryInsertRow
                {
                    { "player_id",       app.PlayerId.Hash() },
                    { "change_datetime", DateTimeParseToUtc(app.Date) },
                    { "voice_chat_flag", app.Values[5] },
                };

                RowsVoiceChat.Add(row);
                insertCount++;
            }
        }

        public override async Task InsertAsync()
        {
            if (RowsGraphic.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "player_graphic_setting_history", RowsGraphic);
            }

            if (RowsVoiceChat.Count > 0)
            {
                await Client.InsertRowsAsync(DatasetId, "player_voice_chat_setting", RowsVoiceChat);
            }
        }
    }
}
