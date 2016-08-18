namespace GeneratedCuboids
{
    public class Pair<T1, T2>
    {
        private T1 first;
        private T2 second;

        public Pair(T1 first, T2 second)
        {
            this.First = first;
            this.Second = second;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Pair<T1, T2> other = obj as Pair<T1, T2>;

            return (other.First.Equals(this.First) && other.Second.Equals(this.Second));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public T1 First
        {
            get
            {
                return first;
            }

            set
            {
                first = value;
            }
        }

        public T2 Second
        {
            get
            {
                return second;
            }

            set
            {
                second = value;
            }
        }
    }
}