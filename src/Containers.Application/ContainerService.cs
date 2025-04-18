// using System.ComponentModel;
// using Containers.Models;

using Containers.Application;
using Containers.Models;
using Microsoft.Data.SqlClient;
// namespace Containers.Application;

public class ContainerService : IContainerService
{
    public bool AddContainer(Container container)
    {
        const string insertString =
            "INSERT INTO Containers(ContainerTypeId, IsHasazrdious, Name) VALUES (@ContainerTypeId, @IsHasazrdious, @Name)";
        int countRowsAdded = -1;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            // connection = command.ExecuteNonQuery();
            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@ContainerTypeId", container.ContainerTypeId);
            command.Parameters.AddWithValue("@IsHazardious", container.IsHazardous);
            command.Parameters.AddWithValue("@Name", container.Name);
            connection.Open();
            countRowsAdded = command.ExecuteNonQuery();
            
        }
        return countRowsAdded != -1;
    }

    private string _connectionString;

    public ContainerService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<Container> GetAllContainers()
    {
        List<Container> containers = [];
        const string queryString = "SELECT * FROM Containers";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var containerRow = new Container
                        {
                            ID = reader.GetInt32(0),
                            ContainerTypeId = reader.GetInt32(1),
                            IsHazardous = reader.GetBoolean(2),
                            Name = reader.GetString(3),
                        };
                    }
                }
            }
            finally
            {
                reader.Close();
            }
        }
        return containers;
    }
}
