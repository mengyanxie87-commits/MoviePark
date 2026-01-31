using System.Data;
using System.Data.SqlClient;

public class ConnectionFactory
{
    // 請依你的 SQL Server 設定調整
    private readonly string _connectionString =
        "Server=localhost;Database=carplace;User Id=sme322;Password=Sme322820827;TrustServerCertificate=True;";
    public ConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("nor");
    }
    public void ConnectToDatabase()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("成功連接到資料庫！");

                // 在這裡執行資料庫操作
            }
            catch (Exception ex)
            {
                Console.WriteLine($"連接失敗: {ex.Message}");
            }
        }
    }
    public void InsertData(int id,string Type, string platform, int whool=4)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Cars (id,Type, platform,whool) VALUES (@id,@Type, @platform,@whool)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@Type", Type);
                command.Parameters.AddWithValue("@platform", platform);
                command.Parameters.AddWithValue("@whool", whool);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("資料插入成功！");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"插入失敗: {ex.Message}");
                }
            }
        }
    }
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}