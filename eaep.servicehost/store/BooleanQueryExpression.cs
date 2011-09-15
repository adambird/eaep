namespace eaep.servicehost.store
{
    public class BooleanQueryExpression : IQueryExpression
    {
        public IQueryExpression Left { get; set;}
        public BooleanOperator Operator {get; set;}
        public IQueryExpression Right { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != this.GetType()) return false;

            BooleanQueryExpression other = (BooleanQueryExpression)obj;

            if (!object.Equals(this.Operator, other.Operator)) return false;
            if (!object.Equals(this.Left, other.Left)) return false;
            if (!object.Equals(this.Right, other.Right)) return false;

            return true;
        }
    }
}
