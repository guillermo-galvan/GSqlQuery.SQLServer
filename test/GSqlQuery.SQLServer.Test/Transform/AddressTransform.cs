﻿using GSqlQuery.Cache;
using GSqlQuery.Runner;
using GSqlQuery.SQLServer.Test.Data.Tables;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Types;

namespace GSqlQuery.SQLServer.Test.Transform
{
    public class AddressTransform : ITransformTo<Address, SqlDataReader>
    {
        private Address _address;

        public AddressTransform()
        {
            _address = new Address();
        }

        private struct AddressOrdinal
        {
            public AddressOrdinal()
            { }

            public int AddressId { get; set; } = -1;

            public int Address1 { get; set; } = -1;

            public int Address2 { get; set; } = -1;

            public int District { get; set; } = -1;

            public int CityId { get; set; } = -1;

            public int PostalCode { get; set; } = -1;

            public int Phone { get; set; } = -1;

            public int Location { get; set; } = -1;

            public int LastUpdate { get; set; } = -1;
        }

        private AddressOrdinal GetAddressOrdinal(IQuery<Address> query, SqlDataReader reader)
        {
            var result = new AddressOrdinal();

            foreach (KeyValuePair<string, PropertyOptions> item in query.Columns)
            {
                switch (item.Value.PropertyInfo.Name)
                {
                    case nameof(Address.AddressId):
                        result.AddressId = reader.GetOrdinal(item.Value.ColumnAttribute.Name);
                        break;
                    case nameof(Address.Address1):
                        result.Address1 = reader.GetOrdinal(item.Value.ColumnAttribute.Name);
                        break;
                    case nameof(Address.Address2):
                        result.Address2 = reader.GetOrdinal(item.Value.ColumnAttribute.Name);
                        break;
                    case nameof(Address.District):
                        result.District = reader.GetOrdinal(item.Value.ColumnAttribute.Name);
                        break;
                    case nameof(Address.CityId):
                        result.CityId = reader.GetOrdinal(item.Value.ColumnAttribute.Name);
                        break;
                    case nameof(Address.PostalCode):
                        result.PostalCode = reader.GetOrdinal(item.Value.ColumnAttribute.Name);
                        break;
                    case nameof(Address.Phone):
                        result.Phone = reader.GetOrdinal(item.Value.ColumnAttribute.Name);
                        break;
                    case nameof(Address.Location):
                        result.Location = reader.GetOrdinal(item.Value.ColumnAttribute.Name);
                        break;
                    case nameof(Address.LastUpdate):
                        result.LastUpdate = reader.GetOrdinal(item.Value.ColumnAttribute.Name);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public IEnumerable<Address> Transform(PropertyOptionsCollection propertyOptions, IQuery<Address> query, SqlDataReader reader, DatabaseManagementEvents events)
        {
            List<Address> result = [];

            AddressOrdinal ordinals = GetAddressOrdinal(query, reader);

            while (reader.Read())
            {
                int addressId = default;
                string address1 = default;
                string address2 = default;
                string district = default;
                int cityId = default;
                string postalCode = default;
                string phone = default;
                SqlGeometry location = default;
                DateTime lastUpdate = default;

                foreach (KeyValuePair<string, PropertyOptions> item in query.Columns)
                {
                    switch (item.Value.PropertyInfo.Name)
                    {
                        case nameof(Address.AddressId):
                            addressId = reader.IsDBNull(ordinals.AddressId) ? addressId : reader.GetInt32(ordinals.AddressId);
                            break;
                        case nameof(Address.Address1):
                            address1 = reader.IsDBNull(ordinals.Address1) ? address1 : reader.GetString(ordinals.Address1);
                            break;
                        case nameof(Address.Address2):
                            address2 = reader.IsDBNull(ordinals.Address2) ? address2 : reader.GetString(ordinals.Address2);
                            break;
                        case nameof(Address.District):
                            district = reader.IsDBNull(ordinals.District) ? district : reader.GetString(ordinals.District);
                            break;
                        case nameof(Address.CityId):
                            cityId = reader.IsDBNull(ordinals.CityId) ? cityId : reader.GetInt32(ordinals.CityId);
                            break;
                        case nameof(Address.PostalCode):
                            postalCode = reader.IsDBNull(ordinals.PostalCode) ? postalCode : reader.GetString(ordinals.PostalCode);
                            break;
                        case nameof(Address.Phone):
                            phone = reader.IsDBNull(ordinals.Phone) ? phone : reader.GetString(ordinals.Phone);
                            break;
                        case nameof(Address.Location):
                            location = reader.IsDBNull(ordinals.Location) ? location : (SqlGeometry)reader.GetValue(ordinals.Location);
                            break;
                        case nameof(Address.LastUpdate):
                            lastUpdate = reader.IsDBNull(ordinals.LastUpdate) ? lastUpdate : reader.GetDateTime(ordinals.LastUpdate);
                            break;
                        default:
                            break;
                    }
                }

                result.Add(new Address(addressId, address1, address2, district, cityId, postalCode, phone, location, lastUpdate));
            }

            return result;
        }

        public async Task<IEnumerable<Address>> TransformAsync(PropertyOptionsCollection propertyOptions, IQuery<Address> query, SqlDataReader reader, DatabaseManagementEvents events, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<Address> result = [];

            AddressOrdinal ordinals = GetAddressOrdinal(query, reader);

            while (await reader.ReadAsync(cancellationToken))
            {
                int addressId = default;
                string address1 = default;
                string address2 = default;
                string district = default;
                int cityId = default;
                string postalCode = default;
                string phone = default;
                SqlGeometry location = default;
                DateTime lastUpdate = default;

                foreach (KeyValuePair<string, PropertyOptions> item in query.Columns)
                {
                    switch (item.Value.PropertyInfo.Name)
                    {
                        case nameof(Address.AddressId):
                            addressId = await reader.IsDBNullAsync(ordinals.AddressId, cancellationToken) ? addressId : reader.GetInt32(ordinals.AddressId);
                            break;
                        case nameof(Address.Address1):
                            address1 = await reader.IsDBNullAsync(ordinals.Address1, cancellationToken) ? address1 : reader.GetString(ordinals.Address1);
                            break;
                        case nameof(Address.Address2):
                            address2 = await reader.IsDBNullAsync(ordinals.Address2, cancellationToken) ? address2 : reader.GetString(ordinals.Address2);
                            break;
                        case nameof(Address.District):
                            district = await reader.IsDBNullAsync(ordinals.District, cancellationToken) ? district : reader.GetString(ordinals.District);
                            break;
                        case nameof(Address.CityId):
                            cityId = await reader.IsDBNullAsync(ordinals.CityId, cancellationToken) ? cityId : reader.GetInt32(ordinals.CityId);
                            break;
                        case nameof(Address.PostalCode):
                            postalCode = await reader.IsDBNullAsync(ordinals.PostalCode, cancellationToken) ? postalCode : reader.GetString(ordinals.PostalCode);
                            break;
                        case nameof(Address.Phone):
                            phone = await reader.IsDBNullAsync(ordinals.Phone, cancellationToken) ? phone : reader.GetString(ordinals.Phone);
                            break;
                        case nameof(Address.Location):
                            location = await reader.IsDBNullAsync(ordinals.Location, cancellationToken) ? location : (SqlGeometry)reader.GetValue(ordinals.Location);
                            break;
                        case nameof(Address.LastUpdate):
                            lastUpdate = await reader.IsDBNullAsync(ordinals.LastUpdate, cancellationToken) ? lastUpdate : reader.GetDateTime(ordinals.LastUpdate);
                            break;
                        default:
                            break;
                    }
                }

                result.Add(new Address(addressId, address1, address2, district, cityId, postalCode, phone, location, lastUpdate));
            }

            return result;
        }

        public void SetValue(PropertyOptions property, object value)
        {
            switch (property.PropertyInfo.Name)
            {
                case nameof(Address.AddressId):
                    _address.AddressId = (int)value;
                    break;
                case nameof(Address.Address1):
                    _address.Address1 = (string)value;
                    break;
                case nameof(Address.Address2):
                    _address.Address2 = (string)value;
                    break;
                case nameof(Address.District):
                    _address.District = (string)value;
                    break;
                case nameof(Address.CityId):
                    _address.CityId = (int)value;
                    break;
                case nameof(Address.PostalCode):
                    _address.PostalCode = (string)value;
                    break;
                case nameof(Address.Phone):
                    _address.Phone = (string)value;
                    break;
                case nameof(Address.Location):
                    _address.Location = (SqlGeometry)value;
                    break;
                case nameof(Address.LastUpdate):
                    _address.LastUpdate = (DateTime)value;
                    break;
                default:
                    break;
            }
        }

        public Address GetEntity()
        {
            Address result = _address;
            _address = new Address();
            return result;
        }        
    }
}
