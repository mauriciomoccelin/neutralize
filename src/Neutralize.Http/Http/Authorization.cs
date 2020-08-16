using System;

namespace Neutralize.Http
{
    public class Authorization : IEquatable<Authorization>
    {
        public static readonly Authorization Empty = new Authorization();

        public string Token { get; }
        public string Method { get; }

        private Authorization() { }
        private Authorization(string token, string method)
        {
            Token = token;
            Method = method;
        }
        
        public bool IsEmpty => Token == null;

        public static Authorization Create(string token, string method = "Bearer")
        {
            return new Authorization(token, method);
        }

        public static implicit operator Authorization(string token) => Create(token);

        public static bool operator ==(Authorization authorization, Authorization other)
        {
            return authorization != null && authorization.Equals(other);
        }

        public static bool operator != (Authorization authorization, Authorization other)
        {
            return !(authorization == other);
        }

        public override string ToString() => IsEmpty ? string.Empty : $"{Method} {Token}";

        public override bool Equals(object obj) => obj is Authorization authorization && Equals(authorization);
        public bool Equals(Authorization other) => other != null && (Token == other.Token && Method == other.Method);

        public override int GetHashCode() => HashCode.Combine(Token, Method);
    }
}
