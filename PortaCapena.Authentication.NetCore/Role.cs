namespace PortaCapena.Authentication.NetCore
{
    public abstract class Role
    {
        /// <summary>
        /// Value that represnets the role
        /// </summary>
        public abstract object Value { get; }
                        
        /// <summary>
        /// String Value which is represented in token
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString();
    }
}
