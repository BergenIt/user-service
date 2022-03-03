using System;

using Novell.Directory.Ldap;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsLdapConnection : ILdapConnection
    {
        public void Add(LdapEntry entry)
        {
        }

        public void Add(LdapEntry entry, LdapConstraints cons)
        {
        }

        public LdapResponseQueue Add(LdapEntry entry, LdapResponseQueue queue)
        {
            throw new NotImplementedException();
        }

        public LdapResponseQueue Add(LdapEntry entry, LdapResponseQueue queue, LdapConstraints cons)
        {
            throw new NotImplementedException();
        }

        public void Bind(string dn, string passwd)
        {
        }

        public void Bind(int version, string dn, string passwd)
        {
        }

        public void Bind(string dn, string passwd, LdapConstraints cons)
        {
        }

        public void Bind(int version, string dn, string passwd, LdapConstraints cons)
        {
        }

        public void Bind(int version, string dn, sbyte[] passwd)
        {
        }

        public void Bind(int version, string dn, sbyte[] passwd, LdapConstraints cons)
        {
        }

        public LdapResponseQueue Bind(int version, string dn, sbyte[] passwd, LdapResponseQueue queue)
        {
            throw new NotImplementedException();
        }

        public LdapResponseQueue Bind(int version, string dn, sbyte[] passwd, LdapResponseQueue queue, LdapConstraints cons)
        {
            throw new NotImplementedException();
        }

        public void Connect(string host, int port)
        {
        }

        public void Delete(string dn)
        {
        }

        public void Delete(string dn, LdapConstraints cons)
        {
        }

        public LdapResponseQueue Delete(string dn, LdapResponseQueue queue)
        {
            throw new NotImplementedException();
        }

        public LdapResponseQueue Delete(string dn, LdapResponseQueue queue, LdapConstraints cons)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public LdapExtendedResponse ExtendedOperation(LdapExtendedOperation op)
        {
            throw new NotImplementedException();
        }

        public LdapExtendedResponse ExtendedOperation(LdapExtendedOperation op, LdapConstraints cons)
        {
            throw new NotImplementedException();
        }

        public LdapResponseQueue ExtendedOperation(LdapExtendedOperation op, LdapResponseQueue queue)
        {
            throw new NotImplementedException();
        }

        public LdapResponseQueue ExtendedOperation(LdapExtendedOperation op, LdapConstraints cons, LdapResponseQueue queue)
        {
            throw new NotImplementedException();
        }

        public void Modify(string dn, LdapModification mod)
        {
        }

        public void Modify(string dn, LdapModification mod, LdapConstraints cons)
        {
        }

        public void Modify(string dn, LdapModification[] mods)
        {
        }

        public void Modify(string dn, LdapModification[] mods, LdapConstraints cons)
        {
        }

        public LdapResponseQueue Modify(string dn, LdapModification mod, LdapResponseQueue queue)
        {
            throw new NotImplementedException();
        }

        public LdapResponseQueue Modify(string dn, LdapModification mod, LdapResponseQueue queue, LdapConstraints cons)
        {
            throw new NotImplementedException();
        }

        public LdapResponseQueue Modify(string dn, LdapModification[] mods, LdapResponseQueue queue)
        {
            throw new NotImplementedException();
        }

        public LdapResponseQueue Modify(string dn, LdapModification[] mods, LdapResponseQueue queue, LdapConstraints cons)
        {
            throw new NotImplementedException();
        }

        public LdapEntry Read(string dn)
        {
            throw new NotImplementedException();
        }

        public LdapEntry Read(string dn, LdapSearchConstraints cons)
        {
            throw new NotImplementedException();
        }

        public LdapEntry Read(string dn, string[] attrs)
        {
            throw new NotImplementedException();
        }

        public LdapEntry Read(string dn, string[] attrs, LdapSearchConstraints cons)
        {
            throw new NotImplementedException();
        }

        public void Rename(string dn, string newRdn, bool deleteOldRdn)
        {
        }

        public void Rename(string dn, string newRdn, bool deleteOldRdn, LdapConstraints cons)
        {
        }

        public void Rename(string dn, string newRdn, string newParentdn, bool deleteOldRdn)
        {
        }

        public void Rename(string dn, string newRdn, string newParentdn, bool deleteOldRdn, LdapConstraints cons)
        {
        }

        public LdapResponseQueue Rename(string dn, string newRdn, bool deleteOldRdn, LdapResponseQueue queue)
        {
            throw new NotImplementedException();
        }

        public LdapResponseQueue Rename(string dn, string newRdn, bool deleteOldRdn, LdapResponseQueue queue, LdapConstraints cons)
        {
            throw new NotImplementedException();
        }

        public LdapResponseQueue Rename(string dn, string newRdn, string newParentdn, bool deleteOldRdn, LdapResponseQueue queue)
        {
            throw new NotImplementedException();
        }

        public LdapResponseQueue Rename(string dn, string newRdn, string newParentdn, bool deleteOldRdn, LdapResponseQueue queue, LdapConstraints cons)
        {
            throw new NotImplementedException();
        }

        public LdapSearchResults Search(string @base, int scope, string filter, string[] attrs, bool typesOnly)
        {
            throw new NotImplementedException();
        }

        public LdapSearchResults Search(string @base, int scope, string filter, string[] attrs, bool typesOnly, LdapSearchConstraints cons)
        {
            throw new NotImplementedException();
        }

        public LdapSearchQueue Search(string @base, int scope, string filter, string[] attrs, bool typesOnly, LdapSearchQueue queue)
        {
            throw new NotImplementedException();
        }

        public LdapSearchQueue Search(string @base, int scope, string filter, string[] attrs, bool typesOnly, LdapSearchQueue queue, LdapSearchConstraints cons)
        {
            throw new NotImplementedException();
        }

        public void StartTls()
        {
        }

        public void StopTls()
        {
        }
    }
}
