namespace Common.Data.Options
{
    public class Setting
    {
        private string _Name;
        private string _Data;

        public string Data
        {
            get { return _Data; }
            set { _Data = value; }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
    }
}