using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class ChallengeConverter : BaseConverter<Challenge>
    {
        public override Challenge Convert(DataSet set, int row)
        {
            var obj = new Challenge();
            obj.ID = set.GetInt(row, "id");
            obj.Name = set.GetString(row, "name");
            obj.Description = set.GetString(row, "description");
            obj.Points = set.GetInt(row, "points");
            obj.ValidityStart = set.GetDateTime(row, "validity_start");
            obj.ValidityEnd = set.GetDateTime(row, "validity_end");
            obj.Category = (ChallengeCategory)set.GetInt(row, "category");
            obj.OwnerID = set.GetInt(row, "owner_id");
            obj.Created = set.GetDateTime(row, "created");
            obj.Prove = (ChallengeProve)set.GetInt(row, "prove");
            obj.SeriesParentID = set.GetInt(row, "series_parent_id");
            obj.Tags = set.GetString(row, "tags_of_challenge");
            obj.ProveAddendum = set.GetString(row, "prove_addendum");
            obj.DateAccepted = set.GetDateTime(row, "dateaccepted");
            return obj;
        }
    }
}

