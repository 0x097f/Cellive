namespace CilDotNet
{
    public class Annotations
    {
        public DateTime Timestamp;
        //Constructure
        public Annotations()
        {
            Timestamp = DateTime.Now;
        }

        public string GenAnnotate(string type, int id)
        {
            return $"// Type:{type} Id:0x{id:X8}";
        }

        public string GenHeader()
        {
            return $"// Generate by CilDotNet\n//Timestamp:{Timestamp}";
        }

        public string GenAny(string msg)
        {
            return $"// {msg}";
        }
    }
}