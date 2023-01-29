namespace ColorzCore.DataTypes
{
    public struct Location
    {
        public string file;
        public int lineNum, colNum;

        public Location(string fileName, int lineNum, int colNum) : this()
        {
            file = fileName;
            this.lineNum = lineNum;
            this.colNum = colNum;
        }
    }
}
