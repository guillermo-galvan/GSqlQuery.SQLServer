namespace GSqlQuery.SQLServer.Test.Data.Tables
{
    [Table("store")]
    public class Store : EntityExecute<Store>
    {
        [Column("store_id", Size = 3, IsAutoIncrementing = true, IsPrimaryKey = true)]
        public byte StoreId { get; set; }

        [Column("manager_staff_id", Size = 3)]
        public byte ManagerStaffId { get; set; }

        [Column("address_id", Size = 5)]
        public long AddressId { get; set; }

        [Column("last_update", Size = 0)]
        public DateTime LastUpdate { get; set; }

        public Store()
        { }

        public Store(byte storeId, byte managerStaffId, long addressId, DateTime lastUpdate)
        {
            StoreId = storeId;
            ManagerStaffId = managerStaffId;
            AddressId = addressId;
            LastUpdate = lastUpdate;
        }
    }
}
