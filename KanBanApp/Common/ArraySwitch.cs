namespace KanBanApp.Common;

public class ArraySwitch<T>
{
    private List<ArraySwitchRegister> Registers { get; }

    public ArraySwitch()
    {
        Registers = new List<ArraySwitchRegister>();
    }

    public ArraySwitchRegister Register(params T[] values)
    {
        var register = new ArraySwitchRegister(this, values);
        
        Registers.Add(register);

        return register;
    }

    public bool Try(T[] array)
    {
        foreach (var register in Registers)
        {
            if (register.Try(array))
            {
                return true;
            }
        }

        return false;
    }

    public class ArraySwitchRegister
    {
        private ArraySwitch<T> Parent { get; }
        private T[] Values { get; }
        private Action? Action { get; set; }

        public ArraySwitchRegister(ArraySwitch<T> parent, T[] values)
        {
            Parent = parent;
            Values = values;
        }

        public ArraySwitch<T> As(Action action)
        {
            Action = action;
            
            return Parent;
        }

        public bool Try(T[] array)
        {
            if (array.SequenceEqual(Values))
            {
                Action?.Invoke();
                
                return true;
            }

            return false;
        }
    }
}