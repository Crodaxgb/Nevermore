using System;
using System.Collections.Generic;

namespace NevermoreStudios.Core
{
    public sealed class FastName : IEquatable<FastName>, IComparable<FastName>
    {
        public static FastName None = new();

        private readonly UInt32 _nameID;

        private FastName()
        {
            _nameID = 0;
        }

        public FastName(string name)
        {
            _nameID = FastNameManager.CreateOrRetrieveID(name);
        }

        public override string ToString() 
        {
            if (this == None)
                return "## None ##";

            return FastNameManager.RetrieveNameFromID(_nameID);
        }

        public override bool Equals(object other)
        {
            return Equals(other as FastName);
        }

        public bool Equals(FastName other)
        {
            return other is not null &&
                   _nameID == other._nameID;
        }

        public override int GetHashCode()
        {
            return _nameID.GetHashCode();
        }

        public int CompareTo(FastName other)
        {
            return _nameID.CompareTo(other._nameID);
        }

        public static bool operator ==(FastName leftName, FastName rightName)
        {
            return EqualityComparer<FastName>.Default.Equals(leftName, rightName);
        }

        public static bool operator !=(FastName leftName, FastName rightName)
        {
            return !(leftName == rightName);
        }

        public static implicit operator bool(FastName other) 
        {
            return other != None;
        }
    }
    
    public class FastNameManager : Singleton<FastNameManager>
    {
        UInt32 _nextNameID = 1;
        private readonly Dictionary<UInt32, string> _nameIDs = new();
        static object _nameIDsLock = new();

        internal static uint CreateOrRetrieveID(string nameParameter)
        {
            if (Instance == null)
            {
                return 0;
            }

            return Instance.CreateOrRetrieveIDInternal(nameParameter);
        }

        internal static string RetrieveNameFromID(uint nameID)
        {
            if (Instance == null)
            {
                return "## NO NameManager ##";
            }

            return Instance.RetrieveNameFromIDInternal(nameID);
        }

        private UInt32 CreateOrRetrieveIDInternal(string nameParameter)
        {
            lock (_nameIDsLock) 
            {
                // does this name already exist?
                UInt32 foundName = 0;
                foreach(var kvp in _nameIDs) 
                { 
                    if (kvp.Value == nameParameter)
                    {
                        foundName = kvp.Key;
                        break;
                    }
                }

                // name ID not found - create
                if (foundName == 0)
                {
                    foundName = _nextNameID++;

                    _nameIDs.Add(foundName, nameParameter);
                }

                return foundName;
            }
        }

        private string RetrieveNameFromIDInternal(UInt32 nameID) 
        { 
            lock(_nameIDsLock)
            {
                return _nameIDs.GetValueOrDefault(nameID, "## Missing ID ##");
            }
        }
    }
}