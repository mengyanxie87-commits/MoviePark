using System.Collections.Generic;

namespace MyApiProject.Services
{
    public interface IOperations<T, K>
    {
        List<T> FindAll();
        T FindById(K id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        // 假設這是一個條件查詢
        // 其他方法的定義
        List<T> FindByCondition(string condition);
    }
}