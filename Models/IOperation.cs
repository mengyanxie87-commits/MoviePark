namespace MyApiProject.Models
{
    public interface IOperations<T,K>
    {
        void Insert(T t); // 修改為 void
        void Update(T t); // 修改為 void
        void Delete(T t); // 修改為 void
        List<T> FindAll(); // 返回 List<T>
        List<T> FindByCondition(K key); // 根據條件查詢


    }
}
