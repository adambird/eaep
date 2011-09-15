namespace eaep.servicehost.store
{
    public class ComparisonQueryExpression : IQueryExpression
    {

        public string Field { get; set; }
        public ExpressionComparator Comparator { get; set; }
        public object Value { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != this.GetType()) return false;

            ComparisonQueryExpression other = (ComparisonQueryExpression)obj;

            if (!object.Equals(this.Comparator, other.Comparator)) return false;
            if (!object.Equals(this.Field, other.Field)) return false;
            if (!object.Equals(this.Value, other.Value)) return false;

            return true;
        }
    }
}
