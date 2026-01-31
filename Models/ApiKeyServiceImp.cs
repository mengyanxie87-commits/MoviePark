namespace MyApiProject.Models
{
    public class ApiKeyServiceImp : IOperations<ApiKey, Guid>
    {
        private List<ApiKey> _apiKeys = new List<ApiKey>();

        public void Insert(ApiKey item)
        {
            _apiKeys.Add(item);
            Console.WriteLine($"Inserted API Key: {item.Key}");
        }

        public void Update(ApiKey item)
        {
            var existingKey = _apiKeys.FirstOrDefault(k => k.Id == item.Id);
            if (existingKey != null)
            {
                existingKey.Key = item.Key;
                Console.WriteLine($"Updated API Key: {item.Key}");
            }
        }

        public void Delete(ApiKey item)
        {
            _apiKeys.Remove(item);
            Console.WriteLine($"Deleted API Key: {item.Key}");
        }

        public List<ApiKey> FindAll()
        {
            return _apiKeys; // 返回完整的 List<ApiKey>
        }

        public List<ApiKey> FindByCondition(Guid id)
        {
            return _apiKeys.Where(k => k.Id == id).ToList(); // 返回符合條件的 List<ApiKey>
        }
    }
}