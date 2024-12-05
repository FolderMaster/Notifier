namespace ConsoleApp.Inspection
{
    public class SavedDataFilter : IFilter
    {
        private IDataBaseContext<DbInspectorViolation> _dataBaseContext;

        public SavedDataFilter(IDataBaseContext<DbInspectorViolation> dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
            _dataBaseContext.Load();
        }

        public IEnumerable<InspectorViolation> Filter(IEnumerable<InspectorViolation> violations)
        {
            var result = new List<InspectorViolation>();
            foreach (var violation in violations)
            {
                if (!_dataBaseContext.Data.Any(v => AreEqualViolationAndDbData(v, violation)))
                {
                    _dataBaseContext.Data.Add(new DbInspectorViolation()
                    {
                        IssueId = violation.Issue.Id.ToString(),
                        UserId = violation.User.Id.ToString(),
                        InspectorType = violation.Inspector.GetType(),
                    });
                    result.Add(violation);
                }
            }
            _dataBaseContext.Save();
            return result;
        }

        private bool AreEqualViolationAndDbData(DbInspectorViolation dbViolation,
            InspectorViolation violation)
        {
            return dbViolation.IssueId == violation.Issue.Id.ToString() &&
                dbViolation.UserId == violation.User.Id.ToString() &&
                dbViolation.InspectorType == violation.Inspector.GetType();    
        }
    }
}
